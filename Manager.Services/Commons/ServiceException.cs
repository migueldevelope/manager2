using Manager.Core.Base;
using Manager.Data;
using System;

namespace Manager.Services.Commons
{
  public class ServiceException : Exception
  {
    //readonly ServiceEdesk edeskService;

    public ServiceException(BaseUser user, Exception innerServiceException, DataContext context)
    {
      try
      {
        /*edeskService = new ServiceEdesk(context);
        edeskService._user = user;

        var view = new ViewEdeskSolicitationNew()
        {
          Subject = "Erro automaticamente gerado pelo sistema.",
          Detailing = innerServiceException.Message + " - " + innerServiceException.InnerException.ToString()
          + " - " + innerServiceException.StackTrace.ToString()
        };

        edeskService.NewSolicitation(view, EnumTypeSolicitation.Error);
        */

        throw innerServiceException;
      }
      catch (Exception)
      {
        throw innerServiceException;
      }
    }

  }
}
