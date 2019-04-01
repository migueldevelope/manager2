using Manager.Views.BusinessList;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudMandatoryTraining: _ViewCrud
  {
    public List<ViewCrudOccupationMandatory> Occupations { get; set; }
    public List<ViewCrudCompanyMandatory> Companys { get; set; }
    public List<ViewCrudPersonMandatory> Persons { get; set; }
    public ViewListCourse Course { get; set; }
  }
}
