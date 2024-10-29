using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Net.Business.Entities;
using Net.Business.Entities.Sap;
using Net.Connection;
using Net.CrossCotting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Net.Data.Sap
{
    public class KardexRepository : RepositoryBase<KardexEntity>, IKardexRepository
    {
        private string _metodoName;
        private string _aplicacionName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        // PARAMETROS DE COXIÓN
        //private readonly string _cnx;
        private readonly string _cnxSap;
        private readonly IConfiguration _configuration;

        // STORED PROCEDURE
        const string DB_ESQUEMA = "";
        const string SP_GET_LIST_KARDEX_SALDO_INICIAL_BY_PERIODO_ARTICULO = DB_ESQUEMA + "FIB_WEB_INVE_SP_GetListKardexSaldoInicialByPeriodoArticulo";


        public KardexRepository(IConnectionSql context, IConfiguration configuration)
            : base(context)
        {
            _cnxSap = Utilidades.GetExtraerCadenaConexion(configuration, "ParametersConectionSap");
            _configuration = configuration;
            _aplicacionName = GetType().Name;
        }


        public async Task<ResultadoTransaccion<KardexSaldoInicialByPeriodoArticuloEntity>> GetListKardexSaldoInicialByPeriodoArticulo(KardexSaldoInicialByPeriodoArticuloFindEntity value)
        {
            var response = new List<KardexSaldoInicialByPeriodoArticuloEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<KardexSaldoInicialByPeriodoArticuloEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_KARDEX_SALDO_INICIAL_BY_PERIODO_ARTICULO, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@PE", value.Periodo));
                        cmd.Parameters.Add(new SqlParameter("@PI", value.ItemCode1));
                        cmd.Parameters.Add(new SqlParameter("@PF", value.ItemCode2));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<KardexSaldoInicialByPeriodoArticuloEntity>)context.ConvertTo<KardexSaldoInicialByPeriodoArticuloEntity>(reader);
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
