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

namespace AngularJS.Api
{
	[RoutePrefix("api/File")]
	public class FileController : ApiController
	{
        private static readonly string ServerUploadFolder = "E:\\Uploads";
		
		[HttpPost]
		[ValidateMimeMultipartContentFilter]
        [ResponseType(typeof(DocumentDTO))]
        public async Task<IHttpActionResult> UploadSingleFile()
		{
			var streamProvider = new MultipartFormDataStreamProvider(ServerUploadFolder);
			await Request.Content.ReadAsMultipartAsync(streamProvider);

            // on upload, files are given a generic name like 'BodyPart_26d6abe1-3ae1-416a-9429-b35f15e6e5d5'
            // this is how you can get the original file name without passing through parameter
            var fileName = GetDeserializedFileName(streamProvider.FileData.First());
			
            // uploadFileInfo will give you some addition information
            var uploadFileInfo = new FileInfo(streamProvider.FileData.First().LocalFileName);

			foreach (var file in streamProvider.FileData)
			{
                // var buffer = await file.Headers;
			}

            var _NewDocument = new DocumentDTO {
                TempName = uploadFileInfo.Name
            };

            return Ok(_NewDocument);
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