using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using UserBusinessEntities;

namespace UserAPI.Controllers
{
    public class UserController : ApiController
    {
        [HttpPost]
        [Route("~/api/User/GetUserList/")]
        public UserListcs GetSalesInvoiceList(SPParametersBE parameters)
        {
            UserListcs user = new UserListcs();
            Int32 recordsTotal = 0;
            ObjectParameter objParam = new ObjectParameter("TotalRecord", recordsTotal);


            var connString = ConfigurationManager.ConnectionStrings["Conn"].ConnectionString;
            //EntityConnection ec = new EntityConnection(connString);


            SqlConnection con = new SqlConnection(connString);

            SqlCommand cmd3 = new SqlCommand();
            cmd3.CommandType = CommandType.StoredProcedure;
            cmd3.Connection = con;
            cmd3.CommandText = "SP_GetUserDetail";
            
            cmd3.Parameters.AddWithValue("@PageNo", parameters.PageNo);
            cmd3.Parameters.AddWithValue("@PageSize", parameters.PageSize);
            cmd3.Parameters.AddWithValue("@SortColumn", parameters.SortColumn);
          
            cmd3.Parameters.AddWithValue("@WhereClause", parameters.WhereClause);
            cmd3.Parameters.Add("@TotalRecord", SqlDbType.Int);
            cmd3.Parameters["@TotalRecord"].Direction = ParameterDirection.Output;
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter adpt = new SqlDataAdapter(cmd3);

            adpt.Fill(dt);
            con.Close();

            var t_userlist = ConvertDataTable<GetUserList>(dt);
            user.TotalRecords = (int)cmd3.Parameters["@TotalRecord"].Value;
            user.list = t_userlist;
            return user;
        }

        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        public static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        object value = dr[column.ColumnName];

                        if (value == DBNull.Value)
                            value = null;
                        pro.SetValue(obj, value, null);

                        break;
                    }
                    else
                        continue;
                }
            }
            return obj;
        }
    }
}
