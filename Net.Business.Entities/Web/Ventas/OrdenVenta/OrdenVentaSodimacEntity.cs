﻿using System;
using System.Collections.Generic;

namespace Net.Business.Entities.Web
{
    public class OrdenVentaSodimacEntity
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
        public string Address { get; set; } = null ;
        public int? IdUsuarioCreate { get; set; } = null;
        public int? IdUsuarioUpdate { get; set; } = null;
        public List<OrdenVentaDetalleSodimacEntity> Item { get; set; } = new List<OrdenVentaDetalleSodimacEntity>();
    }

    public class OrdenVentaDetalleSodimacEntity
    {
        public int IdOrdenVentaSodimac { get; set; }
        public int Line { get; set; }
        public int NumLocal { get; set; }
        public string NomLocal { get; set; }
        public string CodEstado { get; set; }
        public string ItemCode { get; set; }
        public string Sku { get; set; }
        public string Dscription { get; set; }
        public string DscriptionLarga { get; set; }
        public string Ean { get; set; } = null;
        public string Lpn { get; set; } = null;
        public decimal Quantity { get; set; }
    }

    public class OrdenVentaSodimacByFiltroEntity
    {
        public int IdOrdenVentaSodimac { get; set; }
        public int DocNum { get; set; }
        public string NumAtCard { get; set; }
        public string CodEstado { get; set; }
        public string NomEstado { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public DateTime TaxDate { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
    }

    public class OrdenVentaSodimaConsultaFiltroEntity
    {
        public int IdOrdenVentaSodimac { get; set; }
        public int Line { get; set; }
        public int DocNum { get; set; }
        public string NumAtCard { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public DateTime TaxDate { get; set; }
        public int NumLocal { get; set; }
        public string NomLocal { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string ItemCode { get; set; }
        public string Sku { get; set; }
        public string Dscription { get; set; }
        public string DscriptionLarga { get; set; }
        public string Ean { get; set; }
        public string Lpn { get; set; }
        public decimal Quantity { get; set; }
    }

    public class OrdenVentaSodimacSelvaByFechaNumeroEntity
    {
        public string Proveedor { get; set; }
        public string NumAtCard { get; set; }
        public DateTime DocDueDate { get; set; }
        public DateTime TaxDate { get; set; }
        public int NumPallet { get; set; }
        public int NumLocal { get; set; }
        public string NomLocal { get; set; }
        public string Ean { get; set; }
        public string Sku { get; set; }
        public string DscriptionLarga { get; set; }
        public decimal Quantity { get; set; }
    }
}
