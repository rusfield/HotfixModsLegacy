using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.DashboardModels
{
    public interface IRazorDashboardModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public Action EditAction { get; set; }
        public Action DeleteAction { get; set; }
    }
}
