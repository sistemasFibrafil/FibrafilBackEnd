namespace Net.Business.Entities.Sap
{
    public class SolicitudTrasladoSapEntity
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
    }

    public class SolicitudTrasladoDetalleSapEntity
    {
        public int DocEntry { get; set; }
        public int LineNum { get; set; }
        public string ObjType { get; set; }
        public int U_FIB_BASEENTRY { get; set; }
        public int U_FIB_BASELINENUM { get; set; }
    }
}
