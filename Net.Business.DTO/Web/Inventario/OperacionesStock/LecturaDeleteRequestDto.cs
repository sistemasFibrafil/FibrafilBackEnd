using Net.Business.Entities.Web;
namespace Net.Business.DTO.Web
{
    public class LecturaDeleteRequestDto
    {
        public string ObjType { get; set; }
        public int DocEntry { get; set; }

        public LecturaEntity ReturnValue()
        {
            return new LecturaEntity()
            {
                ObjType = this.ObjType,
                DocEntry = this.DocEntry
            };
        }
    }
}
