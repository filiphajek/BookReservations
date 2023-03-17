using Microsoft.EntityFrameworkCore;

namespace BookReservations.Infrastructure.DAL.EFcore;

public interface ISeeder
{
    bool CanSeed();
    void Seed(ModelBuilder modelBuilder);
}