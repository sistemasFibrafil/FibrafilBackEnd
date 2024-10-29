using Net.Business.Entities.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Business.DTO.Web.Ventas.Picking
{
    public class PickingVentaDeleteRequestDTO
    {
        public int IdPicking { get; set; }
        public int IdUsuario { get; set; }

        public PickingVentaEntity RetornaPicking()
        {
            return new PickingVentaEntity
            {
                IdPicking = this.IdPicking,
                IdUsuario = this.IdUsuario
            };
        }
    }
}
