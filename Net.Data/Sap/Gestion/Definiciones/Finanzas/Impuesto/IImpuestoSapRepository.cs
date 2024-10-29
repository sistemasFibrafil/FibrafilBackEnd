using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Sap;
namespace Net.Data.Sap
{
    public interface IImpuestoSapRepository
    {
        Task<ResultadoTransaccion<ImpuestoSapEntity>> GetListByFiltro(FiltroRequestEntity value);
        Task<ResultadoTransaccion<ImpuestoSapEntity>> GetByEmpleadoVenta(FiltroRequestEntity value);
    }
}
