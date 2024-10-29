using Net.Business.Entities.Web;
namespace Net.Business.DTO.Web
{
    public class LecturaCreateRequestDto
    {
        public string ObjType { get; set; }
        public int DocEntry { get; set; }
        public string WhsCode { get; set; }
        public string Barcode { get; set; }
        public int IdUsuarioCreate { get; set; }

        public LecturaEntity ReturnValue()
        {
            return new LecturaEntity()
            {
                ObjType = this.ObjType,
                DocEntry = this.DocEntry,
                WhsCode = this.WhsCode,
                Barcode = this.Barcode,
                IdUsuarioCreate = IdUsuarioCreate,
            };
        }
    }
}
