using IntegrationService.Api;
using IntegrationService.Core;
using IntegrationService.Data;
using IntegrationService.Views;
using IntegrationService.Views.Person;
using System;

namespace IntegrationService.Service
{
  public class ValidMapCollaborator
  {
    public MapPerson Map { get; private set; }
    public ValidMapCollaborator(ViewPersonLogin person, string document, ViewIntegrationMapOfV1 company, string registration, string name)
    {
      try
      {
        ViewIntegrationMapPersonV1 view = new ViewIntegrationMapPersonV1()
        {
          Document = document,
          IdCompany = company.Id,
          Registration = registration,
          Name = name
        };
        PersonIntegration personIntegration = new PersonIntegration(person);
        personIntegration.GetCollaboratorByKey(view);
        Map = new MapPerson()
        {
          Documento = view.Document,
          Empresa = company.Code,
          Matricula = view.Registration,
          Person = view.Person,
          Contract = view.Contract,
          Mensagem = string.Empty
        };
      }
      catch (Exception ex)
      {
        Map = new MapPerson()
        {
          Documento = document,
          Empresa = company.Code,
          Matricula = registration,
          Person = null,
          Contract = null,
          Mensagem = ex.Message
        };
      }
    }
  }
}
