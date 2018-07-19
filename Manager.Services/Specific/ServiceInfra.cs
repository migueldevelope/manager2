using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manager.Services.Specific
{
  public class ServiceInfra : Repository<Career>, IServiceInfra
  {
    private readonly ServiceGeneric<Career> careerService;
    private readonly ServiceGeneric<Sphere> sphereService;
    private readonly ServiceGeneric<DictionarySphere> dictionarySphereService;
    private readonly ServiceGeneric<Axis> axisService;
    private readonly ServiceGeneric<OccupationGroup> occupationGroupService;
    private readonly ServiceGeneric<Occupation> occupationService;
    private readonly ServiceGeneric<Area> areaService;
    private readonly ServiceGeneric<Company> companyService;


    public ServiceInfra(DataContext context)
      : base(context)
    {
      try
      {
        careerService = new ServiceGeneric<Career>(context);
        sphereService = new ServiceGeneric<Sphere>(context);
        dictionarySphereService = new ServiceGeneric<DictionarySphere>(context);
        axisService = new ServiceGeneric<Axis>(context);
        occupationGroupService = new ServiceGeneric<OccupationGroup>(context);
        occupationService = new ServiceGeneric<Occupation>(context);
        areaService = new ServiceGeneric<Area>(context);
        companyService = new ServiceGeneric<Company>(context);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }


    public List<ViewOccupationGroupCareer> GetViewOccupationGroupCareers(string idcompany, EnumTypeAxis type, EnumTypeCareer careerType)
    {
      try
      {
        return occupationGroupService.GetAll(p => p.Company._id == idcompany).ToList()
        .Select(item => new ViewOccupationGroupCareer()
        {
          Id = item._id,
          Name = item.Name,
          Axis = item.Axis.TypeAxis,
          Sphere = item.Sphere.TypeSphere,
          Position = item.Position,
          IdCompany = item.Company._id,
          NameCompany = item.Company.Name
        }).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<ViewCareer> GetCareer(string idcompany)
    {
      try
      {
        return companyService.GetAll(p => p._id == idcompany & p.Career != null).ToList()
                     .Select(item => new ViewCareer
                     {
                       Id = item.Career._id,
                       Name = item.Career.Name,
                       IdCompany = item._id,
                       NameCompany = item.Name,
                       Type = item.Career.Type
                     }).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<ViewCareer> ListGetCareer()
    {
      try
      {
        return companyService.GetAll(p=> p.Career != null).ToList()
                .Select(item => new ViewCareer
                {
                  Id = item.Career._id,
                  Name = item.Career.Name,
                  IdCompany = item._id,
                  NameCompany = item.Name,
                  Type = item.Career.Type
                }).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<ViewOccupationGroupCareer> GetOccupationGroupCareersEdit(string idcompany, string id, EnumTypeCareer careerType)
    {
      try
      {
        return occupationGroupService.GetAll(p => p.Company._id == idcompany
                     & p._id == id & p.Career.Type == careerType).ToList().
                     Select(item => new ViewOccupationGroupCareer
                     {
                       Id = item._id,
                       Name = item.Name,
                       Axis = item.Axis.TypeAxis,
                       Sphere = item.Sphere.TypeSphere,
                       Position = item.Position,
                       IdCompany = item.Company._id,
                       NameCompany = item.Company.Name
                     }).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<ViewOccupationGroupCareer> GetOccupationGroupCareersList(string idcompany, EnumTypeCareer careerType)
    {
      try
      {
        return occupationGroupService.GetAll(p => p.Company._id == idcompany & p.Career.Type == careerType)
                     .ToList().Select(item => new ViewOccupationGroupCareer
                     {
                       Id = item._id,
                       Name = item.Name,
                       Axis = item.Axis.TypeAxis,
                       Sphere = item.Sphere.TypeSphere,
                       Position = item.Position,
                       IdCompany = item.Company._id,
                       NameCompany = item.Company.Name
                     }).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<ViewOccupationLine> GetOccupationList(string idcompany)
    {
      try
      {
        return occupationService.GetAll(p => p.OccupationGroup.Company._id == idcompany).ToList()
          .Select(item => new ViewOccupationLine
          {
            IdOccupation = item._id,
            NameOccupation = item.Name,
            Sphere = item.OccupationGroup.Sphere.TypeSphere,
            Position = item.Position,
            IdCompany = item.OccupationGroup.Company._id,
            NameCompany = item.OccupationGroup.Company.Name,
            IdArea = item.Area._id,
            IdOccupationGroup = item.OccupationGroup._id,
            NameArea = item.Area.Name,
            NameOccupationGroup = item.OccupationGroup.Name
          }).OrderBy(p => p.NameOccupation).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public ViewOccupationLine GetOccupationEdit(string id)
    {
      try
      {
        return occupationService.GetAll(p => p._id == id).ToList()
        .Select(item => new ViewOccupationLine
        {
          IdOccupation = item._id,
          NameOccupation = item.Name,
          Sphere = item.OccupationGroup.Sphere.TypeSphere,
          Position = item.Position,
          IdCompany = item.OccupationGroup.Company._id,
          NameCompany = item.OccupationGroup.Company.Name,
          IdArea = item.Area._id,
          IdOccupationGroup = item.OccupationGroup._id,
          NameArea = item.Area.Name,
          NameOccupationGroup = item.OccupationGroup.Name
        }).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<ViewOccupationGroupCareer> GetOccupationGroupList(string idcompany)
    {
      try
      {
        return occupationGroupService.GetAll(p => p._id == idcompany).OrderBy(p => p.Axis).ThenBy(p => p.Name).ToList()
        .Select(item => new ViewOccupationGroupCareer
        {
          Id = item._id,
          Name = item.Name,
          Axis = item.Axis.TypeAxis,
          Sphere = item.Sphere.TypeSphere,
          Position = item.Position,
          IdCompany = item.Company._id,
          NameCompany = item.Company.Name
        }).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public ViewOccupationGroupCareer GetOccupationGroupEdit(string id)
    {
      try
      {
        return occupationGroupService.GetAll(p => p._id == id).ToList()
        .Select(item => new ViewOccupationGroupCareer
        {
          Id = item._id,
          Name = item.Name,
          Axis = item.Axis.TypeAxis,
          Sphere = item.Sphere.TypeSphere,
          Position = item.Position,
          IdCompany = item.Company._id,
          NameCompany = item.Company.Name
        }).FirstOrDefault();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public long GetMaxPosition(string idcompany, EnumTypeCareer type)
    {
      try
      {
        var company = this.companyService.GetAll(p => p._id == idcompany).FirstOrDefault();
        var max = this.occupationGroupService.GetAll(p => p.Company == company).Max(p => p.Position);
        return max;
      }
      catch (Exception)
      {
        //throw new ServiceException(_user, e, this._context);
        return 0;
      }
    }

    public List<ViewHeadInfraSphere> GetHeadSphere(string idcompany, EnumTypeCareer type)
    {
      try
      {
        return sphereService.GetAll(p => p.Company._id == idcompany)
                    .ToList().Select(sphere => new ViewHeadInfraSphere
                    {
                      Type = sphere.TypeSphere,
                      Name = sphere.Name
                    }).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<ViewHeadInfraAxis> GetHeadAxis(string idcompany, EnumTypeCareer type)
    {
      try
      {
        return axisService.GetAll(p => p.Company._id == idcompany).ToList()
                    .Select(axis => new ViewHeadInfraAxis
                    {
                      Type = axis.TypeAxis,
                      Name = axis.Name
                    }).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<ViewArea> GetArea(string idcompany)
    {
      try
      {
        return areaService.GetAll(p => p.Company._id == idcompany).OrderBy(p => p.Name)
        .ToList().Select(item => new ViewArea
        {
          Id = item._id,
          Name = item.Name
        }).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<ViewAxis> GetAxis(string idcompany)
    {
      try
      {
        return axisService.GetAll(p => p.Company._id == idcompany).OrderBy(p => p.Name)
          .ToList().Select(item => new ViewAxis
          {
            Id = item._id,
            Name = item.Name,
            Type = item.TypeAxis
          }).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<ViewSphere> GetSphere(string idcompany)
    {
      try
      {
        return sphereService.GetAll(p => p.Company._id == idcompany)
          .ToList().Select(item => new ViewSphere
          {
            Id = item._id,
            Name = item.Name,
            Type = item.TypeSphere
          }).OrderBy(p => p.Name).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<ViewLists> GetDictionary(EnumTypeSphere type)
    {
      try
      {
        return dictionarySphereService.GetAll(p => p.Type == type).ToList()
        .Select(item => new ViewLists
        {
          Id = item._id,
          Name = item.Name
        }).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<ViewOccupationLine> GetLinesOccuation(string idcompany)
    {
      try
      {
        return occupationService.GetAll(p => p.OccupationGroup.Company._id == idcompany).ToList()
        .Select(item => new ViewOccupationLine
        {
          IdArea = item.Area._id,
          IdOccupation = item._id,
          NameArea = item.Area.Name,
          NameOccupation = item.Name,
          Position = item.Position,
          Sphere = item.OccupationGroup.Sphere.TypeSphere,
          IdCompany = item.OccupationGroup.Company._id,
          NameCompany = item.OccupationGroup.Company.Name,
          IdOccupationGroup = item.OccupationGroup._id,
          NameOccupationGroup = item.OccupationGroup.Name
        }).OrderBy(p => p.NameArea).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public ViewOccupationLine GetLinesOccuationEdit(string idcompany, string id)
    {
      try
      {
        return occupationService.GetAll(p => p._id == id
        & p.OccupationGroup.Company._id == idcompany).ToList()
        .Select(item => new ViewOccupationLine()
        {
          IdArea = item.Area._id,
          IdOccupation = item._id,
          NameArea = item.Area.Name,
          NameOccupation = item.Name,
          Position = item.Position,
          Sphere = item.OccupationGroup.Sphere.TypeSphere,
          IdCompany = item.OccupationGroup.Company._id,
          NameCompany = item.OccupationGroup.Company.Name,
          IdOccupationGroup = item.OccupationGroup._id,
          NameOccupationGroup = item.OccupationGroup.Name
        }).FirstOrDefault();

      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void NewSphere(ViewSphereNew view)
    {
      try
      {
        var company = companyService.GetAll(p => p._id == view.IdCompany).FirstOrDefault();
        Sphere model = new Sphere()
        {
          Name = view.Name,
          Company = company,
          TypeSphere = view.Type,
          Status = EnumStatus.Enabled
        };

        this.sphereService.Insert(model);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void UpdateSphere(ViewSphereNew view, string id)
    {
      try
      {
        var company = companyService.GetAll(p => p._id == view.IdCompany).FirstOrDefault();

        var model = this.sphereService.GetAll(p => p._id == id).FirstOrDefault();
        model.Name = view.Name;
        model.Company = company;
        model.TypeSphere = view.Type;

        this.sphereService.Update(model, null);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void NewAxis(ViewAxisNew view)
    {
      try
      {
        var company = companyService.GetAll(p => p._id == view.IdCompany).FirstOrDefault();
        Axis model = new Axis()
        {
          Name = view.Name,
          Company = company,
          TypeAxis = view.Type,
          Status = EnumStatus.Enabled
        };

        this.axisService.Insert(model);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void UpdateAxis(ViewAxisNew view, string id)
    {
      try
      {
        var company = companyService.GetAll(p => p._id == view.IdCompany).FirstOrDefault();

        var model = this.axisService.GetAll(p => p._id == id).FirstOrDefault();
        model.Name = view.Name;
        model.Company = company;
        model.TypeAxis = view.Type;

        this.axisService.Update(model, null);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void NewCareer(ViewCareerNew view)
    {
      try
      {

        Career model = new Career()
        {
          Name = view.Name,
          Type = view.Type,
          Status = EnumStatus.Enabled
        };

        this.careerService.Insert(model);
        UpdateCompany(model, view.IdCompany);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public async void UpdateCompany(Career career, string idcompany)
    {
      var company = companyService.GetAll(p => p._id == idcompany).FirstOrDefault();
      company.Career = career;
      companyService.Update(company, null);
    }

    public void UpdateCareer(ViewCareerNew view, string id)
    {
      try
      {
        var model = this.careerService.GetAll(p => p._id == id).FirstOrDefault();
        model.Name = view.Name;
        model.Type = view.Type;

        this.careerService.Update(model, null);
        UpdateCompany(model, view.IdCompany);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public ViewArea NewArea(ViewAreaNew view)
    {
      try
      {
        var company = companyService.GetAll(p => p._id == view.IdCompany).FirstOrDefault();
        Area model = new Area()
        {
          Name = view.Name,
          Company = company,
          Status = EnumStatus.Enabled
        };

        var id = this.areaService.Insert(model)._id;
        return new ViewArea() { Id = id, Name = view.Name };
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void UpdateArea(ViewAreaNew view, string id)
    {
      try
      {
        var company = companyService.GetAll(p => p._id == view.IdCompany).FirstOrDefault();
        var model = this.areaService.GetAll(p => p._id == id).FirstOrDefault();
        model.Name = view.Name;
        model.Company = company;

        this.areaService.Update(model, null);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void NewDictionary(ViewDictionarySphereNew view)
    {
      try
      {
        DictionarySphere model = new DictionarySphere()
        {
          Name = view.Name,
          Type = view.Type,
          Status = EnumStatus.Enabled
        };

        this.dictionarySphereService.Insert(model);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void UpdateDictionary(ViewDictionarySphereNew view, string id)
    {
      try
      {
        var model = this.dictionarySphereService.GetAll(p => p._id == id).FirstOrDefault();
        model.Name = view.Name;
        model.Type = view.Type;

        this.dictionarySphereService.Update(model, null);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void NewOccupationGroupCareer(ViewOccupationGroupCareerNew view, EnumTypeCareer type)
    {
      try
      {
        var sphere = this.sphereService.GetAll(p => p.TypeSphere == view.TypeSphere).FirstOrDefault();
        var axis = this.axisService.GetAll(p => p.TypeAxis == view.TypeAxis).FirstOrDefault();
        var career = this.careerService.GetAll(p => p.Type == type).FirstOrDefault();

        long position = 0;
        IQueryable<OccupationGroup> positionMax;

        if (view.TypeSphere == EnumTypeSphere.Tactical)
        {
          positionMax = this.occupationGroupService.GetAll(p => p.Sphere == sphere & p.Axis == axis & p.Career == career);
          if (positionMax.Count() > 0)
            position = positionMax.Max(p => p.Position) + 1;

          ReorderPosition(view.TypeAxis, position);
        }
        else
        {
          positionMax = this.occupationGroupService.GetAll(p => p.Sphere == sphere & p.Axis == axis & p.Career == career);
          if (positionMax.Count() > 0)
            position = positionMax.Max(p => p.Position) + 1;
          else
            position = 1;
        }

        var model = new OccupationGroup()
        {
          Company = axis.Company,
          Career = career,
          Name = view.NameOccupationGroup,
          Axis = axis,
          Sphere = sphere,
          Position = position,
          Status = EnumStatus.Enabled
        };
        this.occupationGroupService.Insert(model);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void ReorderPosition(EnumTypeAxis typeAxis, long position)
    {
      try
      {
        if (position > 0)
        {
          var list = this.occupationGroupService.GetAll(p => p.Axis.TypeAxis == typeAxis & p.Position >= position).ToList();
          foreach (var item in list)
          {
            item.Position += 1;
            this.occupationGroupService.Update(item, null);
          }
        }
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void UpdateOccupationGroupCareer(ViewOccupationGroupCareerNew view, EnumTypeCareer type, string id)
    {
      try
      {
        var sphere = this.sphereService.GetAll(p => p.TypeSphere == view.TypeSphere).FirstOrDefault();
        var axis = this.axisService.GetAll(p => p.TypeAxis == view.TypeAxis).FirstOrDefault();
        var career = this.careerService.GetAll(p => p.Type == type).FirstOrDefault();


        var model = this.occupationGroupService.GetAll(p => p._id == id).FirstOrDefault();
        model.Company = axis.Company;
        model.Career = career;
        model.Name = view.NameOccupationGroup;
        model.Sphere = sphere;
        model.Axis = axis;
        model.Position = view.Position;

        this.occupationGroupService.Update(model, null);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void UpdatePosition(ViewCareerPosition view, EnumTypeCareer type, string id)
    {
      try
      {
        var career = this.careerService.GetAll(p => p.Type == type).FirstOrDefault();
        var model = this.occupationGroupService.GetAll(p => p._id == id).FirstOrDefault();

        long positionNew = 0;
        if (view.Type == EnumTypeCareerPosition.Down)
          positionNew = view.Position - 1;
        else
          positionNew = view.Position + 1;

        if (positionNew < 0 || positionNew > GetMaxPosition(model.Company._id, type))
          new Exception();
        
        var modelOld = this.occupationGroupService.GetAll(p => p.Position == positionNew & p.Axis == model.Axis & p.Career == model.Career).FirstOrDefault();

        model.Position = positionNew;
        this.occupationGroupService.Update(model, null);

        modelOld.Position = view.Position;
        this.occupationGroupService.Update(modelOld, null);


      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DeleteOccupationGroupCareer(string id)
    {
      try
      {
        this.occupationGroupService.Delete(id, true);
        return "Deleted";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DisconnectOccupationGroupCareer(string id, EnumTypeCareer careerType)
    {
      try
      {
        var career = this.careerService.GetAll(p => p.Type == careerType).FirstOrDefault();
        var item = this.occupationGroupService.GetAll(p => p._id == id).FirstOrDefault();
        var axis = this.occupationGroupService.GetAll(p => p.Career == career & p.Axis.TypeAxis == item.Axis.TypeAxis).OrderBy(p => p.Position).ToList();
        long position = 0;
        foreach (var row in axis)
        {
          if (row._id != id)
          {
            row.Position = position;
            this.occupationGroupService.Update(row, null);
            position += 1;
          }
        }

        item.Career = null;


        this.occupationGroupService.Update(item, null);
        return "Deleted";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string NewOccupationLine(ViewOccupationLineNew view)
    {
      try
      {
        var sphere = this.sphereService.GetAll(p => p.TypeSphere == view.TypeSphere).FirstOrDefault();
        var area = this.areaService.GetAll(p => p._id == view.IdArea).FirstOrDefault();
        var occupationgroup = occupationGroupService.GetAll(p => p._id == view.IdOccupationGroup).FirstOrDefault();
        var positions = this.occupationService.GetAll(p => p.OccupationGroup.Sphere.TypeSphere == sphere.TypeSphere & p.Area == area);

        long position = 0;
        if (positions.Count() > 0)
          position = positions.Max(p => p.Position) + 1;

        var model = new Occupation()
        {
          Area = area,
          OccupationGroup = occupationgroup,
          Name = view.NameOccupation,
          Position = position,
          Status = EnumStatus.Enabled
        };

        return occupationService.Insert(model)._id;
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DeleteArea(string id)
    {
      try
      {
        var model = this.areaService.GetAll(p => p._id == id).FirstOrDefault();
        model.Status = EnumStatus.Disabled;
        this.areaService.Update(model, null);

        return "Deleted";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string UpdateOccupationLine(ViewOccupationLineNew view, string id)
    {
      try
      {
        var model = this.occupationService.GetAll(p => p._id == id).FirstOrDefault();
        var area = this.areaService.GetAll(p => p._id == view.IdArea).FirstOrDefault();
        var occupationGroup = this.occupationGroupService.GetAll(p => p._id == view.IdOccupationGroup).FirstOrDefault();
        var old = model.OccupationGroup;

        model.Area = area;
        model.OccupationGroup = occupationGroup;
        model.Name = view.NameOccupation;
        model.Position = view.Position;

        this.occupationService.Update(model, null);

        return "ok";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DeleteOccupationLine(string id)
    {
      try
      {
        this.occupationService.Delete(id, true);
        return "Deleted";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public string DisconnectOccupationLine(string id)
    {
      try
      {
        var item = this.occupationService.GetAll(p => p._id == id).FirstOrDefault();
        var spheres = this.occupationService.GetAll(p => p.OccupationGroup.Sphere == item.OccupationGroup.Sphere).OrderBy(p => p.Position).ToList();
        long position = 0;
        foreach (var row in spheres)
        {
          if (row._id != id)
          {
            row.Position = position;
            this.occupationService.Update(row, null);
            position += 1;
          }
        }

        item.Area = null;

        this.occupationService.Update(item, null);
        return "Deleted";
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void UpdatePositionLine(ViewCareerPosition view, string id)
    {
      try
      {
        long positionNew = 0;
        if (view.Type == EnumTypeCareerPosition.Down)
          positionNew = view.Position - 1;
        else
          positionNew = view.Position + 1;

        if (positionNew < 0)
          new Exception();


        var model = this.occupationService.GetAll(p => p._id == id).FirstOrDefault();
        var modelOld = this.occupationService.GetAll(p => p.Position == positionNew & p.OccupationGroup.Sphere == model.OccupationGroup.Sphere).FirstOrDefault();

        model.Position = positionNew;
        this.occupationService.Update(model, null);

        modelOld.Position = view.Position;
        this.occupationService.Update(modelOld, null);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public void SetUser(IHttpContextAccessor contextAccessor)
    {
      User(contextAccessor);
      careerService._user = _user;
      sphereService._user = _user;
      dictionarySphereService._user = _user;
      axisService._user = _user;
      occupationGroupService._user = _user;
      occupationService._user = _user;
      areaService._user = _user;
      companyService._user = _user;
    }
  }
}
