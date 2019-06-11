using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Manager.Core.Business;
using Manager.Data;
using Manager.Services.Commons;
using MongoDB.Bson;
using Tools;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Net.Http;
using System.Net;
using Manager.Core.Base;
using Tools.Data;

namespace EdeskIntegration.Controllers
{
  [Route("imagedownload")]
  public class ImageDownloadController : Controller
  {
    private readonly ServiceGeneric<Attachments> service;
    private readonly DataContext context;
    private readonly ObjectId account;
    private readonly string blobKey;
    private readonly HttpContextAccessor aux;

    public ImageDownloadController(IHttpContextAccessor contextAccessor)
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
        if (ci.Type == ClaimTypes.Actor)
          baseUser._idPerson = ci.Value;

      }
      Config conn = XmlConnection.ReadVariablesSystem();
      context = new DataContext(conn.Server, conn.DataBase);
      blobKey = conn.BlobKey;
      service = new ServiceGeneric<Attachments>(context);
      service.User(contextAccessor);
      aux = (HttpContextAccessor)contextAccessor;
    }

    [Authorize]
    [HttpGet("{filename}/{extension}")]
    public async Task<HttpResponseMessage> Get(string filename, string extension)
    {
      CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobKey);
      CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
      CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(account.ToString());
      CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(filename);
      MemoryStream memStream = new MemoryStream();
      if (await blockBlob.ExistsAsync() == false)
      {
        cloudBlobContainer = cloudBlobClient.GetContainerReference("systemimage");
        blockBlob = cloudBlobContainer.GetBlockBlobReference(filename);
      }
      //download    
      //await blockBlob.DownloadToStreamAsync(memStream);
      HttpResponseMessage result = null;
      result = new HttpResponseMessage(HttpStatusCode.OK);
      result.Content = new StreamContent(await blockBlob.OpenReadAsync());
      result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
      {
        FileName = string.Format("{0}.{1}", filename, extension)
      };
      return result;
    }

    [Authorize]
    [HttpGet("{filename}/extension/url")]
    public async Task<string> GetUrl(string filename, string extension)
    {
      CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobKey);
      CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
      CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(account.ToString());
      CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(string.Format("{0}.{1}", filename, extension));
      if (await blockBlob.ExistsAsync() == false)
      {
        cloudBlobContainer = cloudBlobClient.GetContainerReference("systemimage");
        blockBlob = cloudBlobContainer.GetBlockBlobReference(string.Format("{0}.{1}", filename, extension));
      }
      return blockBlob.Uri.ToString();
    }
  }
}
