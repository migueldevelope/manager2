using Manager.Core.Base;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados - textos
  /// </summary>
  public class TextDefault : BaseEntity
  {
    public string Name { get; set; }
    public string Content { get; set; }
    public EnumTypeText TypeText { get; set; }
    public Company Company { get; set; }
    public TextDefault Template { get; set; }
    public ViewListTextDefault GetViewList()
    {
      return new ViewListTextDefault()
      {
        _id = _id,
        Name = Name,
        Content = Content
      };
    }
  }
}
