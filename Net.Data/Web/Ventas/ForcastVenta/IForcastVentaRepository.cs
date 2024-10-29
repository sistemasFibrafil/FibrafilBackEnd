using System;
using System.IO;
using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Web;
namespace Net.Data.Web
{
    public interface IForcastVentaRepository
    {
        Task<ResultadoTransaccion<MemoryStream>> GetForcastVentaPlantillaExcel();
        Task<ResultadoTransaccion<ForcastVentaEntity>> SetImport(ForcastVentaEntity value);
        Task<ResultadoTransaccion<ForcastVentaEntity>> SetCreate(ForcastVentaEntity value);
        Task<ResultadoTransaccion<ForcastVentaEntity>> SetUpdate(ForcastVentaEntity value);
        Task<ResultadoTransaccion<ForcastVentaEntity>> SetDelete(int idForcastVenta);
        Task<ResultadoTransaccion<ForcastVentaEntity>> GetById(int idForcastVenta);
        Task<ResultadoTransaccion<ForcastVentaByFechaEntity>> GetListForcastVentaByFecha(DateTime fecInicial, DateTime fecFinal);
    }
}
