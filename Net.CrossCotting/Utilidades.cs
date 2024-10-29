using System;
using Microsoft.Extensions.Configuration;
using Net.Business.Entities;
namespace Net.CrossCotting
{
    public static class Utilidades
    {
        public static string GetExtraerCadenaConexion(IConfiguration configuration, string _entorno)
        {
            string _cnx = string.Empty;

            _cnx = GetDesencriptarCadenaConexion
                   (
                        configuration.GetConnectionString("cnnSql"),
                        configuration[string.Format("{0}:Source", _entorno)],
                        configuration[string.Format("{0}:Catalog", _entorno)],
                        configuration[string.Format("{0}:User", _entorno)],
                        configuration[string.Format("{0}:Password", _entorno)]
                   );

            return _cnx;
        }

        public static string GetDesencriptarCadenaConexion(string _bodyConexion, string _source, string _catalog, string _user, string _password)
        {
            string _cnx = string.Empty;

            //_source = EncriptaHelper.EncryptStringAES(_source);
            //_catalog = EncriptaHelper.EncryptStringAES(_catalog);
            //_user = EncriptaHelper.EncryptStringAES(_user);
            //_password = EncriptaHelper.EncryptStringAES(_password);

            //_source = EncriptaHelper.DecryptStringAES(_source);
            //_catalog = EncriptaHelper.DecryptStringAES(_catalog);
            //_user = EncriptaHelper.DecryptStringAES(_user);
            //_password = EncriptaHelper.DecryptStringAES(_password);

            _cnx = _bodyConexion.Replace("{Source}", _source).Replace("{Catalog}", _catalog).Replace("{User}", _user).Replace("{Password}", _password);

            return _cnx;
        }


        public static ConnectionSapEntity GetExtraerCadenaConexionDiApiSap(IConfiguration configuration, string _entorno)
        {
            var _cnx = new ConnectionSapEntity()
            {
                Server = configuration[string.Format("{0}:Server", _entorno)],
                LicenseServer = configuration[string.Format("{0}:LicenseServer", _entorno)],
                DbUserName = configuration[string.Format("{0}:DbUserName", _entorno)],
                DbPassword = configuration[string.Format("{0}:DbPassword", _entorno)],
                DbServerType = configuration[string.Format("{0}:DbServerType", _entorno)],

                CompanyDB = configuration[string.Format("{0}:CompanyDB", _entorno)],
                UserName = configuration[string.Format("{0}:UserName", _entorno)],
                Password = configuration[string.Format("{0}:Password", _entorno)],
            };

            return _cnx;
        }

        public static DateTime DateTimeEmpty()
        {
            return new DateTime(1, 1, 1, 0, 0, 0);
        }

        public static DateTime GetFechaHoraInicioActual(DateTime? fecha)
        {
            DateTime? data;

            data = fecha;

            if (fecha == null || fecha.Equals(DateTimeEmpty()))
            {
                data = new DateTime(DateTime.Now.Year, 1, DateTime.Now.Day, 0, 0, 0);
            } else
            {
                data = new DateTime(((DateTime)fecha).Year, ((DateTime)fecha).Month, ((DateTime)fecha).Day, 0, 0, 0);
            }

            return (DateTime)data;
        }

        public static DateTime GetFechaHoraFinActual(DateTime? fecha)
        {
            DateTime? data;

            data = fecha;

            if (fecha == null || fecha.Equals(DateTimeEmpty()))
            {
                data = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            } else
            {
                data = new DateTime(((DateTime)fecha).Year, ((DateTime)fecha).Month, ((DateTime)fecha).Day, 23, 59, 59);
            }

            return (DateTime)data; 
        }

        public static T Clone<T>(this T obj)
        {
            var inst = obj.GetType().GetMethod("MemberwiseClone", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            return (T)inst?.Invoke(obj, null);
        }

        public static string MensajeError(string mensaje)
        {
            int indexDescripcion = mensaje.IndexOf("*");

            string newDescripcion = mensaje;

            if (indexDescripcion > 0)
            {
                newDescripcion = mensaje.Substring(0, indexDescripcion);
                return newDescripcion;
            }

            return newDescripcion;
        }
    }
}
