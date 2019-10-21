using Manager.Core.Base;
using Manager.Views.Enumns;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business.Integration
{
  public class PayrollEmployee : BaseEntity
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idPrevious { get; set; }
    public string Key { get; set; }
    public DateTime DateRegister { get; set; }
    public EnumActionIntegration Action { get; set; }
    public EnumStatusIntegration StatusIntegration { get; set; }
    // Cpf
    public string Document { get; set; }
    // Empresa
    public string Company { get; set; }
    // NomeEmpresa
    public string CompanyName { get; set; }
    // Estabelecimento
    public string Establishment { get; set; }
    // NomeEstabelecimento
    public string EstablishmentName { get; set; }
    // Matricula
    public string Registration { get; set; }
    // Nome
    public string Name { get; set; }
    // Email
    public string Mail { get; set; }
    // Sexo
    public EnumSex Sex { get; set; }
    // Data de Nascimento
    public DateTime? BirthDate { get; set; }
    // CellNumber
    public string CellNumber { get; set; }
    // GrauInstrucao
    public string Schooling { get; set; }
    // NomeGrauInstrucao
    public string SchoolingName { get; set; }
    // Apelido
    public string Nickname { get; set; }
    // Data de Admissão
    public DateTime AdmissionDate { get; set; }
    // Data de Demissão
    public DateTime? DemissionDate { get; set; }
    // Cargo
    public string Occupation { get; set; }
    // NomeCargo
    public string OccupationName { get; set; }
    // DataTrocaCargo
    public DateTime? OccupationChangeDate { get; set; }
    // CentroCusto
    public string CostCenter { get; set; }
    // NomeCentroCusto
    public string CostCenterName { get; set; }
    // DataTrocaCentroCusto
    public DateTime? CostCenterChangeDate { get; set; }
    // SalarioNominal
    public decimal Salary { get; set; }
    // CargaHoraria
    public int Workload { get; set; }
    // DataUltimoReajuste
    public DateTime? SalaryChangeDate { get; set; }
    // MotivoUltimoReajuste
    public string SalaryChangeReason { get; set; }
    // Situacao
    public EnumStatusUser StatusUser { get; set; }
    // CpfGestor
    public string ManagerDocument { get; set; }
    // EmpresaGestor
    public string ManagerCompany { get; set; }
    // NomeEmpresaGestor
    public string ManagerCompanyName { get; set; }
    // EstabelecimentoGestor
    public string ManagerEstablishment { get; set; }
    // NomeEstabelecimentoGestor
    public string ManagerEstablishmentName { get; set; }
    // MatriculaGestor
    public string ManagerRegistration { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string IdUser { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string IdContract { get; set; }
    public List<string> Messages { get; set; }
  }
}
