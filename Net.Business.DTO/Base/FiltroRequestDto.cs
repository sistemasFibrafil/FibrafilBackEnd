using System;
using Net.Business.Entities;
namespace Net.Business.DTO
{
    public class FiltroRequestDto
    {
        public int? Val1 { get; set; } = 0;
        public int? Val2 { get; set; } = 0;
        public int? Val3 { get; set; } = 0;
        public int? Val4 { get; set; } = 0;
        public int? Val5 { get; set; } = 0;
        public int? Id1 { get; set; } = 0;
        public int? Id2 { get; set; } = 0;
        public int? Id3 { get; set; } = 0;
        public int? Id4 { get; set; } = 0;
        public int? Id5 { get; set; } = 0;
        public string Code1 { get; set; } = "";
        public string Code2 { get; set; } = "";
        public string Code3 { get; set; } = "";
        public string Code4 { get; set; } = "";
        public string Code5 { get; set; } = "";
        public DateTime? Fecha1 { get; set; } = null;
        public DateTime? Fecha2 { get; set; } = null;
        public string TextFiltro1 { get; set; } = "";
        public string TextFiltro2 { get; set; } = "";
        public string TextFiltro3 { get; set; } = "";
        public string TextFiltro4 { get; set; } = "";
        public string TextFiltro5 { get; set; } = "";

        public FiltroRequestEntity ReturnValue()
        {
            return new FiltroRequestEntity
            {
                Val1 = this.Val1,
                Val2 = this.Val2,
                Val3 = this.Val3,
                Val4 = this.Val4,
                Val5 = this.Val5,
                Id1 = this.Id1,
                Id2 = this.Id2,
                Id3 = this.Id3,
                Id4 = this.Id4,
                Id5 = this.Id5,
                Code1 = this.Code1,
                Code2 = this.Code2,
                Code3 = this.Code3,
                Code4 = this.Code4,
                Code5 = this.Code5,
                Fecha1 = this.Fecha1,
                Fecha2 = this.Fecha2,
                TextFiltro1 = this.TextFiltro1,
                TextFiltro2 = this.TextFiltro2,
                TextFiltro3 = this.TextFiltro3,
                TextFiltro4 = this.TextFiltro4,
                TextFiltro5 = this.TextFiltro5,
            };
        }
    }
}
