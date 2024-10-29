using System;
using Net.Data;
using System.IO;
using Net.Business.DTO.Sap;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Net.Business.DTO;

namespace Net.Business.Services.Controllers.Sap.Ventas
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ApiExplorerSettings(GroupName = "ApiFibrafil")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class FacturaVentaSapController : Controller
    {
        private readonly IRepositoryWrapper _repository;
        public FacturaVentaSapController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListVentaProyeccionByFecha([FromQuery] DateTime fecInicial, DateTime fecFinal)
        {
            var objectGetAll = await _repository.FacturaVentaSap.GetListVentaProyeccionByFecha(fecInicial, fecFinal);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListVentaResumenByFechaGrupo1([FromQuery] DateTime fecInicial, DateTime fecFinal, string grupo)
        {
            var objectGetAll = await _repository.FacturaVentaSap.GetListVentaResumenByFechaGrupo1(fecInicial, fecFinal, grupo);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListVentaResumenByFechaGrupo2([FromQuery] DateTime fecInicial, DateTime fecFinal, string grupo)
        {
            var objectGetAll = await _repository.FacturaVentaSap.GetListVentaResumenByFechaGrupo2(fecInicial, fecFinal, grupo);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListVentaResumenByFechaGrupo3([FromQuery] DateTime fecInicial, DateTime fecFinal, string grupo)
        {
            var objectGetAll = await _repository.FacturaVentaSap.GetListVentaResumenByFechaGrupo3(fecInicial, fecFinal, grupo);

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
        public async Task<IActionResult> GetVentaResumenExcelByFechaGrupo([FromQuery] DateTime fecInicial, DateTime fecFinal, string grupo)
        {
            try
            {
                var objectGetAll = await _repository.FacturaVentaSap.GetVentaResumenExcelByFechaGrupo(fecInicial, fecFinal, grupo);

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
        public async Task<IActionResult> GetListVentaByFechaAndSlpCode([FromQuery] FiltroRequestDto value)
        {
            var objectGetAll = await _repository.FacturaVentaSap.GetListVentaByFechaAndSlpCode(value.ReturnValue());

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
        public async Task<IActionResult> GetListVentaExcelByFechaAndSlpCode([FromQuery] FiltroRequestDto value)
        {
            try
            {
                var objectGetAll = await _repository.FacturaVentaSap.GetListVentaExcelByFechaAndSlpCode(value.ReturnValue());

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
        public async Task<IActionResult> GetListFacturaVentaByFecha([FromQuery] FiltroRequestDto value)
        {
            var objectGetAll = await _repository.FacturaVentaSap.GetListFacturaVentaByFecha(value.ReturnValue());

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
        public async Task<IActionResult> GetListFacturaVentaExcelByFecha([FromQuery] FiltroRequestDto value)
        {
            try
            {
                var objectGetAll = await _repository.FacturaVentaSap.GetListFacturaVentaExcelByFecha(value.ReturnValue());

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
