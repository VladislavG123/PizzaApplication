using PizzaApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PizzaApplication.DataAccess
{
    public class PizzasTableService
    {
        private readonly string _connectionString = "";
        private SqlTransaction transaction;

        public PizzasTableService()
        {
            _connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;
                                AttachDbFilename=PIZZAAPPLICATION.DATAACCESS\PIZZAAPPLICATION.MDF;
                                Integrated Security=True";
        }

        public List<Pizza> SelectPizzas()
        {
            var data = new List<Pizza>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = "select * from Pizzas";

                    var sqlDataReader = command.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        int id = (int)sqlDataReader["Id"];
                        string name = sqlDataReader["Name"].ToString();
                        string description = sqlDataReader["Description"].ToString();
                        int cost = (int)sqlDataReader["Cost"];
                        int size = (int)sqlDataReader["Size"];

                        data.Add(new Pizza()
                        {
                            Id = id,
                            Name = name,
                            Description = description,
                            Cost = cost,
                            Size = size
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