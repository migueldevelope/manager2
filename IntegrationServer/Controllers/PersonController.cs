using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Core.Views.Integration;
using Manager.Services.Commons;
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
    [Authorize]
    [HttpPost]
    public IActionResult GetPersonByKey([FromBody]ViewIntegrationMapPersonV1  view)
    {
      try
      {
        view.Id = string.Empty;
        view.Name = string.Empty;
        view.Message = string.Empty;
        Person person = service.GetPersonByKey(view.Document,view.IdCompany,view.Registration);
        if (person == null)
        {
          view.Message = "Person not found!";
        }
        else
        {
          view.Id = person._id;
          view.Name = person.Name;
          view.Person = new ViewIntegrationPersonV1()
          {
            _id = person._id,
            Name = person.Name,
            Mail = person.Mail,
            Document = person.Document,
            DateBirth = person.DateBirth,
            Phone = person.Phone,
            PhoneFixed = person.PhoneFixed,
            DocumentID = person.DocumentID,
            DocumentCTPF = person.DocumentCTPF,
            Sex = person.Sex,
            Schooling = new ViewIntegrationMapOfV1() { Id = person.Schooling._id, Name = person.Schooling.Name },
            Contract = new ViewIntegrationContractV1()
            {
              _id = string.Empty,
              Document = person.Document,
              Company = person.Company == null ? null : new ViewIntegrationMapOfV1() { Id = person.Company._id, Name = person.Company.Name },
              Registration = person.Registration,
              Establishment = person.Establishment == null ? null : new ViewIntegrationMapOfV1() { Id = person.Establishment._id, Name = person.Establishment.Name, IdCompany = person.Establishment.Company._id },
              DateAdm = person.DateAdm,
              StatusUser = person.StatusUser,
              HolidayReturn = person.HolidayReturn,
              MotiveAside = person.MotiveAside,
              DateResignation = person.DateResignation,
              Occupation = person.Occupation == null ? null : new ViewIntegrationMapOfV1() { Id = person.Occupation._id, IdCompany = person.Occupation.ProcessLevelTwo.ProcessLevelOne.Area.Company._id, Name = person.Occupation.Name },
              DateLastOccupation = person.DateLastOccupation,
              Salary  = person.Salary,
              DateLastReadjust = person.DateLastReadjust,
              _IdManager = person.Manager?._id,
              DocumentManager = person.Manager?.Document,
              CompanyManager = person.Manager == null ? null : new ViewIntegrationMapOfV1() { Id = person.Manager.Company._id, Name = person.Manager.Company.Name },
              RegistrationManager = person.Manager == null ? 0 : person.Manager.Registration,
              NameManager = person.Manager?.Name,
              TypeUser = person.TypeUser,
              TypeJourney = person.TypeJourney
            },
          };
        }
        return Ok(view);
      }
      catch (Exception ex)
      {
        view.Message = ex.Message;
        return Ok(view);
      }
    }

    [Authorize]
    [HttpPost]
    [Route("manager")]
    public IActionResult GetManagerByKey([FromBody]ViewIntegrationMapManagerV1 view)
    {
      try
      {
        view.IdPerson = string.Empty;
        view.IdContract = string.Empty;
        view.Name = string.Empty;
        view.Message = string.Empty;
        Person person = service.GetPersonByKey(view.Document, view.CompanyId, view.Registration);
        if (person == null)
        {
          view.Message = "Person not found!";
        }
        else
        {
          view.IdPerson = person._id;
          view.Name = person.Name;
        }
        return Ok(view);
      }
      catch (Exception ex)
      {
        view.Message = ex.Message;
        return Ok(view);
      }
    }

    [Authorize]
    [HttpPost]
    [Route("new")]
    public IActionResult PostNewPerson([FromBody]ViewIntegrationPersonV1 view)
    {
      try
      {
        Person personManager = null;
        if (!string.IsNullOrEmpty(view.Contract._IdManager))
          personManager = servicePerson.GetPerson(view.Contract._IdManager);

        Person newPerson = new Person()
        {
          Name = view.Name,
          Document = view.Document,
          Mail = view.Mail,
          Phone = view.Phone,
          TypeUser = (EnumTypeUser)view.Contract.TypeUser,
          StatusUser = (EnumStatusUser)view.Contract.StatusUser,
          Company = service.GetCompany(view.Contract.Company.Id),
          Occupation = service.GetOccupation(view.Contract.Occupation.Id),
          Registration = view.Contract.Registration,
          DateBirth = view.DateBirth,
          DateAdm = view.Contract.DateAdm,
          Schooling = service.GetSchooling(view.Schooling.Id),
          TypeJourney = (EnumTypeJourney)view.Contract.TypeJourney,
          Establishment = service.GetEstablishment(view.Contract.Establishment.Id),
          PhoneFixed = view.PhoneFixed,
          DocumentID = view.DocumentID,
          DocumentCTPF = view.DocumentCTPF,
          Sex = view.Sex,
          HolidayReturn = view.Contract.HolidayReturn,
          MotiveAside = view.Contract.MotiveAside,
          DateLastOccupation =  view.Contract.DateLastOccupation,
          Salary = view.Contract.Salary,
          DateLastReadjust = view.Contract.DateLastReadjust,
          DateResignation = view.Contract.DateResignation,
          Manager = personManager,
          DocumentManager = personManager?.Document
        };
        Person person = servicePerson.NewPersonView(newPerson);
        if (person == null)
          return BadRequest("Person not saved!!!");

        ViewIntegrationMapPersonV1 viewReturn = new ViewIntegrationMapPersonV1()
        {
          Id = person._id,
          Document = person.Document,
          IdCompany = person.Company._id,
          Registration = person.Registration,
          Name = person.Name,
          Person = new ViewIntegrationPersonV1()
          {
            _id = person._id,
            Name = person.Name,
            Mail = person.Mail,
            Document = person.Document,
            DateBirth = person.DateBirth,
            Phone = person.Phone,
            PhoneFixed = person.PhoneFixed,
            DocumentID = person.DocumentID,
            DocumentCTPF = person.DocumentCTPF,
            Sex = person.Sex,
            Schooling = person.Schooling == null ? null : new ViewIntegrationMapOfV1() { Id = person.Schooling._id, Name = person.Schooling.Name },
            Contract = new ViewIntegrationContractV1()
            {
              _id = string.Empty,
              Document = person.Document,
              Company = person.Company == null ? null : new ViewIntegrationMapOfV1() { Id = person.Company._id, Name = person.Company.Name },
              Registration = person.Registration,
              Establishment = person.Establishment == null ? null : new ViewIntegrationMapOfV1() { Id = person.Establishment._id, Name = person.Establishment.Name, IdCompany = person.Establishment.Company._id },
              DateAdm = person.DateAdm,
              StatusUser = person.StatusUser,
              HolidayReturn = person.HolidayReturn,
              MotiveAside = person.MotiveAside,
              DateResignation = person.DateResignation,
              Occupation = person.Occupation == null ? null : new ViewIntegrationMapOfV1() { Id = person.Occupation._id, IdCompany = person.Occupation.ProcessLevelTwo.ProcessLevelOne.Area.Company._id, Name = person.Occupation.Name },
              DateLastOccupation = person.DateLastOccupation,
              Salary = person.Salary,
              DateLastReadjust = person.DateLastReadjust,
              _IdManager = person.Manager?._id,
              DocumentManager = person.Manager?.Document,
              CompanyManager = person.Manager == null ? null : new ViewIntegrationMapOfV1() { Id = person.Manager.Company._id, Name = person.Manager.Company.Name },
              RegistrationManager = person.Manager == null ? 0 : person.Manager.Registration,
              NameManager = person.Manager?.Name,
              TypeUser = person.TypeUser,
              TypeJourney = person.TypeJourney
            }
          }
        };
        return Ok(viewReturn);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.ToString());
      }
    }

    [Authorize]
    [HttpPut]
    [Route("update")]
    public IActionResult PutPerson([FromBody]ViewIntegrationPersonV1 view)
    {
      try
      {
        Person personManager = null;
        if (!string.IsNullOrEmpty(view.Contract._IdManager))
          personManager = servicePerson.GetPerson(view.Contract._IdManager);

        Person changePerson = servicePerson.GetPerson(view._id);
        changePerson.Name = view.Name;
        changePerson.Document = view.Document;
        changePerson.Mail = view.Mail;
        changePerson.Phone = view.Phone;
        changePerson.TypeUser = (EnumTypeUser)view.Contract.TypeUser;
        changePerson.StatusUser = (EnumStatusUser)view.Contract.StatusUser;
        changePerson.Company = service.GetCompany(view.Contract.Company.Id);
        changePerson.Occupation = service.GetOccupation(view.Contract.Occupation.Id);
        changePerson.Registration = view.Contract.Registration;
        changePerson.DateBirth = view.DateBirth;
        changePerson.DateAdm = view.Contract.DateAdm;
        changePerson.Schooling = service.GetSchooling(view.Schooling.Id);
        //changePerson.TypeJourney = (EnumTypeJourney)view.Contract.TypeJourney;
        changePerson.Establishment = service.GetEstablishment(view.Contract.Establishment.Id);
        changePerson.PhoneFixed = view.PhoneFixed;
        changePerson.DocumentID = view.DocumentID;
        changePerson.DocumentCTPF = view.DocumentCTPF;
        changePerson.Sex = view.Sex;
        changePerson.HolidayReturn = view.Contract.HolidayReturn;
        changePerson.MotiveAside = view.Contract.MotiveAside;
        changePerson.DateLastOccupation = view.Contract.DateLastOccupation;
        changePerson.Salary = view.Contract.Salary;
        changePerson.DateLastReadjust = view.Contract.DateLastReadjust;
        changePerson.DateResignation = view.Contract.DateResignation;
        changePerson.Manager = personManager;
        changePerson.DocumentManager = personManager?.Document;
        Person person = servicePerson.UpdatePersonView(changePerson);
        ViewIntegrationMapPersonV1 viewReturn = new ViewIntegrationMapPersonV1()
        {
          Id = person._id,
          Document = person.Document,
          IdCompany = person.Company._id,
          Registration = person.Registration,
          Name = person.Name,
          Person = new ViewIntegrationPersonV1()
          {
            _id = person._id,
            Name = person.Name,
            Mail = person.Mail,
            Document = person.Document,
            DateBirth = person.DateBirth,
            Phone = person.Phone,
            PhoneFixed = person.PhoneFixed,
            DocumentID = person.DocumentID,
            DocumentCTPF = person.DocumentCTPF,
            Sex = person.Sex,
            Schooling = new ViewIntegrationMapOfV1() { Id = person.Schooling._id, Name = person.Schooling.Name },
            Contract = new ViewIntegrationContractV1()
            {
              _id = string.Empty,
              Document = person.Document,
              Company = new ViewIntegrationMapOfV1() { Id = person.Company._id, Name = person.Company.Name },
              Registration = person.Registration,
              Establishment = new ViewIntegrationMapOfV1() { Id = person.Establishment._id, Name = person.Establishment.Name, IdCompany = person.Establishment.Company._id },
              DateAdm = person.DateAdm,
              StatusUser = person.StatusUser,
              HolidayReturn = person.HolidayReturn,
              MotiveAside = person.MotiveAside,
              DateResignation = person.DateResignation,
              Occupation = new ViewIntegrationMapOfV1() { Id = person.Occupation._id, IdCompany = person.Occupation.ProcessLevelTwo.ProcessLevelOne.Area.Company._id, Name = person.Occupation.Name },
              DateLastOccupation = person.DateLastOccupation,
              Salary = person.Salary,
              DateLastReadjust = person.DateLastReadjust,
              _IdManager = person.Manager._id,
              DocumentManager = person.Manager.Document,
              CompanyManager = new ViewIntegrationMapOfV1() { Id = person.Manager.Company._id, Name = person.Manager.Company.Name },
              RegistrationManager = person.Manager.Registration,
              NameManager = person.Manager.Name,
              TypeUser = person.TypeUser,
              TypeJourney = person.TypeJourney
            }
          }
        };
        return Ok(viewReturn);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.ToString());
      }
    }

    [Authorize]
    [HttpPost]
    [Route("schooling")]
    public IActionResult GetSchoolingByName([FromBody]ViewIntegrationMapOfV1 view)
    {
      try
      {
        List<Schooling> schoolings = service.GetIntegrationSchooling(view.Code, view.Name);
        switch (schoolings.Count)
        {
          case 0:
            view.Id = string.Empty;
            view.Message = "Schooling not found!";
            break;
          case 1:
            view.Id = schoolings[0]._id;
            view.Message = string.Empty;
            break;
          default:
            view.Id = string.Empty;
            view.Message = "Schooling duplicated!";
            break;
        }
        return Ok(view);
      }
      catch (Exception ex)
      {
        view.Message = ex.Message;
        return Ok(view);
      }
    }

    [Authorize]
    [HttpPost]
    [Route("company")]
    public IActionResult GetCompanyByName([FromBody]ViewIntegrationMapOfV1 view)
    {
      try
      {
        List<Company> companys = service.GetIntegrationCompany(view.Code, view.Name);
        switch (companys.Count)
        {
          case 0:
            view.Id = string.Empty;
            view.Message = "Company not found!";
            break;
          case 1:
            view.Id = companys[0]._id;
            view.Message = string.Empty;
            break;
          default:
            view.Id = string.Empty;
            view.Message = "Company duplicated!";
            break;
        }
        return Ok(view);
      }
      catch (Exception ex)
      {
        view.Message = ex.Message;
        return Ok(view);
      }
    }

    [Authorize]
    [HttpPost]
    [Route("establishment")]
    public IActionResult GetEstablishmentByName([FromBody]ViewIntegrationMapOfV1 view)
    {
      try
      {
        List<Establishment> establishments = service.GetIntegrationEstablishment(view.IdCompany, view.Code, view.Name);
        switch (establishments.Count)
        {
          case 0:
            view.Id = string.Empty;
            view.Message = "Establishment not found!";
            break;
          case 1:
            view.Id = establishments[0]._id;
            view.Message = string.Empty;
            break;
          default:
            view.Id = string.Empty;
            view.Message = "Establishment duplicated!";
            break;
        }
        return Ok(view);
      }
      catch (Exception ex)
      {
        view.Message = ex.Message;
        return Ok(view);
      }
    }

    [Authorize]
    [HttpPost]
    [Route("occupation")]
    public IActionResult GetOccupationByName([FromBody]ViewIntegrationMapOfV1 view)
    {
      try
      {
        List<Occupation> occupations = service.GetIntegrationOccupation(view.IdCompany, view.Code, view.Name);
        switch (occupations.Count)
        {
          case 0:
            view.Id = string.Empty;
            view.Message = "Occupation not found!";
            break;
          case 1:
            view.Id = occupations[0]._id;
            view.Message = string.Empty;
            break;
          default:
            view.Id = string.Empty;
            view.Message = "Occupation duplicated!";
            break;
        }
        return Ok(view);
      }
      catch (Exception ex)
      {
        view.Message = ex.Message;
        return Ok(view);
      }
    }
  }
}
