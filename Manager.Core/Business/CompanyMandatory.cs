using Manager.Core.Base;
using Manager.Core.Enumns;
using Manager.Views.Enumns;
using System;

namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados para treinamentos obrigatórios da empresa
  /// </summary>
  public class CompanyMandatory: BaseEntity
  {
    public Course Course { get; set; }
    public Company Company { get; set; }
    public DateTime? BeginDate { get; set; }
    public EnumTypeMandatoryTraining TypeMandatoryTraining { get; set; }
  }
}
