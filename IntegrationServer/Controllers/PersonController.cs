using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Manager.Core.Base;
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
    private readonly IServiceUser serviceUser;

    public PersonController(IServiceIntegration _service, IServicePerson _servicePerson, IServiceCompany _serviceCompany,
      IServiceUser _serviceUser, IHttpContextAccessor contextAccessor)
    {
      try
      {
        service = _service;
        servicePerson = _servicePerson;
        serviceUser = _serviceUser;
        service.SetUser(contextAccessor);
        servicePerson.SetUser(contextAccessor);
        serviceUser.SetUser(contextAccessor);
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
          //Registration = person.Registration,
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
          personManager = service.GetPersonByKey(companyManager.IdCompany, establishment.IdEstablishment, view.Colaborador.DocumentoGestor, view.Colaborador.MatriculaGestor);

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

        // Testar se o usuário já existe
        User user = service.GetUserByKey(view.Colaborador.Documento);
        if (user == null)
        {
          user = new User()
          {
            Name = view.Colaborador.Nome,
            Document = view.Colaborador.Documento,
            Mail = view.Colaborador.Email,
            Phone = view.Colaborador.Celular,
            DateAdm = view.Colaborador.DataAdmissao,
            DateBirth = view.Colaborador.DataNascimento,
            Schooling = service.GetSchooling(schooling.IdSchooling),
            PhoneFixed = view.Colaborador.Telefone,
            DocumentID = view.Colaborador.Identidade,
            DocumentCTPF = view.Colaborador.CarteiraProfissional,
            Sex = view.Colaborador.Sexo.StartsWith("M") ? EnumSex.Male : view.Colaborador.Sexo.StartsWith("F") ? EnumSex.Female : EnumSex.Others,
            Password = view.Colaborador.Documento,
          };
          user = serviceUser.NewUserView(user);
        }
        else
        {
          user.Document = view.Colaborador.Documento;
          user.Name = view.Colaborador.Nome;
          user.Mail = view.Colaborador.Email;
          user.Phone = view.Colaborador.Celular;
          user.DateBirth = view.Colaborador.DataNascimento;
          user.DateAdm = view.Colaborador.DataAdmissao;
          user.Schooling = service.GetSchooling(schooling.IdSchooling);
          user.PhoneFixed = view.Colaborador.Telefone;
          user.DocumentID = view.Colaborador.Identidade;
          user.DocumentCTPF = view.Colaborador.CarteiraProfissional;
          user.Sex = view.Colaborador.Sexo.StartsWith("M") ? EnumSex.Male : view.Colaborador.Sexo.StartsWith("F") ? EnumSex.Female : EnumSex.Others;
          user = serviceUser.UpdateUserView(user);
        }
        // Testar se a person já existe
        Person person = service.GetPersonByKey(company.IdCompany, establishment.IdEstablishment, view.Colaborador.Documento, view.Colaborador.Matricula);
        if (person == null)
        {
          person = new Person
          {
            TypeUser = EnumTypeUser.Employee,
            Company = service.GetCompany(company.IdCompany),
            Establishment = string.IsNullOrEmpty(view.Colaborador.NomeEstabelecimento) ? null : service.GetEstablishment(establishment.IdEstablishment),
            Occupation = service.GetOccupation(occupation.IdOccupation),
            Registration = view.Colaborador.Matricula.ToString(),
            HolidayReturn = view.Colaborador.DataRetornoFerias,
            MotiveAside = view.Colaborador.MotivoAfastamento,
            DateLastOccupation = view.Colaborador.DataUltimaTrocaCargo,
            Salary = view.Colaborador.SalarioNominal,
            DateLastReadjust = view.Colaborador.DataUltimoReajuste,
            DateResignation = view.Colaborador.DataDemissao,
            TypeJourney = DateTime.Now.Subtract(((DateTime)view.Colaborador.DataAdmissao)).Days > 90 ? EnumTypeJourney.OnBoardingOccupation : EnumTypeJourney.OnBoarding,
            Manager = new BaseFields() { Mail = personManager.User.Mail, Name = personManager.User.Name, _id = personManager._id },
            DocumentManager = personManager?.DocumentManager,
          };
          switch (view.Colaborador.Situacao.ToLower())
          {
            case "férias": case "ferias":
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
          person.User = user;
          person = servicePerson.NewPersonView(person);
          view.Message = string.Empty;
          view.IdPerson = person._id;
        }
        else
        {
          person.Registration = view.Colaborador.Matricula.ToString();
          person.User.Name = view.Colaborador.Nome;
          person.User.Mail = view.Colaborador.Email;
          person.Company = service.GetCompany(company.IdCompany);
          person.Establishment = string.IsNullOrEmpty(establishment.IdEstablishment) ? null : service.GetEstablishment(establishment.IdEstablishment);
          person.Occupation = service.GetOccupation(occupation.IdOccupation);
          person.User.DateAdm = view.Colaborador.DataAdmissao;
          person.HolidayReturn = view.Colaborador.DataRetornoFerias;
          person.MotiveAside = view.Colaborador.MotivoAfastamento;
          person.DateLastOccupation = view.Colaborador.DataUltimaTrocaCargo;
          person.Salary = view.Colaborador.SalarioNominal;
          person.DateLastReadjust = view.Colaborador.DataUltimoReajuste;
          person.DateResignation = view.Colaborador.DataDemissao;
          switch (view.Colaborador.Situacao.ToLower())
          {
            case "férias": case "ferias":
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
            person.Manager = new BaseFields() { Mail = personManager.User.Mail, Name = personManager.User.Name, _id = personManager._id };
          }
          person.User = user;
          person = servicePerson.UpdatePersonView(person);
          view.IdPerson = person._id;
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
        List<Person> persons = servicePerson.GetPersons("5b91299a17858f95ffdb79f7", string.Empty);
        foreach (Person person in persons)
        {
          if (person.User.Name.Equals(person.User.Name.ToUpper()))
          {
            person.User.Name = Capitalization(person.User.Name);
            if (person.Manager != null)
              person.Manager.Name = Capitalization(person.Manager.Name);
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
