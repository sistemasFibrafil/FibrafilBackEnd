using System;
using System.IO;
using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Sap;

namespace Net.Data.Sap
{
    public interface IArticuloSapRepository
    {
        Task<ResultadoTransaccion<ArticuloSapEntity>> GetListByFiltro(FiltroRequestEntity value);
        Task<ResultadoTransaccion<ArticuloSapEntity>> GetByCode(string itemCode);
        Task<ResultadoTransaccion<MovimientoStockSapByFechaSedeEntity>> GetListMovimientoStockByFechaSede(FiltroRequestEntity value);
        Task<ResultadoTransaccion<MemoryStream>> GetListMovimientoStockExcelByFechaSede(FiltroRequestEntity value);
        Task<ResultadoTransaccion<ArticuloSapEntity>> GetListStockGeneralByAlmacen(FiltroRequestEntity value);
        Task<ResultadoTransaccion<MemoryStream>> GetListStockGeneralByAlmacenExcel(FiltroRequestEntity value);
        Task<ResultadoTransaccion<ArticuloSapEntity>> GetListStockGeneralDetalladoAlmacenByAlmacen(FiltroRequestEntity value);
        Task<ResultadoTransaccion<MemoryStream>> GetListStockGeneralDetalladoAlmacenByAlmacenExcel(FiltroRequestEntity value);
        Task<ResultadoTransaccion<ArticuloVentaStockByGrupoSubGrupo>> GetListArticuloVentaStockByGrupoSubGrupo(FiltroRequestEntity value);
        Task<ResultadoTransaccion<MemoryStream>> GetListArticuloVentaStockExcelByGrupoSubGrupo(FiltroRequestEntity value);
        Task<ResultadoTransaccion<ArticuloVentaByGrupoSubGrupoEstado>> GetListArticuloVentaByGrupoSubGrupoEstado(FiltroRequestEntity value);
        Task<ResultadoTransaccion<MemoryStream>> GetListArticuloVentaExcelByGrupoSubGrupoEstado(FiltroRequestEntity value);
        Task<ResultadoTransaccion<ArticuloSapForSodimacBySkuItemEntity>> GetArticuloForOrdenVentaSodimacBySku(ArticuloSapForSodimacBySkuEntity value);
        Task<ResultadoTransaccion<ArticuloDocumentoSapEntity>> GetArticuloVentaByCode(FiltroRequestEntity value);
    }
}
