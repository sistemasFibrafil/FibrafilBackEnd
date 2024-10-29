using System;
using System.IO;
using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Sap;

namespace Net.Data.Sap
{
    public interface IFacturaVentaSapRepository
    {
        Task<ResultadoTransaccion<VentaProyeccionByFechaEntity>> GetListVentaProyeccionByFecha(DateTime fechInicial, DateTime fecFinal);
        Task<ResultadoTransaccion<VentaResumenByFechaGrupoEntity>> GetListVentaResumenByFechaGrupo1(DateTime fechInicial, DateTime fecFinal, string grupo);
        Task<ResultadoTransaccion<VentaResumenByFechaGrupoEntity>> GetListVentaResumenByFechaGrupo2(DateTime fechInicial, DateTime fecFinal, string grupo);
        Task<ResultadoTransaccion<VentaResumenByFechaGrupoEntity>> GetListVentaResumenByFechaGrupo3(DateTime fechInicial, DateTime fecFinal, string grupo);
        Task<ResultadoTransaccion<MemoryStream>> GetVentaResumenExcelByFechaGrupo(DateTime fecInicial, DateTime fecFinal, string grupo);
        Task<ResultadoTransaccion<VentaByFechaSlpCodeEntity>> GetListVentaByFechaAndSlpCode(FiltroRequestEntity value);
        Task<ResultadoTransaccion<MemoryStream>> GetListVentaExcelByFechaAndSlpCode(FiltroRequestEntity value);
        Task<ResultadoTransaccion<FacturaVentaByFechaEntity>> GetListFacturaVentaByFecha(FiltroRequestEntity value);
        Task<ResultadoTransaccion<MemoryStream>> GetListFacturaVentaExcelByFecha(FiltroRequestEntity value);
    }
}
