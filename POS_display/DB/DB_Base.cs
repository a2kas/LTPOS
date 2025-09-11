using System.Data.SQLite;
using Npgsql;
using POS_display.Utils.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;

namespace POS_display
{
    public delegate void dbDataTableCallback(bool success, DataTable t);
    //--------------
    public delegate void dbDataTableDelegate(NpgsqlCommand cmd, dbDataTableCallback callback);

    public class DB_Base
    {
        #region Callbacks
        public static void DataTableDelegateEnd_cb(IAsyncResult result)
        {
            dbDataTableDelegate caller = (dbDataTableDelegate)((AsyncResult)result).AsyncDelegate;
            caller.EndInvoke(result);   
        }
        #endregion

        public NpgsqlConnection GetConnection()
        {
            if (string.IsNullOrEmpty(Session.ServerIP) || string.IsNullOrEmpty(Session.Database))
                return null;

            string connectionString = String.Format("user id={0};" +
                                                           "password={1};" +
                                                           "server={2};" +
                                                           "database={3};" +
                                                           "pooling=true;" +
                                                           "maxpoolsize=120;",
                                                           Session.Username,
                                                           Session.Password,
                                                           Session.ServerIP,
                                                           Session.Database);
            if (Session.Port != "")
                connectionString += String.Format("port={0}", Session.Port);

            return new NpgsqlConnection(connectionString);
        }

        public NpgsqlConnection GetConnectionWithoutCommandTimeout()
        {
            if (string.IsNullOrEmpty(Session.ServerIP) || string.IsNullOrEmpty(Session.Database))
                return null;

            string connectionString = String.Format("user id={0};" +
                                                           "password={1};" +
                                                           "server={2};" +
                                                           "database={3};" +
                                                           "pooling=true;" +
                                                           "maxpoolsize=120;" +
                                                           "commandtimeout=0;",
                                                           Session.Username,
                                                           Session.Password,
                                                           Session.ServerIP,
                                                           Session.Database);
            if (Session.Port != "")
                connectionString += String.Format("port={0}", Session.Port);

            return new NpgsqlConnection(connectionString);
        }

        public SQLiteConnection GetSqliteConnection()
        {
            return new SQLiteConnection($"Data Source={Session.SqliteDatabase}");
        }

        protected void DoSelect(NpgsqlCommand cmd, dbDataTableCallback callback)
        {
            NpgsqlConnection Conn = GetConnection();
            DataTable t = new DataTable();
            bool success = false;
            try
            {
                Conn.Open();
                cmd.Connection = Conn;
                cmd.CommandTimeout = 180;//3 min
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                adapter.Fill(t);
                success = true;
            }
            catch (Exception e)
            {
                Serilogger.GetLogger().Error(e, e.Message);
                helpers.alert(Enumerator.alert.error, e.Message);
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
                Conn.Close();
                callback(success, t);
            }
        }

        #region Async Tasks
        protected async Task<List<T>> DoSelectList<T>(NpgsqlCommand cmd)
        {
            NpgsqlConnection Conn = GetConnection();
            List<T> list = new List<T>();
            try
            {
                Conn.Open();
                cmd.Connection = Conn;
                cmd.CommandTimeout = 180;//3 min
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader == null)
                        throw new NullReferenceException();
                    list = reader.ToList<T>();
                }
            }
            catch (Exception e)
            {
                Serilogger.GetLogger().Error(e, e.Message);
                helpers.alert(Enumerator.alert.error, e.Message);
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
                Conn.Close();
            }

            return list;
        }

        protected async Task<DataTable> DoSelectDataTable(NpgsqlCommand cmd)
        {
            NpgsqlConnection Conn = GetConnection();
            DataTable t = new DataTable();
            try
            {
                await Conn.OpenAsync();
                cmd.Connection = Conn;
                cmd.CommandTimeout = 180;//3 min
                t = await Task<DataTable>.Run(() =>
                {
                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                });
            }
            catch (Exception e)
            {
                Serilogger.GetLogger().Error(e, e.Message);
                helpers.alert(Enumerator.alert.error, e.Message);
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
                Conn.Close();
            }

            return t;
        }

        protected async Task<T> DoSelectValue<T>(NpgsqlCommand cmd)
        {
            NpgsqlConnection Conn = GetConnection();
            T response = default(T);
            try
            {
                Conn.Open();
                cmd.Connection = Conn;
                cmd.CommandTimeout = 180;//3 min
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        await reader.ReadAsync();
                        response = await reader.GetFieldValueAsync<T>(0);
                    }
                }
            }
            catch (Exception e)
            {
                Serilogger.GetLogger().Error(e, e.Message);
                helpers.alert(Enumerator.alert.error, e.Message);
            }
            finally
            {
                Conn.Close();
                cmd.Dispose();
                cmd = null;
            }

            return response;
        }

        protected async Task<bool> DoSelectVoid(NpgsqlCommand cmd)
        {
            NpgsqlConnection Conn = GetConnection();
            bool result = false;
            try
            {
                Conn.Open();
                cmd.Connection = Conn;
                cmd.CommandTimeout = 300;//5 min
                int res = await cmd.ExecuteNonQueryAsync();
                result = true;
            }
            catch (Exception e)
            {
                Serilogger.GetLogger().Error(e, e.Message);
                helpers.alert(Enumerator.alert.error, e.Message);
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
                Conn.Close();
            }

            return result;
        }
        #endregion

        #region Sync Commands
        protected DataTable queryGetDataTable(NpgsqlCommand cmd)
        {
            NpgsqlConnection Conn = GetConnection();
            DataTable t = new DataTable();
            try
            {
                Conn.Open();
                cmd.Connection = Conn;
                cmd.CommandTimeout = 180;//3 min
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                adapter.Fill(t);
            }
            catch (Exception e)
            {
                Serilogger.GetLogger().Error(e, e.Message);
                helpers.alert(Enumerator.alert.error, e.Message);
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
                Conn.Close();
            }

            return t;
        }

        protected T queryGetClass<T>(NpgsqlCommand cmd)
        {
            NpgsqlConnection Conn = GetConnection();
            T obj = default(T);
            try
            {
                Conn.Open();
                cmd.Connection = Conn;
                cmd.CommandTimeout = 180;//3 min
                using (NpgsqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr == null)
                        throw new NullReferenceException();
                    obj = dr.ToClass<T>();
                }
            }
            catch (Exception e)
            {
                Serilogger.GetLogger().Error(e, e.Message);
                helpers.alert(Enumerator.alert.error, e.Message);
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
                Conn.Close();
            }
            return obj;
        }

        protected List<T> queryGetList<T>(NpgsqlCommand cmd)
        {
            NpgsqlConnection Conn = GetConnection();
            List<T> list = new List<T>();
            try
            {
                Conn.Open();
                cmd.Connection = Conn;
                cmd.CommandTimeout = 180;//3 min
                using (NpgsqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr == null)
                        throw new NullReferenceException();
                    list = dr.ToList<T>();
                }
            }
            catch (Exception e)
            {
                Serilogger.GetLogger().Error(e, e.Message);
                helpers.alert(Enumerator.alert.error, e.Message);
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
                Conn.Close();
            }

            return list;
        }
        #endregion
    }
}
