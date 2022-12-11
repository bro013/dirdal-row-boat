using Azure.Data.Tables;

namespace Bodil.Services
{
    public class TableClientFactory
    {
        private readonly TableServiceClient _tableServiceClient;
        public TableClientFactory(IConfiguration configuration)
        {
            var tableConfig = configuration.GetSection("TableStorage");

            _tableServiceClient = new TableServiceClient(new Uri(tableConfig["Uri"]),
                new TableSharedKeyCredential(tableConfig["AccountName"], tableConfig["AccountKey"]));
        }

        public TableClient GetTableClient(string tableName) => _tableServiceClient.GetTableClient(tableName);

    }
}
