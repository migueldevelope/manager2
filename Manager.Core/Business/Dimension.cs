using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;

namespace Manager.Core.Business
{
  public class Dimension:BaseEntity
  {
    public string Name { get; set; }

    public ViewCrudDimension GetViewCrud()
    {
      return new ViewCrudDimension()
      {
        _id = _id,
        Name = Name
      };
    }

    public ViewListDimension GetViewList()
    {
      return new ViewListDimension()
      {
        _id = _id,
        Name = Name
      };
    }

  }
}
