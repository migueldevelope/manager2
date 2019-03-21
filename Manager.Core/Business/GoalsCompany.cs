using Manager.Core.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados - objetivos da empresa
  /// </summary>
  public class GoalsCompany: BaseEntity
  {
    public GoalsPeriod GoalsPeriod { get; set; }
    public Company Company { get; set; }
    public List<GoalsCompanyList> GoalsCompanyList { get; set; }
  }
}
