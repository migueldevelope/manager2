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
    private List<ViewIntegrationMapManagerV1> Managers;
    public ImportService(ViewPersonLogin person, List<Colaborador> lista)
    {
      try
      {
        Person = person;
        Lista = lista;
        LoadLists();
        Import();
      }
      catch (Exception)
      {

        throw;
      }
    }
    private void LoadLists()
    {
      try
      {
        Companys = new List<ViewIntegrationMapOfV1>();
        if (File.Exists(string.Format("{0}/empresa.txt", Person.IdAccount)))
         Companys = FileClass.ReadFromBinaryFile<List<ViewIntegrationMapOfV1>>(string.Format("{0}/empresa.txt", Person.IdAccount));
        Schoolings = new List<ViewIntegrationMapOfV1>();
        if (File.Exists(string.Format("{0}/grau.txt", Person.IdAccount)))
          Schoolings = FileClass.ReadFromBinaryFile<List<ViewIntegrationMapOfV1>>(string.Format("{0}/grau.txt", Person.IdAccount));
        Establishments = new List<ViewIntegrationMapOfV1>();
        if (File.Exists(string.Format("{0}/estabelecimento.txt", Person.IdAccount)))
          Establishments = FileClass.ReadFromBinaryFile<List<ViewIntegrationMapOfV1>>(string.Format("{0}/estabelecimento.txt", Person.IdAccount));
        Occupations = new List<ViewIntegrationMapOfV1>();
        if (File.Exists(string.Format("{0}/cargo.txt", Person.IdAccount)))
          Occupations = FileClass.ReadFromBinaryFile<List<ViewIntegrationMapOfV1>>(string.Format("{0}/cargo.txt", Person.IdAccount));
        Collaborators = new List<MapPerson>();
        if (File.Exists(string.Format("{0}/colaborador.txt", Person.IdAccount)))
          Collaborators = FileClass.ReadFromBinaryFile<List<MapPerson>>(string.Format("{0}/colaborador.txt", Person.IdAccount));
        Managers = new List<ViewIntegrationMapManagerV1>();
        if (File.Exists(string.Format("{0}/gestor.txt", Person.IdAccount)))
          Managers = FileClass.ReadFromBinaryFile<List<ViewIntegrationMapManagerV1>>(string.Format("{0}/gestor.txt", Person.IdAccount));
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
        MapPerson search = Collaborators.Find(p => p.Documento == colaborador.Documento && p.Empresa == colaborador.Empresa && p.Matricula == colaborador.Matricula);
        if (search == null)
        {
          ValidMapCollaborator validMapCollaborator = new ValidMapCollaborator(Person, colaborador.Documento, company, colaborador.Matricula, colaborador.Nome);
          Collaborators.Add(validMapCollaborator.Map);
        }
        // Testar se tem gestor
        if (!string.IsNullOrEmpty(colaborador.DocumentoChefe) && !string.IsNullOrEmpty(colaborador.EmpresaChefe) && !string.IsNullOrEmpty(colaborador.MatriculaChefe))
        {
          ViewIntegrationMapManagerV1 manager = Managers.Find(p => p.Document == colaborador.DocumentoChefe && p.CompanyCode == colaborador.EmpresaChefe && p.Registration == colaborador.MatriculaChefe);
          if (manager == null)
          {
            // Não tem esta matricula como gestor
            ValidMapManager validMapManager = new ValidMapManager(Person, colaborador.DocumentoChefe, colaborador.EmpresaChefe, colaborador.MatriculaChefe, colaborador.NomeChefe);
            Managers.Add(validMapManager.Map);
          }
        }
      }

      string saveObject = JsonConvert.SerializeObject(Schoolings);
      FileClass.WriteToBinaryFile<string>(string.Format("{0}/grau.txt", Person.IdAccount), saveObject, false);
      saveObject = JsonConvert.SerializeObject(Companys);
      FileClass.WriteToBinaryFile<string>(string.Format("{0}/empresa.txt", Person.IdAccount), saveObject, false);
      saveObject = JsonConvert.SerializeObject(Establishments);
      FileClass.WriteToBinaryFile<string>(string.Format("{0}/estabelecimentos.txt", Person.IdAccount), saveObject, false);
      saveObject = JsonConvert.SerializeObject(Occupations);
      FileClass.WriteToBinaryFile<string>(string.Format("{0}/cargo.txt", Person.IdAccount), saveObject, false);
      saveObject = JsonConvert.SerializeObject(Managers);
      FileClass.WriteToBinaryFile<string>(string.Format("{0}/gestor.txt", Person.IdAccount), saveObject, false);
      saveObject = JsonConvert.SerializeObject(Collaborators);
      FileClass.WriteToBinaryFile<string>(string.Format("{0}/colaborador.txt", Person.IdAccount), saveObject, false);

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
        }
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
      }
      return validMapOf.Map;
    }
    private void UpdatePerson()
    {
      foreach (Colaborador colaborador in Lista)
      {

      }
    }
  }
}
