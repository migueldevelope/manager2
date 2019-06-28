using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Interfaces
{
  public interface IServiceCertification
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    void SetUser(BaseUser user);
     string DeletePerson(string idcertifcation, string idcertificationperson);
    void SetAttachment(string idcertification, string url, string fileName, string attachmentid);
     string DeleteCertification(string idcertification);


     List<ViewListPerson> ListPersons(string idcertification, ref  long total,  string filter, int count, int page);
     ViewListCertificationProfile GetProfile(string idperson);
     ViewCrudCertification NewCertification(ViewListCertificationItem item, string idperson);
     string AddPerson(string idcertification, ViewListPerson person);
     string ApprovedCertification(string idcertificationperson, ViewCrudCertificationPerson view);
     string UpdateCertification(ViewCrudCertification view, string idperson, string idcertification);
     List<ViewListCertification> ListEnded(ref  long total,  string filter, int count, int page);
     List<ViewListCertificationPerson> ListCertificationsWaitPerson(string idperson, ref  long total,  string filter, int count, int page);
     ViewCrudCertification CertificationsWaitPerson(string idcertification);
     string UpdateStatusCertification(ViewCrudCertificationPersonStatus viewcertification, string idperson);
     List<ViewListCertificationItem> ListCertificationPerson(string idperson, ref  long total,  string filter, int count, int page);
  }
}
