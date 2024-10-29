using Net.Business.Entities;

namespace Net.Connection
{
    public interface IConnectionSap
    {
        int ConnectToCompany(ConnectionSapEntity value);
        void DisConnectToCompany();
    }
}
