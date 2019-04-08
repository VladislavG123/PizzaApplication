using PizzaApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PizzaApplication.DataAccess
{
    public class UsersTableService
    {
        private readonly string _connectionString = "";
        private SqlTransaction transaction;

        public UsersTableService()
        {
            _connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;
                                AttachDbFilename=PIZZAAPPLICATION.DATAACCESS\PIZZAAPPLICATION.MDF;
                                Integrated Security=True";
        }

        public List<User> SelectUsers()
        {
            var data = new List<User>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = "select * from Users";

                    var sqlDataReader = command.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        int id = (int)sqlDataReader["Id"];
                        string password = sqlDataReader["Login"].ToString();
                        string login = sqlDataReader["Password"].ToString();
                        string phoneNumber = sqlDataReader["PhoneNumber"].ToString();
                        string fullName = sqlDataReader["FullName"].ToString();
                        int money = (int)sqlDataReader["Money"];
                        int? cardId = (int)sqlDataReader["CardId"];

                        data.Add(new User()
                        {
                            Id = id,
                            Login = login,
                            Password = password,
                            PhoneNumber = phoneNumber,
                            FullName = fullName,
                            Money = money,
                            BankCardId = cardId
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
        public void InsertUser(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    command.CommandText = "insert into Users values(@login, @password, @phoneNumber, @fullName, @money, @cardId)";

                    var loginParameter = new SqlParameter();
                    loginParameter.ParameterName = "@login";
                    loginParameter.SqlDbType = System.Data.SqlDbType.NVarChar;
                    loginParameter.SqlValue = user.Login;

                    var passwordParameter = new SqlParameter();
                    passwordParameter.ParameterName = "@password";
                    passwordParameter.SqlDbType = System.Data.SqlDbType.NVarChar;
                    passwordParameter.SqlValue = user.Password;

                    var phoneNumberParameter = new SqlParameter();
                    phoneNumberParameter.ParameterName = "@phoneNumber";
                    phoneNumberParameter.SqlDbType = System.Data.SqlDbType.NVarChar;
                    phoneNumberParameter.SqlValue = user.PhoneNumber;

                    var fullNameParameter = new SqlParameter();
                    fullNameParameter.ParameterName = "@fullName";
                    fullNameParameter.SqlDbType = System.Data.SqlDbType.NVarChar;
                    fullNameParameter.SqlValue = user.FullName;

                    var moneyParameter = new SqlParameter();
                    moneyParameter.ParameterName = "@money";
                    moneyParameter.SqlDbType = System.Data.SqlDbType.Int;
                    moneyParameter.SqlValue = user.Money;

                    var cardIdParameter = new SqlParameter();
                    cardIdParameter.ParameterName = "@cardId";
                    cardIdParameter.SqlDbType = System.Data.SqlDbType.Int;
                    cardIdParameter.SqlValue = user.Money;


                    command.Parameters.Add(passwordParameter);
                    command.Parameters.Add(loginParameter);
                    command.Parameters.Add(phoneNumberParameter);
                    command.Parameters.Add(fullNameParameter);
                    command.Parameters.Add(moneyParameter);
                    command.Parameters.Add(cardIdParameter);

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
