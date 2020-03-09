using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;

namespace Manager.Core.BusinessModel
{
    public class FluidCareerPlan : BaseEntity
    {
        public string What { get; set; }
        public DateTime? Date { get; set; }
        public string Observation { get; set; }
        public List<ViewListSkill> Skills { get; set; }
        public EnumStatusFluidCareerPlan StatusFluidCareerPlan { get; set; }
        public ViewCrudFluidCareerPlan GetViewCrud()
        {
            return new ViewCrudFluidCareerPlan()
            {
                Date = Date,
                Observation = Observation,
                What = What,
                Skills = Skills,
                StatusFluidCareerPlan = StatusFluidCareerPlan,
                _id = _id
            };
        }
    }
}
