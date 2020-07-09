using Manager.Core.BusinessV2;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Views.BusinessCrud;
using Microsoft.AspNetCore.Http;
using NPOI.HSSF.Record;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Services.V2
{
  public class ServiceUserV2 : RepositoryPublic<UserV2>//, IServiceUser
  {
    #region Constructor
    public ServiceUserV2(DataContext context, DataContext contextLog) : base(context)
    {
      try
      {
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public override void SetUser(IHttpContextAccessor contextAccessor)
    {
      base.SetUser(contextAccessor);
    }
    #endregion

    #region CRUD
    public ViewCrudUserV2 Insert(ViewCrudUserV2 view)
    {
      try
      {
        UserV2 userV2 = new UserV2(view);
        userV2 = base.InsertAsync(userV2).Result;
        return userV2.ToCrud();
      }
      catch (Exception)
      {
        throw;
      }
    }
    #endregion
  }
}
