using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Web;

namespace Net.Data.Web
{
    public interface ISerieRepository
    {
        Task<ResultadoTransaccion<SerieEntity>> GetListByIdSede(int idSede);
        Task<ResultadoTransaccion<SerieEntity>> GetListBySerieIdUsuario(int series, int idUsuario);
        Task<ResultadoTransaccion<SerieEntity>> GetNumeroBySerie(string serieSunat);
        Task<ResultadoTransaccion<SerieEntity>> SetCreate(SerieEntity value);
        Task<ResultadoTransaccion<SerieEntity>> SetUpdate(SerieEntity value);
        Task<ResultadoTransaccion<SerieEntity>> SetDelete(SerieEntity value);
    }
}
