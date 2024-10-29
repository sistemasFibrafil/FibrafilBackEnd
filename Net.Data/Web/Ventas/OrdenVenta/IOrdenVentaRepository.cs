using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Web;
namespace Net.Data.Web
{
    public interface IOrdenVentaRepository
    {
        Task<ResultadoTransaccion<OrdenVentaEntity>> GetNumero();
        Task<ResultadoTransaccion<OrdenVentaEntity>> SetCreate(OrdenVentaEntity value);
    }
}
