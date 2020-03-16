using Manager.Views.Enumns;
using System;

namespace Manager.Views.BusinessView
{
    public class ViewReportsMonitoring
    {
        public string Name { get; set; }
        public DateTime? DateAdm { get; set; }
        public string Schooling { get; set; }
        public string Manager { get; set; }
        public string Occupation { get; set; }
        public EnumTypeJourney TypeJourney { get; set; }

        public string CommentsPerson { get; set; }
        public string CommentsManager { get; set; }
        public string CommentsEnd { get; set; }

        public string Concept { get; set; }
        public EnumTypeSkill TypeSkill { get; set; }
        public long Order { get; set; }
        public string Complement { get; set; }
        public EnumTypeSchooling Type { get; set; }
        public EnumTypeItem TypeItem { get; set; }
        public string NameItem { get; set; }
        public string _idItem { get; set; }
        public string Comments { get; set; }
        public EnumUserComment UserComment { get; set; }
    }
}
