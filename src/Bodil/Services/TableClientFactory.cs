using Azure.Data.Tables;

namespace Bodil.Services
{
    public class TableClientFactory
    {
        private readonly TableServiceClient _tableServiceClient;
        public TableClientFactory(IConfiguration configuration)
        {
            var tableConfig = configuration.GetSection("TableStorage");
            var storageUri = tableConfig.GetValue<string>("Uri");
            var accountName = tableConfig.GetValue<string>("AccountName");
            var storageAccountKey = tableConfig.GetValue<string>("StorageAccountKey");
            _tableServiceClient = new TableServiceClient(new Uri(storageUri), new TableSharedKeyCredential(accountName, storageAccountKey));
        }

        public TableClient GetTableClient(string tableName) => _tableServiceClient.GetTableClient(tableName);

    }
}
