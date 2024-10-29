using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Sap;
namespace Net.Data.Sap
{
    public interface IEmpleadoVentaSapRepository
    {
        Task<ResultadoTransaccion<EmpleadoVentaSapEntity>> GetList();
        Task<ResultadoTransaccion<EmpleadoVentaSapEntity>> GetListByFiltro(FiltroRequestEntity value);
        Task<ResultadoTransaccion<EmpleadoVentaSapEntity>> GetById(int id);
    }
}
