using Manager.Core.Base;
using Manager.Core.BusinessModel;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business
{
    public class OffBoarding : BaseEntity
    {
        public ViewListPersonInfo Person { get; set; }
        public string CompanyName { get; set; }
        public ViewCrudOffBoardingHistory History {get;set;}
        public DateTime? DateBeginStep1 { get; set; }
        public DateTime? DateBeginStep2 { get; set; }
        public DateTime? DateEndStep1 { get; set; }
        public DateTime? DateEndStep2 { get; set; }
        public ViewCrudFormOffBoarding Step1 { get; set; }
        public ViewCrudFormOffBoarding Step2 { get; set; }

        public ViewCrudOffBoarding GetViewCrud()
        {
            return new ViewCrudOffBoarding()
            {
                _id = _id,
                Step1 = Step1,
                Step2 = Step2,
                Person = Person,
                DateEndStep1 = DateEndStep1,
                DateEndStep2 = DateEndStep2,
                DateBeginStep1 = DateBeginStep1,
                DateBeginStep2 = DateBeginStep2,
                History = History,
                CompanyName = CompanyName
            };
        }


    }
}
