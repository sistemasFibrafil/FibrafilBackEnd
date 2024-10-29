using AutoMapper;
using Net.Business.DTO.Sap;
using Net.Business.DTO.Web;
using Net.Business.Entities.Sap;
using Net.Business.Entities.Web;

namespace Net.Business.Services.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // GENERAL
            CreateMap<SerieSapEntity, SerieSapDTO>();
            CreateMap<SerieSapDTO, SerieSapEntity>();

            CreateMap<SerieSapEntity, SerieSapDTO>();
            CreateMap<SerieSapDTO, SerieSapEntity>();


            // ARTICULOS
            CreateMap<GrupoArticuloSapEntity, GrupoArticuloSapDTO>();
            CreateMap<GrupoArticuloSapDTO, GrupoArticuloSapEntity>();

            CreateMap<SubGrupoArticuloSapEntity, SubGrupoArticuloSapDTO>();
            CreateMap<SubGrupoArticuloSapDTO, SubGrupoArticuloSapEntity>();

            CreateMap<SubGrupoArticulo2SapEntity, SubGrupoArticulo2SapDTO>();
            CreateMap<SubGrupoArticulo2SapDTO, SubGrupoArticulo2SapEntity>();

            CreateMap<SedeSapEntity, SedeSapDTO>();
            CreateMap<SedeSapDTO, SedeSapEntity>();



            // SOCIO DE NEGOCIOS
            CreateMap<GrupoSocioNegocioSapEntity, GrupoSocioNegocioSapDTO>();
            CreateMap<GrupoSocioNegocioSapDTO, GrupoSocioNegocioSapEntity>();

            CreateMap<SectorSocioNegocioSapEntity, SectorSocioNegocioSapDTO>();
            CreateMap<SectorSocioNegocioSapDTO, SectorSocioNegocioSapEntity>();

            // VENTAS
            CreateMap<ForcastVentaConSinOcEntity, ForcastVentaConSinOcDTO>();
            CreateMap<ForcastVentaConSinOcDTO, ForcastVentaConSinOcEntity>();

            CreateMap<ForcastVentaNegocioEntity, ForcastVentaNegocioDTO>();
            CreateMap<ForcastVentaNegocioDTO, ForcastVentaNegocioEntity>();

            CreateMap<ForcastVentaEstadoEntity, ForcastVentaEstadoDTO>();
            CreateMap<ForcastVentaEstadoDTO, ForcastVentaEstadoEntity>();
        }
    }
}
