using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Sap;
namespace Net.Data.Sap
{
    public interface ICondidcionPagoSapRepository
    {
        Task<ResultadoTransaccion<CondicionPagoSapEntity>> GetListByFiltro(FiltroRequestEntity value);
        Task<ResultadoTransaccion<CondicionPagoSapEntity>> GetById(int id);
    }
}
