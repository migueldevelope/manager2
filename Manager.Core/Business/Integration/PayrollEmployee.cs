using Manager.Core.Base;
using Manager.Views.Enumns;
using Manager.Views.Integration.V2;
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
    public ColaboradorV2 GetColaboradorV2()
    {
      return new ColaboradorV2()
      {
        Acao = ActionToString(),
        Apelido = Nickname,
        CargaHoraria = Workload,
        Cargo = Occupation,
        Celular = CellNumber,
        CentroCusto = CostCenter,
        Colaborador = new ColaboradorV2Base()
        {
          Cpf = Document,
          Empresa = Company,
          Estabelecimento = Establishment,
          Matricula = Registration,
          NomeEmpresa = CompanyName,
          NomeEstabelecimento = EstablishmentName
        },
        DataAdmissao = AdmissionDate,
        DataDemissao = DemissionDate,
        DataNascimento = BirthDate,
        DataTrocaCargo = OccupationChangeDate,
        DataTrocaCentroCusto = CostCenterChangeDate,
        DataUltimoReajuste = SalaryChangeDate,
        Email = Mail,
        Gestor = string.IsNullOrEmpty(ManagerRegistration) ? null : new ColaboradorV2Base()
        {
          Cpf = ManagerDocument,
          Empresa = ManagerCompany,
          Estabelecimento = ManagerEstablishment,
          Matricula = ManagerRegistration,
          NomeEmpresa = ManagerCompanyName,
          NomeEstabelecimento = ManagerEstablishmentName
        },
        GrauInstrucao = Schooling,
        Mensagens = Messages,
        MotivoUltimoReajuste = SalaryChangeReason,
        Nome = Name,
        NomeCargo = OccupationName,
        NomeCentroCusto = CostCenterName,
        NomeGrauInstrucao = SchoolingName,
        Registro = DateRegister,
        SalarioNominal = Salary,
        Sexo = SexToString(),
        Situacao = StatusUserToString(),
        SituacaoIntegracao = StatusIntegrationToString(),
        _idContract = IdContract,
        _idPayrollEmployee = _id,
        _idPrevious = _idPrevious,
        _idUser = IdUser
      };
    }
    private string ActionToString()
    {
      switch (Action)
      {
        case EnumActionIntegration.Load:
          return "CARGA";
        case EnumActionIntegration.Admission:
          return "ADMISSÃO";
        case EnumActionIntegration.Change:
          return "ATUALIZAÇÃO";
        case EnumActionIntegration.Demission:
          return "DEMISSÃO";
        case EnumActionIntegration.Validation:
          return "VALIDAÇÃO";
        default:
          return "NÃO DEFINIDA";
      }
    }
    private string StatusUserToString()
    {
      switch (StatusUser)
      {
        case EnumStatusUser.Enabled:
          return "ATIVO";
        case EnumStatusUser.Disabled:
          return "DEMITIDO";
        case EnumStatusUser.Away:
          return "AFASTADO";
        case EnumStatusUser.Vacation:
          return "FÉRIAS";
        default:
          return "NÃO DEFINIDO";
      }

    }
    private string SexToString()
    {
      switch (Sex)
      {
        case EnumSex.Male:
          return "Masculino";
        case EnumSex.Female:
          return "Feminino";
        case EnumSex.Others:
          return "Outros";
        default:
          return "Não definido";
      }
    }
    private string StatusIntegrationToString()
    {
      switch (StatusIntegration)
      {
        case EnumStatusIntegration.Saved:
          return "GRAVADO";
        case EnumStatusIntegration.Reject:
          return "REJEITADO";
        case EnumStatusIntegration.Atualized:
          return "ATUALIZADO";
        default:
          return "NÃO DEFINIDO";
      }
    }
    public bool Equal(PayrollEmployee payrollEmployee)
    {
      try
      {
        if (Key != payrollEmployee.Key)
          return false;
        if (Document != payrollEmployee.Document)
          return false;
        if (Company != payrollEmployee.Company)
            return false;
        if (CompanyName != payrollEmployee.CompanyName)
          return false;
        if (Establishment != payrollEmployee.Establishment)
          return false;
        if (EstablishmentName != payrollEmployee.EstablishmentName)
          return false;
        if (Registration != payrollEmployee.Registration)
          return false;
        if (Name != payrollEmployee.Name)
          return false;
        if (Mail != payrollEmployee.Mail)
          return false;
        if (Sex != payrollEmployee.Sex)
          return false;
        if (BirthDate != payrollEmployee.BirthDate)
          return false;
        if (CellNumber != payrollEmployee.CellNumber)
          return false;
        if (Schooling != payrollEmployee.Schooling)
          return false;
        if (SchoolingName != payrollEmployee.SchoolingName)
          return false;
        if (Nickname != payrollEmployee.Nickname)
          return false;
        if (AdmissionDate != payrollEmployee.AdmissionDate)
          return false;
        if (DemissionDate != payrollEmployee.DemissionDate)
          return false;
        if (Occupation != payrollEmployee.Occupation)
          return false;
        if (OccupationName != payrollEmployee.OccupationName)
          return false;
        if (OccupationChangeDate != payrollEmployee.OccupationChangeDate)
          return false;
        if (CostCenter != payrollEmployee.CostCenter)
          return false;
        if (CostCenterName != payrollEmployee.CostCenterName)
          return false;
        if (CostCenterChangeDate != payrollEmployee.CostCenterChangeDate)
          return false;
        if (Salary != payrollEmployee.Salary)
          return false;
        if (Workload != payrollEmployee.Workload)
          return false;
        if (SalaryChangeDate != payrollEmployee.SalaryChangeDate)
          return false;
        if (SalaryChangeReason != payrollEmployee.SalaryChangeReason)
          return false;
        if (StatusUser != payrollEmployee.StatusUser)
          return false;
        if (ManagerDocument != payrollEmployee.ManagerDocument)
          return false;
        if (ManagerCompany != payrollEmployee.ManagerCompany)
          return false;
        if (ManagerCompanyName != payrollEmployee.ManagerCompanyName)
          return false;
        if (ManagerEstablishment != payrollEmployee.ManagerEstablishment)
          return false;
        if (ManagerEstablishmentName != payrollEmployee.ManagerEstablishmentName)
          return false;
        if (ManagerRegistration != payrollEmployee.ManagerRegistration)
          return false;
        return true;
      }
      catch (Exception)
      {
        throw;
      }
    }
  }
}
