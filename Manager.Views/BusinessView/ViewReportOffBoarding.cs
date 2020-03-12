using System;

namespace Manager.Views.BusinessView
{
    public class ViewReportOffBoarding
    {
        public string PersonName { get; set; }
        public string Occupation { get; set; }
        public string Manager { get; set; }
        public string Schooling { get; set; }
        public DateTime? DateAdm { get; set; }
        public string CompanyName { get; set; }
        public string Question { get; set; }
        public string ContentQuestion { get; set; }
        public byte Response { get; set; }
        public int CompanyTime { get; set; }
        public int OccupationTime { get; set; }
        public string CurrentSchooling { get; set; }
        public string OccupationSchooling { get; set; }
        public decimal ActivitieExcellence { get; set; }
        public string Activitie { get; set; }
        public byte MarkActivitie { get; set; }
        public long QtdMonitoring { get; set; }
        public long QtdPraise { get; set; }
        public long QtdPlan { get; set; }
        public long QtdCertification { get; set; }
        public long QtdRecommendation { get; set; }
    }
}
