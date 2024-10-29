using Net.Business.Entities.Sap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Business.DTO.Sap
{
    public class KardexSaldoInicialByPeriodoArticuloFindRequestDTO
    {
        public string Periodo { get; set; }
        public string ItemCode1 { get; set; }
        public string ItemCode2 { get; set; }

        public KardexSaldoInicialByPeriodoArticuloFindEntity RetornaKardexFind()
        {
            return new KardexSaldoInicialByPeriodoArticuloFindEntity
            {
                Periodo = this.Periodo,
                ItemCode1 = this.ItemCode1,
                ItemCode2 = this.ItemCode2,
            };
        }
    }
}
