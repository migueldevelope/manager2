using Manager.Core.Base;
using Manager.Views.Enumns;
using System;

namespace Manager.Core.Business
{
  public class MaturityRegister: BaseEntity
  {
    public string _idPerson { get; set; }
    public EnumTypeMaturity TypeMaturity { get; set; }
    public DateTime? Date { get; set; }
    public string _idRegister { get; set; }
  }
}
