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

namespace Net.Business.Services.Controllers.Sap.Gestion.InicializacionSistema
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ApiExplorerSettings(GroupName = "ApiFibrafil")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class SerieSapController : Controller
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;
        private readonly IRepositoryWrapper _repository;
        public SerieSapController(IMapper mapper, DataContext dataContext, IRepositoryWrapper repository)
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
            var listaDTO = new List<SerieSapDTO>();
            var listaEntity = new List<SerieSapEntity>();

            try
            {
                listaEntity = await _dataContext.SerieSap.FromSqlRaw("[FIB_WEB_GE_SP_GetListSerie]").ToListAsync();
                listaDTO = _mapper.Map<List<SerieSapDTO>>(listaEntity);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(listaDTO);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListByFormularioIdUsuario([FromQuery] string formulario, int idUsuario)
        {
            var objectGetAll = await _repository.SerieSap.GetListByFormularioIdUsuario(formulario, idUsuario);

            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.dataList);
        }


        [HttpGet("{series}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetNumeroBySerie(int series)
        {
            var response = await _repository.SerieSap.GetNumeroBySerie(series);

            if (response.ResultadoCodigo == -1)
            {
                return BadRequest(response);
            }

            return Ok(response.data);
        }
    }
}
