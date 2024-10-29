using System;
using Net.Data;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Net.Business.DTO.Web;
using Microsoft.AspNetCore.Hosting;
namespace Net.Business.Services.Controllers.Web.Ventas
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ApiExplorerSettings(GroupName = "ApiFibrafil")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ForcastVentaController : Controller
    {
        private readonly IRepositoryWrapper _repository;
        public ForcastVentaController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetForcastVentaPlantillaExcel()
        {
            try
            {
                var response = await _repository.ForcastVenta.GetForcastVentaPlantillaExcel();

                response.data.Seek(0, SeekOrigin.Begin);
                var file = response.data.ToArray();

                return new FileContentResult(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetImport([FromBody] ForcastVentaImportRequestDTO value)
        {
            if (value == null)
            {
                return BadRequest("No hay registros a crear ..!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Modelo no válido ..!");
            }

            var objectNew = await _repository.ForcastVenta.SetImport(value.ReturnValue());

            if (objectNew.ResultadoCodigo == -1)
            {
                return BadRequest(objectNew);
            }

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetCreate([FromBody] ForcastventaCreateRequestDTO value)
        {
            if (value == null)
            {
                return BadRequest("No hay registros a crear ..!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Modelo no válido ..!");
            }

            var objectNew = await _repository.ForcastVenta.SetCreate(value.ReturnValue());

            if (objectNew.ResultadoCodigo == -1)
            {
                return BadRequest(objectNew);
            }

            return NoContent();
        }

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetUpdate([FromBody] ForcastventaUpdateRequestDTO value)
        {
            if (value == null)
            {
                return BadRequest("No hay registros a actualizar ..!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Modelo no válido ..!");
            }

            var response = await _repository.ForcastVenta.SetUpdate(value.ReturnValue());

            if (response.ResultadoCodigo == -1)
            {
                return BadRequest(response);
            }

            return NoContent();
        }


        [HttpDelete("{idForcastVenta}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> SetDelete(int idForcastVenta)
        {
            if (idForcastVenta == 0)
            {
                return BadRequest("El número interno de picking no es válido ..!");
            }

            var response = await _repository.ForcastVenta.SetDelete(idForcastVenta);

            if (response.ResultadoCodigo == -1)
            {
                return BadRequest(response);
            }

            return Ok();
        }

        [HttpGet("{idForcastVenta}", Name = "GetForcastVentaById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetById(int idForcastVenta)
        {
            var response = await _repository.ForcastVenta.GetById(idForcastVenta);

            if (response.ResultadoCodigo == -1)
            {
                return BadRequest(response);
            }

            return Ok(response.data);
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListForcastVentaByFecha([FromQuery] DateTime fecInicial, DateTime fecFinal)
        {
            var response = await _repository.ForcastVenta.GetListForcastVentaByFecha(fecInicial, fecFinal);

            if (response.ResultadoCodigo == -1)
            {
                return BadRequest(response);
            }

            return Ok(response.dataList);
        }

        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> SetImport([FromForm] string value, [FromForm] IList<IFormFile> archivo)
        //{
        //    try
        //    {
        //        var forcastVentaDTO = JsonConvert.DeserializeObject<ForcastVentaDTO>(value);

        //        //string path = Path.Combine(this._environment.WebRootPath, "Uploads");

        //        //if (!Directory.Exists(path))
        //        //{
        //        //    Directory.CreateDirectory(path);
        //        //}

        //        //string fileName = Path.GetFileName(postedFile.FileName);
        //        //string filePath = Path.Combine(path, fileName);
        //        //using (FileStream stream = new FileStream(filePath, FileMode.Create))
        //        //{
        //        //    postedFile.CopyTo(stream);
        //        //}


        //        var data = new ForcastVentaImportRequestDTO() { IdUsuario = forcastVentaDTO .IdUsuario, Archivo = archivo[0] };

        //        var response = await _repository.ForcastVenta.SetImport(data.ReturnValue());

        //        return Ok();
        //    }
        //    catch (Exception)
        //    {
        //        return NotFound();
        //    }
        //}
    }
}
