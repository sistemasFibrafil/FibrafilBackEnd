using Net.Business.Entities.Web;
using System.Collections.Generic;

namespace Net.Business.DTO.Web.Ventas.Picking
{
    public class PickingVentaUpdateRequestDTO
    {
        public int IdPicking { get; set; }
        public string CodTipoPicking { get; set; }
        public string CodEstado { get; set; }
        public string Comentarios { get; set; }
        public int IdUsuario { get; set; }
        public List<PickingVentaItemByIdPickingEntity> Item { get; set; }

        public PickingVentaEntity RetornaPickingVenta()
        {
            return new PickingVentaEntity
            {
                IdPicking = this.IdPicking,
                CodTipoPicking = this.CodTipoPicking,
                CodEstado = this.CodEstado,
                Comentarios = this.Comentarios,
                IdUsuario = this.IdUsuario,
                Item = this.Item
            };
        }
    }
}
