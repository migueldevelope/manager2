using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Manager.Core.Business;
using Manager.Data;
using Manager.Services.Commons;
using Tools;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Manager.Core.Base;
using Tools.Data;

namespace EdeskIntegration.Controllers
{
  [Route("download")]
  public class DownloadController : Controller
  {
    private readonly ServiceGeneric<Attachments> service;
    private readonly DataContext context;
    private readonly string account;
    private readonly string blobKey;

    public DownloadController(IHttpContextAccessor contextAccessor)
    {
      BaseUser baseUser = new BaseUser();
      var user = contextAccessor.HttpContext.User;
      foreach (Claim ci in user.Claims)
      {
        if (ci.Type == ClaimTypes.Name)
          baseUser.NamePerson = ci.Value;

        if (ci.Type == ClaimTypes.Hash)
          baseUser._idAccount = ci.Value;

        if (ci.Type == ClaimTypes.Email)
          baseUser.Mail = ci.Value;

        if (ci.Type == ClaimTypes.NameIdentifier)
          baseUser.NameAccount = ci.Value;

        if (ci.Type == ClaimTypes.UserData)
          baseUser._idUser = ci.Value;

      }
      foreach (Claim ci in user.Claims)
      {
        if (ci.Type == ClaimTypes.Hash)
          account = ci.Value;
      }
      Config conn = XmlConnection.ReadConfig();
      context = new DataContext(conn.Server, conn.DataBase);
      blobKey = conn.BlobKey;
      service = new ServiceGeneric<Attachments>(context);
      service.User(contextAccessor);
    }

    [Authorize]
    [HttpGet("{id}/url")]
    public string GetUrl(string id)
    {
      var attachment = service.GetAll(p => p._id == id).FirstOrDefault();
      CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobKey);
      CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
      CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(account.ToString());
      CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(string.Format("{0}{1}", attachment._id.ToString(), attachment.Extension));
      return blockBlob.Uri.ToString();
    }
  }
}
