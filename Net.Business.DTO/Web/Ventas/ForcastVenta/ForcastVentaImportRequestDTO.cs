using Microsoft.AspNetCore.Http;
using Net.Business.Entities.Web;
using System;
using System.Collections.Generic;
using System.Globalization;
namespace Net.Business.DTO.Web
{
    public class ForcastVentaImportRequestDTO
    {
        public int IdUsuario { get; set; }
        public List<ForcastVentaImportDTO> Item { get; set; }

        public ForcastVentaEntity ReturnValue()
        {
            var value = new ForcastVentaEntity() { IdUsuario = this.IdUsuario };

            foreach (var item in Item)
            {
                var import = new ForcastVentaImportEntity()
                {
                    IdConSinOc = item.IdConSinOc,
                    IdNegocio = item.IdNegocio,
                    ItmsGrpCod = item.ItmsGrpCod,
                    ItemCode = item.ItemCode,
                    DocNum = item.DocNum,
                    CardCode = item.CardCode,
                    FecRegistro = DateTime.ParseExact(item.FecRegistro, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    UnidadMedida = item.UnidadMedida,
                    Cantidad = item.Cantidad,
                    Kg = item.Kg,
                    Precio = item.Precio,
                    CodEstado = item.CodEstado,
                };
                value.Item.Add(import);
            }
            return value;
        }
    }
}
