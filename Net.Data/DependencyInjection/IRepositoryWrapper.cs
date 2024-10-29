using Net.Data.Sap;
using Net.Data.Sap.Gestion.TipoCambio;
using Net.Data.Web;
namespace Net.Data
{
    public interface IRepositoryWrapper
    {
        /// <summary>
        /// GESTION
        /// </summary>
        ISerieRepository Serie { get; }
        ILocalRepository Local { get; }
        ISerieSapRepository SerieSap { get; }
        IMonedaSapRepository MonedaSap { get; }
        IAlmacenSapRepository AlmacenSap { get; }
        IImpuestoSapRepository ImpuestoSap { get; }
        IVehiculoSapRepository VehiculoSap { get; }
        IConductorSapRepository ConductorSap { get; }
        ITipoCambioSapRepository TipoCambioSap { get; }
        IEstadoDocumentoRepository EstadoDocumento { get; }
        IEmpleadoVentaSapRepository EmpleadoVentaSap { get; }
        IValorDefinidoSapRepository ValorDefinidoSap { get; }
        ICondidcionPagoSapRepository CondidcionPagoSap { get; }
        IDetalleSociedadSapRepository DetalleSociedadSap { get; }


        /// <summary>
        /// SOCIOS DE NEGOCIOS
        /// </summary>
        ISocioNegocioRepository SocioNegocio { get; }
        IDireccionSapRepository DireccionSap { get; }
        IPersonaContactoSapRepository PersonaContactoSap { get; }


        /// <summary>
        /// INVENTARIO
        /// </summary>
        ILecturaRepository Lectura { get;}
        IArticuloSapRepository ArticuloSap { get; }
        IKardexRepository Kardex { get; }
        IDocumentoLecturaSapRepository DocumentoLectura { get; }
        ISolicitudTrasladoRepository SolicitudTraslado { get;}


        /// <summary>
        /// VENTAS
        /// </summary>
        IEntregaSapRepository EntregaSap { get; }
        IOrdenVentaRepository OrdenVenta { get; }
        IForcastVentaRepository ForcastVenta { get; }
        IEntregaVentaRepository EntregaVenta { get; }
        IPickingVentaRepository PickingVenta { get; }
        IOrdenVentaSapRepository OrdenVentaSap { get; }
        IFacturaVentaSapRepository FacturaVentaSap { get; }
        IOrdenVentaSodimacRepository OrdenVentaSodimac { get; }
        IFacturacionElectronicaSapRepository FacturacionElectronicaSap { get; }


        /// <summary>
        /// PRODUCCIÓN
        /// </summary>
        IAreaSolicitanteProduccionRepository AreaSolicitanteProduccion { get; }
        IOrdenFabricacionSapRepository OrdenFabricacionSap { get; }
        IOrdenMantenimientoWebRepository OrdenMantenimientoWeb { get; }


        /// <summary>
        /// GESTION DE BANCOS
        /// </summary>
        IPagoRecibidoSapRepository PagoRecibidoSap { get; }
    }
}
