using Manager.Views.BusinessCrud;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessList
{
    public class ViewGetPlan : _ViewListBase
    {
        public string Description { get; set; }
        public DateTime? Deadline { get; set; }
        public List<ViewListSkill> Skills { get; set; }
        public string UserInclude { get; set; }
        public DateTime? DateInclude { get; set; }
        public EnumTypePlan TypePlan { get; set; }
        public string _idPerson { get; set; }
        public string NamePerson { get; set; }
        public EnumSourcePlan SourcePlan { get; set; }
        public string IdMonitoring { get; set; }
        public int Evaluation { get; set; }
        public EnumStatusPlan StatusPlan { get; set; }
        public EnumTypeAction TypeAction { get; set; }
        public long Bomb { get; set; }
        public EnumStatusPlanApproved StatusPlanApproved { get; set; }
        public string TextEnd { get; set; }
        public string TextEndManager { get; set; }
        public DateTime? DateEnd { get; set; }
        public EnumStatus Status { get; set; }
        public List<ViewCrudAttachmentField> Attachments { get; set; }
        public ViewCrudPlan PlanNew { get; set; }
        public EnumNewAction NewAction { get; set; }
        public EnumOriginPlan OriginPlan { get; set; }
    }
}
