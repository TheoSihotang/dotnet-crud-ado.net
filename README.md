# CRUD CUSTOMER USING ADO.NET & POSTGRESQL

### Setup
  - create new solution
  - install Npgsql using NuGet
  - Create new directory with the names Models and Repository
  - Create new class in directory Models with name Customer, and add the following properties
    - id -> int
    - name -> string
    - phone_number -> string
    - is_active -> bool
  - Create new Interface in directory repository with name ICustomerRepository
  - Create new Class implementation with name CustomerRepositoryImpl, and implement interface
  ``` bash
// : using to implement ICustomerReposity
public class CustomerRepositoryImpl : ICustomerRepository
```
  - initiate string connection like this :
```bash
 NpgsqlConnectionStringBuilder stringBuilder = new NpgsqlConnectionStringBuilder();
        stringBuilder.Host = "localhost";
        stringBuilder.Port = 5432;
        stringBuilder.Username = "your username";
        stringBuilder.Password = "your password";
        stringBuilder.Database = "your db";

        var connect = stringBuilder.ToString();
```

