using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Manager.Core.Business;
using Manager.Core.Business.Integration;
using Manager.Core.Interfaces;
using Manager.Views.Enumns;
using Manager.Views.Integration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    [HttpGet]
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
        Person person = service.GetPersonByKey(company.IdCompany, string.Empty, view.Document, view.Registration);
        if (person == null)
        {
          view.Message = "Pessoa não encontrada!";
          throw new Exception(view.Message);
        }
        view.IdPerson = person._id;
        view.Person = new ViewIntegrationPerson()
        {
          Name = person.User.Name,
          //Document = person.Document,
          Mail = person.User.Mail,
          //Phone = person.Phone,
          IdCompany = person.Company?._id,
          IdOccupation = person.Occupation?._id,
          Registration = person.Registration,
          IdManager = person.Manager?._id,
          //DateBirth = person.DateBirth,
          DateAdm = person.User.DateAdm,
          DocumentManager = person.DocumentManager,
          //IdSchooling = person.Schooling?._id,
          //PhoneFixed = person.PhoneFixed,
          //DocumentID = person.DocumentID,
          //DocumentCTPF = person.DocumentCTPF,
          //Sex = (int)person.Sex,
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
    [HttpGet]
    [Route("statusintegration")]
    public IActionResult GetStatusIntegration()
    {
      try
      {
        return Ok(service.GetStatusIntegration());
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [Authorize]
    [HttpPost]
    [Route("update")]
    public IActionResult PutPerson([FromBody]ViewIntegrationColaborador view)
    {
      try
      {
        view.Situacao = EnumColaboradorSituacao.ServerError;
        IntegrationCompany company = service.GetIntegrationCompany(view.Colaborador.ChaveEmpresa, view.Colaborador.NomeEmpresa);

        IntegrationSchooling schooling = service.GetIntegrationSchooling(view.Colaborador.ChaveGrauInstrucao, view.Colaborador.NomeGrauInstrucao);

        IntegrationEstablishment establishment = null;
        if (!company.IdCompany.Equals("000000000000000000000000"))
          establishment = service.GetIntegrationEstablishment(view.Colaborador.ChaveEstabelecimento, view.Colaborador.NomeEstabelecimento, company.IdCompany);

        IntegrationOccupation occupation = null;
        if (!company.IdCompany.Equals("000000000000000000000000"))
          occupation = service.GetIntegrationOccupation(view.Colaborador.ChaveCargo, view.Colaborador.NomeCargo, company.IdCompany);

        IntegrationCompany companyManager = null;
        if (!string.IsNullOrEmpty(view.Colaborador.NomeEmpresaGestor))
          companyManager = service.GetIntegrationCompany(view.Colaborador.ChaveEmpresaGestor, view.Colaborador.NomeEmpresaGestor);

        Person personManager = null;
        if (companyManager != null && !string.IsNullOrEmpty(view.Colaborador.DocumentoGestor))
          personManager = service.GetPersonByKey(companyManager.IdCompany, string.Empty, view.Colaborador.DocumentoGestor, view.Colaborador.MatriculaGestor);

        if (company.IdCompany.Equals("000000000000000000000000"))
        {
          view.Message = "Falta integração da empresa!";
          return BadRequest(view);
        }
        if (schooling.IdSchooling.Equals("000000000000000000000000"))
        {
          view.Message = "Falta integração do grau de instrução!";
          return BadRequest(view);
        }
        if (occupation.IdOccupation.Equals("000000000000000000000000"))
        {
          view.Message = "Falta integração do cargo!";
          return BadRequest(view);
        }
        if (establishment.IdEstablishment.Equals("000000000000000000000000"))
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
        // Ler histórico de carga
        IntegrationPerson integrationPerson = service.GetIntegrationPerson(view.Colaborador.ChaveColaborador);
        if (integrationPerson != null && view.CamposAlterados.Count != 0)
        {
          if (view.CamposAlterados == null | view.CamposAlterados.Count == 0)
          {
            view.CamposAlterados = service.EmployeeChange(view.Colaborador, integrationPerson.Employee);
          }
        };

        // Testar se o que está vindo é igual ao que já tinha antes
        Person person = service.GetPersonByKey(company.IdCompany, establishment.IdEstablishment, view.Colaborador.Documento, view.Colaborador.Matricula);
        if (view.Colaborador.DataAdmissao != null)
          view.Colaborador.DataAdmissao = ((DateTime)view.Colaborador.DataAdmissao).ToUniversalTime();
        if (view.Colaborador.DataNascimento != null)
          view.Colaborador.DataNascimento = ((DateTime)view.Colaborador.DataNascimento).ToUniversalTime();
        if (view.Colaborador.DataUltimaTrocaCargo != null)
          view.Colaborador.DataUltimaTrocaCargo = ((DateTime)view.Colaborador.DataUltimaTrocaCargo).ToUniversalTime();
        if (view.Colaborador.DataUltimoReajuste != null)
          view.Colaborador.DataUltimoReajuste = ((DateTime)view.Colaborador.DataUltimoReajuste).ToUniversalTime();
        if (view.Colaborador.DataDemissao != null)
          view.Colaborador.DataDemissao = ((DateTime)view.Colaborador.DataDemissao).ToUniversalTime();
        if (person == null)
        {
          person = new Person
          {
            //Name = view.Colaborador.Nome,
            //Document = view.Colaborador.Documento,
            //Mail = view.Colaborador.Email,
            //Phone = view.Colaborador.Celular,
            //TypeUser = EnumTypeUser.Employee,
            Company = service.GetCompany(company.IdCompany),
            Establishment = string.IsNullOrEmpty(view.Colaborador.NomeEstabelecimento) ? null : service.GetEstablishment(establishment.IdEstablishment),
            Occupation = service.GetOccupation(occupation.IdOccupation),
            Registration = view.Colaborador.Matricula,
            //DateBirth = view.Colaborador.DataNascimento,
            //DateAdm = view.Colaborador.DataAdmissao,
            //Schooling = service.GetSchooling(schooling.IdSchooling),
            //PhoneFixed = view.Colaborador.Telefone,
            //DocumentID = view.Colaborador.Identidade,
            //DocumentCTPF = view.Colaborador.CarteiraProfissional,
            //Sex = view.Colaborador.Sexo.StartsWith("M") ? EnumSex.Male : view.Colaborador.Sexo.StartsWith("F") ? EnumSex.Female : EnumSex.Others,
            HolidayReturn = view.Colaborador.DataRetornoFerias,
            MotiveAside = view.Colaborador.MotivoAfastamento,
            DateLastOccupation = view.Colaborador.DataUltimaTrocaCargo,
            Salary = view.Colaborador.SalarioNominal,
            DateLastReadjust = view.Colaborador.DataUltimoReajuste,
            DateResignation = view.Colaborador.DataDemissao,
            TypeJourney = DateTime.Now.Subtract(((DateTime)view.Colaborador.DataAdmissao)).Days > 90 ? EnumTypeJourney.OnBoardingOccupation : EnumTypeJourney.OnBoarding,
            Manager = personManager,
            DocumentManager = personManager?.DocumentManager,
            //Password = view.Colaborador.Documento
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
          person = servicePerson.NewPersonView(person);
          view.Message = string.Empty;
          view.IdPerson = person._id;
        }
        else
        {
          if (view.CamposAlterados.Count > 0)
          {
            //person.Document = view.Colaborador.Documento;
            person.Registration = view.Colaborador.Matricula;

            if (view.CamposAlterados.Contains("Nome"))
              person.User.Name = view.Colaborador.Nome;

            if (view.CamposAlterados.Contains("Email"))
              person.User.Mail = view.Colaborador.Email;

            //if (view.CamposAlterados.Contains("Celular"))
              //person.Phone = view.Colaborador.Celular;

            if (view.CamposAlterados.Contains("Empresa"))
              person.Company = service.GetCompany(company.IdCompany);

            if (view.CamposAlterados.Contains("Estabelecimento"))
              person.Establishment = string.IsNullOrEmpty(establishment.IdEstablishment) ? null : service.GetEstablishment(establishment.IdEstablishment);

            if (view.CamposAlterados.Contains("Cargo"))
              person.Occupation = service.GetOccupation(occupation.IdOccupation);

            //if (view.CamposAlterados.Contains("DataNascimento"))
              //person.DateBirth = view.Colaborador.DataNascimento;

            if (view.CamposAlterados.Contains("DataAdmissao"))
              person.User.DateAdm = view.Colaborador.DataAdmissao;

            //if (view.CamposAlterados.Contains("GrauInstrucao"))
              //person.Schooling = service.GetSchooling(schooling.IdSchooling);

            //if (view.CamposAlterados.Contains("Telefone"))
              //person.PhoneFixed = view.Colaborador.Telefone;

            //if (8view.CamposAlterados.Contains("Identidade"))
              //person.DocumentID = view.Colaborador.Identidade;

            //if (view.CamposAlterados.Contains("CarteiraProfissional"))
              //person.DocumentCTPF = view.Colaborador.CarteiraProfissional;

            //if (view.CamposAlterados.Contains("Sexo"))
              //person.Sex = view.Colaborador.Sexo.StartsWith("M") ? EnumSex.Male : view.Colaborador.Sexo.StartsWith("F") ? EnumSex.Female : EnumSex.Others;

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
            if (person.Manager == null)
            {
              person.Manager = personManager;
              //person.DocumentManager = personManager?.Document;
            }
            person = servicePerson.UpdatePersonView(person);
            view.IdPerson = person._id;
          }
        }
        if (integrationPerson == null)
        {
          // Novo colaborador na integração
          integrationPerson = new IntegrationPerson()
          {
            Key = view.Colaborador.ChaveColaborador,
            Employee = view.Colaborador
          };
          service.PostIntegrationPerson(integrationPerson);
        }
        else
        {
          integrationPerson.Employee = view.Colaborador;
          service.PutIntegrationPerson(integrationPerson);
        }
        view.Situacao = EnumColaboradorSituacao.Atualized;
        return Ok(view);
      }
      catch (Exception ex)
      {
        view.Situacao = EnumColaboradorSituacao.ServerError;
        return BadRequest(ex.Message);
      }
    }

    private string Capitalization(string nome)
    {
      try
      {
        TextInfo myTI = new CultureInfo("pt-BR", false).TextInfo;
        nome = myTI.ToTitleCase(nome.ToLower()).Replace(" De ", " de ").Replace(" Da ", " da ").Replace(" Dos ", " dos ").Replace(" Do ", " do ");
        return nome;
      }
      catch (Exception)
      {
        throw;
      }
    }
    #endregion

    [Authorize]
    [HttpPost]
    [Route("ajuste")]
    public IActionResult Ajuste()
    {
      try
      {
        List<Person> persons = servicePerson.GetPersons("5b91299a17858f95ffdb79f7",string.Empty);
        foreach (Person person in persons)
        {
          if (person.User.Name.Equals(person.User.Name.ToUpper()))
          {
            person.User.Name = Capitalization(person.User.Name);
            if (person.Manager != null)
              person.Manager.User.Name = Capitalization(person.Manager.User.Name);
            person.TypeJourney = EnumTypeJourney.OnBoardingOccupation;
            var x = servicePerson.UpdatePersonView(person);
          }
        }
        return Ok("Fim");
      }
      catch (Exception ex)
      {
        return BadRequest(ex.ToString());
      }
    }

  }
}
