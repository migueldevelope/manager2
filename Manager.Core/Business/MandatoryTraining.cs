using Manager.Core.Base;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados - treinamentos obrigatórios
  /// </summary>
  public class MandatoryTraining : BaseEntity
  {
    public List<OccupationMandatory> Occupations { get; set; }
    public List<CompanyMandatory> Companys { get; set; }
    public List<PersonMandatory> Persons { get; set; }
    public Course Course { get; set; }
  }
}
