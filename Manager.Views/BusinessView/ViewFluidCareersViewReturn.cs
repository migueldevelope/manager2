using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System.Collections.Generic;

namespace Manager.Views.BusinessView
{
    public class ViewFluidCareersViewReturn : _ViewCrud
    {
        public string Sphere { get; set; }
        public string Group { get; set; }
        public string Occupation { get; set; }
        public decimal Accuracy { get; set; }
        public EnumOccupationColor Color { get; set; }
        public byte Order { get; set; }
        public List<ViewListSkill> SkillsOccupation { get; set; }
    }

}
