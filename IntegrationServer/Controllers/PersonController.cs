using System;
using System.Globalization;
using Manager.Core.Business.Integration;
using Manager.Core.Interfaces;
using Manager.Views.BusinessCrud;
using Manager.Views.Enumns;
using Manager.Views.Integration;
using Manager.Views.Integration.V2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationServer.InfraController
{
  /// <summary>
  /// Controlador para integração de funcionários
  /// </summary>
  [Produces("application/json")]
  [Route("person")]
  public class PersonController : Controller
  {
    private readonly IServiceIntegration service;
    private readonly IServicePerson servicePerson;
    private readonly IServiceUser serviceUser;

    #region Constructor
    /// <summary>
    /// Construtor do controlador
    /// </summary>
    /// <param name="_service">Serviço de integração</param>
    /// <param name="_servicePerson">Serviço específico da pessoa</param>
    /// <param name="_serviceCompany">Serviço da empresa</param>
    /// <param name="_serviceUser">Serviço do Usuário</param>
    /// <param name="contextAccessor">Token de segurança</param>
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
    #endregion

    #region Person
    /// <summary>
    /// Integração de funcionário
    /// </summary>
    /// <param name="view">Objeto de integração do colaborador</param>
    /// <returns>Objeto do colaborador atualizado</returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public ViewIntegrationColaborador PutPerson([FromBody]ViewIntegrationColaborador view)
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

        ViewCrudPerson personManager = null;
        if (!string.IsNullOrEmpty(view.Colaborador.NomeEmpresaGestor) && !string.IsNullOrEmpty(view.Colaborador.NomeEstabelecimentoGestor) && !string.IsNullOrEmpty(view.Colaborador.DocumentoGestor))
        {
          IntegrationCompany companyManager = null;
          companyManager = service.GetIntegrationCompany(view.Colaborador.ChaveEmpresaGestor, view.Colaborador.NomeEmpresaGestor);
          if (!companyManager.IdCompany.Equals("000000000000000000000000"))
          {
            IntegrationEstablishment establishmentManager = null;
            establishmentManager = service.GetIntegrationEstablishment(view.Colaborador.ChaveEstabelecimento, view.Colaborador.NomeEstabelecimento, company.IdCompany);
            if (!establishmentManager.IdEstablishment.Equals("000000000000000000000000"))
              personManager = service.GetPersonByKey(companyManager.IdCompany, establishmentManager.IdEstablishment, view.Colaborador.DocumentoGestor, view.Colaborador.MatriculaGestor);
          }
        }

        if (company.IdCompany.Equals("000000000000000000000000"))
        {
          view.Message = "Falta integração da empresa!";
          return view;
        }
        if (occupation.IdOccupation.Equals("000000000000000000000000"))
        {
          view.Message = "Falta integração do cargo!";
          return view;
        }
        if (establishment.IdEstablishment.Equals("000000000000000000000000"))
        {
          view.Message = "Falta integração do estabelecimento!";
          return view;
        }
        if (string.IsNullOrEmpty(view.Colaborador.Documento))
        {
          view.Message = "Documento deve ser informado!";
          return view;
        }
        if (string.IsNullOrEmpty(view.Colaborador.Nome))
        {
          view.Message = "Nome deve ser informado!";
          return view;
        }
        if (view.Colaborador.DataAdmissao == null)
        {
          view.Message = "Data de admissão não informada!";
          return view;
        }
        if (string.IsNullOrEmpty(view.Colaborador.NomeCargo))
        {
          view.Message = "Descrição do cargo deve ser informado!";
          return view;
        }
        if (string.IsNullOrEmpty(view.Colaborador.NomeGrauInstrucao))
        {
          view.Message = "Descrição do grau de instrução deve ser informado!";
          return view;
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
        ViewCrudUser userView = service.GetUserByKey(view.Colaborador.Documento);
        if (userView == null)
        {
          userView = new ViewCrudUser()
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
            Nickname = view.Colaborador.Apelido
          };
          userView = serviceUser.New(userView);
        }
        else
        {
          userView.Name = view.Colaborador.Nome;
          userView.Document = view.Colaborador.Documento;
          if (!string.IsNullOrEmpty(view.Colaborador.Email))
            userView.Mail = view.Colaborador.Email;
          userView.Phone = view.Colaborador.Celular;
          userView.DateAdm = view.Colaborador.DataAdmissao;
          userView.DateBirth = view.Colaborador.DataNascimento;
          userView.Schooling = service.GetSchooling(schooling.IdSchooling);
          userView.PhoneFixed = view.Colaborador.Telefone;
          userView.DocumentID = view.Colaborador.Identidade;
          userView.DocumentCTPF = view.Colaborador.CarteiraProfissional;
          userView.Sex = view.Colaborador.Sexo.StartsWith("M") ? EnumSex.Male : view.Colaborador.Sexo.StartsWith("F") ? EnumSex.Female : EnumSex.Others;
          userView.Nickname = view.Colaborador.Apelido;
          userView = serviceUser.Update(userView);
        }
        // Verificar se a person já existe
        ViewCrudPerson viewPerson = service.GetPersonByKey(company.IdCompany, establishment.IdEstablishment, view.Colaborador.Documento, view.Colaborador.Matricula);
        if (viewPerson == null)
        {
          viewPerson = new ViewCrudPerson
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
          };
          if (personManager != null)
            viewPerson.Manager = new ViewBaseFields() { Mail = personManager.User.Mail, Name = personManager.User.Name, _id = personManager._id };

          switch (view.Colaborador.Situacao.ToLower())
          {
            case "férias": case "ferias":
              viewPerson.StatusUser = EnumStatusUser.Vacation;
              break;
            case "afastado":
              viewPerson.StatusUser = EnumStatusUser.Away;
              break;
            case "demitido":
              viewPerson.StatusUser = EnumStatusUser.Disabled;
              break;
            default:
              viewPerson.StatusUser = EnumStatusUser.Enabled;
              break;
          }
          viewPerson.User = userView;
          viewPerson = servicePerson.New(viewPerson) ;
          view.Message = "Person Included!";
          view.IdPerson = viewPerson._id;
        }
        else
        {
          viewPerson.Company = service.GetCompany(company.IdCompany);
          viewPerson.Establishment = string.IsNullOrEmpty(view.Colaborador.NomeEstabelecimento) ? null : service.GetEstablishment(establishment.IdEstablishment);
          viewPerson.Occupation = service.GetOccupation(occupation.IdOccupation);
          viewPerson.Registration = view.Colaborador.Matricula.ToString();
          viewPerson.HolidayReturn = view.Colaborador.DataRetornoFerias;
          viewPerson.MotiveAside = view.Colaborador.MotivoAfastamento;
          viewPerson.DateLastOccupation = view.Colaborador.DataUltimaTrocaCargo;
          viewPerson.Salary = view.Colaborador.SalarioNominal;
          viewPerson.DateLastReadjust = view.Colaborador.DataUltimoReajuste;
          viewPerson.DateResignation = view.Colaborador.DataDemissao;

          if (personManager != null)
            viewPerson.Manager = new ViewBaseFields() { Mail = personManager.User.Mail, Name = personManager.User.Name, _id = personManager._id };

          switch (view.Colaborador.Situacao.ToLower())
          {
            case "férias":
            case "ferias":
              viewPerson.StatusUser = EnumStatusUser.Vacation;
              break;
            case "afastado":
              viewPerson.StatusUser = EnumStatusUser.Away;
              break;
            case "demitido":
              viewPerson.StatusUser = EnumStatusUser.Disabled;
              break;
            default:
              viewPerson.StatusUser = EnumStatusUser.Enabled;
              break;
          }
          viewPerson.User = userView;
          servicePerson.Update(viewPerson);
          view.IdPerson = viewPerson._id;
          view.Message = "Person atualized!";
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
      }
      catch (Exception e)
      {
        view.Message = e.Message;
        view.Situacao = EnumColaboradorSituacao.ServerError;
      }
      return view;
    }
    #endregion

    #region Colaborador V2
    /// <summary>
    /// Admissão de colaborador
    /// </summary>
    /// <param name="view">Objeto de integração do colaborador</param>
    /// <returns>Objeto do colaborador atualizado</returns>
    /// <response code="200">Informações sobre a admissão do colaborador</response>
    [Authorize]
    [HttpPost]
    [Route("v2/admissao")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ColaboradorV2Retorno), StatusCodes.Status200OK)]
    public ObjectResult AdmissaoV2([FromBody]ColaboradorV2Admissao view)
    {
      ColaboradorV2Retorno result = new ColaboradorV2Retorno()
      {
        Situacao = EnumSituacaoRetornoIntegracao.Erro,
        Mensagem = "Não implementado"
      };
      return Ok(result);
    }
    /// <summary>
    /// Alteração de Cargo
    /// </summary>
    /// <param name="view">Objeto de alteração do cargo do colaborador</param>
    /// <returns>Objeto do colaborador atualizado</returns>
    [Authorize]
    [HttpPut]
    [Route("v2/cargo")]
    public ColaboradorV2Retorno CargoV2([FromBody]ColaboradorV2Cargo view)
    {
      return new ColaboradorV2Retorno()
      {
        IdContract = string.Empty,
        IdUser = string.Empty,
        Mensagem = "Não implementado",
        Situacao = EnumSituacaoRetornoIntegracao.Erro
      };
    }
    /// <summary>
    /// Alteração de Centro de Custo
    /// </summary>
    /// <param name="view">Objeto de alteração do centro de custo do colaborador</param>
    /// <returns>Objeto do colaborador atualizado</returns>
    [Authorize]
    [HttpPut]
    [Route("v2/centrocusto")]
    public ColaboradorV2Retorno CentroCustoV2([FromBody]ColaboradorV2CentroCusto view)
    {
      return new ColaboradorV2Retorno()
      {
        IdContract = string.Empty,
        IdUser = string.Empty,
        Mensagem = "Não implementado",
        Situacao = EnumSituacaoRetornoIntegracao.Erro
      };
    }
    /// <summary>
    /// Alteração de colaborador
    /// </summary>
    /// <param name="view">Objeto de alteração geral do colaborador</param>
    /// <returns>Objeto do colaborador atualizado</returns>
    [Authorize]
    [HttpPut]
    [Route("v2/completo")]
    public ColaboradorV2Retorno AlteracaoV2([FromBody]ColaboradorV2Completo view)
    {
      return new ColaboradorV2Retorno()
      {
        Mensagem = "Não implementado",
        Situacao = EnumSituacaoRetornoIntegracao.Erro
      };
    }
    /// <summary>
    /// Demissão de colaborador
    /// </summary>
    /// <param name="view">Objeto de demissão do colaborador</param>
    /// <returns>Objeto do colaborador atualizado</returns>
    [Authorize]
    [HttpPut]
    [Route("v2/demissao")]
    public ColaboradorV2Retorno DemissaoV2([FromBody]ColaboradorV2Demissao view)
    {
      return new ColaboradorV2Retorno()
      {
        IdContract = string.Empty,
        IdUser = string.Empty,
        Mensagem = "Não implementado",
        Situacao = EnumSituacaoRetornoIntegracao.Erro
      };
    }
    /// <summary>
    /// Gestor de colaborador
    /// </summary>
    /// <param name="view">Objeto de alteração do gestor do colaborador</param>
    /// <returns>Objeto do colaborador atualizado</returns>
    [Authorize]
    [HttpPut]
    [Route("v2/gestor")]
    public ColaboradorV2Retorno GestorV2([FromBody]ColaboradorV2Gestor view)
    {
      return new ColaboradorV2Retorno()
      {
        IdContract = string.Empty,
        IdUser = string.Empty,
        Mensagem = "Não implementado",
        Situacao = EnumSituacaoRetornoIntegracao.Erro
      };
    }
    /// <summary>
    /// Salário do colaborador
    /// </summary>
    /// <param name="view">Objeto de alteração do salário do colaborador</param>
    /// <returns>Objeto do colaborador atualizado</returns>
    [Authorize]
    [HttpPut]
    [Route("v2/salario")]
    public ColaboradorV2Retorno SalarioV2([FromBody]ColaboradorV2Salario view)
    {
      return new ColaboradorV2Retorno()
      {
        IdContract = string.Empty,
        IdUser = string.Empty,
        Mensagem = "Não implementado",
        Situacao = EnumSituacaoRetornoIntegracao.Erro
      };
    }
    #endregion

    #region Private
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

  }
}
