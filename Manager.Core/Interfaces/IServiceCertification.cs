using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Views;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceCertification
  {
    #region Certification
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);

    string DeletePerson(string idcertifcation, string idcertificationperson);
    void SetAttachment(string idcertification, string url, string fileName, string attachmentid);
    string DeleteCertification(string idcertification);


    List<ViewListPerson> ListPersons(string idcertification, ref long total, string filter, int count, int page);
    ViewListCertificationProfile GetProfile(string idperson);
    ViewCrudCertification NewCertification(ViewListCertificationItem item, string idperson);
    string AddPerson(string idcertification, ViewListPerson person);
    string ApprovedCertification(string idcertificationperson, ViewCrudCertificationPerson view);
    string UpdateCertification(ViewCrudCertification view, string idperson, string idcertification);
    List<ViewListCertification> ListEnded(ref long total, string filter, int count, int page);
    List<ViewListCertificationPerson> ListCertificationsWaitPerson(string idperson, ref long total, string filter, int count, int page);
    ViewCrudCertification CertificationsWaitPerson(string idcertification);
    string UpdateStatusCertification(ViewCrudCertificationPersonStatus viewcertification, string idperson);
    List<ViewListCertificationItem> ListCertificationPerson(string idperson, ref long total, string filter, int count, int page);
    #endregion

    #region Old
    List<BaseFields> ListPersonsOld(string idcertification, ref long total, string filter, int count, int page);
    ViewListCertificationProfile GetProfileOld(string idperson);
    Certification NewCertificationOld(CertificationItem item, string idperson);
    string AddPersonOld(string idcertification, BaseFields person);
    string ApprovedCertificationOld(string idcertificationperson, CertificationPerson view);
    string UpdateCertificationOld(Certification certification, string idperson, string idmonitoring);
    List<Certification> GetListExcludOld(ref long total, string filter, int count, int page);
    Certification CertificationsWaitPersonOld(string idcertification);

    #endregion

  }
}
