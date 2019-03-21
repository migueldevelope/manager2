using Manager.Core.Base;
using Manager.Views.Enumns;

namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados, porém não foi utilizado
  /// </summary>
  public class DictionarySphere : BaseEntity
  {
    public string Name { get; set; }
    public EnumTypeSphere Type { get; set; }
    public DictionarySphere Template { get; set; }
  }
}
