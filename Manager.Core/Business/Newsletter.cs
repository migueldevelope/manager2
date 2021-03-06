﻿using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;

namespace Manager.Core.Business
{
  public class Newsletter : BaseEntity
  {
    public string Title { get; set; }
    public string Description { get; set; }
    public bool Enabled { get; set; }
    public bool Infra { get; set; }
    public bool Manager { get; set; }
    public bool Employee { get; set; }
    public EnumTypeNewsletter TypeNewsletter { get; set; }
    public DateTime? Included { get; set; }
    public DateTime? BeginDate { get; set; }
    public DateTime? EndDate { get; set; }

    public ViewCrudNewsletter GetViewCrud()
    {
      return new ViewCrudNewsletter()
      {
        Description = Description,
        Enabled = Enabled,
        Included = Included,
        Title = Title,
        Infra = Infra,
        Employee = Employee,
        Manager = Manager,
        TypeNewsletter = TypeNewsletter,
        BeginDate = BeginDate.Value.AddHours(3),
        EndDate = EndDate.Value.AddHours(3),
        _id = _id
      };
    }

    public ViewListNewsletter GetViewList()
    {
      return new ViewListNewsletter()
      {
        Title = Title,
        Description = Description,
        _id = _id,
        TypeNewsletter = TypeNewsletter,
        Enabled = Enabled
      };
    }
  }
}
