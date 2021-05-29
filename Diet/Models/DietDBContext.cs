using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Diet.Models
{
    public partial class DietDBContext : DbContext
    {
        public DietDBContext()
        {
        }

        public DietDBContext(DbContextOptions<DietDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ActivityLevel> ActivityLevels { get; set; }
        public virtual DbSet<DiagnosesDish> DiagnosesDishes { get; set; }
        public virtual DbSet<Diagnosis> Diagnoses { get; set; }
        public virtual DbSet<Dish> Dishes { get; set; }
        public virtual DbSet<DishesProduct> DishesProducts { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Indicator> Indicators { get; set; }
        public virtual DbSet<Meal> Meals { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<PatientCard> PatientCards { get; set; }
        public virtual DbSet<PatientDiagnosis> PatientDiagnoses { get; set; }
        public virtual DbSet<PatientIndicator> PatientIndicators { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;DataBase=DietDB;Username=postgres;Password=12345");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           

            modelBuilder.Entity<ActivityLevel>(entity =>
            {
                entity.HasKey(e => e.IdActivityLevels)
                    .HasName("activity_levels_pkey");

                entity.ToTable("activity_levels");

                entity.Property(e => e.IdActivityLevels).HasColumnName("id_activity_levels");

                entity.Property(e => e.ActivityLevels)
                    .IsRequired()
                    .HasColumnName("activity_levels");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description");

                entity.Property(e => e.Value).HasColumnName("value");
            });

            modelBuilder.Entity<DiagnosesDish>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("diagnoses_dishes");

                entity.Property(e => e.Allowed)
                    .HasColumnName("allowed")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.IdDiagnosis).HasColumnName("id_diagnosis");

                entity.Property(e => e.IdProduct).HasColumnName("id_product");

                entity.HasOne(d => d.IdDiagnosisNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdDiagnosis)
                    .HasConstraintName("diagnoses_dishes_id_diagnosis_fkey");

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdProduct)
                    .HasConstraintName("diagnoses_dishes_id_product_fkey");
            });

            modelBuilder.Entity<Diagnosis>(entity =>
            {
                entity.HasKey(e => e.IdDiagnosis)
                    .HasName("diagnoses_pkey");

                entity.ToTable("diagnoses");

                entity.HasIndex(e => e.NameDiagnosis, "diagnoses_name_diagnosis_key")
                    .IsUnique();

                entity.Property(e => e.IdDiagnosis).HasColumnName("id_diagnosis");

                entity.Property(e => e.NameDiagnosis)
                    .IsRequired()
                    .HasColumnName("name_diagnosis");
            });

            modelBuilder.Entity<Dish>(entity =>
            {
                entity.HasKey(e => e.IdDish)
                    .HasName("dishes_pkey");

                entity.ToTable("dishes");

                entity.HasIndex(e => e.Dish1, "dishes_dish_key")
                    .IsUnique();

                entity.Property(e => e.IdDish).HasColumnName("id_dish");

                entity.Property(e => e.Calories).HasColumnName("calories");

                entity.Property(e => e.Carbohydrates).HasColumnName("carbohydrates");

                entity.Property(e => e.Dish1)
                    .IsRequired()
                    .HasColumnName("dish");

                entity.Property(e => e.Fats).HasColumnName("fats");

                entity.Property(e => e.Protein).HasColumnName("protein");
            });

            modelBuilder.Entity<DishesProduct>(entity =>
            {
                entity.HasKey(d => d.id)
                   .HasName("id");

                entity.ToTable("dishes_product");

                entity.Property(e => e.IdDish).HasColumnName("id_dish");

                entity.Property(e => e.IdProduct).HasColumnName("id_product");

                entity.HasOne(d => d.IdDishNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdDish)
                    .HasConstraintName("dishes_product_id_dish_fkey");

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdProduct)
                    .HasConstraintName("dishes_product_id_product_fkey");
            });



            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.IdEmployee)
                    .HasName("employee_pkey");

                entity.ToTable("employee");

                entity.HasIndex(e => e.Login, "employee_login_key")
                    .IsUnique();

                entity.Property(e => e.IdEmployee).HasColumnName("id_employee");

                entity.Property(e => e.IdPosition).HasColumnName("id_position");

                entity.Property(e => e.Lastname).HasColumnName("lastname");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasColumnName("login");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password");

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasColumnName("surname");

                entity.HasOne(d => d.IdPositionNavigation)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.IdPosition)
                    .HasConstraintName("employee_id_position_fkey");
            });

            modelBuilder.Entity<Indicator>(entity =>
            {
                entity.HasKey(e => e.IdIndicator)
                    .HasName("indicators_pkey");

                entity.ToTable("indicators");

                entity.HasIndex(e => e.NameIndicator, "indicators_name_indicator_key")
                    .IsUnique();

                entity.Property(e => e.IdIndicator).HasColumnName("id_indicator");

                entity.Property(e => e.Laboratory)
                    .HasColumnName("laboratory")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.Max).HasColumnName("max");

                entity.Property(e => e.Min).HasColumnName("min");

                entity.Property(e => e.NameIndicator)
                    .IsRequired()
                    .HasColumnName("name_indicator");
            });

            modelBuilder.Entity<Meal>(entity =>
            {
                entity.HasKey(e => e.IdMeals)
                    .HasName("meals_pkey");

                entity.ToTable("meals");

                entity.Property(e => e.IdMeals).HasColumnName("id_meals");

                entity.Property(e => e.Calories).HasColumnName("calories");

                entity.Property(e => e.Carbohydrates).HasColumnName("carbohydrates");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Dish)
                    .IsRequired()
                    .HasColumnName("dish");

                entity.Property(e => e.Fats).HasColumnName("fats");

                entity.Property(e => e.IdCard).HasColumnName("id_card");

                entity.Property(e => e.Protein).HasColumnName("protein");

                entity.HasOne(d => d.IdCardNavigation)
                    .WithMany(p => p.Meals)
                    .HasForeignKey(d => d.IdCard)
                    .HasConstraintName("meals_id_card_fkey");
            });
            var g = modelBuilder.HasPostgresEnum(null, "gender", new[] { "Мужской", "ok", "Женский" })
