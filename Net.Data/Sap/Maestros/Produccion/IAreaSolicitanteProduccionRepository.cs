using System;
using System.Text;
using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Web;
using Net.Business.Entities.Sap;
using System.Collections.Generic;

namespace Net.Data.Sap
{
    public interface IAreaSolicitanteProduccionRepository
    {
        Task<ResultadoTransaccion<AreaSolicitanteProduccionEntity>> GetListAll();
    }
}
