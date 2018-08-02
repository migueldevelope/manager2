using Manager.Core.Business;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Tools;

namespace Manager.Services.Specific
{
  public class ServiceAccount : Repository<Account>, IServiceAccount
  {
    private readonly ServiceGeneric<Account> accountService;
    private readonly ServiceGeneric<Person> personService;
    private readonly ServiceGeneric<Company> companyService;
    private readonly ServiceInfra infraService;

    public ServiceAccount(DataContext context)
      : base(context)
    {
      try
      {
        accountService = new ServiceGeneric<Account>(context);
        personService = new ServiceGeneric<Person>(context);
        companyService = new ServiceGeneric<Company>(context);
        infraService = new ServiceInfra(context);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public Account GeAccount(Expression<Func<Account, bool>> filter)
    {
      return accountService.GetAuthentication(filter).FirstOrDefault();
    }

    public void NewAccount(ViewNewAccount view)
    {
      try
      {
        var account = new Account()
        {
          Name = view.NameAccount,
          Status = EnumStatus.Enabled
        };
        var id = accountService.Insert(account)._id;
        account._idAccount = id;
        accountService.UpdateAccount(account, null);

        var company = new Company()
        {
          Name = view.NameCompany,
          _idAccount = id,
          Skills = new List<Skill>(),
          Status = EnumStatus.Enabled
        };
        var _company = companyService.InsertAccount(company);
        var user = new Person()
        {
          _idAccount = id,
          Name = view.NameAccount,
          Company = _company,
          Mail = view.Mail,
          Document = view.Document,
          StatusUser = EnumStatusUser.Enabled,
          Status = EnumStatus.Enabled,
          Password = EncryptServices.GetMD5Hash(view.Password),
          TypeUser = EnumTypeUser.Administrator
        };
        personService.InsertAccount(user);

        infraService._idAccount = account._id;
        infraService.CopyTemplateInfra(company);
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

    public List<Account> GeAccounts(ref long total, int count = 10, int page = 1, string filter = "")
    {
      try
      {
        int skip = (count * (page - 1));
        var detail = accountService.GetAuthentication(p => p.Name.ToUpper().Contains(filter.ToUpper())).ToList();
        total = detail.Count();

        return detail.Skip(skip).Take(count).ToList();
      }
      catch (Exception e)
      {
        throw new ServiceException(_user, e, this._context);
      }
    }

  }
}
