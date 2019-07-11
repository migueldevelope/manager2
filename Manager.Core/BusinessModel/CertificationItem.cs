using Manager.Core.Base;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Manager.Core.BusinessModel
{
  /// <summary>
  /// Coleção para o item da acreditação
  /// </summary>
  public class CertificationItem : BaseEntityId
  {
    public EnumItemCertification ItemCertification { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string IdItem { get; set; }
    public string Name { get; set; }
    public string Concept { get; set; }
    public ViewListCertificationItem GetViewList()
    {
      return new ViewListCertificationItem()
      {
        _id = _id,
        IdItem = IdItem,
        Concept = Concept,
        ItemCertification = ItemCertification,
        Name = Name,
        ItemCertificationView = ItemCertification == EnumItemCertification.SkillCompanyHard ? EnumItemCertificationView.Company :
          ItemCertification == EnumItemCertification.SkillCompanySoft ? EnumItemCertificationView.Company :
          ItemCertification == EnumItemCertification.SkillGroupHard ? EnumItemCertificationView.Hard :
          ItemCertification == EnumItemCertification.SkillOccupationHard ? EnumItemCertificationView.Hard : EnumItemCertificationView.Soft
      };
    }
  }
}
