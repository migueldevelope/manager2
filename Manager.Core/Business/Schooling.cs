using Manager.Core.Base;
using Manager.Views.Enumns;

namespace Manager.Core.Business
{
    public class Schooling : BaseEntity
  {
    public string Name { get; set; }
    public string Complement { get; set; }
    public EnumTypeSchooling Type { get; set; }
    public Schooling Template { get; set; }
    public long Order { get; set; }
  }
}
