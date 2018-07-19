using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Views;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Core.Interfaces
{
  public interface IServiceInfra
  {
    void SetUser(IHttpContextAccessor contextAccessor);
    List<ViewOccupationGroupCareer> GetViewOccupationGroupCareers(string idcompany, EnumTypeAxis type, EnumTypeCareer careerType);
    List<ViewCareer> GetCareer(string idcompany);
    List<ViewCareer> ListGetCareer();
    List<ViewOccupationGroupCareer> GetOccupationGroupCareersEdit(string idcompany, string id, EnumTypeCareer careerType);
    List<ViewOccupationGroupCareer> GetOccupationGroupCareersList(string idcompany, EnumTypeCareer careerType);
    List<ViewOccupationLine> GetOccupationList(string idcompany);
    ViewOccupationLine GetOccupationEdit(string id);
    List<ViewOccupationGroupCareer> GetOccupationGroupList(string idcompany);
    ViewOccupationGroupCareer GetOccupationGroupEdit(string id);
    long GetMaxPosition(string idcompany, EnumTypeCareer type);
    List<ViewHeadInfraSphere> GetHeadSphere(string idcompany, EnumTypeCareer type);
    List<ViewHeadInfraAxis> GetHeadAxis(string idcompany, EnumTypeCareer type);
    List<ViewArea> GetArea(string idcompany);
    List<ViewAxis> GetAxis(string idcompany);
    List<ViewSphere> GetSphere(string idcompany);
    List<ViewLists> GetDictionary(EnumTypeSphere type);
    List<ViewOccupationLine> GetLinesOccuation(string idcompany);
    ViewOccupationLine GetLinesOccuationEdit(string idcompany, string id);
    void NewSphere(ViewSphereNew view);
    void UpdateSphere(ViewSphereNew view, string id);
    void NewAxis(ViewAxisNew view);
    void UpdateAxis(ViewAxisNew view, string id);
    void NewCareer(ViewCareerNew view);
    void UpdateCompany(Career career, string idcompany);
    void UpdateCareer(ViewCareerNew view, string id);
    ViewArea NewArea(ViewAreaNew view);
    void UpdateArea(ViewAreaNew view, string id);
    void NewDictionary(ViewDictionarySphereNew view);
    void UpdateDictionary(ViewDictionarySphereNew view, string id);
    void NewOccupationGroupCareer(ViewOccupationGroupCareerNew view, EnumTypeCareer type);
    void ReorderPosition(EnumTypeAxis typeAxis, long position);
    void UpdateOccupationGroupCareer(ViewOccupationGroupCareerNew view, EnumTypeCareer type, string id);
    void UpdatePosition(ViewCareerPosition view, EnumTypeCareer type, string id);
    string DeleteOccupationGroupCareer(string id);
    string DisconnectOccupationGroupCareer(string id, EnumTypeCareer careerType);
    string NewOccupationLine(ViewOccupationLineNew view);
    string DeleteArea(string id);
    string UpdateOccupationLine(ViewOccupationLineNew view, string id);
    string DeleteOccupationLine(string id);
    string DisconnectOccupationLine(string id);
    void UpdatePositionLine(ViewCareerPosition view, string id);
  }
}
