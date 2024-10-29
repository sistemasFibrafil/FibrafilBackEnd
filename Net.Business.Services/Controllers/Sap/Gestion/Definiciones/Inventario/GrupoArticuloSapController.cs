using System;
using Net.Data;
using AutoMapper;
using System.Linq;
using Net.Business.DTO.Sap;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Net.Business.Entities.Sap;
using System.Collections.Generic;
using Net.Business.Services.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Net.Business.Services.Controllers.Sap.Gestion.Definiciones.Inventario
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ApiExplorerSettings(GroupName = "ApiFibrafil")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class GrupoArticuloSapController : Controller
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;
        private readonly IRepositoryWrapper _repository;
        public GrupoArticuloSapController(IMapper mapper, DataContext dataContext, IRepositoryWrapper repository)
        {
            _mapper = mapper;
            _dataContext = dataContext;
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListAll()
        {
            var listaDTO = new List<GrupoArticuloSapDTO>();
            var listaEntity = new List<GrupoArticuloSapEntity>();

            try
            {
                listaEntity = await _dataContext.GrupoArticuloSap.Where(x => x.ItmsGrpCod > 197 || x.ItmsGrpCod < 195).ToListAsync();
                listaDTO = _mapper.Map<List<GrupoArticuloSapDTO>>(listaEntity);

                return Ok(listaDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
