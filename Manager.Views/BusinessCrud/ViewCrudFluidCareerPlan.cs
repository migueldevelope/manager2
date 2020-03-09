using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
    public class ViewCrudFluidCareerPlan : _ViewCrud
    {
        public string What { get; set; }
        public DateTime? Date { get; set; }
        public string Observation { get; set; }
        public List<ViewListSkill> Skills { get; set; }
        public EnumStatusFluidCareerPlan StatusFluidCareerPlan { get; set; }
    }
}
