using Net.Business.Entities.Web;
using System.Collections.Generic;

namespace Net.Business.DTO.Web
{
    public class SerieCreateRequestDTO
    {
        public int IdSerie { get; set; }
        public List<SerieDTO> Item { get; set; }

        public SerieEntity ReturnValue()
        {
            var serie = new SerieEntity()
            {
                IdSerie = this.IdSerie
            };

            foreach (var item in Item)
            {
                var value = new SerieItemEntity()
                {
                    IdSede = item.IdSede,
                    ObjectCode = item.ObjectCode,
                    Series = item.Series,
                    SerieSunat = item.SerieSunat,
                    NumeroSunat = item.NumeroSunat,
                };
                serie.Item.Add(value);
            }

            return serie;
        }
    }
}
