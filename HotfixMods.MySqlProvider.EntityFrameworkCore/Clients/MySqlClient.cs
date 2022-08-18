using HotfixMods.Core.Providers;
using HotfixMods.Core.Models.Interfaces;
using HotfixMods.MySqlProvider.EntityFrameworkCore.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace HotfixMods.MySqlProvider.EntityFrameworkCore.Clients
{
    public class MySqlClient : IMySqlProvider
    {
        WorldDbContext _worldDbContext;
        HotfixesDbContext _hotfixesDbContext;
        CharactersDbContext _charactersDbContext;

        public MySqlClient(string server, string user, string password, string worldSchemaName, string charactersSchemaName, string hotfixesSchemaName)
        {
            _worldDbContext = new WorldDbContext($"server={server};user={user};password={password};database={worldSchemaName}");
            _hotfixesDbContext = new HotfixesDbContext($"server={server};user={user};password={password};database={hotfixesSchemaName}");
            _charactersDbContext = new CharactersDbContext($"server={server};user={user};password={password};database={charactersSchemaName}");
        }

        public async Task<T?> GetAsync<T>(Expression<Func<T, bool>> predicate)
            where T : class, ITrinityCore
        {
            return await SetContext<T>().Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<T>> GetManyAsync<T>(Expression<Func<T, bool>> predicate)
            where T : class, ITrinityCore
        {
            return await SetContext<T>().Set<T>().AsNoTracking().Where(predicate).ToListAsync();
        }


        public async Task AddAsync<T>(T entity)
            where T : class, ITrinityCore
        {
            SetContext<T>().Set<T>().Add(entity);
            await SetContext<T>().SaveChangesAsync();
            SetContext<T>().Entry(entity).State = EntityState.Detached;
        }

        public async Task AddManyAsync<T>(IEnumerable<T> entities)
        where T : class, ITrinityCore
        {
            SetContext<T>().Set<T>().AddRange(entities);
            await SetContext<T>().SaveChangesAsync();
            foreach(var entity in entities)
            {
                SetContext<T>().Entry(entity).State = EntityState.Detached;
            }
        }

        public async Task UpdateAsync<T>(T entity)
            where T : class, ITrinityCore
        {
            SetContext<T>().Set<T>().Update(entity);
            await SetContext<T>().SaveChangesAsync();
            SetContext<T>().Entry(entity).State = EntityState.Detached;
        }

        public async Task UpdateManyAsync<T>(IEnumerable<T> entities)
            where T : class, ITrinityCore
        {
            SetContext<T>().Set<T>().UpdateRange(entities);
            await SetContext<T>().SaveChangesAsync();

            foreach(var entity in entities)
            {
                SetContext<T>().Entry(entity).State = EntityState.Detached;
            }
        }

        public async Task DeleteAsync<T>(T entity)
            where T : class, ITrinityCore
        {
            SetContext<T>().Set<T>().Remove(entity);
            await SetContext<T>().SaveChangesAsync();
        }

        public async Task DeleteManyAsync<T>(IEnumerable<T> entities)
            where T : class, ITrinityCore
        {
            SetContext<T>().Set<T>().RemoveRange(entities);
            await SetContext<T>().SaveChangesAsync();
        }

        public async Task<bool> WorldConnectionTestAsync()
        {
            return await _worldDbContext.Database.CanConnectAsync();
        }
        public async Task<bool> CharactersConnectionTestAsync()
        {
            return await _charactersDbContext.Database.CanConnectAsync();
        }
        public async Task<bool> HotfixesConnectionTestAsync()
        {
            return await _hotfixesDbContext.Database.CanConnectAsync();
        }


        DbContext SetContext<T>()
        {
            if (typeof(IWorldSchema).IsAssignableFrom(typeof(T)))
                return _worldDbContext;
            else if (typeof(IHotfixesSchema).IsAssignableFrom(typeof(T)))
                return _hotfixesDbContext;
            else if (typeof(ICharactersSchema).IsAssignableFrom(typeof(T)))
                return _charactersDbContext;
            throw new Exception($"Incorrect implementation of {nameof(T)}. Make sure {nameof(T)} derives from one of the Schema classes.");
        }

        public async Task<bool> TableExists<T>()
            where T : class, ITrinityCore
        {
            var entityType = SetContext<T>().Model.FindEntityType(typeof(T));
            var tableName = entityType.GetTableName();

            using (var command = SetContext<T>().Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = $"SHOW TABLES LIKE @tableName;";
                command.CommandType = CommandType.Text;
                var parameter = command.CreateParameter();
                parameter.ParameterName = "tableName";
                parameter.Value = tableName;
                command.Parameters.Add(parameter);

                SetContext<T>().Database.OpenConnection();

                using (var result = await command.ExecuteReaderAsync())
                {
                    return result.HasRows;
                }
            }
        }

        public Task<bool> CreateCreatureCreatorTableIfNotExist()
        {
            throw new NotImplementedException();
        }

        async Task<bool> CreateTableIfNotExist<T>(string createQuery)
            where T : class, ITrinityCore
        {
            var entityType = SetContext<T>().Model.FindEntityType(typeof(T));

            using (var command = SetContext<T>().Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = $"{createQuery}";
                command.CommandType = CommandType.Text;

                SetContext<T>().Database.OpenConnection();
                await command.ExecuteNonQueryAsync();
            }
            return true;
        }
    }
}
