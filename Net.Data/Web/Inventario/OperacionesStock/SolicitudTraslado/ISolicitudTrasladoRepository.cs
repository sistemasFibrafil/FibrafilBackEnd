using System;
using System.Linq;
using System.Text;
using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Web;
using System.Collections.Generic;

namespace Net.Data.Web
{
    public interface ISolicitudTrasladoRepository
    {
        Task<ResultadoTransaccion<SolicitudTrasladoEntity>> GetNumero();
        Task<ResultadoTransaccion<SolicitudTrasladoEntity>> GetListByFiltro(FiltroRequestEntity value);
        Task<ResultadoTransaccion<SolicitudTrasladoEntity>> GetById(int id);
        Task<ResultadoTransaccion<SolicitudTrasladoEntity>> SetCreate(SolicitudTrasladoEntity value);
        Task<ResultadoTransaccion<SolicitudTrasladoEntity>> SetUpdate(SolicitudTrasladoEntity value);
        Task<ResultadoTransaccion<SolicitudTrasladoEntity>> SetClose(SolicitudTrasladoEntity value);
    }
}
