namespace Net.Business.Entities.Sap
{
    public class EntregaVentaItemSapEntity
    {
        public int IdEntregaVentaItem { get; set; }
        public int IdEntregaVenta { get; set; }
        public int LineNum { get; set; }
        public string ObjType { get; set; }
        public int IdBase { get; set; }
        public int IdBaseItem { get; set; }
        public int BaseType { get; set; }
        public int BaseEntry { get; set; }
        public int BaseLine { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public string WhsCode { get; set; }
        public string UnitMsr { get; set; }
        public double Quantity { get; set; }
        public double Peso { get; set; }
        public string TaxCode { get; set; }
        public string AcctCode { get; set; }
        public string Currency { get; set; }
        public double DiscPrcnt { get; set; }
        public double Price { get; set; }
        public double LineTotal { get; set; }
        public double TotalFrgn { get; set; }
        public double TotalSumSy { get; set; }
    }
}
