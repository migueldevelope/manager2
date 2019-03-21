using Manager.Core.Base;
using Manager.Core.Enumns;
using System;
namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados - objetivos
  /// </summary>
  public class Goals : BaseEntity
  {
    public string Name { get; set; }
    public string Concept { get; set; }
    public EnumTypeGoals TypeGoals { get; set; }
  }
}
