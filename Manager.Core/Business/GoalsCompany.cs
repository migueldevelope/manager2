using Manager.Core.Base;
using Manager.Core.BusinessModel;
using Manager.Views.BusinessList;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados - objetivos da empresa
  /// </summary>
  public class GoalsCompany: BaseEntity
  {
    public ViewListGoalPeriod GoalsPeriod { get; set; }
    public ViewListCompany Company { get; set; }
    public GoalsItem GoalsCompanyList { get; set; }
  }
}
