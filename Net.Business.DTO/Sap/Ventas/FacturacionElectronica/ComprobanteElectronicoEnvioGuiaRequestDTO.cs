using Net.Business.Entities.Sap;
namespace Net.Business.DTO.Sap
{
    public class ComprobanteElectronicoEnvioGuiaRequestDTO
    {
        public int DocEntry { get; set; }

        public FacturacionElectronicaSapEntity RetornaGuia()
        {
            return new FacturacionElectronicaSapEntity
            {
                DocEntry = this.DocEntry
            };
        }
    }
}
