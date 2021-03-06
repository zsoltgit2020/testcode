﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetCoreTestProject.Repositories;

namespace NetCoreTest.Migrations
{
    [DbContext(typeof(TestWorkDBContext))]
    partial class TestWorkDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NetCoreTestProject.DataModel.TestWork", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Attributes");

                    b.Property<DateTime>("CrDate")
                        .HasColumnName("cr_date");

                    b.Property<string>("Email");

                    b.Property<string>("Key");

                    b.HasKey("ID");

                    b.ToTable("TestWork");
                });
#pragma warning restore 612, 618
        }
    }
}
