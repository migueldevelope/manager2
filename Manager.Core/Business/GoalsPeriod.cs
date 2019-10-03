using Manager.Core.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados - ciclo de objetivos
  /// </summary>
  public class GoalsPeriod: BaseEntity
  {
    public string Name { get; set; }
    public DateTime? DateBegin { get; set; }
    public DateTime? DateEnd { get; set; }
    public bool Review { get; set; }
    public bool ChangeCheck { get; set; }
    public decimal PercentCompany { get; set; }
    public decimal PercentTeam { get; set; }
    public decimal PercentPerson { get; set; }
  }
}
