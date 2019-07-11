using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados - treinamentos obrigatórios
  /// </summary>
  public class MandatoryTraining : BaseEntity
  {
    public List<ViewCrudOccupationMandatory> Occupations { get; set; }
    public List<ViewCrudCompanyMandatory> Companys { get; set; }
    public List<ViewCrudPersonMandatory> Persons { get; set; }
    public ViewListCourse Course { get; set; }
  }
}
