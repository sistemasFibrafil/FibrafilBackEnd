using Net.Data.Sap;
using Net.Data.Web;
using Net.Connection;
using System.Net.Http;
using Net.Data.Sap.Gestion.TipoCambio;
using Microsoft.Extensions.Configuration;
namespace Net.Data
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly IConnectionSql _repoContext;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;

        /// <summary>
        /// GESTION
        /// </summary>
        private ISerieRepository _Serie;
        private ILocalRepository _Local;
        private ISerieSapRepository _SerieSap;
        private IMonedaSapRepository _MonedaSap;
        private IAlmacenSapRepository _AlmacenSap;
        private IImpuestoSapRepository _ImpuestoSap;
        private IVehiculoSapRepository _VehiculoSap;
        private IConductorSapRepository _ConductorSap;
        private ITipoCambioSapRepository _TipoCambioSap;
        private IEstadoDocumentoRepository _EstadoDocumento;
        private IValorDefinidoSapRepository _ValorDefinidoSap;
        private IEmpleadoVentaSapRepository _EmpleadoVentaSap;
        private ICondidcionPagoSapRepository _CondidcionPagoSap;
        private IDetalleSociedadSapRepository _DetalleSociedadSap;


        /// <summary>
        /// SOCIOS DE NEGOCIOS
        /// </summary>
        private ISocioNegocioRepository _SocioNegocio;
        private IDireccionSapRepository _DireccionSap;
        private IPersonaContactoSapRepository _PersonaContactoSap;


        /// <summary>
        /// INVENTARIO
        /// </summary>
        private IKardexRepository _Kardex;
        private ILecturaRepository _Lectura;
        private IArticuloSapRepository _ArticuloSap;
        private IDocumentoLecturaSapRepository _DocumentoLectura;
        private ISolicitudTrasladoRepository _SolicitudTraslado;


        /// <summary>
        /// VENTAS
        /// </summary>
        private IEntregaSapRepository _EntregaSap;
        private IOrdenVentaRepository _OrdenVenta;
        private IForcastVentaRepository _ForcastVenta;
        private IEntregaVentaRepository _EntregaVenta;
        private IPickingVentaRepository _PackingVenta;
        private IOrdenVentaSapRepository _OrdenVentaSap;
        private IFacturaVentaSapRepository _FacturaVentaSap;
        private IOrdenVentaSodimacRepository _OrdenVentaSodimac;
        private IFacturacionElectronicaSapRepository _FacturacionElectronicaSap;


        /// <summary>
        /// PRODUCCIÓN
        /// </summary>
        private IOrdenFabricacionSapRepository _OrdenFabricacionSap;
        private IOrdenMantenimientoWebRepository _OrdenMantenimientoWeb;
        private IAreaSolicitanteProduccionRepository _AreaSolicitanteProduccion;


        /// <summary>
        /// PRODUCCIÓN
        /// </summary>
        private IPagoRecibidoSapRepository _PagoRecibidoSap;


        public RepositoryWrapper(IConnectionSql repoContext, IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            _repoContext = repoContext;
            _configuration = configuration;
            _clientFactory = clientFactory;
        }


        /// <summary>
        /// GESTIÓN
        /// </summary>
        public ISerieRepository Serie
        {
            get
            {
                if (_Serie == null)
                {
                    _Serie = new SerieRepository(_repoContext, _configuration);
                }
                return _Serie;
            }
        }
        public ILocalRepository Local
        {
            get
            {
                if (_Local == null)
                {
                    _Local = new LocalRepository(_repoContext, _configuration);
                }
                return _Local;
            }
        }
        public ISerieSapRepository SerieSap
        {
            get
            {
                if (_SerieSap == null)
                {
                    _SerieSap = new SerieSapRepository(_repoContext, _configuration);
                }
                return _SerieSap;
            }
        }
        public IMonedaSapRepository MonedaSap
        {
            get
            {
                if (_MonedaSap == null)
                {
                    _MonedaSap = new MonedaSapRepository(_repoContext, _configuration);
                }
                return _MonedaSap;
            }
        }
        public IAlmacenSapRepository AlmacenSap
        {
            get
            {
                if (_AlmacenSap == null)
                {
                    _AlmacenSap = new AlmacenSapRepository(_repoContext, _configuration);
                }
                return _AlmacenSap;
            }
        }
        public IImpuestoSapRepository ImpuestoSap
        {
            get
            {
                if (_ImpuestoSap == null)
                {
                    _ImpuestoSap = new ImpuestoSapRepository(_repoContext, _configuration);
                }
                return _ImpuestoSap;
            }
        }
        public IVehiculoSapRepository VehiculoSap
        {
            get
            {
                if (_VehiculoSap == null)
                {
                    _VehiculoSap = new VehiculoSapRepository(_repoContext, _configuration);
                }
                return _VehiculoSap;
            }
        }
        public IConductorSapRepository ConductorSap
        {
            get
            {
                if (_ConductorSap == null)
                {
                    _ConductorSap = new ConductorSapRepository(_repoContext, _configuration);
                }
                return _ConductorSap;
            }
        }
        public ITipoCambioSapRepository TipoCambioSap
        {
            get
            {
                if (_TipoCambioSap == null)
                {
                    _TipoCambioSap = new TipoCambioSapRepository(_repoContext, _configuration);
                }
                return _TipoCambioSap;
            }
        }
        public IEstadoDocumentoRepository EstadoDocumento
        {
            get
            {
                if (_EstadoDocumento == null)
                {
                    _EstadoDocumento = new EstadoDocumentoRepository(_repoContext, _configuration);
                }
                return _EstadoDocumento;
            }
        }
        public IValorDefinidoSapRepository ValorDefinidoSap
        {
            get
            {
                if (_ValorDefinidoSap == null)
                {
                    _ValorDefinidoSap = new ValorDefinidoSapRepository(_repoContext, _configuration);
                }
                return _ValorDefinidoSap;
            }
        }
        public IEmpleadoVentaSapRepository EmpleadoVentaSap
        {
            get
            {
                if (_EmpleadoVentaSap == null)
                {
                    _EmpleadoVentaSap = new EmpleadoVentaSapRepository(_repoContext, _configuration);
                }
                return _EmpleadoVentaSap;
            }
        }
        public ICondidcionPagoSapRepository CondidcionPagoSap
        {
            get
            {
                if (_CondidcionPagoSap == null)
                {
                    _CondidcionPagoSap = new CondidcionPagoSapRepository(_repoContext, _configuration);
                }
                return _CondidcionPagoSap;
            }
        }
        public IDetalleSociedadSapRepository DetalleSociedadSap
        {
            get
            {
                if (_DetalleSociedadSap == null)
                {
                    _DetalleSociedadSap = new DetalleSociedadSapRepository(_repoContext, _configuration);
                }
                return _DetalleSociedadSap;
            }
        }



        /// <summary>
        /// SOCIOS DE NEGOCIOS
        /// </summary>
        public ISocioNegocioRepository SocioNegocio
        {
            get
            {
                if (_SocioNegocio == null)
                {
                    _SocioNegocio = new SocioNegocioRepository(_repoContext, _configuration);
                }
                return _SocioNegocio;
            }
        }
        public IDireccionSapRepository DireccionSap
        {
            get
            {
                if (_DireccionSap == null)
                {
                    _DireccionSap = new DireccionSapRepository(_repoContext, _configuration);
                }
                return _DireccionSap;
            }
        }
        public IPersonaContactoSapRepository PersonaContactoSap
        {
            get
            {
                if (_PersonaContactoSap == null)
                {
                    _PersonaContactoSap = new PersonaContactoSapRepository(_repoContext, _configuration);
                }
                return _PersonaContactoSap;
            }
        }



        /// <summary>
        /// INVENTARIO
        /// </summary>
        public IKardexRepository Kardex
        {
            get
            {
                if (_Kardex == null)
                {
                    _Kardex = new KardexRepository(_repoContext, _configuration);
                }
                return _Kardex;
            }
        }
        public ILecturaRepository Lectura
        {
            get
            {
                if (_Lectura == null)
                {
                    _Lectura = new LecturaRepository(_repoContext, _configuration);
                }
                return _Lectura;
            }
        }
        public IArticuloSapRepository ArticuloSap
        {
            get
            {
                if (_ArticuloSap == null)
                {
                    _ArticuloSap = new ArticuloSapRepository(_repoContext, _configuration);
                }
                return _ArticuloSap;
            }
        }
        public IDocumentoLecturaSapRepository DocumentoLectura
        {
            get
            {
                if (_DocumentoLectura == null)
                {
                    _DocumentoLectura = new DocumentoLecturaSapRepository(_repoContext, _configuration);
                }
                return _DocumentoLectura;
            }
        }
        public ISolicitudTrasladoRepository SolicitudTraslado
        {
            get
            {
                if (_SolicitudTraslado == null)
                {
                    _SolicitudTraslado = new SolicitudTrasladoRepository(_repoContext, _configuration);
                }
                return _SolicitudTraslado;
            }
        }



        /// <summary>
        /// VENTAS
        /// </summary>
        public IEntregaSapRepository EntregaSap
        {
            get
            {
                if (_EntregaSap == null)
                {
                    _EntregaSap = new EntregaSapRepository(_repoContext, _configuration);
                }
                return _EntregaSap;
            }
        }
        public IOrdenVentaRepository OrdenVenta
        {
            get
            {
                if (_OrdenVenta == null)
                {
                    _OrdenVenta = new OrdenVentaRepository(_repoContext, _configuration);
                }
                return _OrdenVenta;
            }
        }
        public IForcastVentaRepository ForcastVenta
        {
            get
            {
                if (_ForcastVenta == null)
                {
                    _ForcastVenta = new ForcastventaRepository(_repoContext, _configuration);
                }
                return _ForcastVenta;
            }
        }
        public IEntregaVentaRepository EntregaVenta
        {
            get
            {
                if (_EntregaVenta == null)
                {
                    _EntregaVenta = new EntregaVentaRepository(_repoContext, _configuration);
                }
                return _EntregaVenta;
            }
        }
        public IPickingVentaRepository PickingVenta
        {
            get
            {
                if (_PackingVenta == null)
                {
                    _PackingVenta = new PickingVentaRepository(_repoContext, _configuration);
                }
                return _PackingVenta;
            }
        }
        public IOrdenVentaSapRepository OrdenVentaSap
        {
            get
            {
                if (_OrdenVentaSap == null)
                {
                    _OrdenVentaSap = new OrdenVentaSapRepository(_repoContext, _configuration);
                }
                return _OrdenVentaSap;
            }
        }
        public IFacturaVentaSapRepository FacturaVentaSap
        {
            get
            {
                if (_FacturaVentaSap == null)
                {
                    _FacturaVentaSap = new FacturaVentaSapRepository(_repoContext, _configuration);
                }
                return _FacturaVentaSap;
            }
        }
        public IOrdenVentaSodimacRepository OrdenVentaSodimac
        {
            get
            {
                if (_OrdenVentaSodimac == null)
                {
                    _OrdenVentaSodimac = new OrdenVentaSodimacRepository(_repoContext, _configuration);
                }
                return _OrdenVentaSodimac;
            }
        }
        public IFacturacionElectronicaSapRepository FacturacionElectronicaSap
        {
            get
            {
                if (_FacturacionElectronicaSap == null)
                {
                    _FacturacionElectronicaSap = new FacturacionElectronicaSapRepository(_repoContext, _configuration);
                }
                return _FacturacionElectronicaSap;
            }
        }
        
        

        /// <summary>
        /// PRODUCCIÓN
        /// </summary>
        public IOrdenFabricacionSapRepository OrdenFabricacionSap
        {
            get
            {
                if (_OrdenFabricacionSap == null)
                {
                    _OrdenFabricacionSap = new OrdenFabricacionSapRepository(_repoContext, _configuration);
                }
                return _OrdenFabricacionSap;
            }
        }
        public IOrdenMantenimientoWebRepository OrdenMantenimientoWeb
        {
            get
            {
                if (_OrdenMantenimientoWeb == null)
                {
                    _OrdenMantenimientoWeb = new OrdenMantenimientoSapRepository(_repoContext, _configuration);
                }
                return _OrdenMantenimientoWeb;
            }
        }
        public IAreaSolicitanteProduccionRepository AreaSolicitanteProduccion
        {
            get
            {
                if (_AreaSolicitanteProduccion == null)
                {
                    _AreaSolicitanteProduccion = new AreaSolicitanteProduccionRepository(_repoContext, _configuration);
                }
                return _AreaSolicitanteProduccion;
            }
        }



        /// <summary>
        /// GESTION DE BANCOS
        /// </summary>
        public IPagoRecibidoSapRepository PagoRecibidoSap
        {
            get
            {
                if (_PagoRecibidoSap == null)
                {
                    _PagoRecibidoSap = new PagoRecibidoSapRepository(_repoContext, _configuration);
                }
                return _PagoRecibidoSap;
            }
        }
    }
}
