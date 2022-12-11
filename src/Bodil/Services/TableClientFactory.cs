using Azure.Data.Tables;

namespace Bodil.Services
{
    public class TableClientFactory
    {
        private readonly TableServiceClient _tableServiceClient;
        private readonly ILogger<TableClientFactory> _logger;
        public TableClientFactory(IConfiguration configuration, ILogger<TableClientFactory> logger)
        {
            _logger = logger;
            foreach (var conf in configuration.AsEnumerable())
            {
                _logger.LogInformation("{Key} : {Value}", conf.Key, conf.Value);
            }
            var tableConfig = configuration.GetSection("TableStorage");
            var storageUri = tableConfig.GetValue<string>("Uri");
            var accountName = tableConfig.GetValue<string>("AccountName");
            var storageAccountKey = configuration.GetValue<string>("storageAccountKey");
            _tableServiceClient = new TableServiceClient(new Uri(storageUri), new TableSharedKeyCredential(accountName, storageAccountKey));
        }

        public TableClient GetTableClient(string tableName) => _tableServiceClient.GetTableClient(tableName);

    }
}
