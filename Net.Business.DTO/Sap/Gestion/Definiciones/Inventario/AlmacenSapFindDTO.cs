using Net.Business.Entities.Sap;

namespace Net.Business.DTO.Sap
{
    public class AlmacenSapFindDTO
    {
        public string Inactive { get; set; }

        public AlmacenSapEntity ReturnValue()
        {
            return new AlmacenSapEntity
            {
                Inactive = this.Inactive,
            };
        }
    }
}
