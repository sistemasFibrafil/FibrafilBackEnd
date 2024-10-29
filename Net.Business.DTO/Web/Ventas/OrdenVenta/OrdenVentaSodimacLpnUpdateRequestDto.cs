using Net.Business.Entities.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Business.DTO.Web
{
    public class OrdenVentaSodimacLpnUpdateRequestDto
    {
        public int IdOrdenVentaSodimac { get; set; }
        public string Numero { get; set; }
        public List<OrdenVentaDetalleSodimacLpnUpdateRequestDto> Item { get; set; } = new List<OrdenVentaDetalleSodimacLpnUpdateRequestDto>();

        public OrdenVentaSodimacEntity ReturnValue()
        {
            var value = new OrdenVentaSodimacEntity()
            {
                IdOrdenVentaSodimac = this.IdOrdenVentaSodimac,
            };

            foreach (var item in Item)
            {
                value.Item.Add(new OrdenVentaDetalleSodimacEntity()
                {
                    IdOrdenVentaSodimac = item.IdOrdenVentaSodimac,
                    Line = item.Line,
                    NumLocal = item.NumLocal,
                });
            }

            return value;
    } }

    public class OrdenVentaDetalleSodimacLpnUpdateRequestDto
    {
        public int IdOrdenVentaSodimac { get; set; }
        public int Line { get; set; }
        public int NumLocal { get; set; }
    }
}
