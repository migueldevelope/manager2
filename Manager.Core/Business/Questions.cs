using Manager.Core.Base;
using Manager.Core.Enumns;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados - perguntas
  /// </summary>
  public class Questions : BaseEntity
  {
    public string Name { get; set; }
    public string Content { get; set; }
    public Company Company { get; set; }
    public EnumTypeQuestion TypeQuestion { get; set; }
    public long Order { get; set; }
    public Questions Template { get; set; }
    public EnumTypeRotine TypeRotine { get; set; }
  }
}
