using Manager.Views.Enumns;
using System;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudMaturityRegister: _ViewCrud
  {
    public string _idPerson { get; set; }
    public EnumTypeMaturity TypeMaturity { get; set; }
    public string _idRegister { get; set; }
    public DateTime? Date { get; set; }
    public int Evaluation { get; set; }
    public string _idAccount { get; set; }
  }
}
