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
    public class SedeSapController : Controller
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;
        private readonly IRepositoryWrapper _repository;
        public SedeSapController(IMapper mapper, DataContext dataContext, IRepositoryWrapper repository)
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
            var listaDTO = new List<SedeSapDTO>();
            var listaEntity = new List<SedeSapEntity>();

            try
            {
                listaEntity = await _dataContext.SedeSap.OrderBy(x => x.Code).ToListAsync();
                listaDTO = _mapper.Map<List<SedeSapDTO>>(listaEntity);

                return Ok(listaDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}", Name = "GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var sedeDTO = new SedeSapDTO();
            var sedeEntity = new SedeSapEntity();

            try
            {
                sedeEntity = await _dataContext.SedeSap.Where(x => x.Code == id).FirstOrDefaultAsync();
                sedeDTO = _mapper.Map<SedeSapDTO>(sedeEntity);
                return Ok(sedeDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
