using System.Collections.Generic;

namespace Net.Business.Entities.Sap
{
    public class AlmacenSapEntity
    {
        public string WhsCode { get; set; }
        public string WhsName { get; set; }
        public string FullDescr { get; set; }
        public string Inactive { get; set; }
        public decimal OnHand { get; set; }
        public List<AlmacenProduccionSapEntity> lstProd { get; set; } = new List<AlmacenProduccionSapEntity>();


        /// <summary>
        /// Filtros
        /// </summary>
        public string Demandante { get; set; }
        public string ItemCode { get; set; }
        public string Filtro { get; set; }
    }

    public class AlmacenProduccionSapEntity
    {
        public string WhsCode { get; set; }
        public string WhsName { get; set; }
        public string FullDescr { get; set; }
        public string Inactive { get; set; }
        public decimal OnHand { get; set; }
    }
}
