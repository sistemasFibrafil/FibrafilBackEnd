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
    public class LecturaRepository : RepositoryBase<LecturaEntity>, ILecturaRepository
    {
        private string _metodoName;
        private string _aplicacionName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        // PARAMETROS DE COXIÓN
        private readonly string _cnxDos;
        private readonly IConfiguration _configuration;

        // STORED PROCEDURE
        const string DB_ESQUEMA = "";
        const string SP_SET_CREATE = DB_ESQUEMA + "INV_SP_SetLecturaCreate";
        const string SP_SET_DELETE1 = DB_ESQUEMA + "INV_SP_SetLecturaDeleteMultiple";
        const string SP_SET_DELETE2 = DB_ESQUEMA + "INV_SP_SetLecturaDelete";
        const string SP_GET_LIST_BY_OBJTYPE_AND_DOCENTRY = DB_ESQUEMA + "INV_SP_GetListLecturaByObjTypeDocEntry";
        const string SP_GET_LIST_BY_FILTRO = DB_ESQUEMA + "INV_SP_GetListLecturaByFiltro";
        const string SP_GET_LIST_BY_DOCENTRY_AND_OBJTYPE_AND_FILTRO = DB_ESQUEMA + "INV_SP_GetListLecturaBarcodeByDocEntryAndObjTypeAndFiltro";


        public LecturaRepository(IConnectionSql context, IConfiguration configuration)
            : base(context)
        {
            _configuration = configuration;
            _aplicacionName = GetType().Name;
            _cnxDos = Utilidades.GetExtraerCadenaConexion(configuration, "ParametersConectionDos");
        }

        public async Task<ResultadoTransaccion<LecturaEntity>> SetCreate(LecturaEntity value)
        {
            var resultadoTransaccion = new ResultadoTransaccion<LecturaEntity>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxDos))
                {
                    using (CommittableTransaction transaction = new CommittableTransaction())
                    {
                        await conn.OpenAsync();
                        conn.EnlistTransaction(transaction);

                        try
                        {
                            using (SqlCommand cmdItem = new SqlCommand(SP_SET_CREATE, conn))
                            {
                                cmdItem.CommandType = CommandType.StoredProcedure;
                                cmdItem.CommandTimeout = 0;
                                cmdItem.Parameters.Add(new SqlParameter("@ObjType", value.ObjType));
                                cmdItem.Parameters.Add(new SqlParameter("@DocEntry", value.DocEntry));
                                cmdItem.Parameters.Add(new SqlParameter("@WhsCode", value.WhsCode));
                                cmdItem.Parameters.Add(new SqlParameter("@Barcode", value.Barcode));
                                cmdItem.Parameters.Add(new SqlParameter("@IdUsuarioCreate", value.IdUsuarioCreate));

                                await cmdItem.ExecuteNonQueryAsync();
                            }

                            resultadoTransaccion.IdRegistro = 0;
                            resultadoTransaccion.ResultadoCodigo = 0;
                            resultadoTransaccion.ResultadoDescripcion = "Registro creado con éxito ..!";

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

        public async Task<ResultadoTransaccion<LecturaEntity>> SetDeleteMultiple(LecturaEntity value)
        {
            var resultadoTransaccion = new ResultadoTransaccion<LecturaEntity>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxDos))
                {
                    using (CommittableTransaction transaction = new CommittableTransaction())
                    {
                        await conn.OpenAsync();
                        conn.EnlistTransaction(transaction);

                        try
                        {
                            using (SqlCommand cmdItem = new SqlCommand(SP_SET_DELETE1, conn))
                            {
                                cmdItem.CommandType = CommandType.StoredProcedure;
                                cmdItem.CommandTimeout = 0;
                                cmdItem.Parameters.Add(new SqlParameter("@ObjType", value.ObjType));
                                cmdItem.Parameters.Add(new SqlParameter("@DocEntry", value.DocEntry));

                                await cmdItem.ExecuteNonQueryAsync();
                            }

                            resultadoTransaccion.IdRegistro = 0;
                            resultadoTransaccion.ResultadoCodigo = 0;
                            resultadoTransaccion.ResultadoDescripcion = "Registro elminado con éxito ..!";

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
        public async Task<ResultadoTransaccion<LecturaEntity>> SetDelete(int id)
        {
            var resultadoTransaccion = new ResultadoTransaccion<LecturaEntity>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxDos))
                {
                    using (CommittableTransaction transaction = new CommittableTransaction())
                    {
                        await conn.OpenAsync();
                        conn.EnlistTransaction(transaction);

                        try
                        {
                            using (SqlCommand cmdItem = new SqlCommand(SP_SET_DELETE2, conn))
                            {
                                cmdItem.CommandType = CommandType.StoredProcedure;
                                cmdItem.CommandTimeout = 0;
                                cmdItem.Parameters.Add(new SqlParameter("@IdLectura", id));

                                await cmdItem.ExecuteNonQueryAsync();
                            }

                            resultadoTransaccion.IdRegistro = 0;
                            resultadoTransaccion.ResultadoCodigo = 0;
                            resultadoTransaccion.ResultadoDescripcion = "Registro elminado con éxito ..!";

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

        public async Task<ResultadoTransaccion<LecturaByObjTypeAndDocEntryEntity>> GetListByObjTypeAndDocEntry(FiltroRequestEntity value)
        {
            var response = new List<LecturaByObjTypeAndDocEntryEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<LecturaByObjTypeAndDocEntryEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxDos))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_BY_OBJTYPE_AND_DOCENTRY, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@ObjType", value.Code1));
                        cmd.Parameters.Add(new SqlParameter("@DocEntry", value.Id1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<LecturaByObjTypeAndDocEntryEntity>)context.ConvertTo<LecturaByObjTypeAndDocEntryEntity>(reader);
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

        public async Task<ResultadoTransaccion<LecturaEntity>> GetListByFiltro(FiltroRequestEntity value)
        {
            var response = new List<LecturaEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<LecturaEntity>();

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
                        cmd.Parameters.Add(new SqlParameter("@FecInicial", value.Fecha1));
                        cmd.Parameters.Add(new SqlParameter("@FecFinal", value.Fecha2));
                        cmd.Parameters.Add(new SqlParameter("@Estado", value.Code1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<LecturaEntity>)context.ConvertTo<LecturaEntity>(reader);
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

        public async Task<ResultadoTransaccion<LecturaBarcodeByIdAndFiltro>> GetListByDocEntryAndObjTypeAndFiltro(FiltroRequestEntity value)
        {
            var response = new List<LecturaBarcodeByIdAndFiltro>();
            var resultadoTransaccion = new ResultadoTransaccion<LecturaBarcodeByIdAndFiltro>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxDos))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_BY_DOCENTRY_AND_OBJTYPE_AND_FILTRO, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@Id", value.Id1));
                        cmd.Parameters.Add(new SqlParameter("@ObjType", value.Code1));
                        cmd.Parameters.Add(new SqlParameter("@Filtro", value.TextFiltro1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<LecturaBarcodeByIdAndFiltro>)context.ConvertTo<LecturaBarcodeByIdAndFiltro>(reader);
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
