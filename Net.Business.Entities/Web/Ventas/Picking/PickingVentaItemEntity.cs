namespace Net.Business.Entities.Web
{
    public class PickingVentaItemEntity
    {
        public int IdPickingItem { get; set; }
        public int IdPicking { get; set; }
        public int LineNumItem { get; set; }
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public int LineNum { get; set; }
        public string ObjType { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public string WhsCode { get; set; }
        public string CodEstado { get; set; }
        public int IdUsuario { get; set; }
    }

    public class PickingVentaBarCodeEntity
    {
        public int IdPickingBarCode { get; set; }
        public int IdPickingItem { get; set; }
        public int LineNumBarCode { get; set; }
        public string BarCode { get; set; }
        public string UnitMsr { get; set; }
        public decimal Quantity { get; set; }
        public decimal Peso { get; set; }
    }

    public class PickingVentaItemByIdPickingEntity
    {
        public int IdPicking { get; set; }
        
        public int IdPickingItem { get; set; }
        public int LineNumItem { get; set; }
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public int LineNum { get; set; }
        public string ObjType { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public string WhsCode { get; set; }

        public int IdPickingBarCode { get; set; }
        public int LineNumBarCode { get; set; }
        public string BarCode { get; set; }
        public string UnitMsr { get; set; }
        public decimal Quantity { get; set; }
        public decimal Peso { get; set; }

        public string CodEstado { get; set; }
        public int IdUsuario { get; set; }
    }

    public class PickingVentaItem1Entity
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public string CardName { get; set; }
        public string Contenedor { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
    }

    public class PickingVentaItem2Entity
    {
        public int Id { get; set; }
        public string CodeBar1 { get; set; }
        public string CodeBar2 { get; set; }
        public string CodeBar3 { get; set; }
        public string CodeBar4 { get; set; }
        public int TotalItem { get; set; }
        public decimal PesoTotal { get; set; }
    }

    public class PickingVentaItemForEntregaByIdPickingEntity
    {
        public int IdPicking { get; set; }
        public int IdPickingItem { get; set; }
        public int LineNumItem { get; set; }
        public int DocEntry { get; set; }
        public int LineNum { get; set; }
        public string ObjType { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public string WhsCode { get; set; }
        public string UnitMsr { get; set; }
        public decimal Quantity { get; set; }
        public decimal Peso { get; set; }
        public string TaxCode { get; set; }
        public string AcctCode { get; set; }
        public string Currency { get; set; }
        public decimal PriceBefDi { get; set; }
        public decimal DiscPrcnt { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }
}
