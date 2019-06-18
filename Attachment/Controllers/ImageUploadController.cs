using System;
using System.IO;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using MongoDB.Bson;
using Manager.Core.Business;
using Manager.Data;
using Manager.Services.Commons;
using Tools;
using Manager.Core.Base;
using Tools.Data;

namespace EdeskIntegration.Controllers
{
  [Route("imageupload")]
  public class ImageController : Controller
  {
    private readonly ServiceGeneric<Attachments> service;
    private readonly DataContext context;
    private readonly ObjectId account;
    private readonly string blobKey;

    public ImageController(IHttpContextAccessor contextAccessor)
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
    }

    [Authorize]
    [HttpPost("{filename}/{extension}")]
    public async Task<ObjectResult> Post(string filename, string extension)
    {
      return await UploadImage(string.Format("{0}.{1}", filename, extension), "systemimage");
    }

    [Authorize]
    [HttpPost("{filename}/{extension}/account")]
    public async Task<ObjectResult> PostKeyAccount(string filename, string extension)
    {
      return await UploadImage(string.Format("{0}.{1}", filename, extension), account.ToString());
    }

    private async Task<ObjectResult> UploadImage(string filename, string container)
    {
      if (HttpContext.Request.Form.Files.Count != 1)
        return BadRequest("Somente um arquivo por vez.");

      var ext = Path.GetExtension(HttpContext.Request.Form.Files[0].FileName).ToLower();
      if (!(ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".gif"))
        return BadRequest("Bad file type (PNG, JPG, GIF, JPEG.");

      try
      {
        CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobKey);
        CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
        CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(container);
        if (await cloudBlobContainer.CreateIfNotExistsAsync())
        {
          await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions
          {
            PublicAccess = BlobContainerPublicAccessType.Blob
          });
        }
        CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(filename);
        await blockBlob.DeleteIfExistsAsync();
        blockBlob.Properties.ContentType = HttpContext.Request.Form.Files[0].ContentType;
        await blockBlob.UploadFromStreamAsync(HttpContext.Request.Form.Files[0].OpenReadStream());
      }
      catch (Exception)
      {
        throw;
      }
      return Ok("Imagem carregada!");
    }
  }
}
