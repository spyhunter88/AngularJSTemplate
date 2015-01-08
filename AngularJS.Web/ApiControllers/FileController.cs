using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Taks;
using System.Web.Http;
using AngularJS.Controllers.Filter;

namespace AngularJS.Api
{
	[RoutePrefix("api/File")]
	public class FileController : ApiController
	{
		private static readonly string ServerUploadFolder = "E:\\workspace";
		
		[HttpPost]
		[ValidateMimeMultipartContentFilter]
		public async Task<FileResult> UploadSingleFile()
		{
			var streamProvider = new MultiparFormDataStreamProvider(ServerUploadFolder);
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