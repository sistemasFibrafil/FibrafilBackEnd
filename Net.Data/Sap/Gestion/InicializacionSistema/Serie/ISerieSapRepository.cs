using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Sap;

namespace Net.Data.Sap
{
    public interface ISerieSapRepository
    {
        Task<ResultadoTransaccion<SerieSapEntity>> GetListByFormularioIdUsuario(string formulario, int idUsuario);
        Task<ResultadoTransaccion<SerieSapEntity>> GetNumeroBySerie(int series);
    }
}
