using Net.Business.Entities.Web;

namespace Net.Business.DTO.Web
{
    public class PickingVentaItemDeleteRequestDTO
    {
        public int IdPicking { get; set; }
        public int DocEntry { get; set; }
        public string ObjType { get; set; }
        public int LineNum { get; set; }
        public string ItemCode { get; set; }
        public int IdUsuario { get; set; }

        public PickingVentaItemEntity RetornaPickingItem()
        {
            return new PickingVentaItemEntity
            {
                IdPicking = this.IdPicking,
                DocEntry = this.DocEntry,
                ObjType = this.ObjType,
                LineNum = this.LineNum,
                ItemCode = this.ItemCode,
                IdUsuario = this.IdUsuario
            };
        }
    }
}
