using Manager.Core.Base;
using Manager.Core.Enumns;

namespace Manager.Core.Business
{
  public class DictionarySphere : BaseEntity
  {
    public string Name { get; set; }
    public EnumTypeSphere Type { get; set; }
    public DictionarySphere Template { get; set; }
  }
}
