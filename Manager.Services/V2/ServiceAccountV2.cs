using Manager.Core.Business;
using Manager.Core.BusinessV2;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Auth;
using Manager.Views.BusinessNew;
using Microsoft.AspNetCore.Http;
using System;

namespace Manager.Services.V2
{
  public class ServiceAccountV2 : RepositoryPublic<AccountV2>//, IServiceAccount
  {
    private readonly ServiceUserV2 serviceUserV2;
    private readonly ServiceAuthV2 serviceAuthV2;
    private readonly ServicePersonV2 servicePersonV2;

    #region Constructor
    public ServiceAccountV2(DataContext context, DataContext contextLog) : base(context)
    {
      try
      {
        serviceUserV2 = new ServiceUserV2(context, contextLog);
        //serviceAuthV2 = new ServiceAuthV2(context, contextLog);
        //servicePersonV2 = new ServicePersonV2(context, contextLog);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    public override void SetUser(IHttpContextAccessor contextAccessor)
    {
      base.SetUser(contextAccessor);
      serviceUserV2.SetUser(GetUser());
    }
    #endregion

    #region Methods
    public void NewAccount(ViewNewAccount view)
    {
      try
      {
        // Incluir a nova conta
        AccountV2 account = new AccountV2()
        {
          Name = view.NameAccount,
          Nickname = view.Nickname,
          InfoClient = view.InfoClient
        };
        account = InsertAsync(account).Result;
        // Incluir o person suporte
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion
  }
}
