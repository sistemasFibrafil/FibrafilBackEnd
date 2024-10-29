using System;
using Net.Data;
using Net.Business.DTO.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Net.Business.DTO.Web.Ventas.Picking;
namespace Net.Business.Services.Controllers.Web.Ventas
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ApiExplorerSettings(GroupName = "ApiFibrafil")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PickingController : Controller
    {
        private readonly IRepositoryWrapper _repository;
        public PickingController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPickingNumero()
        {
            var response = await _repository.PickingVenta.GetPickingNumero();

            if (response.ResultadoCodigo == -1)
            {
                return BadRequest(response);
            }

            return Ok(response.data);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListPickingVentaByFiltro([FromQuery] DateTime fecInicial, DateTime fecFinal)
        {
            var response = await _repository.PickingVenta.GetListPickingVentaByFiltro(fecInicial, fecFinal);

            if (response.ResultadoCodigo == -1)
            {
                return BadRequest(response);
            }

            return Ok(response.dataList);
        }

        [HttpGet("{idPicking}", Name = "GetPickingVentaByIdPicking")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetPickingVentaByIdPicking(int idPicking)
        {
            var response = await _repository.PickingVenta.GetPickingVentaByIdPicking(idPicking);

            if (response.ResultadoCodigo == -1)
            {
                return BadRequest(response);
            }

            return Ok(response.data);
        }

        [HttpGet("{idPicking}", Name = "GetListPickingVentaItemByIdPicking")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetListPickingVentaItemByIdPicking(int idPicking)
        {
            var response = await _repository.PickingVenta.GetListPickingVentaItemByIdPicking(idPicking);

            if (response.ResultadoCodigo == -1)
            {
                return BadRequest(response);
            }

            return Ok(response.dataList);
        }

        [HttpGet("{idPicking}", Name = "GetPickingVentaForEntregaByIdPicking")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetPickingVentaForEntregaByIdPicking(int idPicking)
        {
            var response = await _repository.PickingVenta.GetPickingVentaForEntregaByIdPicking(idPicking);

            if (response.ResultadoCodigo == -1)
            {
                return BadRequest(response);
            }

            return Ok(response.data);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetCreate([FromBody] PickingVentaCreateRequestDTO value)
        {
            if (value == null)
            {
                return BadRequest("No hay registros a crear ..!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Modelo no válido ..!");
            }

            var objectNew = await _repository.PickingVenta.SetCreate(value.RetornaPickingVenta());

            if (objectNew.ResultadoCodigo == -1)
            {
                return BadRequest(objectNew);
            }

            return NoContent();
        }


        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetUpdate([FromBody] PickingVentaUpdateRequestDTO value)
        {
            if (value == null)
            {
                return BadRequest("No hay registros a actualizar ..!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Modelo no válido ..!");
            }

            var response = await _repository.PickingVenta.SetUpdate(value.RetornaPickingVenta());

            if (response.ResultadoCodigo == -1)
            {
                return BadRequest(response);
            }

            return NoContent();
        }


        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> SetDelete([FromBody] PickingVentaDeleteRequestDTO value)
        {
            if (value == null)
            {
                return BadRequest("No hay registro a eliminar ..!");
            }

            var response = await _repository.PickingVenta.SetDelete(value.RetornaPicking());

            if (response.ResultadoCodigo == -1)
            {
                return BadRequest(response);
            }

            return Ok();
        }

        [HttpDelete("{idPickingItem}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> SetDeleteItem(int idPickingItem)
        {
            if (idPickingItem == 0)
            {
                return BadRequest("El número interno de picking no es válido ..!");
            }

            var response = await _repository.PickingVenta.SetDeleteItem(idPickingItem);

            if (response.ResultadoCodigo == -1)
            {
                return BadRequest(response);
            }

            return Ok();
        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> SetDeleteItemAll([FromBody] PickingVentaItemDeleteRequestDTO value)
        {
            if (value == null)
            {
                return BadRequest("No hay registro a eliminar ..!");
            }

            var response = await _repository.PickingVenta.SetDeleteItemAll(value.RetornaPickingItem());

            if (response.ResultadoCodigo == -1)
            {
                return BadRequest(response);
            }

            return Ok();
        }


        [HttpGet("{docEntry}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<FileContentResult> GetListPickingPdfByDocEntry(int docEntry)
        {
            var objectGetById = await _repository.PickingVenta.GetListPickingPdfByDocEntry(docEntry);

            var nombreArchivo = string.Format("Picking List - {0}", DateTime.Now.ToString("dd-MM-yyyy").ToString());

            var pdf = File(objectGetById.data.GetBuffer(), "applicacion/pdf", nombreArchivo + ".pdf");

            return pdf;
        }
    }
}
