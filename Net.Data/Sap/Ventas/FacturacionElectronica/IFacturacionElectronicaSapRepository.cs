using System;
using System.Text;
using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Sap;

namespace Net.Data.Sap
{
    public interface IFacturacionElectronicaSapRepository
    {
        Task<ResultadoTransaccion<Facturas>> GetListFacturaElectronica(string docNum);
        Task<ResultadoTransaccion<FacturacionElectronicaSapEntity>> GetListGuiaElectronicaByFechaAndNumero(FiltroRequestEntity value);
        Task<ResultadoTransaccion<FacturacionElectronicaSapEntity>> EnviarGuiaElectronica(FacturacionElectronicaSapEntity value);
        Task<ResultadoTransaccion<FacturacionElectronicaSapEntity>> GetListGuiaInternaElectronicaByFechaAndNumero(FiltroRequestEntity value);
        Task<ResultadoTransaccion<FacturacionElectronicaSapEntity>> EnviarGuiaInternaElectronica(FacturacionElectronicaSapEntity value);
    }
}
