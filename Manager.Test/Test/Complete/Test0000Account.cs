using Manager.Core.Business;
using Manager.Services.Specific;
using Manager.Test.Commons;
using Manager.Views.BusinessList;
using Manager.Views.BusinessNew;
using System;
using System.Collections.Generic;
using Xunit;

namespace Manager.Test.Test.Complete
{
  public class Test0000Account : TestCommons<Account>
  {
    public Test0000Account()
    {
      try
      {
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    [Fact]
    public void AccountTest()
    {
      try
      {
        InitOffBase();
        // Create new account
        ServiceAccount serviceAccount = new ServiceAccount(context, context);
        serviceAccount.NewAccount(new ViewNewAccount()
        {
          Mail = "analisa@jmsoft.com.br",
          NameAccount = "Teste Account",
          NameCompany = "Teste Company",
          Password = "1234",
          Nickname = "Teste"
        });

        long total = 0;
        List<ViewListAccount> accounts = serviceAccount.GetAccountList(ref total, 10, 1, "Teste Account");
        if (accounts.Count != 1)
          throw new Exception("Conta padrão de teste não cridada!");
        if (!accounts[0].Name.Equals("Teste Account"))
          throw new Exception("Conta padrão de teste inválida!");

      }
      catch (Exception e)
      {
        throw e;
      }
    }

    [Fact]
    public void AuthenticationTest()
    {
      try
      {
        InitUserInfraTest();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
  }
}
