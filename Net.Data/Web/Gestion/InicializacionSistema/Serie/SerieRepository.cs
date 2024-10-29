using System;
using System.Data;
using Net.Connection;
using Net.CrossCotting;
using System.Transactions;
using Net.Business.Entities;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Net.Business.Entities.Web;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace Net.Data.Web
{
    public class SerieRepository : RepositoryBase<SerieEntity>, ISerieRepository
    {
        private string _metodoName;
        private string _aplicacionName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        // PARAMETROS DE COXIÓN
        private readonly string _cnxSap;
        private readonly IConfiguration _configuration;

        // STORED PROCEDURE
        const string DB_ESQUEMA = "";
        const string SP_SET_CREATE = DB_ESQUEMA + "FIB_WEB_GE_SP_SetSerieCreate";
        const string SP_SET_UPDATE = DB_ESQUEMA + "FIB_WEB_GE_SP_SetSerieUpdate";
        const string SP_SET_DELETE = DB_ESQUEMA + "FIB_WEB_GE_SP_SetSerieDelete";
        const string SP_GET_LIST_BY_IDSEDE = DB_ESQUEMA + "FIB_WEB_GE_SP_GetListSerieByIdSede";
        const string SP_GET_LIST_BY_SERIE_IDUSUARIO = DB_ESQUEMA + "FIB_WEB_GE_SP_GetListSerieSunatBySerieSapIdUsuario";
        const string SP_GET_BY_SERIE = DB_ESQUEMA + "FIB_WEB_GE_SP_GetNumeroSunatBySerieSunat";

        public SerieRepository(IConnectionSql context, IConfiguration configuration)
            : base(context)
        {
            _cnxSap = Utilidades.GetExtraerCadenaConexion(configuration, "ParametersConectionSap");
            _configuration = configuration;
            _aplicacionName = GetType().Name;
        }

        public async Task<ResultadoTransaccion<SerieEntity>> GetListByIdSede(int idSede)
        {
            var response = new List<SerieEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<SerieEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_BY_IDSEDE, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@IdSede", idSede));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<SerieEntity>)context.ConvertTo<SerieEntity>(reader);
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

        public async Task<ResultadoTransaccion<SerieEntity>> GetListBySerieIdUsuario(int series, int idUsuario)
        {
            var response = new List<SerieEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<SerieEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_BY_SERIE_IDUSUARIO, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@Series", series));
                        cmd.Parameters.Add(new SqlParameter("@IdUsuario", idUsuario));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<SerieEntity>)context.ConvertTo<SerieEntity>(reader);
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

        public async Task<ResultadoTransaccion<SerieEntity>> GetNumeroBySerie(string serieSunat)
        {
            var response = new SerieEntity();
            var resultadoTransaccion = new ResultadoTransaccion<SerieEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_BY_SERIE, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@SerieSunat", serieSunat));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = context.Convert<SerieEntity>(reader);
                        }
                    }

                    resultadoTransaccion.IdRegistro = 0;
                    resultadoTransaccion.ResultadoCodigo = 0;
                    resultadoTransaccion.ResultadoDescripcion = "Dato obtenido con éxito.";
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

        public async Task<ResultadoTransaccion<SerieEntity>> SetCreate(SerieEntity value)
        {
            var resultadoTransaccion = new ResultadoTransaccion<SerieEntity>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    using (CommittableTransaction transaction = new CommittableTransaction())
                    {
                        await conn.OpenAsync();
                        conn.EnlistTransaction(transaction);

                        try
                        {
                            using (SqlCommand cmd = new SqlCommand(SP_SET_CREATE, conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                for (int i = 0; i < value.Item.Count; i++)
                                {
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.Add(new SqlParameter("@IdSede", value.Item[i].IdSede));
                                    cmd.Parameters.Add(new SqlParameter("@ObjectCode", value.Item[i].ObjectCode));
                                    cmd.Parameters.Add(new SqlParameter("@Series", value.Item[i].Series));
                                    cmd.Parameters.Add(new SqlParameter("@SerieSunat", value.Item[i].SerieSunat));
                                    cmd.Parameters.Add(new SqlParameter("@NumeroSunat", value.Item[i].NumeroSunat));

                                    await cmd.ExecuteNonQueryAsync();
                                }
                            }

                            resultadoTransaccion.IdRegistro = 0;
                            resultadoTransaccion.ResultadoCodigo = 0;
                            resultadoTransaccion.ResultadoDescripcion = "Se realizo con Exito...!!!";

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            resultadoTransaccion.IdRegistro = -1;
                            resultadoTransaccion.ResultadoCodigo = -1;
                            resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                        }
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

        public async Task<ResultadoTransaccion<SerieEntity>> SetUpdate(SerieEntity value)
        {
            var resultadoTransaccion = new ResultadoTransaccion<SerieEntity>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    using (CommittableTransaction transaction = new CommittableTransaction())
                    {
                        await conn.OpenAsync();
                        conn.EnlistTransaction(transaction);

                        try
                        {
                            using (SqlCommand cmd = new SqlCommand(SP_SET_UPDATE, conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@IdSerie", value.IdSerie));
                                cmd.Parameters.Add(new SqlParameter("@SerieSunat", value.SerieSunat));
                                cmd.Parameters.Add(new SqlParameter("@NumeroSunat", value.NumeroSunat));

                                await cmd.ExecuteNonQueryAsync();
                            }

                            resultadoTransaccion.IdRegistro = 0;
                            resultadoTransaccion.ResultadoCodigo = 0;
                            resultadoTransaccion.ResultadoDescripcion = "Se realizo con Exito...!!!";

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            resultadoTransaccion.IdRegistro = -1;
                            resultadoTransaccion.ResultadoCodigo = -1;
                            resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                        }
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

        public async Task<ResultadoTransaccion<SerieEntity>> SetDelete(SerieEntity value)
        {
            var resultadoTransaccion = new ResultadoTransaccion<SerieEntity>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    using (CommittableTransaction transaction = new CommittableTransaction())
                    {
                        await conn.OpenAsync();
                        conn.EnlistTransaction(transaction);

                        try
                        {
                            using (SqlCommand cmd = new SqlCommand(SP_SET_DELETE, conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@IdSerie", value.IdSerie));

                                await cmd.ExecuteNonQueryAsync();
                            }

                            resultadoTransaccion.IdRegistro = 0;
                            resultadoTransaccion.ResultadoCodigo = 0;
                            resultadoTransaccion.ResultadoDescripcion = "Se realizo con Exito...!!!";

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            resultadoTransaccion.IdRegistro = -1;
                            resultadoTransaccion.ResultadoCodigo = -1;
                            resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                        }
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
