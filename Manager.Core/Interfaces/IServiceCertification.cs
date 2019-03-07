﻿using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Views;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manager.Core.Interfaces
{
  public interface IServiceCertification
  {
    List<BaseFields> ListPersons(string idcertification, ref long total, string filter, int count, int page);
    ViewCertificationProfile GetProfile(string idperson);
    Certification NewCertification(CertificationItem item, string idperson);
    string AddPerson(string idcertification, BaseFields person);
    string ApprovedCertification(string idcertificationperson, CertificationPerson view);
    string RemovePerson(string idcertifcation, string idcertificationperson);
    string UpdateCertification(Certification certification, string idperson, string idmonitoring);
    string RemoveCertification(string idcertification);
    List<Certification> GetListExclud(ref long total, string filter, int count, int page);
    void SetUser(IHttpContextAccessor contextAccessor);
    List<ViewCertification> ListCertificationsWaitPerson(string idperson, ref long total, string filter, int count, int page);
    Certification CertificationsWaitPerson(string idcertification);
    void SetAttachment(string idcertification, string url, string fileName, string attachmentid);
    string UpdateStatusCertification(ViewCertificationStatus viewcertification, string idperson);
    List<ViewCertificationItem> ListCertificationPerson(string idperson, ref long total, string filter, int count, int page);
  }
}