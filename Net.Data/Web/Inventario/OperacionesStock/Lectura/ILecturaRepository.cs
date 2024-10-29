using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Web;
namespace Net.Data.Web
{
    public interface ILecturaRepository
    {
        Task<ResultadoTransaccion<LecturaEntity>> SetCreate(LecturaEntity value);
        Task<ResultadoTransaccion<LecturaEntity>> SetDeleteMultiple(LecturaEntity value);
        Task<ResultadoTransaccion<LecturaEntity>> SetDelete(int id);
        Task<ResultadoTransaccion<LecturaByObjTypeAndDocEntryEntity>> GetListByObjTypeAndDocEntry(FiltroRequestEntity value);
        Task<ResultadoTransaccion<LecturaEntity>> GetListByFiltro(FiltroRequestEntity value);
        Task<ResultadoTransaccion<LecturaBarcodeByIdAndFiltro>> GetListByDocEntryAndObjTypeAndFiltro(FiltroRequestEntity value);
    }
}
