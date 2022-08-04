using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.DashboardModels
{
    public class ItemDashboard : IDashboardModel
    {
        public ItemDashboard(int id, string name, string avatarUrl, string page, Action deleteAction)
        {
            Id = id;
            Name = name;
            AvatarUrl = avatarUrl;
            Page = page;
            DeleteAction = deleteAction;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public string Page { get; set; }
        public Action DeleteAction { get; set; }
    }
}
