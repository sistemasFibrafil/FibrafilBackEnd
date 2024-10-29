using System;
using Net.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Net.Business.DTO;

namespace Net.Business.Services.Controllers.Sap.GestionBancos.PagosRecibidos
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ApiExplorerSettings(GroupName = "ApiFibrafil")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PagoRecibidoSapController : Controller
    {
        private readonly IRepositoryWrapper _repository;
        public PagoRecibidoSapController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListCobranzaCarteraVencidaByFechaCorte([FromQuery] FiltroRequestDto value)
        {
            var objectGetAll = await _repository.PagoRecibidoSap.GetListCobranzaCarteraVencidaByFechaCorte(value.ReturnValue());

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
        public async Task<IActionResult> GetListCobranzaCarteraVencidaExcelByFechaCorte([FromQuery] FiltroRequestDto value)
        {
            try
            {
                var objectGetAll = await _repository.PagoRecibidoSap.GetListCobranzaCarteraVencidaExcelByFechaCorte(value.ReturnValue());

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
