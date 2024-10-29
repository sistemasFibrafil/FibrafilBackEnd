using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Web;
namespace Net.Data.Web
{
    public interface ILocalRepository
    {
        Task<ResultadoTransaccion<LocalEntity>> GetListByFiltro(FiltroRequestEntity value);
        Task<ResultadoTransaccion<LocalEntity>> GetByNumLocal(string numLocal);
        Task<ResultadoTransaccion<LocalEntity>> SetCreate(LocalEntity value);
        Task<ResultadoTransaccion<LocalEntity>> SetUpdate(LocalEntity value);
        Task<ResultadoTransaccion<LocalEntity>> SetDelete(int numLocal);
    }
}
