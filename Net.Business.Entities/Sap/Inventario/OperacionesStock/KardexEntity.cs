using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Business.Entities.Sap
{
    public class KardexEntity
    {
        public int TransNum { get; set; }
    }

    public class KardexSaldoInicialByPeriodoArticuloEntity
    {
        public DateTime FecSaldoInicial { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string WhsCode { get; set; }
        public string Cuenta { get; set; }
        public decimal CantidadTotalSaldoFinal { get; set; }
        public decimal CostoTotalSaldoFinal { get; set; }
        public decimal CostoUnitario { get; set; }
    }

    public class KardexSaldoInicialByPeriodoArticuloFindEntity
    {
        public string Periodo { get; set; }
        public string ItemCode1 { get; set; }
        public string ItemCode2 { get; set; }
    }
}
