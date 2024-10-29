using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Web;
using System.IO;
using System;
namespace Net.Data.Web
{
    public interface IOrdenVentaSodimacRepository
    {
        Task<ResultadoTransaccion<OrdenVentaSodimacEntity>> SetCreate(OrdenVentaSodimacEntity value);
        Task<ResultadoTransaccion<OrdenVentaSodimacByFiltroEntity>> GetListOrdenVentaSodimacByFiltro(FiltroRequestEntity value);
        Task<ResultadoTransaccion<OrdenVentaSodimacEntity>> GetOrdenVentaSodimacById(int id);
        Task<ResultadoTransaccion<OrdenVentaSodimaConsultaFiltroEntity>> GetListOrdenVentaSodimacPendienteLpnByFiltro(FiltroRequestEntity value);
        Task<ResultadoTransaccion<OrdenVentaSodimaConsultaFiltroEntity>> GetListOrdenVentaSodimacDetallePendienteLpnByIdAndFiltro(FiltroRequestEntity value);
        Task<ResultadoTransaccion<OrdenVentaSodimaConsultaFiltroEntity>> GetListOrdenVentaSodimacLpnByFiltro(FiltroRequestEntity value);
        Task<ResultadoTransaccion<OrdenVentaSodimaConsultaFiltroEntity>> GetListOrdenVentaSodimacDetalleById(FiltroRequestEntity value);
        Task<ResultadoTransaccion<OrdenVentaSodimacEntity>> SetLpnUpdate(OrdenVentaSodimacEntity value);
        Task<ResultadoTransaccion<MemoryStream>> GetBarcodeLpnPdfById(int id);
        Task<ResultadoTransaccion<MemoryStream>> GetListBarcodeEanPdfByEan(string ean);
        Task<ResultadoTransaccion<OrdenVentaSodimaConsultaFiltroEntity>> GetListOrdenVentaSodimacByFechaNumero(FiltroRequestEntity value);
        Task<ResultadoTransaccion<MemoryStream>> GetListOrdenVentaSodimacExcelByFechaNumero(FiltroRequestEntity value);
        Task<ResultadoTransaccion<OrdenVentaSodimacSelvaByFechaNumeroEntity>> GetListOrdenVentaSodimacSelvaFechaNumero(FiltroRequestEntity value);
        Task<ResultadoTransaccion<MemoryStream>> GetListOrdenVentaSodimacSelvaPdfByFechaNumero(FiltroRequestEntity value);
    }
}
