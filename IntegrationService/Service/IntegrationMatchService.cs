using Manager.Views.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegrationService.Api;

namespace IntegrationService.Service
{
  public class IntegrationMatchService
  {
    private readonly ViewPersonLogin Person;
    private readonly MatchIntegration matchIntegration;

    #region Construtores
    public IntegrationMatchService(ViewPersonLogin person)
    {
      try
      {
        Person = person;
        matchIntegration = new MatchIntegration(Person);
      }
      catch (Exception)
      {
        throw;
      }
    }
    #endregion

    #region Company
    public List<ViewIntegrationCompany> GetIntegrationCompany()
    {
      try
      {
        return null;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public void CompanyMacth(string _id, string _idMatch, string Name)
    {
      try
      {

      }
      catch (Exception)
      {
        throw;
      }
    }
    #endregion

  }
}
