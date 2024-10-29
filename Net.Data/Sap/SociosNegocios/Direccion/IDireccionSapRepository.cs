using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Sap;

namespace Net.Data.Sap
{
    public interface IDireccionSapRepository
    {
        Task<ResultadoTransaccion<DireccionSapEntity>> GetListByFiltro(FiltroRequestEntity value);
        Task<ResultadoTransaccion<DireccionSapEntity>> GetByCode(FiltroRequestEntity value);
    }
}
