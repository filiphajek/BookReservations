namespace BookReservations.Infrastructure.BL.Services;

public class AzureBlobStorageOptions
{
    public string ContainerName { get; set; } = default!;
    public string ConnectionString { get; set; } = default!;
    public string AccountName { get; set; } = default!;
}