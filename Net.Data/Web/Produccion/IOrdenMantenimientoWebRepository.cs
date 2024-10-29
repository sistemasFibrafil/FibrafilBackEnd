using System;
using System.IO;
using System.Text;
using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Web;
using System.Collections.Generic;

namespace Net.Data.Web
{
    public interface IOrdenMantenimientoWebRepository
    {
        Task<ResultadoTransaccion<OrdenMantenimientoByFechaAndIdEstadoAndNumeroEntity>> GetListOrdenMatenimientoByFechaAndIdEstadoAndNumero(DateTime? fecInicial, DateTime? fecFinal, string idEstado, string numero);
        Task<ResultadoTransaccion<MemoryStream>> GetOrdenMatenimientoExcelByFechaAndIdEstadoAndNumero(DateTime? fecInicial, DateTime? fecFinal, string idEstado, string numero);
    }
}
