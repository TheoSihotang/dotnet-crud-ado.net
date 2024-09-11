using ADO.NET.Models;
using Npgsql;

namespace ADO.NET.Repository.Impl;

public class CustomerRepositoryImpl : ICustomerRepository
{
    private readonly NpgsqlConnection _connection;

    public CustomerRepositoryImpl(NpgsqlConnection connection)
    {
        _connection = connection;
    }


    // public CustomerRepositoryImpl(string? stringConnection)
    // {
    //     _connection = new NpgsqlConnection(stringConnection);
    // }

    public void Save(Customer customer)
    {
        try
        {
            _connection.Open();
            // var uuid = Guid.NewGuid().ToString(); // untuk generete uuid
            var rnd = new Random();
            customer.Id = rnd.Next();
            var sql = "INSERT INTO m_customer(id, name, phone_number, is_active) VALUES(@id, @name, @phone, 'true')";

            NpgsqlCommand command = new NpgsqlCommand(sql, _connection);
            command.Parameters.AddWithValue("@id", customer.Id);
            command.Parameters.AddWithValue("@name", customer.Name);
            command.Parameters.AddWithValue("@phone", customer.PhoneNumber);
            var execute = command.ExecuteNonQuery();
            Console.WriteLine("Success add new Customer");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            _connection.Close();
        }
    }

    public Customer GetById(int id)
    {
        Customer? customer = null;
        try
        {
            _connection.Open();
            const string sql = "SELECT * FROM m_customer WHERE id = @id";
            var command = new NpgsqlCommand(sql, _connection);

            command.Parameters.AddWithValue("@id",id);

            // EcecuteReader akan mengembalikan tipe data data reader, yang dapat digunakan untuk mengembalikan data
            var dataReader = command.ExecuteReader();
            if (dataReader.Read()) // jika ada data, maka akan melakukan print data dibawah
            {
                customer = new Customer
                {
                    Id = Convert.ToInt32(dataReader["id"]),
                    Name = dataReader["name"].ToString(),
                    PhoneNumber = dataReader["phone_number"].ToString(),
                    IsActive = Convert.ToBoolean(dataReader["is_active"])
                };
            }
            else
            {
                Console.WriteLine($"Data with ID {id} Not Found");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            _connection.Close();
        }

        return customer;
    }

    public List<Customer> GetAll()
    {
        var customers = new List<Customer>();
        try
        {
            _connection.Open();
            const string sql = "SELECT * FROM m_customer";
            var command = new NpgsqlCommand(sql, _connection);
            var dataReader = command.ExecuteReader();
            var i = 0;
            while (dataReader.Read())
            {
                customers.Add(new Customer
                {
                    Id = Convert.ToInt32(dataReader["id"]),
                    Name = dataReader["name"].ToString(),
                    PhoneNumber = dataReader["phone_number"].ToString(),
                    IsActive = Convert.ToBoolean(dataReader["is_active"])
                });
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            _connection.Close();
        }

        return customers;
    }

    public void Update(Customer customer)
    {
        try
        {
            _connection.Open();
            var sql = "UPDATE m_customer SET id = @id, name = @name, phone_number = @phone, is_active = @is_active WHERE id = @id";
            var command = new NpgsqlCommand(sql, _connection);
            command.Parameters.AddWithValue("@id", customer.Id);
            command.Parameters.AddWithValue("@name", customer.Name);
            command.Parameters.AddWithValue("@phone", customer.PhoneNumber);
            command.Parameters.AddWithValue("@is_active", customer.IsActive);
            command.ExecuteNonQuery();
            Console.WriteLine($"Successfully Update Customer with ID {customer.Id}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            _connection.Close();
        }
    }

    public void Delete(int id)
    {
        try
        {
            var customer = GetById(id);
            if (customer is null) return;
            _connection.Open();
                var sql = "DELETE FROM m_customer WHERE id = @id";
                var command = new NpgsqlCommand(sql, _connection);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                Console.WriteLine($"Successfully Delete Customer with ID {id}");   
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            _connection.Close();
        }
    }
}