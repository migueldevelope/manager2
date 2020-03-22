using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
    public class ViewCrudMonitoringMobile : _ViewCrud
    {
        public ViewListPersonInfo Person { get; set; }
        public string CommentsEnd { get; set; }
        public string CommentsManager { get; set; }
        public string CommentsPerson { get; set; }
        public List<ViewListItensMobile> Items { get; set; }
        public EnumStatusMonitoring StatusMonitoring { get; set; }
    }
}
