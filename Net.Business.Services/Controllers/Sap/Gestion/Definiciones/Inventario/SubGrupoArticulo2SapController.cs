using System;
using Net.Data;
using AutoMapper;
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
    public class SubGrupoArticulo2SapController : Controller
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;
        private readonly IRepositoryWrapper _repository;
        public SubGrupoArticulo2SapController(IMapper mapper, DataContext dataContext, IRepositoryWrapper repository)
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
            var listaDTO = new List<SubGrupoArticulo2SapDTO>();
            var listaEntity = new List<SubGrupoArticulo2SapEntity>();

            try
            {
                listaEntity = await _dataContext.SubGrupoArticulo2Sap.ToListAsync();
                listaDTO = _mapper.Map<List<SubGrupoArticulo2SapDTO>>(listaEntity);

                return Ok(listaDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
