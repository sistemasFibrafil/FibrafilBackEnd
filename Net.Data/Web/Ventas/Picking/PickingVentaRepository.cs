using System;
using System.IO;
using System.Data;
using System.Linq;
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

namespace Net.Data.Sap
{
    public class PickingVentaRepository : RepositoryBase<PickingVentaEntity>, IPickingVentaRepository
    {
        private string _metodoName;
        private string _aplicacionName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        // PARAMETROS DE COXIÓN
        private readonly string _cnxDos;
        private readonly string _cnxSap;
        private readonly IConfiguration _configuration;

        // STORED PROCEDURE
        const string DB_ESQUEMA = "";
        const string SP_GET_PICKING_NUMERO = DB_ESQUEMA + "FIB_WEB_VE_SP_GetPickingVentaNumero";
        const string SP_GET_LIST_PICKING_BY_FILTRO = DB_ESQUEMA + "FIB_WEB_VE_SP_GetListPickingVentaByFiltro";
        const string SP_GET_PICKING_BY_IDPICKING = DB_ESQUEMA + "FIB_WEB_VE_SP_GetPickingVentaByIdPicking";
        const string SP_GET_LIST_PICKING_ITEM_BY_IDPICKING = DB_ESQUEMA + "FIB_WEB_VE_SP_GetListPickingVentaItemByIdPicking";

        const string SP_SET_PICKING_CREATE = DB_ESQUEMA + "FIB_WEB_VE_SP_SetPickingVentaCreate";
        const string SP_SET_PICKING_ITEM_CREATE = DB_ESQUEMA + "FIB_WEB_VE_SP_SetPickingVentaItemCreate";
        const string SP_SET_PICKING_BARCODE_CREATE = DB_ESQUEMA + "FIB_WEB_VE_SP_SetPickingVentaBarCodeCreate";
        const string SP_SET_PICKING_UPDATE = DB_ESQUEMA + "FIB_WEB_VE_SP_SetPickingVentaUpdate";

        const string SP_SET_PICKING_DELETE = DB_ESQUEMA + "FIB_WEB_VE_SP_SetPickingVentaDelete";
        const string SP_SET_PICKING_DELETE_ITEM = DB_ESQUEMA + "FIB_WEB_VE_SP_SetPickingVentaItemDelete";
        const string SP_SET_PICKING_DELETE_ITEM_ALL = DB_ESQUEMA + "FIB_WEB_VE_SP_SetPickingVentaItemDeleteAll";

        const string SP_GET_PICKING_FOR_ENTREGA_BY_IDPICKING = DB_ESQUEMA + "FIB_WEB_VE_SP_GetPickingVentaForEntregaByIdPicking";
        const string SP_GET_LIST_PICKING_ITEM_FOR_ENTREGA_BY_IDPICKING = DB_ESQUEMA + "FIB_WEB_VE_SP_GetListPickingVentaItemForEntregaByIdPicking";


        // ANTIGUO
        const string SP_GET_LIST_BY_DOCENTRY = DB_ESQUEMA + "FIB_SP_WEB_GetListPackListByDocEntry";
        const string SP_GET_LIST_ITEM_BY_DOCENTRY = DB_ESQUEMA + "FIB_SP_WEB_GetListPackListItemByDocEntry";


        public PickingVentaRepository(IConnectionSql context, IConfiguration configuration)
            : base(context)
        {
            _cnxDos = Utilidades.GetExtraerCadenaConexion(configuration, "ParametersConectionDos");
            _cnxSap = Utilidades.GetExtraerCadenaConexion(configuration, "ParametersConectionSap");
            _configuration = configuration;
            _aplicacionName = GetType().Name;
        }

        public async Task<ResultadoTransaccion<PickingVentaEntity>> GetPickingNumero()
        {
            var response = new PickingVentaEntity();
            var resultadoTransaccion = new ResultadoTransaccion<PickingVentaEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxDos))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_PICKING_NUMERO, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = context.Convert<PickingVentaEntity>(reader);
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

