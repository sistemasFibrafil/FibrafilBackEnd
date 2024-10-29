using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Sap;
namespace Net.Data.Sap
{
    public interface IValorDefinidoSapRepository
    {
        Task<ResultadoTransaccion<ValorDefinidoSapEntity>> GetListByFiltro(FiltroRequestEntity value);
        Task<ResultadoTransaccion<ValorDefinidoSapEntity>> GetByCode(FiltroRequestEntity value);
    }
}
