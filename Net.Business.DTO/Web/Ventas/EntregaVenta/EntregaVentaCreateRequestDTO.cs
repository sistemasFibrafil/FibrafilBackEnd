using System;
using Net.Business.Entities.Web;
using System.Collections.Generic;
namespace Net.Business.DTO.Web
{
    public class EntregaVentaCreateRequestDTO
    {
        public int DocEntry { get; set; }
        public string ObjType { get; set; }
        public int Series { get; set; }
        public int DocNum { get; set; }
        public string DocStatus { get; set; }
        public string TipDocSunat { get; set; }
        public string SerSunat { get; set; }
        public string NumSunat { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public DateTime TaxDate { get; set; }

        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string LicTradNum { get; set; }
        public string DocCur { get; set; }
        public double DocRate { get; set; }

        public string ShipToCode { get; set; }
        public string Address2 { get; set; }
        public string PayToCode { get; set; }
        public string Address { get; set; }
        public string CodMotTraslado { get; set; }
        public string OtrMotTraslado { get; set; }

        public int CodCondicionPago { get; set; }

        public string RucDesInternacional { get; set; }
        public string DesGuiaInternacional { get; set; }
        public string DirDesInternacional { get; set; }
        public string NumContenedor { get; set; }
        public string NumPrecinto1 { get; set; }
        public string NumPrecinto2 { get; set; }
        public string NumPrecinto3 { get; set; }
        public string NumPrecinto4 { get; set; }

        //Transportista
        public string CodTipTransporte { get; set; }
        //Transportista 1
        public bool ManTransportista1 { get; set; }
        public string CodTransportista1 { get; set; }
        public string CodTipDocIdeTransportista1 { get; set; }
        public string RucTransportista1 { get; set; }
        public string NomTransportista1 { get; set; }
        public string NumPlaca1 { get; set; }
        public string CodTipDocIdeConductor1 { get; set; }
        public string NumDocIdeConductor1 { get; set; }
        public string NomConductor1 { get; set; }
        public string ApeConductor1 { get; set; }
        public string DenConductor1 { get; set; }
        public string LicConductor1 { get; set; }
        //Transportista 2
        public bool ManTransportista2 { get; set; }
        public string CodTransportista2 { get; set; }
        public string RucTransportista2 { get; set; }
        public string NomTransportista2 { get; set; }
        public string DirTransportista2 { get; set; }

        public int SlpCode { get; set; }
        public double TotalBulto { get; set; }
        public double TotalKilo { get; set; }
        public string Comments { get; set; }

        public double VatSum { get; set; }
        public double VatSumFC { get; set; }
        public double VatSumy { get; set; }

        public double DocTotal { get; set; }
        public double DocTotalFC { get; set; }
        public double DocTotalSy { get; set; }

        public int IdUsuario { get; set; }

        public List<EntregaVentaItemDTO> Item { get; set; } 

        public EntregaVentaEntity ReturnValue()
        {
            var value = new EntregaVentaEntity()
            {
                DocEntry = this.DocEntry,
                ObjType = this.ObjType,
                Series = this.Series,
                DocNum = this.DocNum,
                DocStatus = this.DocStatus,
                TipDocSunat = this.TipDocSunat,
                SerSunat = this.SerSunat,
                NumSunat = this.NumSunat,
                DocDate = this.DocDate,
                DocDueDate = this.DocDueDate,
                TaxDate = this.TaxDate,

                CardCode = this.CardCode,
                CardName = this.CardName,
                LicTradNum = this.LicTradNum,
                DocCur = this.DocCur,
                DocRate = this.DocRate,

                ShipToCode = this.ShipToCode,
                Address2 = this.Address2,
                PayToCode = this.PayToCode,
                Address = this.Address,

                CodMotTraslado = this.CodMotTraslado,
                OtrMotTraslado = this.OtrMotTraslado,

                CodCondicionPago = this.CodCondicionPago,

                RucDesInternacional = this.RucDesInternacional,
                DesGuiaInternacional = this.DesGuiaInternacional,
                DirDesInternacional = this.DirDesInternacional,
                NumContenedor = this.NumContenedor,
                NumPrecinto1 = this.NumPrecinto1,
                NumPrecinto2 = this.NumPrecinto2,
                NumPrecinto3 = this.NumPrecinto3,
                NumPrecinto4 = this.NumPrecinto4,

                CodTipTransporte = this.CodTipTransporte,
                ManTransportista1 = this.ManTransportista1,
                CodTipDocIdeTransportista1 = this.CodTipDocIdeTransportista1,
                CodTransportista1 = this.CodTransportista1,
                RucTransportista1 = this.RucTransportista1,
                NomTransportista1 = this.NomTransportista1,
                NumPlaca1 = this.NumPlaca1,

                CodTipDocIdeConductor1 = this.CodTipDocIdeConductor1,
                NumDocIdeConductor1 = this.NumDocIdeConductor1,
                DenConductor1 = this.DenConductor1,
                NomConductor1 = this.NomConductor1,
                ApeConductor1 = this.ApeConductor1,
                LicConductor1 = this.LicConductor1,

                ManTransportista2 = this.ManTransportista2,
                CodTransportista2 = this.CodTransportista2,
                RucTransportista2 = this.RucTransportista2,
                NomTransportista2 = this.NomTransportista2,
                DirTransportista2 = this.DirTransportista2,

                SlpCode = this.SlpCode,
                TotalBulto = this.TotalBulto,
                TotalKilo = this.TotalKilo,
                Comments = this.Comments,

                VatSum = this.VatSum,
                VatSumFC = this.VatSumFC,
                VatSumy = this.VatSumy,

                DocTotal = this.DocTotal,
                DocTotalFC = this.DocTotalFC,
                DocTotalSy = this.DocTotalSy,

                IdUsuario = this.IdUsuario
            };

            foreach (var item in Item)
            {
                value.Item.Add(new EntregaVentaItemEntity()
                {
                    LineNum = item.LineNum,
                    IdBase = item.IdBase,
                    LineBase = item.LineBase,
                    BaseType = item.BaseType,
                    BaseEntry = item.BaseEntry,
                    BaseLine = item.BaseLine,
                    ItemCode = item.ItemCode,
                    Dscription = item.Dscription,
                    WhsCode = item.WhsCode,
                    UnitMsr = item.UnitMsr,
                    Quantity = item.Quantity,
                    Peso = item.Peso,
                    TaxCode = item.TaxCode,
                    AcctCode = item.AcctCode,
                    Currency = item.Currency,
                    DiscPrcnt = item.DiscPrcnt,
                    Price = item.Price,
                    LineTotal = item.LineTotal,
                    TotalFrgn = item.TotalFrgn,
                    TotalSumSy = item.TotalSumSy,
                });
            }

            return value;
        }
    }
}
