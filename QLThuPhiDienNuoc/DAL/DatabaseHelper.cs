using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QLThuPhiDienNuoc.DAL
{
    internal class DatabaseHelper
    {
        private string connectionString = @"Server=DESKTOP-I03QOJG; Database=QuanLyDienNuoc;User Id=sa; Password=123; Integrated Security=True; TrustServerCertificate=True;";

        public DataTable ExecuteQuery(string query, object[] parameter = null)
        {
            DataTable data = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                if (parameter != null)
                {
                    var listPara = Regex.Matches(query, @"@\w+").Cast<Match>().Select(m => m.Value).Distinct().ToList();

                    for (int i = 0; i < listPara.Count; i++)
                    {
                        command.Parameters.AddWithValue(listPara[i], parameter[i] ?? DBNull.Value);
                    }
                }
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(data);
                connection.Close();
            }
            return data;
        }

        public int ExecuteNonQuery(string query, object[] parameter = null)
        {
            int data = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                if (parameter != null)
                {

                    var listPara = Regex.Matches(query, @"@\w+").Cast<Match>().Select(m => m.Value).Distinct().ToList();

                    for (int i = 0; i < listPara.Count; i++)
                    {
                        command.Parameters.AddWithValue(listPara[i], parameter[i] ?? DBNull.Value);
                    }
                }
                data = command.ExecuteNonQuery();
                connection.Close();
            }
            return data;
        }
    }
}
