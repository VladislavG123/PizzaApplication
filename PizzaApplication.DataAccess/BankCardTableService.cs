using PizzaApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PizzaApplication.DataAccess
{
    public class BankCardTableService
    {
        private readonly string _connectionString = "";
        private SqlTransaction transaction;

        public BankCardTableService()
        {
            _connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;
                                AttachDbFilename=PIZZAAPPLICATION.DATAACCESS\PIZZAAPPLICATION.MDF;
                                Integrated Security=True";
        }
        public void InsertBankCard(BankCard bankCard)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    command.CommandText = "insert into BankCards values(@number, @cvc, @validity)";

                    var numberParameter = new SqlParameter();
                    numberParameter.ParameterName = "@number";
                    numberParameter.SqlDbType = System.Data.SqlDbType.Int;
                    numberParameter.SqlValue = bankCard.Number;

                    var cvcParameter = new SqlParameter();
                    cvcParameter.ParameterName = "@cvc";
                    cvcParameter.SqlDbType = System.Data.SqlDbType.Int;
                    cvcParameter.SqlValue = bankCard.Cvc;

                    var validityParameter = new SqlParameter();
                    validityParameter.ParameterName = "@phoneNumber";
                    validityParameter.SqlDbType = System.Data.SqlDbType.DateTime;
                    validityParameter.SqlValue = bankCard.Validity;

                    command.Parameters.Add(numberParameter);
                    command.Parameters.Add(cvcParameter);
                    command.Parameters.Add(validityParameter);

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