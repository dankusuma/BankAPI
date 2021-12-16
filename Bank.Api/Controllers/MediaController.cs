using Bank.Infrastructure.AWS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Bank.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class MediaController : ControllerBase
    {

        private readonly S3 _S3;
        public MediaController(S3 S3)
        {
            _S3 = S3;
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetImage(string id)
        {
 

            Stream imageStream = await _S3.ReadObjectDataAsync(id);

            Response.Headers.Add("Content-Disposition", new ContentDisposition
            {
                FileName = id+".jpg",
                Inline = true // false = prompt the user for downloading; true = browser to try to show the file inline
            }.ToString());

            return File(imageStream, "image/jpeg");
        }


        [HttpGet("id")]
        public async Task<IActionResult> GetVideo(string id)
        {


            Stream videoStream = await _S3.ReadObjectDataAsync(id);

            Response.Headers.Add("Content-Disposition", new ContentDisposition
            {
                FileName = id + ".webm",
                Inline = true // false = prompt the user for downloading; true = browser to try to show the file inline
            }.ToString());

            return File(videoStream, "video/webm");
        }


        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile content)
        {
            string fileid = await _S3.UploadObjectFromContentAsync(content);
            return Ok();
        }

    }
}