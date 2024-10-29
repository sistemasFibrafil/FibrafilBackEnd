using System;
using System.Collections.Generic;
namespace Net.Business.Entities.Web
{
    public class OrdenVentaEntity
    {
        public int IdOrdenVenta { get; set; }
        public string Numero { get; set; }
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public string ObjType { get; set; }
        public string DocStatus { get; set; }
        public string DocStatusRd { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public DateTime TaxDate { get; set; }
        public string CardCode { get; set; }
        public string LicTradNum { get; set; }
        public string CardName { get; set; }
        public int CntctCode { get; set; } = 0;
        public string PayToCode { get; set; }
        public string Address { get; set; }
        public string ShipToCode { get; set; }
        public string Address2 { get; set; }
        public string NumOrdCom { get; set; }
        public string DocCur { get; set; }
        public double DocRate { get; set; }
        public int GroupNum { get; set; }

        public string CodAgencia { get; set; }
        public string RucAgencia { get; set; }
        public string NomAgencia { get; set; }
        public string CodDirAgencia { get; set; }
        public string DirAgencia { get; set; }

        public string CodTipFlete { get; set; }
        public double ValorFlete { get; set; }
        public double TotalFlete { get; set; }
        public double ImporteSeguro { get; set; }
        public string Puerto { get; set; }

        public string CodTipVenta { get; set; }

        public int SlpCode { get; set; }
        public string Comments { get; set; }

        public double DiscPrcnt { get; set; }
        public double DiscSum { get; set; }
        public double VatSum { get; set; }
        public double DocTotal { get; set; }

        public int IdUsuarioCreate { get; set; }
        public int IdUsuarioUpdate { get; set; }

        public List<OrdenVentaDetalleEntity> Linea { get; set; } = new List<OrdenVentaDetalleEntity>();
    }

    public class OrdenVentaDetalleEntity
    {
        public int IdOrdenVenta { get; set; }
        public int Line { get; set; }
        public int DocEntry { get; set; }
        public int LineNum { get; set; }
        public string ObjType { get; set; }
        public string LineStatus { get; set; }
        public string LineStatusRd { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public string WhsCode { get; set; }
        public string UnitMsr { get; set; }
        public double Quantity { get; set; }
        public double OpenQty { get; set; }
        public double OpenQtyRd { get; set; }
        public string Currency { get; set; }
        public double PriceBefDi { get; set; }
        public double DiscPrcnt { get; set; }
        public double Price { get; set; }
        public double LineTotal { get; set; }
        public string TaxCode { get; set; }
        public double VatPrcnt { get; set; }
        public double VatSum { get; set; }
    }
}
