using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Net.Business.Entities.Web;
using Microsoft.Extensions.Configuration;
using Net.Connection;
using Net.CrossCotting;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using Net.Business.Entities;
using System.Data;

namespace Net.Data.Web
{
    public class EstadoDocumentoRepository : RepositoryBase<EstadoDocumentoEntity>, IEstadoDocumentoRepository
    {
        private string _metodoName;
        private string _aplicacionName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        // PARAMETROS DE COXIÓN
        private readonly string _cnxSap;
        private readonly IConfiguration _configuration;

        // STORED PROCEDURE
        const string DB_ESQUEMA = "";
        const string SP_GET_LIST = DB_ESQUEMA + "GES_GetListEstadoDocumentoAll";

        public EstadoDocumentoRepository(IConnectionSql context, IConfiguration configuration)
            : base(context)
        {
            _configuration = configuration;
            _aplicacionName = GetType().Name;
            _cnxSap = Utilidades.GetExtraerCadenaConexion(configuration, "ParametersConectionDos");
        }

        public async Task<ResultadoTransaccion<EstadoDocumentoEntity>> GetListAll()
        {
            var response = new List<EstadoDocumentoEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<EstadoDocumentoEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<EstadoDocumentoEntity>)context.ConvertTo<EstadoDocumentoEntity>(reader);
                        }
                    }

                    resultadoTransaccion.IdRegistro = 0;
                    resultadoTransaccion.ResultadoCodigo = 0;
                    resultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", response.Count);
                    resultadoTransaccion.dataList = response;
                }
            }
            catch (Exception ex)
            {
                resultadoTransaccion.IdRegistro = -1;
                resultadoTransaccion.ResultadoCodigo = -1;
                resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return resultadoTransaccion;
        }
    }
}
