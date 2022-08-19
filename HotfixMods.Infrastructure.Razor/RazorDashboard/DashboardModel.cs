using HotfixMods.Infrastructure.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Razor.RazorDashboard
{
    public class RazorDashboardModel : DashboardModel
    {
        public RazorDashboardModel(int id, string name, string avatarUrl, string comment, Action editAction, Action deleteAction)
        {
            Id = id;
            Name = name;
            AvatarUrl = avatarUrl;
            Comment = comment;
            EditAction=editAction;
            DeleteAction=deleteAction;
        }

        public Action EditAction { get; set; }
        public Action DeleteAction { get; set; }
    }
}
