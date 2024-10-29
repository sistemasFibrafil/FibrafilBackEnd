using Net.Business.Entities.Sap;

namespace Net.Business.DTO.Sap
{
    public class AlmacenSapFindByCodeDTO
    {
        public string WhsCode { get; set; }

        public AlmacenSapEntity ReturnValue()
        {
            return new AlmacenSapEntity
            {
                WhsCode = this.WhsCode,
            };
        }
    }
}
