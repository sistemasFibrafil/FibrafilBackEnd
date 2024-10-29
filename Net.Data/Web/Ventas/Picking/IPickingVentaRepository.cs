using System.IO;
using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Web;
using System;

namespace Net.Data.Sap
{
    public interface IPickingVentaRepository
    {
        Task<ResultadoTransaccion<PickingVentaEntity>> GetPickingNumero();
        Task<ResultadoTransaccion<PickingVentaByFiltroEntity>> GetListPickingVentaByFiltro(DateTime fecInicial, DateTime fecFinal);
        Task<ResultadoTransaccion<PickingVentaByIdPicking>> GetPickingVentaByIdPicking(int idPicking);
        Task<ResultadoTransaccion<PickingVentaItemEntity>> GetListPickingVentaItemByIdPicking(int idPicking);
        Task<ResultadoTransaccion<PickingVentaEntity>> SetCreate(PickingVentaEntity value);
        Task<ResultadoTransaccion<PickingVentaEntity>> SetUpdate(PickingVentaEntity value);
        Task<ResultadoTransaccion<PickingVentaEntity>> SetDelete(PickingVentaEntity value);
        Task<ResultadoTransaccion<PickingVentaItemEntity>> SetDeleteItem(int idPickingItem);
        Task<ResultadoTransaccion<PickingVentaItemEntity>> SetDeleteItemAll(PickingVentaItemEntity value);
        Task<ResultadoTransaccion<PickingVentaForEntregaByIdPicking>> GetPickingVentaForEntregaByIdPicking(int idPicking);
        Task<ResultadoTransaccion<MemoryStream>> GetListPickingPdfByDocEntry(int docEntry);
    }
}
