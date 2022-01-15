using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ut9._03___App_gestión_de_EmpresaOnline
{
   public class BaseDatos
    {
        private SqlConnection conn;
        private SqlCommand cmd;

        public SqlConnection Conn { get => conn; set => conn = value; }

        //private SqlTransaction transaction;

        public BaseDatos()
        {
            conn = new SqlConnection(@"Data Source=DESKTOP-JIFRN8E\DAM1SQLSERVER;Initial Catalog=EmpresaOnline;Persist Security Info=True;User ID=sa;Password=dam1");

            cmd = new SqlCommand();

            cmd.Connection = conn;

        }
        public bool abrir()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
                return true;
            }

            return false;
        }

        public bool cerrar()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
                return true;
            }

            return false;
        }
        public DataSet CargarSQL(string sql)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                this.cmd.CommandText = sql;

                da.SelectCommand = cmd;
                da.Fill(ds, "TCliente");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Se produjo el siguiente error: \n" + ex.Message, "DB ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            return ds;
        }
        public int SelectMax()
        {
            cmd.CommandText = "SELECT MAX(nClienteID) FROM TCliente";
            int numMax = int.Parse(cmd.ExecuteScalar().ToString()) + 1;
            return numMax;
        }
        public object EjecutarSQL(string cmd)
        {
            this.cmd.CommandText = cmd.Trim();

            if (this.cmd.CommandText.ToUpper().StartsWith("SELECT"))
            {
                return EjecutarSQLEscalar();
            }
            else if (this.cmd.CommandText.ToUpper().StartsWith("INSERT") ||
                     this.cmd.CommandText.ToUpper().StartsWith("UPDATE") ||
                     this.cmd.CommandText.ToUpper().StartsWith("DELETE"))
            {
                return EjecutarSQLNonQuery();
            }

            return null;
        }
        public object EjecutarSQLEscalar()
        {
            abrir();

            object ret = null;

            try
            {
                ret = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Se producjo el siguiente error:\n" + ex.Message, "DB Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            cerrar();

            return ret;
        }

        public int EjecutarSQLNonQuery()
        {
            int filasAfectadas = -1;

            try
            {
                filasAfectadas = cmd.ExecuteNonQuery();
            }
            catch (Exception )
            {
                MessageBox.Show("Se escriberon mal los datos", "DB Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return filasAfectadas;
        }

        public int SelectMaxProd()
        {
            cmd.CommandText = "SELECT MAX(nNumProd) FROM TProducto";
            int numMax = int.Parse(cmd.ExecuteScalar().ToString()) + 1;
            return numMax;
        }

        public int SelectMaxProv()
        {
            cmd.CommandText = "SELECT MAX(nProveedorID) FROM TProveedor";
            int numMax = int.Parse(cmd.ExecuteScalar().ToString()) + 1;
            return numMax;
        }

    }

}
