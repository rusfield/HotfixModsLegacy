using HotfixMods.Infrastructure.DashboardModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Razor.DashboardModels
{
    public class RazorCreatureDashboard : CreatureDashboard, IRazorDashboardModel
    {
        public RazorCreatureDashboard(int id, string name, string avatarUrl, Action editAction, Action deleteAction)
        {
            Id = id;
            Name = name;
            AvatarUrl = avatarUrl;
            EditAction = editAction;
            DeleteAction = deleteAction;
        }

        public Action EditAction { get; set; }
        public Action DeleteAction { get; set; }
    }
}
