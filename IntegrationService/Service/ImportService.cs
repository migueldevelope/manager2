using IntegrationService.Api;
using IntegrationService.Core;
using IntegrationService.Data;
using IntegrationService.Tools;
using IntegrationService.Views;
using IntegrationService.Views.Person;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace IntegrationService.Service
{
  public class ImportService
  {
    private readonly List<Colaborador> Lista;
    private readonly ViewPersonLogin Person;
    public string Message { get; set; }
    private List<ViewIntegrationMapOfV1> Schoolings;
    private List<ViewIntegrationMapOfV1> Companys;
    private List<ViewIntegrationMapOfV1> Establishments;
    private List<ViewIntegrationMapOfV1> Occupations;
    private List<MapPerson> Collaborators;
    public ImportService(ViewPersonLogin person, List<Colaborador> lista)
    {
      try
      {
        Person = person;
        Lista = lista;
        LoadLists();
        Import();
      }
      catch (Exception ex)
      {
        throw;
      }
    }
    private void LoadLists()
    {
      try
      {
        Companys = new List<ViewIntegrationMapOfV1>();
        if (File.Exists(string.Format("{0}/integration/empresa.txt", Person.IdAccount)))
          Companys = JsonConvert.DeserializeObject<List<ViewIntegrationMapOfV1>>(FileClass.ReadFromBinaryFile<string>(string.Format("{0}/integration/empresa.txt", Person.IdAccount)));
        Schoolings = new List<ViewIntegrationMapOfV1>();
        if (File.Exists(string.Format("{0}/integration/grau.txt", Person.IdAccount)))
          Schoolings = JsonConvert.DeserializeObject<List<ViewIntegrationMapOfV1>>(FileClass.ReadFromBinaryFile<string>(string.Format("{0}/integration/grau.txt", Person.IdAccount)));
        Establishments = new List<ViewIntegrationMapOfV1>();
        if (File.Exists(string.Format("{0}/integration/estabelecimento.txt", Person.IdAccount)))
          Establishments = JsonConvert.DeserializeObject<List<ViewIntegrationMapOfV1>>(FileClass.ReadFromBinaryFile<string>(string.Format("{0}/integration/estabelecimento.txt", Person.IdAccount)));
        Occupations = new List<ViewIntegrationMapOfV1>();
        if (File.Exists(string.Format("{0}/integration/cargo.txt", Person.IdAccount)))
          Occupations = JsonConvert.DeserializeObject<List<ViewIntegrationMapOfV1>>(FileClass.ReadFromBinaryFile<string>(string.Format("{0}/integration/cargo.txt", Person.IdAccount)));
        Collaborators = new List<MapPerson>();
        if (File.Exists(string.Format("{0}/integration/colaborador.txt", Person.IdAccount)))
          Collaborators = JsonConvert.DeserializeObject<List<MapPerson>>(FileClass.ReadFromBinaryFile<string>(string.Format("{0}/integration/colaborador.txt", Person.IdAccount)));
      }
      catch (Exception)
      {
        throw;
      }
    }
    private void Import()
    {
      foreach (var colaborador in Lista)
      {
        // Validar Grau Instrução, Empresa, Estabelecimento, Cargo
        ViewIntegrationMapOfV1 schooling = VerifyMapOf(Schoolings, EnumValidKey.Schooling, colaborador.GrauInstrucao, colaborador.NomeGrauInstrucao);
        ViewIntegrationMapOfV1 company = VerifyMapOf(Companys, EnumValidKey.Company, colaborador.Empresa, colaborador.NomeEmpresa);
        ViewIntegrationMapOfV1 establishment = VerifyMapOf(Establishments, EnumValidKey.Establishment, colaborador.Estabelecimento, colaborador.NomeEstabelecimento, company.Id);
        ViewIntegrationMapOfV1 occupation = VerifyMapOf(Occupations, EnumValidKey.Occupation, colaborador.Cargo, colaborador.NomeCargo, company.Id);

        // Validar Colaborador
        int search = Collaborators.FindIndex(p => p.Documento == colaborador.Documento && p.Empresa == colaborador.Empresa && p.Matricula == colaborador.Matricula);
        if (search == -1)
        {
          ValidMapCollaborator validMapCollaborator = new ValidMapCollaborator(Person, colaborador.Documento, company, Convert.ToInt64(colaborador.Matricula), colaborador.Nome);
          validMapCollaborator.Map.Colaborador = colaborador;
          validMapCollaborator.Map.ColaboradorAnterior = colaborador;
          validMapCollaborator.Map.Schooling = schooling;
          validMapCollaborator.Map.Company = company;
          validMapCollaborator.Map.Establishment = establishment;
          validMapCollaborator.Map.Occupation = occupation;
          // Verificar se a pessoa já existe
          PersonIntegration personIntegration = new PersonIntegration(Person);
          ViewIntegrationMapPersonV1 view = new ViewIntegrationMapPersonV1()
          {
            Document = colaborador.Documento,
            IdCompany = company.Id,
            Registration = colaborador.Matricula,
            Name = colaborador.Nome,
            Id = string.Empty,
            Person = null,
            Message = string.Empty
          };
          view = personIntegration.GetCollaboratorByKey(view);
          if (view.Person != null)
            validMapCollaborator.Map.Person = view.Person;

          Collaborators.Add(validMapCollaborator.Map);
        }
        else
        {
          Collaborators[search].Schooling = schooling;
          Collaborators[search].Company = company;
          Collaborators[search].Establishment = establishment;
          Collaborators[search].Occupation = occupation;
          Collaborators[search].Colaborador = colaborador;
          if (Collaborators[search].Person == null)
          {
            // Verificar se a pessoa já existe

          }
        }
      }

      string saveObject = JsonConvert.SerializeObject(Schoolings);
      if (!Directory.Exists(string.Format("{0}/integration", Person.IdAccount)))
        Directory.CreateDirectory(string.Format("{0}/integration", Person.IdAccount));
      FileClass.WriteToBinaryFile<string>(string.Format("{0}/integration/grau.txt", Person.IdAccount), saveObject, false);
      saveObject = JsonConvert.SerializeObject(Companys);
      FileClass.WriteToBinaryFile<string>(string.Format("{0}/integration/empresa.txt", Person.IdAccount), saveObject, false);
      saveObject = JsonConvert.SerializeObject(Establishments);
      FileClass.WriteToBinaryFile<string>(string.Format("{0}/integration/estabelecimentos.txt", Person.IdAccount), saveObject, false);
      saveObject = JsonConvert.SerializeObject(Occupations);
      FileClass.WriteToBinaryFile<string>(string.Format("{0}/integration/cargo.txt", Person.IdAccount), saveObject, false);
      saveObject = JsonConvert.SerializeObject(Collaborators);
      FileClass.WriteToBinaryFile<string>(string.Format("{0}/integration/colaborador.txt", Person.IdAccount), saveObject, false);

      Message = string.Empty;
      foreach (ViewIntegrationMapOfV1 item in Schoolings)
        if (string.IsNullOrEmpty(item.Id))
          Message = string.Format("{0}{1}Problema no DE --> PARA de grau de instrução!",Message,Environment.NewLine);
      foreach (ViewIntegrationMapOfV1 item in Companys)
        if (string.IsNullOrEmpty(item.Id))
          Message = string.Format("{0}{1}Problema no DE --> PARA de empresa!", Message, Environment.NewLine);
      foreach (ViewIntegrationMapOfV1 item in Establishments)
        if (string.IsNullOrEmpty(item.Id))
          Message = string.Format("{0}{1}Problema no DE --> PARA de estabelecimento!", Message, Environment.NewLine);
      foreach (ViewIntegrationMapOfV1 item in Occupations)
        if (string.IsNullOrEmpty(item.Id))
          Message = string.Format("{0}{1}Problema no DE --> PARA de cargo!", Message, Environment.NewLine);

      if (string.IsNullOrEmpty(Message))
        UpdatePerson();
    }
    private ViewIntegrationMapOfV1 VerifyMapOf(List<ViewIntegrationMapOfV1> lista, EnumValidKey key, string code, string name)
    {
      // Validar Empresa
      ViewIntegrationMapOfV1 search = lista.Find(p => p.Name == name);
      ValidMapOf validMapOf = null;
      if (search == null)
      {
        validMapOf = new ValidMapOf(Person, key, code, name, string.Empty);
        lista.Add(validMapOf.Map);
      }
      else
      {
        if (string.IsNullOrEmpty(search.Id))
        {
          validMapOf = new ValidMapOf(Person, key, code, name, string.Empty);
          lista.Find(p => p.Name == name).Id = validMapOf.Map.Id;
          lista.Find(p => p.Name == name).Message = string.Empty;
        }
        else
          return search;
      }
      return validMapOf.Map;
    }
    private ViewIntegrationMapOfV1 VerifyMapOf(List<ViewIntegrationMapOfV1> lista, EnumValidKey key, string code, string name, string idCompany)
    {
      // Validar Empresa
      ViewIntegrationMapOfV1 search = lista.Find(p => p.Name == name && p.IdCompany == idCompany);
      ValidMapOf validMapOf = null;
      if (search == null)
      {
        validMapOf = new ValidMapOf(Person, key, code, name, idCompany);
        lista.Add(validMapOf.Map);
      }
      else
      {
        if (string.IsNullOrEmpty(search.Id))
        {
          validMapOf = new ValidMapOf(Person, key, code, name, idCompany);
          lista.Find(p => p.Name == name).Id = validMapOf.Map.Id;
        }
        else
          return search;
      }
      return validMapOf.Map;
    }
    private void UpdatePerson()
    {
      foreach (MapPerson collaborator in Collaborators)
      {
        // Novo Colaborador
        int situacao = 0;
        switch (collaborator.Colaborador.Situacao.ToLower())
        {
          case "férias":
            situacao = 3;
            break;
          case "afastado":
            situacao = 2;
            break;
          case "demitido":
            situacao = 2;
            break;
          default:
            situacao = 0;
            break;
        }
        if (collaborator.Person == null)
        {
          PersonIntegration personIntegration = new PersonIntegration(Person);
          ViewIntegrationPersonV1 newPerson = new ViewIntegrationPersonV1()
          {
            Name = collaborator.Colaborador.Nome,
            Mail = collaborator.Colaborador.Email,
            Document = collaborator.Colaborador.Documento,
            DateBirth = collaborator.Colaborador.DataNascimento,
            Phone = collaborator.Colaborador.Celular,
            PhoneFixed = collaborator.Colaborador.Telefone,
            DocumentID = collaborator.Colaborador.Identidade,
            DocumentCTPF = collaborator.Colaborador.CarteiraProfissional,
            Sex = collaborator.Colaborador.Sexo,
            Schooling = collaborator.Schooling,
            Contract = new ViewIntegrationContractV1()
            {
              Document = collaborator.Colaborador.Documento,
              Company  = collaborator.Company,
              Registration = collaborator.Colaborador.Matricula,
              Establishment = collaborator.Establishment,
              DateAdm = collaborator.Colaborador.DataAdmissao,
              StatusUser = situacao,
              HolidayReturn = collaborator.Colaborador.DataRetornoFerias,
              MotiveAside = collaborator.Colaborador.MotivoAfastamento,
              DateResignation =  collaborator.Colaborador.DataDemissao,
              Occupation = collaborator.Occupation,
              DateLastOccupation =  collaborator.Colaborador.DataUltimaTrocaCargo,
              Salary = collaborator.Colaborador.SalarioNominal,
              DateLastReadjust = collaborator.Colaborador.DataUltimoReajuste,
              TypeUser = 3,
              TypeJourney = 0,
              _IdManager = string.Empty,
              DocumentManager = collaborator.Colaborador.DocumentoChefe,
              CompanyManager = collaborator.CompanyManager,
              RegistrationManager = collaborator.Colaborador.MatriculaChefe,
              NameManager = collaborator.Colaborador.NomeChefe
            }
          };
          ViewIntegrationMapPersonV1 viewReturn = personIntegration.PostNewPerson(newPerson);
          int search = Collaborators.FindIndex(p => p.Documento == collaborator.Colaborador.Documento && p.Empresa == collaborator.Colaborador.Empresa && p.Matricula == collaborator.Colaborador.Matricula);
          Collaborators[search].Person = viewReturn.Person;
        }
        if (!collaborator.Colaborador.TestarMudanca(collaborator.ColaboradorAnterior))
        {
          PersonIntegration personIntegration = new PersonIntegration(Person);
          ViewIntegrationPersonV1 changePerson = new ViewIntegrationPersonV1()
          {
            _id = collaborator.Person._id,
            Name = collaborator.Colaborador.Nome,
            Mail = collaborator.Colaborador.Email,
            Document = collaborator.Colaborador.Documento,
            DateBirth = collaborator.Colaborador.DataNascimento,
            Phone = collaborator.Colaborador.Celular,
            PhoneFixed = collaborator.Colaborador.Telefone,
            DocumentID = collaborator.Colaborador.Identidade,
            DocumentCTPF = collaborator.Colaborador.CarteiraProfissional,
            Sex = collaborator.Colaborador.Sexo,
            Schooling = collaborator.Schooling,
            Contract = new ViewIntegrationContractV1()
            {
              Document = collaborator.Colaborador.Documento,
              Company = collaborator.Company,
              Registration = collaborator.Colaborador.Matricula,
              Establishment = collaborator.Establishment,
              DateAdm = collaborator.Colaborador.DataAdmissao,
              StatusUser = situacao,
              HolidayReturn = collaborator.Colaborador.DataRetornoFerias,
              MotiveAside = collaborator.Colaborador.MotivoAfastamento,
              DateResignation = collaborator.Colaborador.DataDemissao,
              Occupation = collaborator.Occupation,
              DateLastOccupation = collaborator.Colaborador.DataUltimaTrocaCargo,
              Salary = collaborator.Colaborador.SalarioNominal,
              DateLastReadjust = collaborator.Colaborador.DataUltimoReajuste,
              TypeUser = 3,
              TypeJourney = 0,
              _IdManager = string.Empty,
              DocumentManager = collaborator.Colaborador.DocumentoChefe,
              CompanyManager = collaborator.CompanyManager,
              RegistrationManager = collaborator.Colaborador.MatriculaChefe,
              NameManager = collaborator.Colaborador.NomeChefe
            }
          };
          ViewIntegrationMapPersonV1 viewReturn = personIntegration.PostNewPerson(changePerson);
          int search = Collaborators.FindIndex(p => p.Documento == collaborator.Colaborador.Documento && p.Empresa == collaborator.Colaborador.Empresa && p.Matricula == collaborator.Colaborador.Matricula);
          Collaborators[search].Person = viewReturn.Person;
          Collaborators[search].ColaboradorAnterior = Collaborators[search].Colaborador;
        }
      }
      string saveObject = JsonConvert.SerializeObject(Collaborators);
      FileClass.WriteToBinaryFile<string>(string.Format("{0}/integration/colaborador.txt", Person.IdAccount), saveObject, false);
      Message = "Fim de integração!";
    }
  }
}
