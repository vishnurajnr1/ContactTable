using System.Threading.Tasks;
using System.Collections.Generic;
using ContactsCoreMVC.Models.Abstract;
using ContactsCoreMVC.Models.Entities;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace ContactsCoreMVC.Models.Concrete
{
  public class TContactRepository : IContactRepository
  {
    private readonly CloudStorageAccount _cloudStorageAccount;
    private readonly CloudTableClient _cloudTableClient;
    private readonly CloudTable _cloudTable;

    public TContactRepository(IOptions<StorageUtility> storageUtility)
    {
      _cloudStorageAccount = storageUtility.Value.StorageAccount;
      _cloudTableClient = _cloudStorageAccount.CreateCloudTableClient();
      _cloudTable = _cloudTableClient.GetTableReference("Contacts");
      _cloudTable.CreateIfNotExistsAsync().GetAwaiter().GetResult();
    }

    public async Task<ContactTable> CreateAsync(ContactTable contactTable)
    {
      return null;
    }

    public async Task<List<ContactTable>> GetAllContactsAsync()
    {
      return null;
    }

    public async Task<ContactTable> FindContactAsync(string partitionKey, string rowKey)
    {
      return null;
    }

    public async Task<List<ContactTable>> FindContactByRowKeyAsync(string rowKey)
    {
      return null;
    }

    public async Task<List<ContactTable>> FindContactsByPartitionKeyAsync(string partitionKey)
    {
      return null;
    }

    public async Task<ContactTable> UpdateAsync(ContactTable contactTable)
    {
      return null;
    }

    public async Task DeleteAsync(string partitionKey, string rowKey)
    {
      return null;
    }
  }
}