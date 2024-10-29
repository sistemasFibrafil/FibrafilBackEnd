using System;
using Net.Business.Entities.Web;
using System.Collections.Generic;

namespace Net.Business.DTO.Web
{
    public class PickingVentaCreateRequestDTO
    {
        public int IdPicking { get; set; }
        public string NumPicking { get; set; }
        public DateTime FecPicking { get; set; }
        public string CodTipoPicking { get; set; }
        public string CodEstado { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string LicTradNum { get; set; }
        public string Comentarios { get; set; }
        public int IdUsuario { get; set; }
        public List<PickingVentaItemByIdPickingEntity> Item { get; set; }

        public PickingVentaEntity RetornaPickingVenta()
        {
            return new PickingVentaEntity
            {
                IdPicking = this.IdPicking,
                NumPicking = this.NumPicking,
                FecPicking = this.FecPicking,
                CodTipoPicking = this.CodTipoPicking,
                CodEstado = this.CodEstado,
                CardCode = this.CardCode,
                CardName = this.CardName,
                LicTradNum = this.LicTradNum,
                Comentarios = this.Comentarios,
                IdUsuario = this.IdUsuario,
                Item = this.Item
            };
        }
    }
}
