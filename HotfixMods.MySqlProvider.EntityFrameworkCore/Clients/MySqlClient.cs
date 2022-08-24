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

        public async Task<T?> GetSingleAsync<T>(Expression<Func<T, bool>> predicate)
            where T : class, ITrinityCore
        {
            using (var context = GetContext<T>())
            {
                return await context.Set<T>().FirstOrDefaultAsync(predicate);
            }
        }

        public async Task<IEnumerable<T>> GetAsync<T>(Expression<Func<T, bool>> predicate)
            where T : class, ITrinityCore
        {
            using (var context = GetContext<T>())
            {
                return await context.Set<T>().Where(predicate).ToListAsync();
            }
        }

        public async Task AddOrUpdateAsync<T>(params T[] entities)
            where T : class, ITrinityCore
        {
            using (var context = GetContext<T>())
            {
                // Multiple db queries for each add/update is bad, but its limited how mch traffic this software will cause.
                foreach(var entity in entities)
                {
                    _ = context.Set<T>().Any(e => e == entity) ? context.Set<T>().Update(entity) : context.Set<T>().Add(entity);
                }
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync<T>(params T[] entities)
            where T : class, ITrinityCore
        {
            using (var context = GetContext<T>())
            {
                context.Set<T>().RemoveRange(entities);
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> WorldConnectionTestAsync()
        {
            using (var context = GetContext<IWorldSchema>())
            {
                return await context.Database.CanConnectAsync();
            }
        }
        public async Task<bool> CharactersConnectionTestAsync()
        {
            using (var context = GetContext<ICharactersSchema>())
            {
                return await context.Database.CanConnectAsync();
            }
        }
        public async Task<bool> HotfixesConnectionTestAsync()
        {
            using (var context = GetContext<IHotfixesSchema>())
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
