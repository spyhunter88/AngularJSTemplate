using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AngularJS.Controllers.Filter;
using Newtonsoft.Json;
using AngularJS.Services.DTO;
using System.Web.Http.Description;

namespace AngularJS.Web.Api
{
    [AllowAnonymous]
	[RoutePrefix("api/File")]
	public class FileController : BaseController
	{
        //
        // POST api/File
		[HttpPost]
		[ValidateMimeMultipartContentFilter]
        [ResponseType(typeof(DocumentDTO))]
        public async Task<IHttpActionResult> UploadSingleFile()
		{
            string ServerUploadFolder = GetUploadPath() + "/Temps";
			var streamProvider = new MultipartFormDataStreamProvider(ServerUploadFolder);
			await Request.Content.ReadAsMultipartAsync(streamProvider);

            // on upload, files are given a generic name like 'BodyPart_26d6abe1-3ae1-416a-9429-b35f15e6e5d5'
            // this is how you can get the original file name without passing through parameter
            var fileName = GetDeserializedFileName(streamProvider.FileData.First());
			
            // uploadFileInfo will give you some addition information
            var uploadFileName = new FileInfo(streamProvider.FileData.First().LocalFileName).Name;
            var newFileName = uploadFileName;

            var extension = fileName.Split(new char[] { '.' });
            if (extension.Length > 1) newFileName += "." + extension.LastOrDefault();
            // Rename file's extension
            File.Move(ServerUploadFolder + "/" + uploadFileName, ServerUploadFolder + "/" + newFileName);

            /*
			foreach (var file in streamProvider.FileData)
			{
                var buffer = await file.Headers;
			}
            */

            var _newDocument = new DocumentDTO {
                TempName = newFileName
            };

            return Ok(_newDocument);
		}

        [HttpPost]
        [ValidateMimeMultipartContentFilter]
        [Route("api/File/Claim")]
        public IHttpActionResult UploadFileClaim()
        {
            var result = new {  };

            return Ok();
        }

        #region Separate when Entity is exists
        private void MoveToClaim(int claimId, string source, string dest)
        {

        }
        #endregion

        #region Support
        // You could extract these two private methods to a separate utility class since
        // they do not really belong to a controller class but that is up to you
        private MultipartFormDataStreamProvider GetMultipartProvider()
        {
            var uploadFolder = "~/App_Data/Tmp/FileUploads"; // you could put this to web.config
            var root = HttpContext.Current.Server.MapPath(uploadFolder);
            Directory.CreateDirectory(root);
            return new MultipartFormDataStreamProvider(root);
        }

        // Extracts Request FormatData as a strongly typed model
        private object GetFormData<T>(MultipartFormDataStreamProvider result)
        {
            if (result.FormData.HasKeys())
            {
                var unescapedFormData = Uri.UnescapeDataString(result.FormData
                    .GetValues(0).FirstOrDefault() ?? String.Empty);
                if (!String.IsNullOrEmpty(unescapedFormData))
                    return JsonConvert.DeserializeObject<T>(unescapedFormData);
            }

            return null;
        }

        private string GetDeserializedFileName(MultipartFileData fileData)
        {
            var fileName = GetFileName(fileData);
            return JsonConvert.DeserializeObject(fileName).ToString();
        }

        public string GetFileName(MultipartFileData fileData)
        {
            return fileData.Headers.ContentDisposition.FileName;
        }
        #endregion
    }
}