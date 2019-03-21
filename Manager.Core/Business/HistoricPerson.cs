using Manager.Core.Base;
using Manager.Core.Enumns;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  /// <summary>
  /// OBjeto persiste no banco de dados
  /// </summary>
  public class HistoricPerson : BaseEntity
  {
    public string _idPerson { get; set; }
    public string Name { get; set; }
    public string Old { get; set; }
    public string New { get; set; }
    public DateTime? Date { get; set; }
    public EnumTypeHistoric TypeHistoric { get; set; }

  }
}
