using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using System;

namespace Manager.Core.Business
{
  public class NewsletterRead : BaseEntity
  {
    public string _idNewsletter { get; set; }
    public string _idUser { get; set; }
    public DateTime? ReadDate { get; set; }
    public bool DontShow { get; set; }

    public ViewCrudNewsletterRead GetViewCrud()
    {
      return new ViewCrudNewsletterRead()
      {
        _id = _id,
        DontShow = DontShow,
        ReadDate = ReadDate,
        _idNewsletter = _idNewsletter,
        _idUser = _idUser
      };
    }

    public ViewListNewsletterRead GetViewList()
    {
      return new ViewListNewsletterRead()
      {
        _id = _id
      };
    }

  }
}
