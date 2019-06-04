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
    Task<string> DeletePerson(string idcertifcation, string idcertificationperson);
    Task SetAttachment(string idcertification, string url, string fileName, string attachmentid);
    Task<string> DeleteCertification(string idcertification);


    Task<List<ViewListPerson>> ListPersons(string idcertification,  string filter, int count, int page);
    Task<ViewListCertificationProfile> GetProfile(string idperson);
    Task<ViewCrudCertification> NewCertification(ViewListCertificationItem item, string idperson);
    Task<string> AddPerson(string idcertification, ViewListPerson person);
    Task<string> ApprovedCertification(string idcertificationperson, ViewCrudCertificationPerson view);
    Task<string> UpdateCertification(ViewCrudCertification view, string idperson, string idcertification);
    Task<List<ViewListCertification>> ListEnded( string filter, int count, int page);
    Task<List<ViewListCertificationPerson>> ListCertificationsWaitPerson(string idperson,  string filter, int count, int page);
    Task<ViewCrudCertification> CertificationsWaitPerson(string idcertification);
    Task<string> UpdateStatusCertification(ViewCrudCertificationPersonStatus viewcertification, string idperson);
    Task<List<ViewListCertificationItem>> ListCertificationPerson(string idperson,  string filter, int count, int page);
  }
}
