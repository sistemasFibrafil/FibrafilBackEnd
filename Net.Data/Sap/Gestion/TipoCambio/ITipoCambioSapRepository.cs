using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Sap;
namespace Net.Data.Sap.Gestion.TipoCambio
{
    public interface ITipoCambioSapRepository
    {
        Task<ResultadoTransaccion<TipoCambioSapEntity>> GetByFechaCode(FiltroRequestEntity value);
    }
}
