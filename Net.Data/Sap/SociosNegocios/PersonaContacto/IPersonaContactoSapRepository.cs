using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Sap;

namespace Net.Data.Sap
{
    public interface IPersonaContactoSapRepository
    {
        Task<ResultadoTransaccion<PersonaContactoSapEntity>> GetListByFiltro(FiltroRequestEntity value);
        Task<ResultadoTransaccion<PersonaContactoSapEntity>> GetById(FiltroRequestEntity value);
    }
}
