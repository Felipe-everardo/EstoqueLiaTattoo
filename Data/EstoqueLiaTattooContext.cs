using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EstoqueLiaTattoo.Models;

namespace EstoqueLiaTattoo.Data
{
    public class EstoqueLiaTattooContext : DbContext
    {
        public EstoqueLiaTattooContext(DbContextOptions<EstoqueLiaTattooContext> options)
            : base(options)
        {
        }

        public DbSet<EstoqueLiaTattoo.Models.Movimentacao> Movimentacao { get; set; } = default!;
        public DbSet<EstoqueLiaTattoo.Models.Material> Material { get; set; } = default!;
        public DbSet<EstoqueLiaTattoo.Models.Categoria> Categoria { get; set; } = default!;
    }
}
