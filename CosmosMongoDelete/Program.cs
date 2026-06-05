using CosmosMongoDelete.Models;
using CosmosMongoDelete.Services;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables()
    .Build();

if (args.Length < 2 || (args[0] != "delete-documents" && args[0] != "count-documents"))
{
    Console.Error.WriteLine("Usage: CosmosMongoDelete <command> <DocType>");
    Console.Error.WriteLine("  Commands:");
    Console.Error.WriteLine("    delete-documents <DocType>  Delete all documents of the given DocType");
    Console.Error.WriteLine("    count-documents <DocType>   Count all documents of the given DocType");
    Console.Error.WriteLine("  Example: CosmosMongoDelete count-documents MetadataDefinition");
    return 1;
}

var docType = args[1];
var command = args[0];

var connectionString = config["CosmosDb:ConnectionString"]
    ?? throw new InvalidOperationException("CosmosDb:ConnectionString is not configured.");
var databaseName = config["CosmosDb:DatabaseName"]
    ?? throw new InvalidOperationException("CosmosDb:DatabaseName is not configured.");
var collectionName = config["CosmosDb:CollectionName"]
    ?? throw new InvalidOperationException("CosmosDb:CollectionName is not configured.");

var mongoClient = new MongoClient(connectionString);
var database = mongoClient.GetDatabase(databaseName);
var collection = database.GetCollection<CosmosDocument>(collectionName);

var service = new BulkDeleteService(collection);

using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    Console.WriteLine("\nCancellation requested, finishing current batch...");
    cts.Cancel();
};

try
{
    if (command == "count-documents")
    {
        await service.CountByDocTypeAsync(docType, cts.Token);
    }
    else
    {
        await service.DeleteByDocTypeAsync(docType, cts.Token);
    }
    return 0;
}
catch (OperationCanceledException)
{
    Console.WriteLine("Operation was cancelled.");
    return 2;
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Error: {ex.Message}");
    return 1;
}
