﻿using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Sap;
namespace Net.Data.Sap
{
    public interface IVehiculoSapRepository
    {
        Task<ResultadoTransaccion<VehiculoSapEntity>> GetListByFiltro(FiltroRequestEntity value);
    }
}
