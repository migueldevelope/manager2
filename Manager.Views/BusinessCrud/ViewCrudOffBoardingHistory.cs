using Manager.Views.BusinessList;
namespace Manager.Views.BusinessCrud
{
    public class ViewCrudOffBoardingHistory
    {
        public int CompanyTime { get; set; }
        public int OccupationTime { get; set; }
        public string CurrentSchooling { get; set; }
        public string OccupationSchooling { get; set; }
        public decimal ActivitieExcellence { get; set; }
        public long QtdMonitoring { get; set; }
        public long QtdPraise { get; set; }
        public long QtdPlan { get; set; }
        public long QtdCertification { get; set; }
        public long QtdRecommendation { get; set; }
    }
}
