using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Sap;
namespace Net.Data.Sap
{
    public interface IAlmacenSapRepository
    {
        Task<ResultadoTransaccion<AlmacenSapEntity>> GetListByEstado(AlmacenSapEntity value);
        Task<ResultadoTransaccion<AlmacenSapEntity>> GetListProduccion();
        Task<ResultadoTransaccion<AlmacenSapEntity>> GetListByFiltro(FiltroRequestEntity value);
        Task<ResultadoTransaccion<ArticuloAlmacenSapEntity>> GetListByWhsCodeAndItemCode(FiltroRequestEntity value);
        Task<ResultadoTransaccion<AlmacenSapEntity>> GetByCode(string code);
    }
}
