// <auto-generated />
using System;
using BHSW2_2.Pinion.DataService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BHSW2_2.Pinion.DataService.Migrations
{
    [DbContext(typeof(SapConnectorContext))]
    [Migration("20211218034715_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.17")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BHSW2_2.Pinion.DataService.SapRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Error")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FunctionName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("Modified")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("Retries")
                        .HasColumnType("int");

                    b.Property<int>("SapRequestStatus")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("SapRequests");
                });

            modelBuilder.Entity("BHSW2_2.Pinion.DataService.SapRequestHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("FunctionName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SapRequestHistories");
                });
#pragma warning restore 612, 618
        }
    }
}
