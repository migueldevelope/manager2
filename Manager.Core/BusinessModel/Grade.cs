using Manager.Core.Base;
using Manager.Views.Enumns;
using System.Collections.Generic;

namespace Manager.Core.BusinessModel
{
  /// <summary>
  /// Objeto coleção da tabela salarial
  /// </summary>
  public class Grade
  {
    public string _id { get; set; }
    public string Name { get; set; }
    public EnumSteps StepMedium { get; set; }
    public List<ListSteps> ListSteps { get; set; }
    public int Order { get; set; }
    public int Workload { get; set; }
  }
}
