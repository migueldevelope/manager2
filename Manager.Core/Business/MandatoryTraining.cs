using Manager.Core.Base;
using Manager.Core.Enumns;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class MandatoryTraining : BaseEntity
  {
    public List<OccupationMandatory> Occupations { get; set; }
    public List<CompanyMandatory> Companys { get; set; }
    public List<PersonMandatory> Persons { get; set; }
    public Course Course { get; set; }
  }
}
