using Manager.Views.BusinessCrud;
using System.Collections.Generic;

namespace Manager.Views.BusinessList
{
    public class ViewListMeritocracyActivitie
    {
        public ViewListActivitie Activitie { get; set; }
        public byte Mark { get; set; }
        public List<ViewCrudComment> Comments { get; set; }
        public List<string> Praises { get; set; }
        public List<ViewCrudPlan> Plans { get; set; }
    }
}
