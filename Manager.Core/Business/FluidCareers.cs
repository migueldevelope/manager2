using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class FluidCareers: BaseEntity
  {
    public ViewListPersonInfo Person { get; set; }
    public List<ViewFluidCareers> FluidCareersView { get; set; }
    public DateTime? Date { get; set; }
  }
}
