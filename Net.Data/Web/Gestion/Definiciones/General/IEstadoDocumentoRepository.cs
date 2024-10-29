using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Web;

namespace Net.Data.Web
{
    public interface IEstadoDocumentoRepository
    {
        Task<ResultadoTransaccion<EstadoDocumentoEntity>> GetListAll();
    }
}
