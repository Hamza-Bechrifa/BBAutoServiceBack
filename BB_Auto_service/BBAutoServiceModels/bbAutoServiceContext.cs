using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BB_Auto_service.BBAutoServiceModels
{
    public partial class bbAutoServiceContext : DbContext
    {
        public bbAutoServiceContext()
        {
        }

        public bbAutoServiceContext(DbContextOptions<bbAutoServiceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Article> Article { get; set; }
        public virtual DbSet<BonDeReception> BonDeReception { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<DetailleBr> DetailleBr { get; set; }
        public virtual DbSet<DetailleDevisClient> DetailleDevisClient { get; set; }
        public virtual DbSet<DetailleOr> DetailleOr { get; set; }
        public virtual DbSet<DevisClient> DevisClient { get; set; }
        public virtual DbSet<EncaissementClient> EncaissementClient { get; set; }
        public virtual DbSet<Fournisseur> Fournisseur { get; set; }
        public virtual DbSet<OrdreDeReparation> OrdreDeReparation { get; set; }
        public virtual DbSet<ReglementClient> ReglementClient { get; set; }
        public virtual DbSet<ReglementFournisseur> ReglementFournisseur { get; set; }
        public virtual DbSet<Voiture> Voiture { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=TEC-HAMZAB\\SQLEXPRESS;Database=bbAutoService;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Article>(entity =>
            {
                entity.HasIndex(e => e.Reference)
                    .HasName("referenceUnique")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Designation)
                    .HasColumnName("designation")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Emplacement)
                    .HasColumnName("emplacement")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Fodec).HasColumnName("fodec");

                entity.Property(e => e.Marge).HasColumnName("marge");

                entity.Property(e => e.Marque)
                    .HasColumnName("marque")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PrixHt).HasColumnName("prixHt");

                entity.Property(e => e.PrixTtc).HasColumnName("prixTTc");

                entity.Property(e => e.Reference)
                    .IsRequired()
                    .HasColumnName("reference")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReferenceOrigine)
                    .HasColumnName("referenceOrigine")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StockAlerte).HasColumnName("stockAlerte");

                entity.Property(e => e.StockInitial).HasColumnName("stockInitial");

                entity.Property(e => e.StockReel).HasColumnName("stockReel");

                entity.Property(e => e.Tva).HasColumnName("tva");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BonDeReception>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CommentaireExterne)
                    .HasColumnName("commentaireExterne")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CommentaireInterne)
                    .HasColumnName("commentaireInterne")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateCreation)
                    .HasColumnName("dateCreation")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateDocument)
                    .HasColumnName("dateDocument")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Fournisseur).HasColumnName("fournisseur");

                entity.Property(e => e.TotalHt).HasColumnName("totalHT");

                entity.Property(e => e.TotalTtc).HasColumnName("totalTTC");

                entity.HasOne(d => d.FournisseurNavigation)
                    .WithMany(p => p.BonDeReception)
                    .HasForeignKey(d => d.Fournisseur)
                    .HasConstraintName("FK_BonDeReception_Fournisseur");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Adresse)
                    .HasColumnName("adresse")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Cin)
                    .HasColumnName("cin")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NomPrenom)
                    .HasColumnName("nomPrenom")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Plafond).HasColumnName("plafond");

                entity.Property(e => e.Solde).HasColumnName("solde");

                entity.Property(e => e.Telephone)
                    .HasColumnName("telephone")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DetailleBr>(entity =>
            {
                entity.ToTable("DetailleBR");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Article).HasColumnName("article");

                entity.Property(e => e.BonDeReception).HasColumnName("bonDeReception");

                entity.Property(e => e.PrixHt).HasColumnName("prixHt");

                entity.Property(e => e.Quantite).HasColumnName("quantite");

                entity.Property(e => e.Remise).HasColumnName("remise");

                entity.Property(e => e.TotalTtc).HasColumnName("totalTtc");

                entity.Property(e => e.Tva).HasColumnName("tva");
            });

            modelBuilder.Entity<DetailleDevisClient>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Article).HasColumnName("article");

                entity.Property(e => e.DevisClient).HasColumnName("devisClient");

                entity.Property(e => e.PrixHt).HasColumnName("prixHt");

                entity.Property(e => e.Quantite).HasColumnName("quantite");

                entity.Property(e => e.Remise).HasColumnName("remise");

                entity.Property(e => e.TotalTtc).HasColumnName("totalTtc");

                entity.Property(e => e.Tva).HasColumnName("tva");
            });

            modelBuilder.Entity<DetailleOr>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Article).HasColumnName("article");

                entity.Property(e => e.OrdreDeReparation).HasColumnName("ordreDeReparation");

                entity.Property(e => e.PrixHt).HasColumnName("prixHt");

                entity.Property(e => e.Quantite).HasColumnName("quantite");

                entity.Property(e => e.Remise).HasColumnName("remise");

                entity.Property(e => e.TotalTtc).HasColumnName("totalTtc");

                entity.Property(e => e.Tva).HasColumnName("tva");
            });

            modelBuilder.Entity<DevisClient>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Client).HasColumnName("client");

                entity.Property(e => e.CommentaireExterne)
                    .HasColumnName("commentaireExterne")
                    .IsUnicode(false);

                entity.Property(e => e.CommentaireInterne)
                    .HasColumnName("commentaireInterne")
                    .IsUnicode(false);

                entity.Property(e => e.DateCreation)
                    .HasColumnName("dateCreation")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateDocument)
                    .HasColumnName("dateDocument")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Kilometrage).HasColumnName("kilometrage");

                entity.Property(e => e.TotalHt).HasColumnName("totalHt");

                entity.Property(e => e.TotalTtc).HasColumnName("totalTtc");

                entity.Property(e => e.Voiture).HasColumnName("voiture");
            });

            modelBuilder.Entity<EncaissementClient>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Client).HasColumnName("client");

                entity.Property(e => e.DateCreation)
                    .HasColumnName("dateCreation")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateReglement)
                    .HasColumnName("dateReglement")
                    .HasColumnType("datetime");

                entity.Property(e => e.Montant).HasColumnName("montant");

                entity.Property(e => e.OrdreDeReparation).HasColumnName("ordreDeReparation");
            });

            modelBuilder.Entity<Fournisseur>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Adresse)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MatriculeFiscale)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nom)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Telephone)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OrdreDeReparation>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Client).HasColumnName("client");

                entity.Property(e => e.CommentaireExterne)
                    .HasColumnName("commentaireExterne")
                    .IsUnicode(false);

                entity.Property(e => e.CommentaireInterne)
                    .HasColumnName("commentaireInterne")
                    .IsUnicode(false);

                entity.Property(e => e.DateCreation)
                    .HasColumnName("dateCreation")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateDocument)
                    .HasColumnName("dateDocument")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Kilometrage).HasColumnName("kilometrage");

                entity.Property(e => e.ResteApaye).HasColumnName("resteAPaye");

                entity.Property(e => e.TotalHt).HasColumnName("totalHt");

                entity.Property(e => e.TotalTtc).HasColumnName("totalTtc");

                entity.Property(e => e.Voiture).HasColumnName("voiture");

                entity.HasOne(d => d.ClientNavigation)
                    .WithMany(p => p.OrdreDeReparation)
                    .HasForeignKey(d => d.Client)
                    .HasConstraintName("FK_OrdreDeReparation_Client");
            });

            modelBuilder.Entity<ReglementClient>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Client).HasColumnName("client");

                entity.Property(e => e.DateOperation)
                    .IsRequired()
                    .HasColumnName("dateOperation")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateReglement)
                    .IsRequired()
                    .HasColumnName("dateReglement")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FactureClient).HasColumnName("factureClient");

                entity.Property(e => e.Mode)
                    .HasColumnName("mode")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Montant).HasColumnName("montant");

                entity.Property(e => e.OrdreDeReparation).HasColumnName("ordreDeReparation");
            });

            modelBuilder.Entity<ReglementFournisseur>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BonDeReception).HasColumnName("bonDeReception");

                entity.Property(e => e.DateOperation)
                    .IsRequired()
                    .HasColumnName("dateOperation")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateReglement)
                    .IsRequired()
                    .HasColumnName("dateReglement")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FactureFournisseur).HasColumnName("factureFournisseur");

                entity.Property(e => e.Fournisseur).HasColumnName("fournisseur");

                entity.Property(e => e.Mode)
                    .HasColumnName("mode")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Montant).HasColumnName("montant");
            });

            modelBuilder.Entity<Voiture>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Client).HasColumnName("client");

                entity.Property(e => e.DateMiseEnCirculation)
                    .HasColumnName("dateMiseEnCirculation")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Kilometrage).HasColumnName("kilometrage");

                entity.Property(e => e.Matricule)
                    .HasColumnName("matricule")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Modele)
                    .HasColumnName("modele")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Vin)
                    .HasColumnName("vin")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
