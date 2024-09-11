using ADO.NET.Models;
using ADO.NET.Repository;
using ADO.NET.Repository.Impl;
using Npgsql;

public class Program
{
    public static void Main(string[] args)
    {
        /*
         * connection = digunakan untuk mengkoneksikan ke database
         * Command = untuk melakukan eksekusi operasi query
         * executeNonQuery = digunakan untuk melakukan eksekusi terhadap query DDL ataupun DML
         * executeReader = digunakan untuk eksekusi query DQL seperti select
         */
        //build connection database using string builder
        // NpgsqlConnectionStringBuilder stringBuilder = new NpgsqlConnectionStringBuilder();
        // stringBuilder.Host = "localhost";
        // stringBuilder.Port = 5432;
        // stringBuilder.Username = "postgres";
        // stringBuilder.Password = "1";
        // stringBuilder.Database = "tokonyadia";
        // var connect = stringBuilder.ToString();
        
        // bisa juga membuat connection seperti ini
        var connectionSting = "Host=localhost;Username=postgres;Password=1;Database=tokonyadia;Port=5432;";
        NpgsqlConnection connect = new NpgsqlConnection(connectionSting);  

        ICustomerRepository customerRepository = new CustomerRepositoryImpl(connect);
        // customerRepository.Save(new Customer
        // {
        //     Name = "Kaguya sama",
        //     PhoneNumber = "082123312"
        // });

        // get by id
        // var byId = customerRepository.GetById(2);
        // Console.WriteLine($"ID : {byId.Id}");
        // Console.WriteLine($"Name : {byId.Name}");
        // Console.WriteLine($"Phone : {byId.PhoneNumber}");
        // Console.WriteLine($"is_active : {byId.IsActive}");

        //Get All

        /*
         var customers = customerRepository.GetAll();
        foreach (var customer in customers)
        {
            Console.WriteLine($"ID : {customer.Id}");
            Console.WriteLine($"Name : {customer.Name}");
            Console.WriteLine($"Phone : {customer.PhoneNumber}");
            Console.WriteLine($"is_active : {customer.IsActive}");
            Console.WriteLine();
        }
        */

        // update data
        var customer = customerRepository.GetById(2);
        // customer.Name = "Uraraka";
        // customer.PhoneNumber = "0821984512";
        // customerRepository.Update(customer);
        
        
        // delete data
        
        // customerRepository.Delete(2);
        
        //GetAllData
        var customers = customerRepository.GetAll();
        var id = 1;
        foreach (var cust in customers)
        {
            Console.WriteLine($"NO            : {id++}");
            Console.WriteLine($"Nama Customer : {cust.Name}");
            Console.WriteLine($"Nomor Telepon : {cust.PhoneNumber}");
            Console.WriteLine($"Status        : {cust.IsActive}");
            Console.WriteLine();
        }
    }
    

    private static void GetAllData(NpgsqlConnection connect)
    {
        try
        {
            connect.Open();
            const string sql = "SELECT * FROM m_customer";
            var command = new NpgsqlCommand(sql, connect);
            var dataReader = command.ExecuteReader();
            var i = 0;
            while (dataReader.Read())
            {
                Console.WriteLine(++i);
                Console.WriteLine($"ID : {dataReader.GetValue(0)}");
                Console.WriteLine($"Name : {dataReader.GetValue(1)}");
                Console.WriteLine($"Phone : {dataReader.GetValue(2)}");
                Console.WriteLine($"is_active : {dataReader.GetValue(3)} \n");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            connect.Close();
        }
    }

    private static void GetDataById(NpgsqlConnection connect)
    {
        try
        {
            connect.Open();
            Console.WriteLine("Masukkan Id yang akan di cari : ");
            var id = Convert.ToInt32(Console.ReadLine());
            const string sql = "SELECT * FROM m_customer WHERE id = @id";
            var command = new NpgsqlCommand(sql, connect);

            command.Parameters.AddWithValue("@id", id);

            // EcecuteReader akan mengembalikan tipe data data reader, yang dapat digunakan untuk mengembalikan data
            var dataReader = command.ExecuteReader();
            if (dataReader.Read()) // jika ada data, maka akan melakukan print data dibawah
            {
                Console.WriteLine($"ID : {dataReader.GetInt32(0)}");
                Console.WriteLine($"Name : {dataReader.GetString(1)}");
                Console.WriteLine($"Phone : {dataReader.GetString(2)}");
                Console.WriteLine($"is_active : {dataReader.GetBoolean(3)}");
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
            connect.Close();
        }
    }

    private static void InsertData(NpgsqlConnection connect)
    {
        try
        {
            connect.Open();
            // var uuid = Guid.NewGuid().ToString(); // untuk generete uuid
            var rnd = new Random();
            var id = rnd.Next();
            // bisa seperti ini
            // string sql = $"INSERT INTO m_customer(id, name, phone_number, is_active) VALUES({id}, 'Sasuke', '08124733', true)";

            // bisa juga menggunakan parameter seperti ini
            Console.Write("Masukkan Nama Anda : ");
            var name = Console.ReadLine();

            Console.WriteLine("Masukkan Nomor telephone : ");
            var phone = Console.ReadLine();
            var sql = "INSERT INTO m_customer(id, name, phone_number, is_active) VALUES(@id, @name, @phone, 'true')";

            NpgsqlCommand command = new NpgsqlCommand(sql, connect);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@phone", phone);
            var execute = command.ExecuteNonQuery();
            Console.WriteLine(execute > 0 ? "Successfully create new Data" : "Error : Can't add new data");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            connect.Close();
        }
    }


    private static void CreateTableCustomer(NpgsqlConnection connect)
    {
        try
        {
            // connect to database first way
            // NpgsqlConnection connect = new NpgsqlConnection(stringBuilder.ConnectionString);
            // connect.Open();
            // Console.WriteLine("Database successfully connected");
            // connect.Close();

            // connect to database second way
            // auto close
            connect.Open();
            Console.WriteLine("Database successfully connected");

            NpgsqlCommand command = new NpgsqlCommand(@"CREATE TABLE m_customer(
                id INT primary key,
                name VARCHAR(100),
                phone_number VARCHAR(15),
                is_actice bit
            )", connect);

            // execute non query -> untuk query DML dan DDL
            command.ExecuteNonQuery();
            Console.WriteLine("Table successfully created");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}