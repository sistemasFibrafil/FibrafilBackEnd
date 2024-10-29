namespace Net.Business.Entities.Web
{
    public class LecturaEntity
    {
        public int IdLectura { get; set; }
        public int IdBase { get; set; }
        public int LineBase { get; set; }
        public string Numero { get; set; }
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public string ObjType { get; set; }
        public int LineNum { get; set; }
        public string CodEstado { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public string Barcode { get; set; }
        public string WhsCode { get; set; }
        public string ToWhsCode { get; set; } = null;
        public string UnitMsr { get; set; }
        public decimal Quantity { get; set; }
        public decimal Peso { get; set; }
        public int IdUsuarioCreate { get; set; }
    }

    public class LecturaByObjTypeAndDocEntryEntity
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public string ObjType { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public string UnitMsr { get; set; }
        public decimal Quantity { get; set; }
        public decimal Peso { get; set; }
    }

    public class LecturaBarcodeByIdAndFiltro
    {
        public int IdLectura { get; set; }
        public int Line { get; set; }
        public string ItemCode { get; set; }
        public string Barcode { get; set; }
        public decimal Peso { get; set; }
    }
}
