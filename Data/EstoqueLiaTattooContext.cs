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

        public DbSet<Movimentacao> Movimentacao { get; set; } = default!;
        public DbSet<Material> Material { get; set; } = default!;
        public DbSet<Categoria> Categoria { get; set; } = default!;
        public DbSet<Tinta> Tinta { get; set; } = default!;
    }
}
