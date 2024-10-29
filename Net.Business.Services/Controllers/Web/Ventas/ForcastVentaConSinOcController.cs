using System;
using Net.Data;
using AutoMapper;
using System.Linq;
using Net.Business.DTO.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Net.Business.Entities.Web;
using System.Collections.Generic;
using Net.Business.Services.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
namespace Net.Business.Services.Controllers.Web.Ventas
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ApiExplorerSettings(GroupName = "ApiFibrafil")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ForcastVentaConSinOcController : Controller
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;
        private readonly IRepositoryWrapper _repository;
        public ForcastVentaConSinOcController(IMapper mapper, DataContext dataContext, IRepositoryWrapper repository)
        {
            this._mapper = mapper;
            this._dataContext = dataContext;
            this._repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListAll()
        {
            var listaDTO = new List<ForcastVentaConSinOcDTO>();
            var listaEntity = new List<ForcastVentaConSinOcEntity>();

            try
            {
                listaEntity = await _dataContext.ForcastVentaConSinOc.OrderBy(x => x.IdConSinOc).ToListAsync();

                listaDTO = _mapper.Map<List<ForcastVentaConSinOcDTO>>(listaEntity);

                return Ok(listaDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