        public async Task<ResultadoTransaccion<PickingVentaByFiltroEntity>> GetListPickingVentaByFiltro(DateTime fecInicial, DateTime fecFinal)
        {
            var response = new List<PickingVentaByFiltroEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<PickingVentaByFiltroEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxDos))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_PICKING_BY_FILTRO, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@FecInicial", fecInicial));
                        cmd.Parameters.Add(new SqlParameter("@FecFinal", fecFinal));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<PickingVentaByFiltroEntity>)context.ConvertTo<PickingVentaByFiltroEntity>(reader);
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

        public async Task<ResultadoTransaccion<PickingVentaByIdPicking>> GetPickingVentaByIdPicking(int idPicking)
        {
            var response = new PickingVentaByIdPicking();
            var resultadoTransaccion = new ResultadoTransaccion<PickingVentaByIdPicking>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxDos))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_PICKING_BY_IDPICKING, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@IdPicking", idPicking));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = context.Convert<PickingVentaByIdPicking>(reader);
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_PICKING_ITEM_BY_IDPICKING, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@IdPicking", idPicking));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response.Item = (List<PickingVentaItemByIdPickingEntity>)context.ConvertTo<PickingVentaItemByIdPickingEntity>(reader);
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

        public async Task<ResultadoTransaccion<PickingVentaItemEntity>> GetListPickingVentaItemByIdPicking(int idPicking)
        {
            var response = new List<PickingVentaItemEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<PickingVentaItemEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxDos))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_PICKING_ITEM_BY_IDPICKING, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@IdPicking", idPicking));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<PickingVentaItemEntity>)context.ConvertTo<PickingVentaItemEntity>(reader);
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

        public async Task<ResultadoTransaccion<PickingVentaEntity>> SetCreate(PickingVentaEntity value)
        {
            var idPicking = 0;
            var resultadoTransaccion = new ResultadoTransaccion<PickingVentaEntity>();

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
                            using (SqlCommand cmd = new SqlCommand(SP_SET_PICKING_CREATE, conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 0;

                                cmd.Parameters.Add(new SqlParameter("@IdPicking", SqlDbType.Int)).Direction = ParameterDirection.Output;
                                cmd.Parameters.Add(new SqlParameter("@NumPicking", value.NumPicking));
                                cmd.Parameters.Add(new SqlParameter("@FecPicking", value.FecPicking));
                                cmd.Parameters.Add(new SqlParameter("@CodTipoPicking", value.CodTipoPicking));
                                cmd.Parameters.Add(new SqlParameter("@CodEstado", value.CodEstado));
                                cmd.Parameters.Add(new SqlParameter("@CardCode", value.CardCode));
                                cmd.Parameters.Add(new SqlParameter("@CardName", value.CardName));
                                cmd.Parameters.Add(new SqlParameter("@LicTradNum", value.LicTradNum));
                                cmd.Parameters.Add(new SqlParameter("@Comentarios", value.Comentarios));
                                cmd.Parameters.Add(new SqlParameter("@IdUsuarioCreate", value.IdUsuario));

                                await cmd.ExecuteNonQueryAsync();

                                idPicking = (int)cmd.Parameters["@IdPicking"].Value;
                            }

                            var listItem = (from t1 in value.Item select new { IdPicking = idPicking, t1.IdPickingItem, t1.DocEntry, t1.DocNum, t1.LineNum, t1.ObjType, t1.ItemCode, t1.Dscription, t1.WhsCode, t1.CodEstado, t1.IdUsuario, value.CodTipoPicking }).Distinct().ToList();

                            using (SqlCommand cmdItem = new SqlCommand(SP_SET_PICKING_ITEM_CREATE, conn))
                            {
                                cmdItem.CommandType = CommandType.StoredProcedure;
                                cmdItem.CommandTimeout = 0;

                                foreach (var item in listItem)
                                {
                                    cmdItem.Parameters.Clear();
                                    cmdItem.Parameters.Add(new SqlParameter("@IdPickingItem", SqlDbType.Int)).Direction = ParameterDirection.Output;
                                    cmdItem.Parameters.Add(new SqlParameter("@IdPicking", item.IdPicking));
                                    cmdItem.Parameters.Add(new SqlParameter("@DocEntry", item.DocEntry));
                                    cmdItem.Parameters.Add(new SqlParameter("@DocNum", item.DocNum));
                                    cmdItem.Parameters.Add(new SqlParameter("@LineNum", item.LineNum));
                                    cmdItem.Parameters.Add(new SqlParameter("@ObjType", item.ObjType));
                                    cmdItem.Parameters.Add(new SqlParameter("@ItemCode", item.ItemCode));
                                    cmdItem.Parameters.Add(new SqlParameter("@Dscription", item.Dscription));
                                    cmdItem.Parameters.Add(new SqlParameter("@WhsCode", item.WhsCode));
                                    cmdItem.Parameters.Add(new SqlParameter("@CodEstado", item.CodEstado));
                                    cmdItem.Parameters.Add(new SqlParameter("@IdUsuarioCreate", item.IdUsuario));

                                    await cmdItem.ExecuteNonQueryAsync();

                                    var idPickingItem = (int)cmdItem.Parameters["@IdPickingItem"].Value;

                                    var listBarCode = (from t1 in value.Item select new { IdPickingItem = idPickingItem, t1.DocEntry, t1.DocNum, t1.LineNum, t1.ObjType, t1.ItemCode, t1.BarCode, t1.WhsCode, t1.UnitMsr, t1.Quantity, t1.Peso, value.CodTipoPicking }).Where(x => x.DocEntry == item.DocEntry && x.DocNum == item.DocNum && x.LineNum == item.LineNum && x.ObjType == item.ObjType && x.ItemCode == item.ItemCode).Distinct().ToList();

                                    using (SqlCommand cmdBarCode = new SqlCommand(SP_SET_PICKING_BARCODE_CREATE, conn))
                                    {
                                        cmdBarCode.CommandType = CommandType.StoredProcedure;
                                        cmdBarCode.CommandTimeout = 0;

                                        foreach (var barCode in listBarCode)
                                        {
                                            cmdBarCode.Parameters.Clear();
                                            cmdBarCode.Parameters.Add(new SqlParameter("@IdPickingItem", barCode.IdPickingItem));
                                            cmdBarCode.Parameters.Add(new SqlParameter("@DocEntry", barCode.DocEntry));
                                            cmdBarCode.Parameters.Add(new SqlParameter("@ObjType", barCode.ObjType));
                                            cmdBarCode.Parameters.Add(new SqlParameter("@ItemCode", barCode.ItemCode));
                                            cmdBarCode.Parameters.Add(new SqlParameter("@BarCode", barCode.BarCode));
                                            cmdBarCode.Parameters.Add(new SqlParameter("@WhsCode", barCode.WhsCode));
                                            cmdBarCode.Parameters.Add(new SqlParameter("@UnitMsr", barCode.UnitMsr));
                                            cmdBarCode.Parameters.Add(new SqlParameter("@Quantity", barCode.Quantity));
                                            cmdBarCode.Parameters.Add(new SqlParameter("@Peso", barCode.Peso));

                                            await cmdBarCode.ExecuteNonQueryAsync();
                                        }
                                    }
                                }
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

        public async Task<ResultadoTransaccion<PickingVentaEntity>> SetUpdate(PickingVentaEntity value)
        {
            var idPickingItem = 0;
            var response = new PickingVentaEntity();
            var resultadoTransaccion = new ResultadoTransaccion<PickingVentaEntity>();

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
                            using (SqlCommand cmd = new SqlCommand(SP_SET_PICKING_UPDATE, conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 0;

                                cmd.Parameters.Add(new SqlParameter("@IdPicking", value.IdPicking));
                                cmd.Parameters.Add(new SqlParameter("@Comentarios", value.Comentarios));
                                cmd.Parameters.Add(new SqlParameter("@IdUsuarioUpdate", value.IdUsuario));

                                await cmd.ExecuteNonQueryAsync();
                            }

                            var listItem = (from t1 in value.Item
                                            select new { t1.IdPicking, t1.IdPickingItem, t1.DocEntry, t1.DocNum, t1.LineNum, t1.ObjType, t1.ItemCode, t1.Dscription, t1.WhsCode, t1.CodEstado, t1.IdUsuario, value.CodTipoPicking })
                                            .Distinct()
                                            .ToList();

                            using (SqlCommand cmdItem = new SqlCommand(SP_SET_PICKING_ITEM_CREATE, conn))
                            {
                                cmdItem.CommandType = CommandType.StoredProcedure;
                                cmdItem.CommandTimeout = 0;

                                foreach (var item in listItem)
                                {
                                    if (item.IdPickingItem == 0)
                                    {
                                        cmdItem.Parameters.Clear();
                                        cmdItem.Parameters.Add(new SqlParameter("@IdPickingItem", SqlDbType.Int)).Direction = ParameterDirection.Output;
                                        cmdItem.Parameters.Add(new SqlParameter("@IdPicking", item.IdPicking));
                                        cmdItem.Parameters.Add(new SqlParameter("@DocEntry", item.DocEntry));
                                        cmdItem.Parameters.Add(new SqlParameter("@DocNum", item.DocNum));
                                        cmdItem.Parameters.Add(new SqlParameter("@LineNum", item.LineNum));
                                        cmdItem.Parameters.Add(new SqlParameter("@ObjType", item.ObjType));
                                        cmdItem.Parameters.Add(new SqlParameter("@ItemCode", item.ItemCode));
                                        cmdItem.Parameters.Add(new SqlParameter("@Dscription", item.Dscription));
                                        cmdItem.Parameters.Add(new SqlParameter("@WhsCode", item.WhsCode));
                                        cmdItem.Parameters.Add(new SqlParameter("@CodEstado", item.CodEstado));
                                        cmdItem.Parameters.Add(new SqlParameter("@IdUsuarioCreate", item.IdUsuario));
                                        cmdItem.Parameters.Add(new SqlParameter("@CodTipoPicking", item.CodTipoPicking));

                                        await cmdItem.ExecuteNonQueryAsync();

                                        idPickingItem = (int)cmdItem.Parameters["@IdPickingItem"].Value;
                                    }

                                    var listBarCode = (from t1 in value.Item
                                                       select new { IdPickingItem = t1.IdPickingItem == 0 ? idPickingItem : t1.IdPickingItem, t1.DocEntry, t1.DocNum, t1.LineNum, t1.ObjType, t1.ItemCode, t1.BarCode, t1.WhsCode, t1.UnitMsr, t1.Quantity, t1.Peso, value.CodTipoPicking })
                                                       .Where(x => x.DocEntry == item.DocEntry && x.DocNum == item.DocNum && x.LineNum == item.LineNum && x.ObjType == item.ObjType && x.ItemCode == item.ItemCode)
                                                       .Distinct()
                                                       .ToList();

                                    using (SqlCommand cmdBarCode = new SqlCommand(SP_SET_PICKING_BARCODE_CREATE, conn))
                                    {
                                        cmdBarCode.CommandType = CommandType.StoredProcedure;
                                        cmdBarCode.CommandTimeout = 0;

                                        foreach (var barCode in listBarCode)
                                        {
                                            cmdBarCode.Parameters.Clear();
                                            cmdBarCode.Parameters.Add(new SqlParameter("@IdPickingItem", barCode.IdPickingItem));
                                            cmdBarCode.Parameters.Add(new SqlParameter("@DocEntry", barCode.DocEntry));
                                            cmdBarCode.Parameters.Add(new SqlParameter("@ObjType", barCode.ObjType));
                                            cmdBarCode.Parameters.Add(new SqlParameter("@ItemCode", barCode.ItemCode));
                                            cmdBarCode.Parameters.Add(new SqlParameter("@BarCode", barCode.BarCode));
                                            cmdBarCode.Parameters.Add(new SqlParameter("@WhsCode", barCode.WhsCode));
                                            cmdBarCode.Parameters.Add(new SqlParameter("@UnitMsr", barCode.UnitMsr));
                                            cmdBarCode.Parameters.Add(new SqlParameter("@Quantity", barCode.Quantity));
                                            cmdBarCode.Parameters.Add(new SqlParameter("@Peso", barCode.Peso));
                                            cmdBarCode.Parameters.Add(new SqlParameter("@CodTipoPicking", value.CodTipoPicking));

                                            await cmdBarCode.ExecuteNonQueryAsync();
                                        }
                                    }
                                }
                            }

                            resultadoTransaccion.IdRegistro = 0;
                            resultadoTransaccion.ResultadoCodigo = 0;
                            resultadoTransaccion.ResultadoDescripcion = "Registro actualizado con éxito ..!";
                            resultadoTransaccion.data = response;

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

        public async Task<ResultadoTransaccion<PickingVentaEntity>> SetDelete(PickingVentaEntity value)
        {
            var resultadoTransaccion = new ResultadoTransaccion<PickingVentaEntity>();

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
                            using (SqlCommand cmd = new SqlCommand(SP_SET_PICKING_DELETE, conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 0;
                                cmd.Parameters.Add(new SqlParameter("@IdPicking", value.IdPicking));
                                cmd.Parameters.Add(new SqlParameter("@IdUsuarioDelete", value.IdUsuario));

                                await cmd.ExecuteNonQueryAsync();
                            }

                            resultadoTransaccion.IdRegistro = 0;
                            resultadoTransaccion.ResultadoCodigo = 0;
                            resultadoTransaccion.ResultadoDescripcion = "Registro eliminado con éxito ..!";

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

        public async Task<ResultadoTransaccion<PickingVentaItemEntity>> SetDeleteItem(int idPickingItem)
        {
            var resultadoTransaccion = new ResultadoTransaccion<PickingVentaItemEntity>();

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
                            using (SqlCommand cmd = new SqlCommand(SP_SET_PICKING_DELETE_ITEM, conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 0;
                                cmd.Parameters.Add(new SqlParameter("@IdPickingItem", idPickingItem));

                                await cmd.ExecuteNonQueryAsync();
                            }

                            resultadoTransaccion.IdRegistro = 0;
                            resultadoTransaccion.ResultadoCodigo = 0;
                            resultadoTransaccion.ResultadoDescripcion = "Registro eliminado con éxito ..!";

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

        public async Task<ResultadoTransaccion<PickingVentaItemEntity>> SetDeleteItemAll(PickingVentaItemEntity value)
        {
            var resultadoTransaccion = new ResultadoTransaccion<PickingVentaItemEntity>();

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
                            using (SqlCommand cmd = new SqlCommand(SP_SET_PICKING_DELETE_ITEM_ALL, conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 0;
                                cmd.Parameters.Add(new SqlParameter("@IdPicking", value.IdPicking));
                                cmd.Parameters.Add(new SqlParameter("@DocEntry", value.DocEntry));
                                cmd.Parameters.Add(new SqlParameter("@ObjType", value.ObjType));
                                cmd.Parameters.Add(new SqlParameter("@LineNum", value.LineNum));
                                cmd.Parameters.Add(new SqlParameter("@ItemCode", value.ItemCode));
                                cmd.Parameters.Add(new SqlParameter("@IdUsuarioDelete", value.IdUsuario));

                                await cmd.ExecuteNonQueryAsync();
                            }

                            resultadoTransaccion.IdRegistro = 0;
                            resultadoTransaccion.ResultadoCodigo = 0;
                            resultadoTransaccion.ResultadoDescripcion = "Registro eliminado con éxito ..!";

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


        public async Task<ResultadoTransaccion<PickingVentaForEntregaByIdPicking>> GetPickingVentaForEntregaByIdPicking(int idPicking)
        {
            var response = new PickingVentaForEntregaByIdPicking();
            var resultadoTransaccion = new ResultadoTransaccion<PickingVentaForEntregaByIdPicking>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxDos))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_PICKING_FOR_ENTREGA_BY_IDPICKING, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@IdPicking", idPicking));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = context.Convert<PickingVentaForEntregaByIdPicking>(reader);
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_PICKING_ITEM_FOR_ENTREGA_BY_IDPICKING, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@IdPicking", idPicking));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response.Item = (List<PickingVentaItemForEntregaByIdPickingEntity>)context.ConvertTo<PickingVentaItemForEntregaByIdPickingEntity>(reader);
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


        public async Task<ResultadoTransaccion<MemoryStream>> GetListPickingPdfByDocEntry(int docEntry)
        {
            var listpackingListSap = new List<PickingVentaItem1Entity>();
            var listpackingListItemSap = new List<PickingVentaItem2Entity>();
            var resultadoTransaccion = new ResultadoTransaccion<MemoryStream>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_BY_DOCENTRY, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@DocEntry", docEntry));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            listpackingListSap = (List<PickingVentaItem1Entity>)context.ConvertTo<PickingVentaItem1Entity>(reader);
                        }
                    }


                    iTextSharp.text.Document doc = new iTextSharp.text.Document();
                    doc.SetPageSize(iTextSharp.text.PageSize.A4);
                    doc.SetMargins(15f, 10f, 120f, 15f);
                    MemoryStream ms = new MemoryStream();
                    iTextSharp.text.pdf.PdfWriter write = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, ms);
                    write.ViewerPreferences = iTextSharp.text.pdf.PdfWriter.PageModeUseOutlines;
                    // Our custom Header and Footer is done using Event Handler
                    PageEventHelperPicking pageEventHelperSolicitudViaje = new PageEventHelperPicking();
                    write.PageEvent = pageEventHelperSolicitudViaje;

                    // Colocamos la fuente que deseamos que tenga el documento
                    iTextSharp.text.pdf.BaseFont helvetica = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA, iTextSharp.text.pdf.BaseFont.CP1250, true);
                    // Titulo
                    iTextSharp.text.Font parrafoNegro = new iTextSharp.text.Font(helvetica, 11f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.Black);
                    iTextSharp.text.Font parrafoItem = new iTextSharp.text.Font(helvetica, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.Black);
                    iTextSharp.text.Font parrafoNormal = new iTextSharp.text.Font(helvetica, 11f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.Black);
                    iTextSharp.text.Font parrafoNegroItalic = new iTextSharp.text.Font(helvetica, 10f, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.Black);

                    // Define the page header
                    pageEventHelperSolicitudViaje.Title = "PICKING LIST";
                    pageEventHelperSolicitudViaje.Cliente = listpackingListSap.Count == 0? "" : listpackingListSap[0].CardName;
                    pageEventHelperSolicitudViaje.Contenedor = listpackingListSap.Count == 0 ? "" : listpackingListSap[0].Contenedor;

                    try
                    {
                        doc.Open();
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    
                    foreach (var packingListSap in listpackingListSap)
                    {
                        //============================
                        //Tabla: 2
                        var tbl = new iTextSharp.text.pdf.PdfPTable(new float[] { 100f }) { WidthPercentage = 100 };
                        //Línea 1
                        var c1 = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(packingListSap.ItemName, parrafoNormal)) { BorderWidth = 0, PaddingBottom = 10, PaddingTop = 10 };
                        tbl.AddCell(c1);

                        doc.Add(tbl);

                        using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_ITEM_BY_DOCENTRY, conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add(new SqlParameter("@DocEntry", docEntry));
                            cmd.Parameters.Add(new SqlParameter("@ItemCode", packingListSap.ItemCode));

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                listpackingListItemSap = (List<PickingVentaItem2Entity>)context.ConvertTo<PickingVentaItem2Entity>(reader);
                            }
                        }

                        //============================
                        //Tabla: 3
                        tbl = new iTextSharp.text.pdf.PdfPTable(new float[] { 25f, 25f, 25f, 25f }) { WidthPercentage = 100 };

                        foreach (var packingListItemSap in listpackingListItemSap)
                        {
                            c1 = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(packingListItemSap.CodeBar1, parrafoItem)) { BorderWidth = 1, Padding = 5 };
                            tbl.AddCell(c1);
                            c1 = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(packingListItemSap.CodeBar2, parrafoItem)) { BorderWidth = 1, Padding = 5 };
                            tbl.AddCell(c1);
                            c1 = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(packingListItemSap.CodeBar3, parrafoItem)) { BorderWidth = 1, Padding = 5 };
                            tbl.AddCell(c1);
                            c1 = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(packingListItemSap.CodeBar4, parrafoItem)) { BorderWidth = 1, Padding = 5 };
                            tbl.AddCell(c1);
                        }

                        doc.Add(tbl);

                        //============================
                        //Tabla: 4
                        tbl = new iTextSharp.text.pdf.PdfPTable(new float[] { 15f, 2f, 15f, 36f, 15f, 2f, 15f }) { WidthPercentage = 100 };
                        c1 = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Total Items", parrafoNegro)) { BorderWidth = 0, PaddingBottom = 10, PaddingTop = 10 };
                        tbl.AddCell(c1);
                        c1 = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(":", parrafoNegro)) { BorderWidth = 0, PaddingBottom = 10, PaddingTop = 10 };
                        tbl.AddCell(c1);
                        c1 = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(listpackingListItemSap[0].TotalItem.ToString(), parrafoNormal)) { BorderWidth = 0, PaddingBottom = 10, PaddingTop = 10 };
                        tbl.AddCell(c1);
                        c1 = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("", parrafoNormal)) { BorderWidth = 0, PaddingBottom = 10, PaddingTop = 10 };
                        tbl.AddCell(c1);
                        c1 = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Peso Total", parrafoNegro)) { BorderWidth = 0, PaddingBottom = 10, PaddingTop = 10 };
                        tbl.AddCell(c1);
                        c1 = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(":", parrafoNegro)) { BorderWidth = 0, PaddingBottom = 10, PaddingTop = 10 };
                        tbl.AddCell(c1);
                        c1 = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(listpackingListItemSap[0].PesoTotal.ToString(), parrafoNormal)) { BorderWidth = 0, PaddingBottom = 10, PaddingTop = 10 };
                        tbl.AddCell(c1);

                        doc.Add(tbl);
                    }


                    write.Close();
                    doc.Close();
                    ms.Seek(0, SeekOrigin.Begin);
                    var file = ms;

                    resultadoTransaccion.IdRegistro = 0;
                    resultadoTransaccion.ResultadoCodigo = 0;
                    resultadoTransaccion.ResultadoDescripcion = "Se generó correctamente el archivo.s";
                    resultadoTransaccion.data = file;
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

    public class PageEventHelperPicking : iTextSharp.text.pdf.PdfPageEventHelper
    {
        iTextSharp.text.pdf.PdfContentByte cb;
        iTextSharp.text.pdf.PdfTemplate footerTemplate;
        iTextSharp.text.pdf.BaseFont bfTitulo = null;
        iTextSharp.text.pdf.BaseFont bfTexto = null;
        DateTime PrintTime = DateTime.Now;

        #region Properties
        public string Title { get; set; }
        public string Cliente { get; set; }
        public string Contenedor { get; set; }
        #endregion

        // we override the onOpenDocument method
        public override void OnOpenDocument(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
        {
            try
            {
                bfTitulo = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA_BOLD, iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED);
                bfTexto = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA, iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                PrintTime = DateTime.Now;
                footerTemplate = cb.CreateTemplate(50, 50);
            }
            catch (iTextSharp.text.DocumentException)
            {
            }
            catch (IOException)
            {
            }
        }

        public override void OnStartPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
        {
            base.OnStartPage(writer, document);
            iTextSharp.text.Rectangle pageSize = document.PageSize;
            iTextSharp.text.Font parrafoNormal = new iTextSharp.text.Font(bfTitulo, 11f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.Black);

            if (Title != string.Empty)
            {
                //Titulo
                cb.BeginText();
                cb.SetFontAndSize(bfTitulo, 25);
                cb.SetTextMatrix(pageSize.GetRight(380), pageSize.GetTop(55));
                cb.ShowText(Title);
                cb.EndText();

                //Logo
                var pathLogo = Path.Combine(Environment.CurrentDirectory, "logos", "fibrafil-logo.jpg");

                var logo = iTextSharp.text.Image.GetInstance(pathLogo);

                logo.ScaleToFit(100f, 35f);
                logo.SetAbsolutePosition(document.Left, pageSize.GetTop(55));
                cb.AddImage(logo);

                ////=========================
                /// INICIO: CLIENTE
                ////=========================
                cb.BeginText();
                cb.SetFontAndSize(bfTitulo, 12);
                cb.SetTextMatrix(pageSize.GetLeft(15), pageSize.GetTop(90));
                cb.ShowText("CLIENTE");
                cb.EndText();

                cb.BeginText();
                cb.SetFontAndSize(bfTitulo, 12);
                cb.SetTextMatrix(pageSize.GetLeft(105), pageSize.GetTop(90));
                cb.ShowText(":");
                cb.EndText();

                cb.BeginText();
                cb.SetFontAndSize(bfTexto, 12);
                cb.SetTextMatrix(pageSize.GetLeft(115), pageSize.GetTop(90));
                cb.ShowText(Cliente);
                cb.EndText();

                ////=========================
                /// FIN: CLIENTE
                ////=========================


                ////=========================
                /// INICIO: CONTENEDOR
                ////=========================
                cb.BeginText();
                cb.SetFontAndSize(bfTitulo, 12);
                cb.SetTextMatrix(pageSize.GetLeft(15), pageSize.GetTop(110));
                cb.ShowText("CONTENEDOR");
                cb.EndText();

                cb.BeginText();
                cb.SetFontAndSize(bfTitulo, 12);
                cb.SetTextMatrix(pageSize.GetLeft(105), pageSize.GetTop(110));
                cb.ShowText(":");
                cb.EndText();

                cb.BeginText();
                cb.SetFontAndSize(bfTexto, 12);
                cb.SetTextMatrix(pageSize.GetLeft(115), pageSize.GetTop(110));
                cb.ShowText(Contenedor);
                cb.EndText();

                ////=========================
                /// FIN: CONTENEDOR
                ////=========================
            }
        }
        public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
        {
            base.OnEndPage(writer, document);

            /*
                =====================================================
                Codigo para que el número de página muestre en el pie
                =====================================================
            */
            int pageN = writer.PageNumber;
            string text = "Página " + pageN + "/";
            float len = bfTexto.GetWidthPoint(text, 8);
            iTextSharp.text.Rectangle pageSize = document.PageSize;
            cb.SetRgbColorFill(100, 100, 100);
            cb.BeginText();
            cb.SetFontAndSize(bfTexto, 8);
            cb.SetTextMatrix(pageSize.GetLeft(15), pageSize.GetBottom(30));
            cb.ShowText(text);
            cb.EndText();
            cb.AddTemplate(footerTemplate, pageSize.GetLeft(15) + len, pageSize.GetBottom(30));
        }
        public override void OnCloseDocument(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
        {
            base.OnCloseDocument(writer, document);

            /*
               =====================================================
               Codigo para que el número de página muestre en el pie
               =====================================================
           */
            footerTemplate.BeginText();
            footerTemplate.SetFontAndSize(bfTexto, 8);
            footerTemplate.SetTextMatrix(0, 0);
            footerTemplate.ShowText("" + (writer.PageNumber - 1));
            footerTemplate.EndText();
        }
    }
}
