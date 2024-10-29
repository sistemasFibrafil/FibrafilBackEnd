namespace Net.Business.DTO.Web
{
    public class ForcastVentaImportDTO
    {
        public int IdForcastVenta { get; set; }
        public int IdConSinOc { get; set; }
        public int IdNegocio { get; set; }
        public int ItmsGrpCod { get; set; }
        public string ItemCode { get; set; }
        public int DocNum { get; set; }
        public string CardCode { get; set; }
        public string FecRegistro { get; set; }
        public string UnidadMedida { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Kg { get; set; }
        public decimal Precio { get; set; }
        public string CodEstado { get; set; }
    }
}
