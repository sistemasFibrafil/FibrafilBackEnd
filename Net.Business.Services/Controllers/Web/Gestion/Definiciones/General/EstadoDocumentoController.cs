using Net.Data;
using AutoMapper;
using Net.Business.DTO.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Net.Business.Services.Models;
using Microsoft.AspNetCore.Authorization;
namespace Net.Business.Services.Controllers.Web.Gestion.Definiciones.General
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ApiExplorerSettings(GroupName = "ApiFibrafil")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class EstadoDocumentoController : Controller
    {
        private readonly IRepositoryWrapper _repository;
        public EstadoDocumentoController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetListAll()
        {
            var response = await _repository.EstadoDocumento.GetListAll();

            if (response.ResultadoCodigo == -1)
            {
                return BadRequest(response);
            }

            return Ok(response.dataList);
        }
    }
}
