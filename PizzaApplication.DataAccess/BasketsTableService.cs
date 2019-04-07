using PizzaApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApplication.DataAccess
{
    public class BasketsTableService
    {

        private readonly string _connectionString = "";
        private SqlTransaction transaction;

        public BasketsTableService()
        {
            _connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;
                                AttachDbFilename=PIZZAAPPLICATION.DATAACCESS\PIZZAAPPLICATION.MDF;
                                Integrated Security=True";
        }

        public List<Basket> SelectUsers()
        {
            var data = new List<Basket>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = "select * from Baskets";

                    var sqlDataReader = command.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        int id = (int)sqlDataReader["Id"];
                        int userId = (int)sqlDataReader["UserId"];

                        data.Add(new Basket()
                        {
                            Id = id,
                            UserId = userId
                        });
                    }
                }
                catch (SqlException exception)
                {
                    throw;
                }
                catch (Exception exception)
                {
                    throw;
                }
            }
            return data;
        }

        public void InsertBasket(Basket basket)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    command.CommandText = "insert into Users values(@userId)";

                    var userIdParameter = new SqlParameter();
                    userIdParameter.ParameterName = "@login";
                    userIdParameter.SqlDbType = System.Data.SqlDbType.Int;
                    userIdParameter.SqlValue = basket.UserId;

                    command.Parameters.Add(userIdParameter);

                    command.Transaction = transaction;

                    int affectedRows = command.ExecuteNonQuery();

                    if (affectedRows < 1)
                    {
                        throw new Exception("Вставка не удалась");
                    }

                    transaction.Commit();
                }
                catch (SqlException exception)
                {
                    transaction?.Rollback();
                    throw;
                }
                catch (Exception exception)
                {
                    transaction?.Rollback();
                    throw;
                }
            }
        }
    }
}
