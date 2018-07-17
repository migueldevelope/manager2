using Manager.Core.Base;
using Manager.Core.Interfaces;
using Manager.Data;
using System;

namespace Manager.Services.Commons
{
  public class ServiceGeneric<TEntity> : Repository<TEntity> where TEntity : BaseEntity
  {
    public ServiceGeneric(DataContext context, BaseUser user)
      : base(context, user._idAccount)
    {
    }


    public ServiceGeneric(DataContext context)
     : base(context)
    {
    }


  }
}
