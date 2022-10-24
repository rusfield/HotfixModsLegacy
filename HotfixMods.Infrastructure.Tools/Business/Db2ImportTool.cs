using HotfixMods.Providers.MySqlConnector.Client;
using HotfixMods.Providers.WowDev.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Tools.Business
{
    public class Db2ImportTool
    {
        public async Task Db2FileToDb2MySql(string db2Name, string db2Build, string db2FilePath, string mySqlSchema, string mySqlServer, string mySqlPort, string mySqlUsername, string mySqlPassword)
        {
            var mySqlClient = new MySqlClient("localhost", "3306", "root", "root");
            var db2Client = new Db2Client(db2Build);

            var beforeStart = DateTime.UtcNow;
            var def = await db2Client.GetDefinitionAsync(db2FilePath, db2Name);
            var data = await db2Client.GetAsync("C:\\Users\\Disconnected\\Downloads", "ItemSparse", def);
            var afterGet = DateTime.UtcNow;
            Console.WriteLine($"Getting data took {(afterGet - beforeStart).TotalSeconds} seconds");

            await mySqlClient.AddOrUpdateAsync("hotfix_mods", "item_sparse", data.ToArray());
            var afterInsert = DateTime.UtcNow;


            Console.WriteLine($"Inserting data took {(afterInsert - afterGet).TotalSeconds} seconds");
            Console.WriteLine($"Total time {(afterInsert - beforeStart).TotalSeconds} seconds");



            await mySqlClient.CreateTableIfNotExistsAsync("hotfix_mods", "item_sparse", await definitionClient.GetDefinitionAsync(null, "ItemSparse"));
        }
    }
}
