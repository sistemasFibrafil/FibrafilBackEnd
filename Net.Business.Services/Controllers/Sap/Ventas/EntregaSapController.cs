using System;
using Net.Data;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
namespace Net.Business.Services.Controllers.Sap.Ventas
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ApiExplorerSettings(GroupName = "ApiFibrafil")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class EntregaSapController : Controller
    {
        private readonly IRepositoryWrapper _repository;
        public EntregaSapController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListGuiaDespachoMercaderiaByFechaSede([FromQuery] DateTime fecIni, DateTime fecFin, string location)
        {
            var objectGetAll = await _repository.EntregaSap.GetListGuiaDespachoMercaderiaByFechaSede(fecIni, fecFin, location);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetGuiaDespachoMercaderiaExcelByFechaSede([FromQuery] DateTime fecIni, DateTime fecFin, string location)
        {
            try
            {
                var objectGetAll = await _repository.EntregaSap.GetGuiaDespachoMercaderiaExcelByFechaSede(fecIni, fecFin, location);

                objectGetAll.data.Seek(0, SeekOrigin.Begin);
                var file = objectGetAll.data.ToArray();

                return new FileContentResult(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
