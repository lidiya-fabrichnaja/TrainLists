using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainLists.Application.Services;
using TrainLists.Infrastructure;

namespace TrainLists.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ExchangeDataController : ControllerBase
    {
        private const long MAXFILESIZE = 10000000;
        private readonly IExchangeService _service;
        private readonly string _dirPath;

        public ExchangeDataController(IExchangeService service, IWebHostEnvironment env)
        {
            _service = service;
            _dirPath = Path.Combine(env.WebRootPath,"ExchangeFiles");
        }

        [HttpPost]
        public async Task<IActionResult> ImportXml(IFormFile formFile)
        {
            if(formFile == null)      
                return BadRequest();

            var fileName = formFile.FileName.ToLower();

            var ext = fileName.Split(".").LastOrDefault() ?? "";

            if(ext != "xml" ) 
                return BadRequest("Неверный формат файла");

            if(formFile.Length >= MAXFILESIZE)
                return BadRequest($"Размер файла превышает максимально допустимый размер ({MAXFILESIZE})");

            var targetFilePath = Path.Combine(_dirPath,$"{Guid.NewGuid()}_{fileName}");

            using(var fileStream = System.IO.File.Create(targetFilePath)){
                formFile.CopyTo(fileStream);
            }
                

            return Ok(await _service.ParseXml(targetFilePath));
        }
        
    }
}