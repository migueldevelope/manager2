using Manager.Core.Base;
using Manager.Data;

namespace Manager.Services.Commons
{
  public class ServiceGeneric<TEntity> : Repository<TEntity> where TEntity : BaseEntity
  {
    public ServiceGeneric(DataContext context) : base(context)
    {
    }
  }
}
