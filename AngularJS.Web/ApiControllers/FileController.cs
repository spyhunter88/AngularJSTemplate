using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AngularJS.Controllers.Filter;

namespace AngularJS.Api
{
	[RoutePrefix("api/File")]
	public class FileController : ApiController
	{
		private static readonly string ServerUploadFolder = "E:\\Uploads";
		
		[HttpPost]
		[ValidateMimeMultipartContentFilter]
        public async Task<IHttpActionResult> UploadSingleFile()
		{
			var streamProvider = new MultipartFormDataStreamProvider(ServerUploadFolder);
			await Request.Content.ReadAsMultipartAsync(streamProvider);
			
			foreach (var file in streamProvider.Contents)
			{
				var fileName = file.Headers.ContentDisposition.FileName.Trim('\"');
				var buffer = await file.ReadAsByteArrayAsync();
			}
			
			return Ok();
		}
	}
}