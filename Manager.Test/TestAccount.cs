using Manager.Core.Interfaces;
using Manager.Services.Specific;
using Manager.Test.Commons;

namespace Manager.Test.Test.Complete
{
  public class TestAccount : TestCommons<TestAccount>
  {
    private readonly IServiceAccount service;
    public TestAccount()
    {
      Init();
      service = new ServiceAccount(context, context);
    }
  }
}