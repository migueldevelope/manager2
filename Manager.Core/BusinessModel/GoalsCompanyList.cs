using Manager.Core.Base;
using Manager.Core.Business;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Core.BusinessModel
{
  /// <summary>
  /// Coleção para objetivos da empresa
  /// </summary>
  public class GoalsCompanyList: BaseEntity
  {
    public Goals Goals { get; set; }
    public string Goal { get; set; }
    public byte Weight { get; set; }
    public DateTime? Deadline { get; set; }
    public string Realized { get; set; }
    public string Result { get; set; }
    public decimal Achievement { get; set; }
  }
}
