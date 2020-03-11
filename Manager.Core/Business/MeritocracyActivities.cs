using Manager.Core.BusinessModel;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business
{
    public class MeritocracyActivities
    {
        public ViewListActivitie Activities { get; set; }
        public byte Mark { get; set; }

        public ViewCrudMeritocracyActivities GetViewCrud()
        {
            return new ViewCrudMeritocracyActivities()
            {
                Activities = Activities,
                Mark = Mark
            };
        }
    }
}
