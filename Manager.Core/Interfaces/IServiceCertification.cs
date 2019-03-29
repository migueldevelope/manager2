using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Enumns;
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
    string RemovePerson(string idcertifcation, string idcertificationperson);
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetAttachment(string idcertification, string url, string fileName, string attachmentid);
    string RemoveCertification(string idcertification);


    List<ViewListBasePerson> ListPersons(string idcertification, ref long total, string filter, int count, int page);
    ViewCertificationProfile GetProfile(string idperson);
    ViewCrudCertification NewCertification(CertificationItem item, string idperson);
    string AddPerson(string idcertification, ViewListBasePerson person);
    string ApprovedCertification(string idcertificationperson, ViewCrudCertificationPerson view);
    string UpdateCertification(ViewCrudCertification view, string idperson, string idcertification);
    List<ViewListCertification> GetListExclud(ref long total, string filter, int count, int page);
    List<ViewCertification> ListCertificationsWaitPerson(string idperson, ref long total, string filter, int count, int page);
    ViewListCertification CertificationsWaitPerson(string idcertification);
    string UpdateStatusCertification(ViewCertificationStatus viewcertification, string idperson);
    List<ViewCertificationItem> ListCertificationPerson(string idperson, ref long total, string filter, int count, int page);
    #endregion

    #region Old
    List<BaseFields> ListPersonsOld(string idcertification, ref long total, string filter, int count, int page);
    ViewCertificationProfile GetProfileOld(string idperson);
    Certification NewCertificationOld(CertificationItem item, string idperson);
    string AddPersonOld(string idcertification, BaseFields person);
    string ApprovedCertificationOld(string idcertificationperson, CertificationPerson view);
    string UpdateCertificationOld(Certification certification, string idperson, string idmonitoring);
    List<Certification> GetListExcludOld(ref long total, string filter, int count, int page);
    Certification CertificationsWaitPersonOld(string idcertification);

    #endregion

  }
}
