using Redis.OM.Modeling;

namespace IranKhodro;

[Document(StorageType = StorageType.Json, Prefixes = new[] { "Circular" })]
public class Circular
{
    [RedisIdField]
    [Indexed]
    public int Id { get; set; }
    [Indexed]
    public int RowNumber { get; set; }
    [Indexed]
    public string? Title { get; set; }
    [Indexed]
    public string? IkcoGoyaNo { get; set; }
}