using HotfixMods.Providers.MySqlConnector.Client;
using HotfixMods.Providers.WowDev.Client;

namespace HotfixMods.Tools.Initializer.Business
{
    public class Db2ImportTool
    {
        public async Task Db2FileToDb2MySql(string db2Build, string db2FilePath, string db2Name, string mySqlSchemaName, string mySqlTableName, string mySqlServer, string mySqlPort, string mySqlUsername, string mySqlPassword)
        {
            /*
            var mySqlClient = new MySqlClient(mySqlServer, mySqlPort, mySqlUsername, mySqlPassword);
            var db2Client = new Db2Client(db2Build);

            var dbDefinition = await db2Client.GetDefinitionAsync(db2Name);
            await mySqlClient.CreateTableIfNotExistsAsync(mySqlSchemaName, mySqlTableName, dbDefinition);

            var data = await db2Client.GetAsync(db2FilePath, db2Name, dbDefinition);
            await mySqlClient.AddOrUpdateAsync(mySqlSchemaName, mySqlTableName, data.ToArray());
            */
        }
    }
}
