using Manager.Core.Base;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados - tabela salarial
  /// </summary>
  public class SalaryScale : BaseEntity
  {
    public Company Company { get; set; }
    public string Name { get; set; }
    public List<Grade> Grades { get; set; }
  }
}
