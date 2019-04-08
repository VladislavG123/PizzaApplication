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

                    command.CommandText = "insert into BankCards values(@number, @cvv, @validity, @cardHolderName, @moneyAmount)";

                    var numberParameter = new SqlParameter();
                    numberParameter.ParameterName = "@number";
                    numberParameter.SqlDbType = System.Data.SqlDbType.Int;
                    numberParameter.SqlValue = bankCard.Number;

                    var cvcParameter = new SqlParameter();
                    cvcParameter.ParameterName = "@cvv";
                    cvcParameter.SqlDbType = System.Data.SqlDbType.Int;
                    cvcParameter.SqlValue = bankCard.CVV;

                    var validityParameter = new SqlParameter();
                    validityParameter.ParameterName = "@validity";
                    validityParameter.SqlDbType = System.Data.SqlDbType.DateTime;
                    validityParameter.SqlValue = bankCard.Validity;

                    var cardHolderNameParameter = new SqlParameter();
                    cardHolderNameParameter.ParameterName = "@validity";
                    cardHolderNameParameter.SqlDbType = System.Data.SqlDbType.NVarChar;
                    cardHolderNameParameter.SqlValue = bankCard.CardHolderName;

                    var moneyAmountParameter = new SqlParameter();
                    moneyAmountParameter.ParameterName = "@moneyAmount";
                    moneyAmountParameter.SqlDbType = System.Data.SqlDbType.Int;
                    moneyAmountParameter.SqlValue = bankCard.Validity;

                    command.Parameters.Add(numberParameter);
                    command.Parameters.Add(cvcParameter);
                    command.Parameters.Add(validityParameter);
                    command.Parameters.Add(cardHolderNameParameter);
                    command.Parameters.Add(moneyAmountParameter);

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

        public List<BankCard> SelectCards()
        {
            var data = new List<BankCard>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = "select * from BankCards";

                    var sqlDataReader = command.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        int id = (int)sqlDataReader["Id"];
                        string number = sqlDataReader["Number"].ToString();
                        string cardHolderName = sqlDataReader["CardHolderName"].ToString();
                        int cvv = (int)sqlDataReader["Cvv"];
                        string validity = sqlDataReader["Validity"].ToString();
                        string[] splittedValidity = validity.Split('-');
                        DateTime dateTime = new DateTime(int.Parse(splittedValidity[0]), int.Parse(splittedValidity[1]), int.Parse(splittedValidity[2]));
                        int moneyAmount = (int)sqlDataReader["MoneyAmount"];

                        data.Add(new BankCard()
                        {
                            Id = id,
                            Number = number,
                            CardHolderName = cardHolderName,
                            CVV = cvv,
                            Validity = dateTime,
                            MoneyAmount = moneyAmount
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
    }
}