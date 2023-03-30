using Microsoft.AspNetCore.Mvc;
using TrainLists.Application.Services;
using TrainLists.Application.Models;
using TrainLists.Application.Models.Export;
using Microsoft.AspNetCore.Authorization;

namespace TrainLists.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ReportController : ControllerBase
    {
        private const string CONTENT_TYPE_XLSX = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly IReportService _reportService;

        private readonly ISearchService _searchService;

        private readonly string _dirPath;
        

        public ReportController(IReportService reportService, ISearchService searchService, IWebHostEnvironment env)
        {
            _reportService = reportService;
            _searchService = searchService;
            _dirPath = Path.Combine(env.WebRootPath,"ReportTemplate");
        }

        [HttpGet]
        [ProducesResponseType(typeof(FileContentResult),200)]
        [ProducesResponseType(typeof(AppResponce),400)]
        public async Task<IActionResult> ExportXlsx([FromQuery]int trainNumber)
        {
            var responce = await _searchService.Search(trainNumber);

            if(responce.Success) 
            {
                var data = _reportService.CreateXlsx(responce.Data,_dirPath);
                var fileName = $"Натурный лист поезда {trainNumber}.xlsx";
                return File(data, CONTENT_TYPE_XLSX, fileName);
            }
            else
            {
                return BadRequest(responce);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(AppResponce<ReportModel>),200)]
        public async Task<IActionResult> ExportJson([FromQuery]int trainNumber)
        {
            return Ok(await _searchService.Search(trainNumber));
        }
    }
}