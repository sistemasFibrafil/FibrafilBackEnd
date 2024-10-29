using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Sap;
namespace Net.Data.Sap
{
    public interface IMonedaSapRepository
    {
        Task<ResultadoTransaccion<MonedaSapEntity>> GetListByFiltro(FiltroRequestEntity value);
        Task<ResultadoTransaccion<MonedaSapEntity>> GetByCode(FiltroRequestEntity value);
    }
}
