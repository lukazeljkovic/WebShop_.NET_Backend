using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NajlONline.Models
{
    public class DataBaseContext : DbContext
    {
        private readonly IConfiguration configuration;

        public DataBaseContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            this.configuration = configuration;
        }

        public DataBaseContext()
        {

        }

        public DbSet<AdresaModel> Adrese { get; set; }
        public DbSet<KarticaModel> Kartice { get; set; }
        public DbSet<KorisnikModel> Korisnici { get; set; }
        public DbSet<KategorijaModel> Kategorije { get; set; }
        public DbSet<KupovinaModel> Kupovine { get; set; }
        public DbSet<ProizvodModel> Proizvodi { get; set; }
        public DbSet<SezonaModel> Sezone { get; set; }
        public DbSet<VrstaProizvodaModel> VrsteProizvoda { get; set; }
        public DbSet<UlogaModel> Uloge { get; set; }

        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("NajlONlineDB"));
           // optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KorisnikModel>()
         .HasIndex(u => u.KorisnickoIme)
         .IsUnique();
        }
    }
}
