using System;
using System.IO;
using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Sap;

namespace Net.Data.Sap
{
    public interface IOrdenVentaSapRepository
    {
        Task<ResultadoTransaccion<OrdenVentaSapByFechaEntity>> GetListOrdenVentaSeguimientoByFecha(FiltroRequestEntity value);
        Task<ResultadoTransaccion<MemoryStream>> GetOrdenVentaSeguimientoExcelByFecha(FiltroRequestEntity value);
        Task<ResultadoTransaccion<OrdenVentaSapByFechaEntity>> GetListOrdenVentaSeguimientoDetalladoByFecha(FiltroRequestEntity value);
        Task<ResultadoTransaccion<MemoryStream>> GetOrdenVentaSeguimientoDetalladoExcelByFecha(FiltroRequestEntity value);
        Task<ResultadoTransaccion<OrdenVentaSapByFechaEntity>> GetListOrdenVentaPendienteStockAlmacenProduccionByFecha(FiltroRequestEntity value);
        Task<ResultadoTransaccion<MemoryStream>> GetOrdenVentaPendienteStockAlmacenProduccionExcelByFecha(FiltroRequestEntity value);
        Task<ResultadoTransaccion<OrdenVentaSapByFechaEntity>> GetListOrdenVentaProgramacionByFecha(FiltroRequestEntity value);
        Task<ResultadoTransaccion<MemoryStream>> GetOrdenVentaProgramacionExcelByFecha(FiltroRequestEntity value);
        Task<ResultadoTransaccion<OrdenVentaSodimacSapEntity>> GetListOrdenVentaSodimacPendienteByFiltro(FiltroRequestEntity value);
        Task<ResultadoTransaccion<OrdenVentaSodimacSapEntity>> GetOrdenVentaSodimacPendienteByDocEntry(int docEntry);
    }
}