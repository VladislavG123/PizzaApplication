using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApplication.DataAccess
{
    public class BasketsAndPizzasTableService
    {
        private readonly string _connectionString = "";
        private SqlTransaction transaction;

        public BasketsAndPizzasTableService()
        {
            _connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;
                                AttachDbFilename=PIZZAAPPLICATION.DATAACCESS\PIZZAAPPLICATION.MDF;
                                Integrated Security=True";
        }

        public List<int[]> SelectValues()
        {
            var data = new List<int[]>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = "select * from BasketsAndPizzas";

                    var sqlDataReader = command.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        int basketId = (int)sqlDataReader["BasketId"];
                        int pizzaId = (int)sqlDataReader["PizzaId"];

                        int[] tmp = new int[] { basketId, pizzaId};

                        data.Add(tmp);
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

        public void InsertValues(int basketId, int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    command.CommandText = "insert into Users values(@basketId, @pizzaId)";

                    var basketIdParameter = new SqlParameter();
                    basketIdParameter.ParameterName = "@basketId";
                    basketIdParameter.SqlDbType = System.Data.SqlDbType.Int;
                    basketIdParameter.SqlValue = basketId;

                    var pizzaIdParameter = new SqlParameter();
                    pizzaIdParameter.ParameterName = "@pizzaId";
                    pizzaIdParameter.SqlDbType = System.Data.SqlDbType.Int;
                    pizzaIdParameter.SqlValue = userId;

                    command.Parameters.Add(basketIdParameter);
                    command.Parameters.Add(pizzaIdParameter);

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
