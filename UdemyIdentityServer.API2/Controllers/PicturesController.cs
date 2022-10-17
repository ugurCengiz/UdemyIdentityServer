using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using UdemyIdentityServer.API2.Models;

namespace UdemyIdentityServer.API2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult GetPictures()
        {
            var pictures = new List<Picture>
            {
                new Picture{Id=1,Name="Doğa Resmi",Url="dogaresmi.jpg"},
                new Picture{Id=2,Name="Fil Resmi",Url="dogaresmi.jpg"},
                new Picture{Id=3,Name="Aslan Resmi",Url="dogaresmi.jpg"},
                new Picture{Id=4,Name="Köpek Resmi",Url="dogaresmi.jpg"},
                new Picture{Id=5,Name="Kedi Resmi",Url="dogaresmi.jpg"}
            };

            return Ok(pictures);
        }
    }
}


