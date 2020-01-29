using System.Collections.Generic;
using System.Threading.Tasks;
using ContactsCoreMVC.Models.Entities;

namespace ContactsCoreMVC.Models.Abstract
{
  public interface IContactRepository
  {
    Task<List<ContactTable>> GetAllContactsAsync();
    Task<List<ContactTable>> FindContactByRowKeyAsync(string rowKey);
    Task<List<ContactTable>> FindContactsByPartitionKeyAsync(string partitionKey);
    Task<ContactTable> FindContactAsync(string partitionKey, string rowKey);
    Task<ContactTable> CreateAsync(ContactTable contactTable);
    Task<ContactTable> UpdateAsync(ContactTable contactTable);
    Task DeleteAsync(string partitionKey, string rowKey);
  }
}