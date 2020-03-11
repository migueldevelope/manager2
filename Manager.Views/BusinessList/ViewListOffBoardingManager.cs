using Manager.Views.Enumns;

namespace Manager.Views.BusinessList
{
    public class ViewListOffBoardingManager:_ViewListBase
    {
        public string _idPerson { get; set; }
        public string OccupationName { get; set; }
        public EnumStatusFormOffBoarding StatusOffBoarding { get; set; }
        public string Photo { get; set; }
    }
}
