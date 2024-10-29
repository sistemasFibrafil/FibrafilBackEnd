using System;
using System.IO;
using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Sap;

namespace Net.Data.Sap
{
    public interface IOrdenFabricacionSapRepository
    {
        Task<ResultadoTransaccion<OrdenFabricacionGeneralSapBySedeEntity>> GetListOrdenFabricacionGeneralBySede(DateTime fecInicial, DateTime fecFinal, string location);
        Task<ResultadoTransaccion<MemoryStream>> GetOrdenFabricacionGeneralExcelBySede(DateTime fecInicial, DateTime fecFinal, string location);
    }
}
