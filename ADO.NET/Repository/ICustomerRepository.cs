using ADO.NET.Models;

namespace ADO.NET.Repository;

public interface ICustomerRepository
{
    void Save(Customer customer);
    Customer GetById(int id);
    List<Customer> GetAll();
    void Update(Customer customer);
    void Delete(int id);
}