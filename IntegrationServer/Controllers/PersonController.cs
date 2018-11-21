using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.Business;
using Manager.Core.Business.Integration;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Services.Commons;
using Manager.Views.Enumns;
using Manager.Views.Integration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace IntegrationServer.InfraController
{
  [Produces("application/json")]
  [Route("person")]
  public class PersonController : Controller
  {
    private readonly IServiceIntegration service;
    private readonly IServicePerson servicePerson;

    public PersonController(IServiceIntegration _service, IServicePerson _servicePerson, IServiceCompany _serviceCompany,
      IHttpContextAccessor contextAccessor)
    {
      try
      {
        service = _service;
        servicePerson = _servicePerson;
        service.SetUser(contextAccessor);
        servicePerson.SetUser(contextAccessor);
      }
      catch (Exception)
      {
        throw;
      }
    }

    #region Person
    [Authorize]
    [HttpPost]
    public IActionResult GetPersonByKey([FromBody]ViewIntegrationMapPersonV1 view)
    {
      try
      {
        view.IdPerson = string.Empty;
        view.IdContract = string.Empty;
        view.Message = string.Empty;
        IntegrationCompany company = service.GetIntegrationCompany(view.CompanyKey, view.CompanyName);
        if (string.IsNullOrEmpty(company.IdCompany))
        {
          view.Message = "Falta integração da empresa!";
          throw new Exception(view.Message);
        }
        Person person = service.GetPersonByKey(company.IdCompany, view.Document, view.Registration);
        if (person == null)
        {
          view.Message = "Pessoa não encontrada!";
          throw new Exception(view.Message);
        }
        view.IdPerson = person._id;
        view.Person = new ViewIntegrationPerson()
        {
          Name = person.Name,
          Document = person.Document,
          Mail = person.Mail,
          Phone = person.Phone,
          IdCompany = person.Company?._id,
          IdOccupation = person.Occupation?._id,
          Registration = person.Registration,
          IdManager = person.Manager?._id,
          DateBirth = person.DateBirth,
          DateAdm = person.DateAdm,
          DocumentManager = person.DocumentManager,
          IdSchooling = person.Schooling?._id,
          PhoneFixed = person.PhoneFixed,
          DocumentID = person.DocumentID,
          DocumentCTPF = person.DocumentCTPF,
          Sex = (int)person.Sex,
          HolidayReturn = person.HolidayReturn,
          MotiveAside = person.MotiveAside,
          DateLastOccupation = person.DateLastOccupation,
          Salary = person.Salary,
          DateLastReadjust = person.DateLastReadjust,
          DateResignation = person.DateResignation
        };
        return Ok(view);
      }
      catch (Exception)
      {
        return BadRequest(view);
      }
    }
    [Authorize]
    [HttpPost]
    [Route("update")]
    public IActionResult PutPerson([FromBody]ViewIntegrationColaborador view)
    {
      try
      {
        IntegrationCompany company = service.GetIntegrationCompany(view.Colaborador.ChaveEmpresa, view.Colaborador.NomeEmpresa);

        IntegrationSchooling schooling = service.GetIntegrationSchooling(view.Colaborador.ChaveGrauInstrucao, view.Colaborador.NomeGrauInstrucao);

        IntegrationEstablishment establishment = null;
        if (!string.IsNullOrEmpty(company.IdCompany) && !string.IsNullOrEmpty(view.Colaborador.NomeEstabelecimento))
          establishment = service.GetIntegrationEstablishment(view.Colaborador.ChaveEstabelecimento, view.Colaborador.NomeEstabelecimento, company.IdCompany);

        IntegrationOccupation occupation = null;
        if (!string.IsNullOrEmpty(company.IdCompany))
          occupation = service.GetIntegrationOccupation(view.Colaborador.ChaveCargo, view.Colaborador.NomeCargo, company.IdCompany);

        IntegrationCompany companyManager = null;
        if (!string.IsNullOrEmpty(view.Colaborador.NomeEmpresaGestor))
          companyManager = service.GetIntegrationCompany(view.Colaborador.ChaveEmpresaGestor, view.Colaborador.NomeEmpresaGestor);

        Person personManager = null;
        if (companyManager != null && !string.IsNullOrEmpty(view.Colaborador.DocumentoGestor))
          personManager = service.GetPersonByKey(companyManager.IdCompany, view.Colaborador.DocumentoGestor, view.Colaborador.MatriculaGestor);

        if (string.IsNullOrEmpty(company.IdCompany))
        {
          view.Message = "Falta integração da empresa!";
          return BadRequest(view);
        }
        if (string.IsNullOrEmpty(schooling.IdSchooling))
        {
          view.Message = "Falta integração do grau de instrução!";
          return BadRequest(view);
        }
        if (string.IsNullOrEmpty(occupation.IdOccupation))
        {
          view.Message = "Falta integração do cargo!";
          return BadRequest(view);
        }
        if (!string.IsNullOrEmpty(view.Colaborador.NomeEstabelecimento) && string.IsNullOrEmpty(establishment.IdEstablishment))
        {
          view.Message = "Falta integração do estabelecimento!";
          return BadRequest(view);
        }
        if (string.IsNullOrEmpty(view.Colaborador.Documento))
        {
          view.Message = "Documento deve ser informado!";
          return BadRequest(view);
        }
        if (string.IsNullOrEmpty(view.Colaborador.Nome))
        {
          view.Message = "Nome deve ser informado!";
          return BadRequest(view);
        }
        if (view.Colaborador.DataAdmissao == null)
        {
          view.Message = "Data de admissão não informada!";
          return BadRequest(view);
        }
        if (string.IsNullOrEmpty(view.Colaborador.NomeCargo))
        {
          view.Message = "Descrição do cargo deve ser informado!";
          return BadRequest(view);
        }
        if (string.IsNullOrEmpty(view.Colaborador.NomeGrauInstrucao))
        {
          view.Message = "Descrição do grau de instrução deve ser informado!";
          return BadRequest(view);
        }

        Person person = null;
        switch ((EnumColaboradorAcao)view.Acao)
        {
          case EnumColaboradorAcao.Insert:
            person = new Person
            {
              Name = Capitalization(view.Colaborador.Nome),
              Document = view.Colaborador.Documento,
              Mail = view.Colaborador.Email,
              Phone = view.Colaborador.Celular,
              TypeUser = (EnumTypeUser)view.Colaborador.TypeUser,
              Company = service.GetCompany(company.IdCompany),
              Establishment = string.IsNullOrEmpty(view.Colaborador.NomeEstabelecimento) ? null : service.GetEstablishment(establishment.IdEstablishment),
              Occupation = service.GetOccupation(occupation.IdOccupation),
              Registration = view.Colaborador.Matricula,
              DateBirth = view.Colaborador.DataNascimento,
              DateAdm = view.Colaborador.DataAdmissao,
              Schooling = service.GetSchooling(schooling.IdSchooling),
              PhoneFixed = view.Colaborador.Telefone,
              DocumentID = view.Colaborador.Identidade,
              DocumentCTPF = view.Colaborador.CarteiraProfissional,
              Sex = view.Colaborador.Sexo.StartsWith("M") ? EnumSex.Male : view.Colaborador.Sexo.StartsWith("F") ? EnumSex.Female : EnumSex.Others,
              HolidayReturn = view.Colaborador.DataRetornoFerias,
              MotiveAside = view.Colaborador.MotivoAfastamento,
              DateLastOccupation = view.Colaborador.DataUltimaTrocaCargo,
              Salary = view.Colaborador.SalarioNominal,
              DateLastReadjust = view.Colaborador.DataUltimoReajuste,
              DateResignation = view.Colaborador.DataDemissao,
              TypeJourney = DateTime.Now.Subtract(((DateTime)view.Colaborador.DataAdmissao)).Days > 90 ? EnumTypeJourney.Monitoring : EnumTypeJourney.OnBoarding
            };
            switch (view.Colaborador.Situacao.ToLower())
            {
              case "férias":
                person.StatusUser = EnumStatusUser.Vacation;
                break;
              case "afastado":
                person.StatusUser = EnumStatusUser.Away;
                break;
              case "demitido":
                person.StatusUser = EnumStatusUser.Disabled;
                break;
              default:
                person.StatusUser = EnumStatusUser.Enabled;
                break;
            }
            if (personManager != null)
            {
              person.Manager = personManager;
              person.DocumentManager = personManager?.Document;
            }
            person = servicePerson.NewPersonView(person);
            view.Message = string.Empty;
            view.IdPerson = person._id;
            view.Situacao = (int)EnumColaboradorSituacao.Atualized;
            break;
          case EnumColaboradorAcao.Update:
            person = servicePerson.GetPerson(view.IdPerson);

            person.Document = view.Colaborador.Documento;
            person.Registration = view.Colaborador.Matricula;

            if (view.CamposAlterados.Contains("Nome"))
              person.Name = Capitalization(view.Colaborador.Nome);

            if (view.CamposAlterados.Contains("Email"))
              person.Mail = view.Colaborador.Email;

            if (view.CamposAlterados.Contains("Celular"))
              person.Phone = view.Colaborador.Celular;

            if (view.CamposAlterados.Contains("ChaveEmpresa"))
              person.Company = service.GetCompany(company.IdCompany);

            if (view.CamposAlterados.Contains("ChaveEstabelecimento"))
              person.Establishment = string.IsNullOrEmpty(establishment.IdEstablishment) ? null : service.GetEstablishment(establishment.IdEstablishment);

            if (view.CamposAlterados.Contains("ChaveCargo"))
              person.Occupation = service.GetOccupation(occupation.IdOccupation);

            if (view.CamposAlterados.Contains("DataNascimento"))
              person.DateBirth = view.Colaborador.DataNascimento;

            if (view.CamposAlterados.Contains("DataAdmissao"))
              person.DateAdm = view.Colaborador.DataAdmissao;

            if (view.CamposAlterados.Contains("ChaveGrauInstrucao"))
              person.Schooling = service.GetSchooling(schooling.IdSchooling);

            if (view.CamposAlterados.Contains("Telefone"))
              person.PhoneFixed = view.Colaborador.Telefone;

            if (view.CamposAlterados.Contains("Identidade"))
              person.DocumentID = view.Colaborador.Identidade;

            if (view.CamposAlterados.Contains("CarteiraProfissional"))
              person.DocumentCTPF = view.Colaborador.CarteiraProfissional;

            if (view.CamposAlterados.Contains("Sexo"))
              person.Sex = view.Colaborador.Sexo.StartsWith("M") ? EnumSex.Male : view.Colaborador.Sexo.StartsWith("F") ? EnumSex.Female : EnumSex.Others;

            if (view.CamposAlterados.Contains("DataRetornoFerias"))
              person.HolidayReturn = view.Colaborador.DataRetornoFerias;

            if (view.CamposAlterados.Contains("MotivoAfastramento"))
              person.MotiveAside = view.Colaborador.MotivoAfastamento;

            if (view.CamposAlterados.Contains("DataUltimaTrocaCargo"))
              person.DateLastOccupation = view.Colaborador.DataUltimaTrocaCargo;

            if (view.CamposAlterados.Contains("SalarioNominal"))
              person.Salary = view.Colaborador.SalarioNominal;

            if (view.CamposAlterados.Contains("DataUltimoReajuste"))
              person.DateLastReadjust = view.Colaborador.DataUltimoReajuste;

            if (view.CamposAlterados.Contains("DataDemissao"))
              person.DateResignation = view.Colaborador.DataDemissao;

            if (view.CamposAlterados.Contains("TypeUser"))
              person.TypeUser = (EnumTypeUser)view.Colaborador.TypeUser;

            if (view.CamposAlterados.Contains("Situacao"))
            {
              switch (view.Colaborador.Situacao.ToLower())
              {
                case "férias":
                  person.StatusUser = EnumStatusUser.Vacation;
                  break;
                case "afastado":
                  person.StatusUser = EnumStatusUser.Away;
                  break;
                case "demitido":
                  person.StatusUser = EnumStatusUser.Disabled;
                  break;
                default:
                  person.StatusUser = EnumStatusUser.Enabled;
                  break;
              }
            }

            if (view.CamposAlterados.Contains("ChaveGestor"))
              if (personManager != null)
              {
                person.Manager = personManager;
                person.DocumentManager = personManager?.Document;
              }

            person = servicePerson.UpdatePersonView(person);
            view.IdPerson = person._id;
            view.Situacao = (int)EnumColaboradorSituacao.Atualized;
            break;
          case EnumColaboradorAcao.Passed:
            view.Message = "Chamada inválida!!!";
            break;
          default:
            view.Message = "Chamada inválida!!!";
            break;
        }
        return Ok(view);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.ToString());
      }
    }

    private string Capitalization(string nome)
    {
      try
      {
        TextInfo myTI = new CultureInfo("pt-BR", false).TextInfo;
        nome = myTI.ToTitleCase(nome).Replace(" De ", " de ").Replace(" Da ", " da ").Replace(" Dos ", " dos ").Replace(" Do ", " do ");
        return nome;
      }
      catch (Exception)
      {
        throw;
      }
    }
    #endregion
  }
}
