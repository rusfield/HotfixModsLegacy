using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Services
{
    public partial class Service
    {
        protected void DefaultProgressCallback(string title, string subtitle, int progress)
        {
            Console.WriteLine($"{progress} %: {title} => {subtitle}");
        }

        protected string QueryBuilder()
        {
            return $"WHERE VerifiedBuild = {VerifiedBuild};";
        }

        protected string QueryBuilder(int id, string idParamName = "ID")
        {
            return $"WHERE {idParamName} = {id} AND VerifiedBuild = {VerifiedBuild};";
        }


    }
}
