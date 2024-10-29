using System.IO;
using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Sap;

namespace Net.Data.Sap
{
    public interface ISocioNegocioRepository
    {
        Task<ResultadoTransaccion<SocioNegocioSapEntity>> GetListByFiltro(FiltroRequestEntity value);
        Task<ResultadoTransaccion<SocioNegocioSapEntity>> GetByCardCode(string cardCode);
        Task<ResultadoTransaccion<SocioNegocioSapEntity>> GetLitClienteBySectorEstado(string sector, string estado, string filtro);
        Task<ResultadoTransaccion<MemoryStream>> GetLitClienteExcelBySectorEstado(string sector, string estado, string filtro);
    }
}
