using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;

namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados para treinamentos obrigatórios da empresa
  /// </summary>
  public class CompanyMandatory : BaseEntity
  {
    public ViewListCourse Course { get; set; }
    public ViewListCompany Company { get; set; }
    public DateTime? BeginDate { get; set; }
    public EnumTypeMandatoryTraining TypeMandatoryTraining { get; set; }
    public ViewCrudCompanyMandatory GetViewCrud()
    {
      return new ViewCrudCompanyMandatory()
      {
        BeginDate = BeginDate,
        Company = Company,
        Course = Course,
        Name = Company.Name,
        TypeMandatoryTraining = TypeMandatoryTraining,
        _id = _id
      };
    }
  }
}
