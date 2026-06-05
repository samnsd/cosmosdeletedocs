using CosmosMongoDelete.Models;
using MongoDB.Driver;

namespace CosmosMongoDelete.Services;

public class BulkDeleteService
{
    private readonly IMongoCollection<CosmosDocument> _collection;
    private const int BatchSize = 500;

    public BulkDeleteService(IMongoCollection<CosmosDocument> collection)
    {
        _collection = collection;
    }

    public async Task DeleteByDocTypeAsync(string docType, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CosmosDocument>.Filter.Eq(d => d.DocType, docType);
        

        long totalDeleted = 0;
        int batchNumber = 0;

        Console.WriteLine($"Starting bulk delete for DocType='{docType}'...");

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var ids = await _collection
                .Find(filter)
                .Project(Builders<CosmosDocument>.Projection.Include(d => d.Id))
                .Limit(BatchSize)
                .As<CosmosDocument>()
                .ToListAsync(cancellationToken);

            Console.WriteLine(ids.Count.ToString());

            if (ids.Count == 0)
                break;

            batchNumber++;
            //var idFilter = Builders<CosmosDocument>.Filter.In(d => d.Id, ids.Select(d => d.Id));
            //var result = await _collection.DeleteManyAsync(idFilter, cancellationToken);

            //totalDeleted += result.DeletedCount;
            //Console.WriteLine($"  Batch {batchNumber}: deleted {result.DeletedCount} documents (total: {totalDeleted})");
        }

        Console.WriteLine($"Done. Total deleted: {totalDeleted} documents with DocType='{docType}'.");
    }
}
