using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Tools.Dev.Business
{
    public class ListfileConverterTool
    {
        public void SplitListfile(string listfilePath, string listfileName)
        {
            string outputFolder = "output";
            Directory.CreateDirectory(Path.Combine(listfilePath, outputFolder));
            var writers = new Dictionary<string, StreamWriter>();
            using (StreamReader reader = new StreamReader(Path.Combine(listfilePath, listfileName)))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var file = line.Split(";")[1];
                    var splitFileName = file.Split("/")[0];

                    if(!writers.ContainsKey(splitFileName))
                        writers[splitFileName] = new StreamWriter(Path.Combine(listfilePath, outputFolder, $"{splitFileName}.csv"), true);

                    writers[splitFileName].WriteLine(line);
                }
            }

            foreach (var writer in writers)
                writer.Value.Flush();
        }
    }
}
