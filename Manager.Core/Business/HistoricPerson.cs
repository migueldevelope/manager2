using Manager.Core.Base;
using Manager.Core.Enumns;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class HistoricPerson : BaseEntity
  {
    public string _idPerson { get; set; }
    public string Name { get; set; }
    public DateTime? Date { get; set; }
    public EnumTypeHistoric TypeHistoric { get; set; }
    public Occupation OccuationOld { get; set; }
    public Occupation OccuationNew { get; set; }
    public EnumStatusUser StatusUser { get; set; }


  }
}
