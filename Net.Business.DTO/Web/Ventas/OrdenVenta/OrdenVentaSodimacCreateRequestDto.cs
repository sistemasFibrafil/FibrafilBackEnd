using System;
using Net.Business.Entities.Web;
using System.Collections.Generic;
namespace Net.Business.DTO.Web
{
    public class OrdenVentaSodimacCreateRequestDto
    {
        public int IdOrdenVentaSodimac { get; set; }
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public string NumAtCard { get; set; }
        public string CodEstado { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public DateTime TaxDate { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public int CntctCode { get; set; } = 0;
        public string CntctName { get; set; } = null;
        public string Address { get; set; } = null;
        public int? IdUsuarioCreate { get; set; } = null;
        public List<OrdenVentaDetalleSodimacDto> Item { get; set; } = new List<OrdenVentaDetalleSodimacDto>();

        public OrdenVentaSodimacEntity ReturnValue()
        {
            var value = new OrdenVentaSodimacEntity()
            {
                IdOrdenVentaSodimac = this.IdOrdenVentaSodimac,
                DocEntry = this.DocEntry,
                DocNum = this.DocNum,
                NumAtCard = this.NumAtCard,
                CodEstado = this.CodEstado,
                DocDate = this.DocDate,
                DocDueDate = this.DocDueDate,
                TaxDate = this.TaxDate,
                CardCode = this.CardCode,
                CardName = this.CardName,
                CntctCode = this.CntctCode,
                CntctName = this.CntctName,
                Address = this.Address,
                IdUsuarioCreate = this.IdUsuarioCreate
            };

            foreach (var item in Item)
            {
                value.Item.Add(new OrdenVentaDetalleSodimacEntity()
                {
                    IdOrdenVentaSodimac = item.IdOrdenVentaSodimac,
                    Line = item.Line,
                    NumLocal = item.NumLocal,
                    CodEstado = item.CodEstado,
                    ItemCode = item.ItemCode,
                    Sku = item.Sku,
                    Dscription = item.Dscription,
                    DscriptionLarga = item.DscriptionLarga,
                    Ean = item.Ean,
                    Quantity = item.Quantity
                });
            }

            return value;
        }
    }

    public class OrdenVentaDetalleSodimacDto
    {
        public int IdOrdenVentaSodimac { get; set; }
        public int Line { get; set; }
        public int NumLocal { get; set; }
        public string CodEstado { get; set; }
        public string ItemCode { get; set; }
        public string Sku { get; set; }
        public string Dscription { get; set; }
        public string DscriptionLarga { get; set; }
        public string Ean { get; set; } = null;
        public decimal Quantity { get; set; }
    }
}
