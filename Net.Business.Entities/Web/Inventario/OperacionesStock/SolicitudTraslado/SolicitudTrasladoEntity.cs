using System;
using System.Collections.Generic;
namespace Net.Business.Entities.Web
{
    public class SolicitudTrasladoEntity
    {
        public int IdSolicitudTraslado { get; set; }
        public string Numero { get; set; } = null;
        public int DocEntry { get; set; } = 0;
        public int DocNum { get; set; } = 0;
        public string DocStatus { get; set; } = null;
        public string StatusName { get; set; } = null;
        public string DocStatusRd { get; set; } = null;
        public string DocManClose { get; set; } = null;
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
        public int? IdUsuarioUpdate { get; set; } = null;
        public int? IdUsuarioClose { get; set; } = null;
        public List<SolicitudTrasladoDetalleEntity> Item { get; set; } = new List<SolicitudTrasladoDetalleEntity>();
    }

    public class SolicitudTrasladoDetalleEntity
    {
        public int IdSolicitudTraslado { get; set; }
        public int Line { get; set; }
        public int DocEntry { get; set; }
        public int LineNum { get; set; }
        public string LineStatus { get; set; }
        public string LineStatusRd { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public string FromWhsCode { get; set; }
        public string ToWhsCode { get; set; }
        public string UnitMsr { get; set; }
        public double Quantity { get; set; }
        public double OpenQty { get; set; }
        public double OpenQtyRd { get; set; }
        public int? IdUsuarioCreate { get; set; } = null;
        public int? IdUsuarioUpdate { get; set; } = null;
        public int? IdUsuarioClose { get; set; } = null;
    }
}
