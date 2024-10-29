using System;
using Net.Business.Entities.Web;
using System.Collections.Generic;
namespace Net.Business.DTO
{
    public class OrdenVentaCreateDto
    {
        public DateTime DocDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public DateTime TaxDate { get; set; }
        public string CardCode { get; set; }
        public string LicTradNum { get; set; }
        public string CardName { get; set; }
        public int CntctCode { get; set; }
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
        public List<OrdenVentaDetalleDto> Linea { get; set; } = new List<OrdenVentaDetalleDto>();

        public OrdenVentaEntity ReturnValue()
        {
            var value = new OrdenVentaEntity()
            {
                DocDate = this.DocDate,
                DocDueDate = this.DocDueDate,
                TaxDate = this.TaxDate,
                CardCode = this.CardCode,
                LicTradNum = this.LicTradNum,
                CardName = this.CardName,
                CntctCode = this.CntctCode,
                PayToCode = this.PayToCode,
                Address = this.Address,
                ShipToCode = this.ShipToCode,
                Address2 = this.Address2,
                NumOrdCom = this.NumOrdCom,
                DocCur = this.DocCur,
                DocRate = this.DocRate,
                GroupNum = this.GroupNum,

                CodAgencia = this.CodAgencia,
                RucAgencia = this.RucAgencia,
                NomAgencia = this.NomAgencia,
                CodDirAgencia = this.CodDirAgencia,
                DirAgencia = this.DirAgencia,

                CodTipFlete = this.CodTipFlete,
                ValorFlete = this.ValorFlete,
                TotalFlete = this.TotalFlete,
                ImporteSeguro = this.ImporteSeguro,
                Puerto = this.Puerto,

                CodTipVenta = this.CodTipVenta,

                SlpCode = this.SlpCode,
                Comments = this.Comments,

                DiscPrcnt = this.DiscPrcnt,
                DiscSum = this.DiscSum,
                VatSum = this.VatSum,
                DocTotal = this.DocTotal,

                IdUsuarioCreate = this.IdUsuarioCreate
            };

            foreach (var item in Linea)
            {
                value.Linea.Add(new OrdenVentaDetalleEntity()
                {
                    Line = item.Line,
                    ItemCode = item.ItemCode,
                    Dscription = item.Dscription,
                    WhsCode = item.WhsCode,
                    UnitMsr = item.UnitMsr,
                    Quantity = item.Quantity,
                    OpenQty = item.OpenQty,
                    OpenQtyRd = item.OpenQtyRd,
                    Currency = item.Currency,
                    PriceBefDi = item.PriceBefDi,
                    DiscPrcnt = item.DiscPrcnt,
                    Price = item.Price,
                    LineTotal = item.LineTotal,
                    TaxCode = item.TaxCode,
                    VatPrcnt = item.VatPrcnt,
                    VatSum = item.VatSum,
                });
            }

            return value;
        }
    }
    public class OrdenVentaDetalleDto
    {
        public int Line { get; set; }
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
