using System;
using Net.Business.Entities.Web;
using System.Collections.Generic;

namespace Net.Business.DTO.Web
{
    public class SolicitudTrasladoCreateRequestDto
    {
        public int IdSolicitudTraslado { get; set; }
        public string Numero { get; set; } = null;
        public DateTime DocDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public DateTime TaxDate { get; set; }
        public string CardCode { get; set; } = null;
        public string CardName { get; set; } = null;
        public int CntctCode { get; set; } = 0;
        public string Address { get; set; } = null;
        public string FromWhsCode { get; set; }
        public string ToWhsCode { get; set; }
        public string CodTipTraslado { get; set; }
        public string CodMotTraslado { get; set; }
        public string CodTipSalida { get; set; }
        public int SlpCode { get; set; }
        public string JrnlMemo { get; set; } = null;
        public string Comments { get; set; } = null;
        public int? IdUsuarioCreate { get; set; } = null;
        public List<SolicitudTrasladoDetalleCreateRequestDto> Item { get; set; } = new List<SolicitudTrasladoDetalleCreateRequestDto>();

        public SolicitudTrasladoEntity ReturnValue()
        {
            var value = new SolicitudTrasladoEntity()
            {
                IdSolicitudTraslado = IdSolicitudTraslado,
                Numero = Numero,
                DocDate = DocDate,
                DocDueDate = DocDueDate,
                TaxDate = TaxDate,
                CardCode = CardCode,
                CardName = CardName,
                CntctCode = CntctCode,
                Address = Address,
                FromWhsCode = FromWhsCode,
                ToWhsCode = ToWhsCode,
                CodTipTraslado = CodTipTraslado,
                CodMotTraslado = CodMotTraslado,
                CodTipSalida = CodTipSalida,
                SlpCode = SlpCode,
                JrnlMemo = JrnlMemo,
                Comments = Comments,
                IdUsuarioCreate = IdUsuarioCreate,
            };

            foreach (var item in Item)
            {
                value.Item.Add(new SolicitudTrasladoDetalleEntity()
                {
                    IdSolicitudTraslado = item.IdSolicitudTraslado,
                    Line = item.Line,
                    ItemCode = item.ItemCode,
                    Dscription = item.Dscription,
                    FromWhsCode = item.FromWhsCode,
                    ToWhsCode = item.ToWhsCode,
                    UnitMsr = item.UnitMsr,
                    Quantity = item.Quantity,
                    OpenQty = item.OpenQty,
                    OpenQtyRd = item.OpenQtyRd,
                    IdUsuarioCreate = item.IdUsuarioCreate,
                });
            }

            return value;
        }
    }

    public class SolicitudTrasladoDetalleCreateRequestDto
    {
        public int IdSolicitudTraslado { get; set; }
        public int Line { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public string FromWhsCode { get; set; }
        public string ToWhsCode { get; set; }
        public string UnitMsr { get; set; }
        public double Quantity { get; set; }
        public double OpenQty { get; set; }
        public double OpenQtyRd { get; set; }
        public int? IdUsuarioCreate { get; set; } = null;
    }

}