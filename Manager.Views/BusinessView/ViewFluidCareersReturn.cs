using Manager.Views.BusinessCrud;
using System.Collections.Generic;

namespace Manager.Views.BusinessView
{
    public class ViewFluidCareersReturn : _ViewCrud
    {
        public string _idPerson { get; set; }
        public List<ViewFluidCareersViewReturn> FluidCareersView { get; set; }
        public ViewCrudFluidCareerPlan Plan { get; set; }

    }
}
