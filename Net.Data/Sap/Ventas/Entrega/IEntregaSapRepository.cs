using System;
using System.IO;
using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Sap;
namespace Net.Data.Sap
{
    public interface IEntregaSapRepository
    {
        Task<ResultadoTransaccion<GuiaDespachoMercaderiaSapByFechaSedeEntity>> GetListGuiaDespachoMercaderiaByFechaSede(DateTime fecIni, DateTime fecFin, string location);
        Task<ResultadoTransaccion<MemoryStream>> GetGuiaDespachoMercaderiaExcelByFechaSede(DateTime fecIni, DateTime fecFin, string location);
    }
}
