using System;
using Net.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.IO;

namespace Net.Business.Services.Controllers.Web.Mantenimiento
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ApiExplorerSettings(GroupName = "ApiFibrafil")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class OrdenMantenimientoWebController : Controller
    {
        private readonly IRepositoryWrapper _repository;
        public OrdenMantenimientoWebController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListOrdenMatenimientoByFechaAndIdEstadoAndNumero([FromQuery] DateTime? fecInicial, DateTime? fecFinal, string idEstado, string numero)
        {
            numero = numero ?? "";

            var objectGetAll = await _repository.OrdenMantenimientoWeb.GetListOrdenMatenimientoByFechaAndIdEstadoAndNumero(fecInicial, fecFinal, idEstado, numero);

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
        public async Task<IActionResult> GetOrdenMatenimientoExcelByFechaAndIdEstadoAndNumero([FromQuery] DateTime? fecInicial, DateTime? fecFinal, string idEstado, string numero)
        {
            numero = numero ?? "";

            try
            {
                var objectGetAll = await _repository.OrdenMantenimientoWeb.GetOrdenMatenimientoExcelByFechaAndIdEstadoAndNumero(fecInicial, fecFinal, idEstado, numero);

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
