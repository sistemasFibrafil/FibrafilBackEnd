using System;
using Net.Data;
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
    public class FacturacionElectronicaSapController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        public FacturacionElectronicaSapController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListFacturaElectronica([FromQuery] string docNum)
        {
            docNum = docNum ?? "";

            var objectGetAll = await _repository.FacturacionElectronicaSap.GetListFacturaElectronica(docNum);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListGuiaElectronicaByFechaAndNumero([FromQuery] FiltroRequestDto value)
        {
            var objectGetAll = await _repository.FacturacionElectronicaSap.GetListGuiaElectronicaByFechaAndNumero(value.ReturnValue());

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EnviarGuiaElectronica([FromBody] ComprobanteElectronicoEnvioGuiaRequestDTO value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest(ModelState);
                }

                var response = await _repository.FacturacionElectronicaSap.EnviarGuiaElectronica(value.RetornaGuia());

                if (response.ResultadoCodigo == -1)
                {
                    return BadRequest(response);
                }

                return Ok(value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListGuiaInternaElectronicaByFechaAndNumero([FromQuery] FiltroRequestDto value)
        {
            var objectGetAll = await _repository.FacturacionElectronicaSap.GetListGuiaInternaElectronicaByFechaAndNumero(value.ReturnValue());

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EnviarGuiaInternaElectronica([FromBody] ComprobanteElectronicoEnvioGuiaRequestDTO value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest(ModelState);
                }

                var response = await _repository.FacturacionElectronicaSap.EnviarGuiaInternaElectronica(value.RetornaGuia());

                if (response.ResultadoCodigo == -1)
                {
                    return BadRequest(response);
                }

                return Ok(value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
