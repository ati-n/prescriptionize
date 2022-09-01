using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace PrescribeAPI.Models
{
    public class PrescriptionContext : DbContext
    {
        public PrescriptionContext(DbContextOptions<PrescriptionContext> options)
            : base(options)
        {
        }

        public DbSet<PrescriptionItem> PrescriptionItems { get; set; } = null!;
    }
}

