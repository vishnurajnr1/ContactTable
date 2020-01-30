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
      TableOperation insertOperation = TableOperation.Insert(contactTable);
      TableResult tableResult = await _cloudTable.ExecuteAsync(insertOperation);
      ContactTable insertedContact = tableResult.Result as ContactTable;
      return insertedContact;
    }

    public async Task<List<ContactTable>> GetAllContactsAsync()
    {
      TableQuery<ContactTable> query = new TableQuery<ContactTable>();
      TableContinuationToken tcToken = null;
     var result = await  _cloudTable.ExecuteQuerySegmentedAsync(query, tcToken);
     return result.Results;

    }

    public async Task<ContactTable> FindContactAsync(string partitionKey, string rowKey)
    {
      
      TableOperation retrieveOperation  = TableOperation.Retrieve<ContactTable>
                                              (partitionKey,rowKey);
      TableResult tableResult = await _cloudTable.ExecuteAsync(retrieveOperation);
      var result = tableResult.Result as ContactTable;
      return result;

    }

    public async Task<List<ContactTable>> FindContactByRowKeyAsync(string rowKey)
    {
      TableQuery<ContactTable> query = new TableQuery<ContactTable>()
      .Where(TableQuery.GenerateFilterCondition("RowKey",QueryComparisons.Equal, rowKey));

      TableContinuationToken tcToken = null;
      var contactTableResult = await _cloudTable.ExecuteQuerySegmentedAsync(query, tcToken);
      return contactTableResult.Results;

    }

    public async Task<List<ContactTable>> FindContactsByPartitionKeyAsync(string partitionKey)
    {
      TableQuery<ContactTable> query = new TableQuery<ContactTable>()
      .Where(TableQuery.GenerateFilterCondition("PartitionKey",QueryComparisons.Equal, partitionKey));

      TableContinuationToken tcToken = null;
      var contactTableResult = await _cloudTable.ExecuteQuerySegmentedAsync(query, tcToken);
      return contactTableResult.Results;
    }

    public async Task<ContactTable> UpdateAsync(ContactTable contactTable)
    {
      TableOperation retrieveOperation = TableOperation.Retrieve<ContactTable>
      (contactTable.PartitionKey, contactTable.RowKey);
      TableResult tableResult = await _cloudTable.ExecuteAsync(retrieveOperation);
      var contactToUpdate = tableResult.Result as ContactTable;
      if(contactToUpdate != null){
          contactToUpdate.ContactType = contactTable.ContactType;
          contactToUpdate.Email = contactTable.Email;
          TableOperation updateContact = TableOperation.Replace(contactTable);
          var updateResult = await _cloudTable.ExecuteAsync(updateContact);
          return updateResult.Result as ContactTable;
      }
      
      return null;
    }

    public async Task DeleteAsync(string partitionKey, string rowKey)
    {
      TableOperation retrieveOperation = TableOperation.Retrieve<ContactTable>
      (rowKey,partitionKey);
      TableResult tResult = await _cloudTable.ExecuteAsync(retrieveOperation);
      var contactToDelete = tResult.Result as ContactTable;
      if(contactToDelete != null){
        TableOperation deleteContact = TableOperation.Delete(contactToDelete);
          var delteResult = await _cloudTable.ExecuteAsync(deleteContact);
         // return delteResult.Result as ContactTable;
         //Console.WriteLine($"result of delete: {result.Result}");
      }
    }
    
  }
}