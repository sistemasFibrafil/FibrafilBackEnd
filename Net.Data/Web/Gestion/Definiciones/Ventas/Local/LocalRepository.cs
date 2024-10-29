using System;
using System.Data;
using Net.Connection;
using Net.CrossCotting;
using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Web;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
namespace Net.Data.Web
{
    public class LocalRepository : RepositoryBase<LocalEntity>, ILocalRepository
    {
        private string _metodoName;
        private string _aplicacionName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        // PARAMETROS DE COXIÓN
        private readonly string _cnxDos;
        private readonly IConfiguration _configuration;

        // STORED PROCEDURE
        const string DB_ESQUEMA = "";
        const string SP_GET_LIST_BY_FILTRO = DB_ESQUEMA + "GES_SP_GetListLocalByFiltro";
        const string SP_GET_BY_NUMLOCAL = DB_ESQUEMA + "GES_SP_GetLocalByNumLocal";
        const string SP_SET_CREATE = DB_ESQUEMA + "GES_SP_SetLocalCreate";
        const string SP_SET_UPDATE = DB_ESQUEMA + "GES_SP_SetLocalUpdate";
        const string SP_SET_DELETE = DB_ESQUEMA + "GES_SP_SetLocalDelete";


        public LocalRepository(IConnectionSql context, IConfiguration configuration)
            : base(context)
        {
            _configuration = configuration;
            _aplicacionName = GetType().Name;
            _cnxDos = Utilidades.GetExtraerCadenaConexion(configuration, "ParametersConectionDos");
        }


        public async Task<ResultadoTransaccion<LocalEntity>> GetListByFiltro(FiltroRequestEntity value)
        {
            var response = new List<LocalEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<LocalEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxDos))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_BY_FILTRO, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@Filtro", value.TextFiltro1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<LocalEntity>)context.ConvertTo<LocalEntity>(reader);
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
        public async Task<ResultadoTransaccion<LocalEntity>> GetByNumLocal(string numLocal)
        {
            var response = new LocalEntity();
            var resultadoTransaccion = new ResultadoTransaccion<LocalEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxDos))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_BY_NUMLOCAL, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@NumLocal", numLocal));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = context.Convert<LocalEntity>(reader);
                        }
                    }

                    resultadoTransaccion.IdRegistro = 0;
                    resultadoTransaccion.ResultadoCodigo = 0;
                    resultadoTransaccion.ResultadoDescripcion = "Datos obtenidos con éxito ..!";
                    resultadoTransaccion.data = response;
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
        public async Task<ResultadoTransaccion<LocalEntity>> SetCreate(LocalEntity value)
        {
            var responde = new LocalEntity();
            var resultadoTransaccion = new ResultadoTransaccion<LocalEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxDos))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(SP_SET_CREATE, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        // LOCAL
                        cmd.Parameters.Add(new SqlParameter("@NumLocal", value.NumLocal));
                        cmd.Parameters.Add(new SqlParameter("@NomLocal", value.NomLocal));
                        cmd.Parameters.Add(new SqlParameter("@EsOriente", value.EsOriente));
                        // CLIENTE
                        cmd.Parameters.Add(new SqlParameter("@CardCode", value.CardCode));
                        cmd.Parameters.Add(new SqlParameter("@CardName", value.CardName));
                        //USUARIO
                        cmd.Parameters.Add(new SqlParameter("@IdUsuarioCreate", value.IdUsuarioCreate));

                        await cmd.ExecuteNonQueryAsync();

                    }
                }

                resultadoTransaccion.IdRegistro = 0;
                resultadoTransaccion.ResultadoCodigo = 0;
                resultadoTransaccion.ResultadoDescripcion = "Registro procesado con éxito ..!";
                return resultadoTransaccion;
            }
            catch (Exception ex)
            {
                resultadoTransaccion.IdRegistro = -1;
                resultadoTransaccion.ResultadoCodigo = -1;
                resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                return resultadoTransaccion;
            }
        }
        public async Task<ResultadoTransaccion<LocalEntity>> SetUpdate(LocalEntity value)
        {
            var responde = new LocalEntity();
            var resultadoTransaccion = new ResultadoTransaccion<LocalEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxDos))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(SP_SET_UPDATE, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        // LOCAL
                        cmd.Parameters.Add(new SqlParameter("@NumLocal", value.NumLocal));
                        cmd.Parameters.Add(new SqlParameter("@NomLocal", value.NomLocal));
                        cmd.Parameters.Add(new SqlParameter("@EsOriente", value.EsOriente));
                        // CLIENTE
                        cmd.Parameters.Add(new SqlParameter("@CardCode", value.CardCode));
                        cmd.Parameters.Add(new SqlParameter("@CardName", value.CardName));
                        //USUARIO
                        cmd.Parameters.Add(new SqlParameter("@IdUsuarioUpdate", value.IdUsuarioUpdate));

                        await cmd.ExecuteNonQueryAsync();

                    }
                }

                resultadoTransaccion.IdRegistro = 0;
                resultadoTransaccion.ResultadoCodigo = 0;
                resultadoTransaccion.ResultadoDescripcion = "Registro procesado con éxito ..!";
                return resultadoTransaccion;
            }
            catch (Exception ex)
            {
                resultadoTransaccion.IdRegistro = -1;
                resultadoTransaccion.ResultadoCodigo = -1;
                resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                return resultadoTransaccion;
            }
        }
        public async Task<ResultadoTransaccion<LocalEntity>> SetDelete(int numLocal)
        {
            var resultadoTransaccion = new ResultadoTransaccion<LocalEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxDos))
                {
                    await conn.OpenAsync();

                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(SP_SET_DELETE, conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;
                            cmd.Parameters.Add(new SqlParameter("@NumLocal", numLocal));

                            await cmd.ExecuteNonQueryAsync();
                        }

                        resultadoTransaccion.IdRegistro = 0;
                        resultadoTransaccion.ResultadoCodigo = 0;
                        resultadoTransaccion.ResultadoDescripcion = "Registro eliminado con éxito ..!";
                    }
                    catch (Exception ex)
                    {
                        resultadoTransaccion.IdRegistro = -1;
                        resultadoTransaccion.ResultadoCodigo = -1;
                        resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                    }
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
