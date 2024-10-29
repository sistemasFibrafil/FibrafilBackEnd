using System;
using System.IO;
using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Sap;

namespace Net.Data.Sap
{
    public interface IPagoRecibidoSapRepository
    {
        Task<ResultadoTransaccion<CobranzaCarteraVencidaByFechaCorteSapEntity>> GetListCobranzaCarteraVencidaByFechaCorte(FiltroRequestEntity value);
        Task<ResultadoTransaccion<MemoryStream>> GetListCobranzaCarteraVencidaExcelByFechaCorte(FiltroRequestEntity value);
    }
}