.HasAnnotation("Relational:Collation", "Russian_Russia.1251");


            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.IdPatient)
                    .HasName("patient_pkey");

                entity.ToTable("patient");

                entity.HasIndex(e => e.Login, "patient_login_key")
                    .IsUnique();

                entity.Property(e => e.IdPatient).HasColumnName("id_patient");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnType("date")
                    .HasColumnName("date_of_birth");

                entity.Property(e => e.Lastname).HasColumnName("lastname");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasColumnName("login");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.Property(e => e.Password).HasColumnName("password");

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasColumnName("surname");

                entity.Property(e => e.Woman)
                   .HasColumnName("gender_woman")
                   .HasDefaultValueSql("false");
            });

            modelBuilder.Entity<PatientCard>(entity =>
            {
                entity.HasKey(e => e.IdCard)
                    .HasName("patient_cards_pkey");

                entity.ToTable("patient_cards");

                entity.Property(e => e.IdCard).HasColumnName("id_card");

                entity.Property(e => e.DailyCalories).HasColumnName("daily_calories");

                entity.Property(e => e.DailyCarbohydrates).HasColumnName("daily_carbohydrates");

                entity.Property(e => e.DailyFats).HasColumnName("daily_fats");

                entity.Property(e => e.DailyProtein).HasColumnName("daily_protein");

                entity.Property(e => e.FinishDiet)
                    .HasColumnType("date")
                    .HasColumnName("finish_diet");

                entity.Property(e => e.IdActivityLevels).HasColumnName("id_activity_levels");

                entity.Property(e => e.IdEmployee).HasColumnName("id_employee");

                entity.Property(e => e.IdPatient).HasColumnName("id_patient");

                entity.Property(e => e.StartDiet)
                    .HasColumnType("date")
                    .HasColumnName("start_diet");

                entity.HasOne(d => d.IdActivityLevelsNavigation)
                    .WithMany(p => p.PatientCards)
                    .HasForeignKey(d => d.IdActivityLevels)
                    .HasConstraintName("patient_cards_id_activity_levels_fkey");

                entity.HasOne(d => d.IdEmployeeNavigation)
                    .WithMany(p => p.PatientCards)
                    .HasForeignKey(d => d.IdEmployee)
                    .HasConstraintName("patient_cards_id_employee_fkey");

                entity.HasOne(d => d.IdPatientNavigation)
                    .WithMany(p => p.PatientCards)
                    .HasForeignKey(d => d.IdPatient)
                    .HasConstraintName("patient_cards_id_patient_fkey");
            });

            modelBuilder.Entity<PatientDiagnosis>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("patient_diagnoses");

                entity.Property(e => e.IdCard).HasColumnName("id_card");

                entity.Property(e => e.IdDiagnosis).HasColumnName("id_diagnosis");

                entity.HasOne(d => d.IdCardNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdCard)
                    .HasConstraintName("patient_diagnoses_id_card_fkey");

                entity.HasOne(d => d.IdDiagnosisNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdDiagnosis)
                    .HasConstraintName("patient_diagnoses_id_diagnosis_fkey");
            });

            modelBuilder.Entity<PatientIndicator>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("patient_indicators");

                entity.Property(e => e.DateIndicator)
                    .HasColumnType("date")
                    .HasColumnName("date_indicator");

                entity.Property(e => e.IdCard).HasColumnName("id_card");

                entity.Property(e => e.IdIndicator).HasColumnName("id_indicator");

                entity.Property(e => e.Result)
                    .HasColumnName("result")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.ValueIndicator)
                    .IsRequired()
                    .HasColumnName("value_indicator");

                entity.HasOne(d => d.IdCardNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdCard)
                    .HasConstraintName("patient_indicators_id_card_fkey");

                entity.HasOne(d => d.IdIndicatorNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdIndicator)
                    .HasConstraintName("patient_indicators_id_indicator_fkey");
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasKey(e => e.IdPosition)
                    .HasName("positions_pkey");

                entity.ToTable("positions");

                entity.Property(e => e.IdPosition).HasColumnName("id_position");

                entity.Property(e => e.Position1)
                    .IsRequired()
                    .HasColumnName("position");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.IdProduct)
                    .HasName("products_pkey");

                entity.ToTable("products");

                entity.HasIndex(e => e.Product1, "products_product_key")
                    .IsUnique();

                entity.Property(e => e.IdProduct).HasColumnName("id_product");

                entity.Property(e => e.Product1)
                    .IsRequired()
                    .HasColumnName("product");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
