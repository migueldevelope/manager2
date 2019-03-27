using Manager.Views.Enumns;

namespace Manager.Core.BusinessModel
{
  /// <summary>
  /// coleção para steps de um grade da tabela salarial
  /// </summary>
  public class ListSteps
  {
    public EnumSteps Step { get; set; }
    public decimal Salary { get; set; }
  }
}
