using Manager.Views.BusinessList;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessCrud
{
    public class ViewCrudOffBoarding:_ViewCrud
    {
        public ViewListPersonInfo Person { get; set; }
        public string CompanyName { get; set; }
        public ViewCrudOffBoardingHistory History { get; set; }
        public DateTime? DateBeginStep1 { get; set; }
        public DateTime? DateBeginStep2 { get; set; }
        public DateTime? DateEndStep1 { get; set; }
        public DateTime? DateEndStep2 { get; set; }
        public ViewCrudFormOffBoarding Step1 { get; set; }
        public ViewCrudFormOffBoarding Step2 { get; set; }
    }
}
