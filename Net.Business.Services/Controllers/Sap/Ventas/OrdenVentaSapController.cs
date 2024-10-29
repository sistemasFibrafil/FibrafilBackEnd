using System;
using Net.Data;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Net.Business.DTO.Sap;
using Net.Business.DTO;

namespace Net.Business.Services.Controllers.Sap.Ventas
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ApiExplorerSettings(GroupName = "ApiFibrafil")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class OrdenVentaSapController : Controller
    {
        private readonly IRepositoryWrapper _repository;
        public OrdenVentaSapController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListOrdenVentaSeguimientoByFecha([FromQuery] FiltroRequestDto value)
        {
            var objectGetAll = await _repository.OrdenVentaSap.GetListOrdenVentaSeguimientoByFecha(value.ReturnValue());

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
        public async Task<IActionResult> GetOrdenVentaSeguimientoExcelByFecha([FromQuery] FiltroRequestDto value)
        {
            try
            {
                var objectGetAll = await _repository.OrdenVentaSap.GetOrdenVentaSeguimientoExcelByFecha(value.ReturnValue());

                objectGetAll.data.Seek(0, SeekOrigin.Begin);
                var file = objectGetAll.data.ToArray();

                return new FileContentResult(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListOrdenVentaSeguimientoDetalladoByFecha([FromQuery] FiltroRequestDto value)
        {
            var objectGetAll = await _repository.OrdenVentaSap.GetListOrdenVentaSeguimientoDetalladoByFecha(value.ReturnValue());

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
        public async Task<IActionResult> GetOrdenVentaSeguimientoDetalladoExcelByFecha([FromQuery] FiltroRequestDto value)
        {
            try
            {
                var objectGetAll = await _repository.OrdenVentaSap.GetOrdenVentaSeguimientoDetalladoExcelByFecha(value.ReturnValue());

                objectGetAll.data.Seek(0, SeekOrigin.Begin);
                var file = objectGetAll.data.ToArray();

                return new FileContentResult(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListOrdenVentaPendienteStockAlmacenProduccionByFecha([FromQuery] FiltroRequestDto value)
        {
            var objectGetAll = await _repository.OrdenVentaSap.GetListOrdenVentaPendienteStockAlmacenProduccionByFecha(value.ReturnValue());

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
        public async Task<IActionResult> GetOrdenVentaPendienteStockAlmacenProduccionExcelByFecha([FromQuery] FiltroRequestDto value)
        {
            try
            {
                var objectGetAll = await _repository.OrdenVentaSap.GetOrdenVentaPendienteStockAlmacenProduccionExcelByFecha(value.ReturnValue());

                objectGetAll.data.Seek(0, SeekOrigin.Begin);
                var file = objectGetAll.data.ToArray();

                return new FileContentResult(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListOrdenVentaProgramacionByFecha([FromQuery] FiltroRequestDto value)
        {
            var objectGetAll = await _repository.OrdenVentaSap.GetListOrdenVentaProgramacionByFecha(value.ReturnValue());

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
        public async Task<IActionResult> GetOrdenVentaProgramacionExcelByFecha([FromQuery] FiltroRequestDto value)
        {
            try
            {
                var objectGetAll = await _repository.OrdenVentaSap.GetOrdenVentaProgramacionExcelByFecha(value.ReturnValue());

                objectGetAll.data.Seek(0, SeekOrigin.Begin);
                var file = objectGetAll.data.ToArray();

                return new FileContentResult(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListOrdenVentaSodimacPendienteByFiltro([FromQuery] FiltroRequestDto value)
        {
            var objectGetAll = await _repository.OrdenVentaSap.GetListOrdenVentaSodimacPendienteByFiltro(value.ReturnValue());

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        [HttpGet("{docEntry}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetOrdenVentaSodimacPendienteByDocEntry(int docEntry)
        {
            var response = await _repository.OrdenVentaSap.GetOrdenVentaSodimacPendienteByDocEntry(docEntry);

            if (response.ResultadoCodigo == -1)
            {
                return BadRequest(response);
            }

            return Ok(response.data);
        }
    }
}
