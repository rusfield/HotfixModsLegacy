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
        string _server;
        string _user;
        string _password;
        string _worldSchemaName;
        string _charactersSchemaName;
        string _hotfixesSchemaName;

        public MySqlClient(string server, string user, string password, string worldSchemaName, string charactersSchemaName, string hotfixesSchemaName)
        {
            _server = server;
            _user = user;
            _password = password;
            _worldSchemaName = worldSchemaName;
            _charactersSchemaName = charactersSchemaName;
            _hotfixesSchemaName = hotfixesSchemaName;
        }

        public async Task<T?> GetAsync<T>(Expression<Func<T, bool>> predicate)
            where T : class, ITrinityCore
        {
            using(var context = GetContext<T>())
            {
                return await context.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate);
            }
        }

        public async Task<IEnumerable<T>> GetManyAsync<T>(Expression<Func<T, bool>> predicate)
            where T : class, ITrinityCore
        {
            return await GetContext<T>().Set<T>().AsNoTracking().Where(predicate).ToListAsync();
        }


        public async Task AddAsync<T>(T entity)
            where T : class, ITrinityCore
        {
            GetContext<T>().Set<T>().Add(entity);
            await GetContext<T>().SaveChangesAsync();
            GetContext<T>().Entry(entity).State = EntityState.Detached;
        }

        public async Task AddManyAsync<T>(IEnumerable<T> entities)
        where T : class, ITrinityCore
        {
            GetContext<T>().Set<T>().AddRange(entities);
            await GetContext<T>().SaveChangesAsync();
            foreach(var entity in entities)
            {
                GetContext<T>().Entry(entity).State = EntityState.Detached;
            }
        }

        public async Task UpdateAsync<T>(T entity)
            where T : class, ITrinityCore
        {
            GetContext<T>().Set<T>().Update(entity);
            await GetContext<T>().SaveChangesAsync();
            GetContext<T>().Entry(entity).State = EntityState.Detached;
        }

        public async Task UpdateManyAsync<T>(IEnumerable<T> entities)
            where T : class, ITrinityCore
        {
            GetContext<T>().Set<T>().UpdateRange(entities);
            await GetContext<T>().SaveChangesAsync();

            foreach(var entity in entities)
            {
                GetContext<T>().Entry(entity).State = EntityState.Detached;
            }
        }

        public async Task DeleteAsync<T>(T entity)
            where T : class, ITrinityCore
        {
            GetContext<T>().Set<T>().Remove(entity);
            await GetContext<T>().SaveChangesAsync();
        }

        public async Task DeleteManyAsync<T>(IEnumerable<T> entities)
            where T : class, ITrinityCore
        {
            GetContext<T>().Set<T>().RemoveRange(entities);
            await GetContext<T>().SaveChangesAsync();
        }

        public async Task<bool> WorldConnectionTestAsync()
        {
            using(var context = GetContext<IWorldSchema>())
            {
                return await context.Database.CanConnectAsync();
            }
        }
        public async Task<bool> CharactersConnectionTestAsync()
        {
            using(var context = GetContext<ICharactersSchema>())
            {
                return await context.Database.CanConnectAsync();
            }   
        }
        public async Task<bool> HotfixesConnectionTestAsync()
        {
            using(var context = GetContext<IHotfixesSchema>())
            {
                return await context.Database.CanConnectAsync();
            }
        }


        DbContext GetContext<T>()
        {
            if (typeof(IWorldSchema).IsAssignableFrom(typeof(T)))
                return new WorldDbContext($"server={_server};user={_user};password={_password};database={_worldSchemaName}");
            else if (typeof(IHotfixesSchema).IsAssignableFrom(typeof(T)))
                return new HotfixesDbContext($"server={_server};user={_user};password={_password};database={_hotfixesSchemaName}");
            else if (typeof(ICharactersSchema).IsAssignableFrom(typeof(T)))
                return new CharactersDbContext($"server={_server};user={_user};password={_password};database={_charactersSchemaName}");

            throw new Exception($"Incorrect implementation of {nameof(T)}. Make sure {nameof(T)} derives from one of the Schema classes.");
        }

        public async Task<bool> TableExists<T>()
            where T : class, ITrinityCore
        {
            var entityType = GetContext<T>().Model.FindEntityType(typeof(T));
            var tableName = entityType.GetTableName();

            using (var command = GetContext<T>().Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = $"SHOW TABLES LIKE @tableName;";
                command.CommandType = CommandType.Text;
                var parameter = command.CreateParameter();
                parameter.ParameterName = "tableName";
                parameter.Value = tableName;
                command.Parameters.Add(parameter);

                GetContext<T>().Database.OpenConnection();

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
            var entityType = GetContext<T>().Model.FindEntityType(typeof(T));

            using (var command = GetContext<T>().Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = $"{createQuery}";
                command.CommandType = CommandType.Text;

                GetContext<T>().Database.OpenConnection();
                await command.ExecuteNonQueryAsync();
            }
            return true;
        }
    }
}
