﻿using FastFood.Models;
using FastFood.Service;
using FastFood.Service.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq.Expressions;

namespace FastFood.Service
{
    public class DataConnection : IDataConnection
    {
        public async Task<List<Employee>> GetAllEmployee()
        {
            List<Employee> result = new();
            const string sqlExpression = "sp_getAllEmployee";

            using (SqlConnection connection = new(GlobalConfig.ConnectionString))
            {
                try
                {
                    SqlCommand command = new(sqlExpression, connection);
                    command.CommandType = CommandType.StoredProcedure;

                    await connection.OpenAsync();

                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new Employee
                            {
                                Id = reader.GetInt32(0),
                                Pin = reader.GetString(1)
                            });
                        }
                    }
                }
                catch (SqlException)
                {

                    throw;
                }
                finally
                {
                    await connection.CloseAsync();
                }
            }
            return result;
        }

        public async Task<General> GetBalance()
        {
            const string sqlExpression = "sp_getWholeBalance";
            General result = new();

            using (SqlConnection connection = new(GlobalConfig.ConnectionString))
            {
                try
                {
                    SqlCommand command = new(sqlExpression, connection);
                    command.CommandType = CommandType.StoredProcedure;

                    await connection.OpenAsync();
                    SqlDataReader reader =  await command.ExecuteReaderAsync();
                    if(reader.HasRows)
                    {
                        while(await reader.ReadAsync())
                        {
                            result.Balance = reader.GetDouble(0);
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    await connection.CloseAsync();
                }

                return result;
            }
        }

        public async Task<Employee> GetEmployeeByPin(string pin)
        {
            Employee result = new();
            const string sqlExpression = "sp_getEmployeeByPin";

            using (SqlConnection connection = new(GlobalConfig.ConnectionString))
            {
                try
                {
                    SqlCommand command = new(sqlExpression,connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("employeePin", pin);

                    await connection.OpenAsync();
                    SqlDataReader reader =  await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Id = reader.GetInt32(0);
                            result.Pin = reader.GetString(1);
                        }
                    }
                }
                catch (SqlException)
                {

                    throw;
                }
                finally
                {
                    await connection.CloseAsync();
                }
            }
            return result;
        }
    }
}