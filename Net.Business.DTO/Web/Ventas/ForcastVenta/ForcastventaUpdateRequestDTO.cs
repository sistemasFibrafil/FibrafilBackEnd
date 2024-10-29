using Net.Business.Entities.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Business.DTO.Web
{
    public class ForcastventaUpdateRequestDTO
    {
        public int IdForcastventa { get; set; }
        public int IdConSinOc { get; set; }
        public int IdNegocio { get; set; }
        public int ItmsGrpCod { get; set; }
        public string ItemCode { get; set; }
        public int DocNum { get; set; }
        public string CardCode { get; set; }
        public DateTime FecRegistro { get; set; }
        public string UnidadMedida { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Kg { get; set; }
        public decimal Precio { get; set; }
        public string CodEstado { get; set; }
        public int IdUsuario { get; set; }

        public ForcastVentaEntity ReturnValue()
        {
            return new ForcastVentaEntity
            {
                IdForcastVenta = this.IdForcastventa,
                IdConSinOc = this.IdConSinOc,
                IdNegocio = this.IdNegocio,
                ItmsGrpCod = this.ItmsGrpCod,
                ItemCode = this.ItemCode,
                DocNum = this.DocNum,
                CardCode = this.CardCode,
                FecRegistro = this.FecRegistro,
                UnidadMedida = this.UnidadMedida,
                Cantidad = this.Cantidad,
                Kg = this.Kg,
                Precio = this.Precio,
                CodEstado = this.CodEstado,
                IdUsuario = this.IdUsuario
            };
        }
    }
}
