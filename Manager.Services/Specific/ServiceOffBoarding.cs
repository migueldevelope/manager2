using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Services.Specific
{
    public class ServiceOffBoarding : Repository<OffBoarding>, IServiceOffBoarding
    {
        private readonly ServiceGeneric<OffBoarding> serviceOffBoarding;
        private readonly ServiceGeneric<Person> servicePerson;

        #region Constructor
        public ServiceOffBoarding(DataContext context) : base(context)
        {
            try
            {
                serviceOffBoarding = new ServiceGeneric<OffBoarding>(context);
                servicePerson = new ServiceGeneric<Person>(context);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void SetUser(IHttpContextAccessor contextAccessor)
        {
            User(contextAccessor);
            serviceOffBoarding._user = _user;
            servicePerson._user = _user;
        }
        public void SetUser(BaseUser user)
        {
            _user = user;
            serviceOffBoarding._user = user;
            servicePerson._user = user;
        }
        #endregion

        #region OffBoarding
        public string Delete(string id)
        {
            try
            {
                OffBoarding item = serviceOffBoarding.GetNewVersion(p => p._id == id).Result;
                item.Status = EnumStatus.Disabled;
                serviceOffBoarding.Update(item, null).Wait();
                return "OffBoarding deleted!";
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string New(ViewCrudOffBoarding view)
        {
            try
            {
                OffBoarding offboarding = serviceOffBoarding.InsertNewVersion(new OffBoarding()
                {
                    _id = view._id,
                    
                }).Result;

                return "OffBoarding added!";
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string Update(ViewCrudOffBoarding view)
        {
            try
            {
                OffBoarding offboarding = serviceOffBoarding.GetNewVersion(p => p._id == view._id).Result;
                
                serviceOffBoarding.Update(offboarding, null).Wait();

                return "OffBoarding altered!";
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public ViewCrudOffBoarding Get(string id)
        {
            try
            {
                return serviceOffBoarding.GetNewVersion(p => p._id == id).Result.GetViewCrud();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ViewListOffBoarding> List(ref long total, int count = 10, int page = 1, string filter = "")
        {
            try
            {
                List<ViewListOffBoarding> detail = serviceOffBoarding.GetAllNewVersion(p => p.Person.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Person.Name").Result
                  .Select(x => new ViewListOffBoarding()
                  {
                      _id = x._id,
                      Name = x.Person.Name
                  }).ToList();
                total = serviceOffBoarding.CountNewVersion(p => p.Person.Name.ToUpper().Contains(filter.ToUpper())).Result;
                return detail;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion


    }
}
