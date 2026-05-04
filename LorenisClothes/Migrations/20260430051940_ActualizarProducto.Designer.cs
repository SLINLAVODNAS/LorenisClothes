using System;
using LorenisClothes.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LorenisClothes.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260430051940_ActualizarProducto")]
    partial class ActualizarProducto
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "10.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LorenisClothes.Models.Administrador", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<string>("Password")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Usuario")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.ToTable("Administradores");
            });

            modelBuilder.Entity("LorenisClothes.Models.DetallePedido", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<int>("Cantidad")
                    .HasColumnType("int");

                b.Property<int>("PedidoId")
                    .HasColumnType("int");

                b.Property<int>("ProductoId")
                    .HasColumnType("int");

                b.HasKey("Id");

                b.HasIndex("PedidoId");

                b.HasIndex("ProductoId");

                b.ToTable("DetallesPedido");
            });

            modelBuilder.Entity("LorenisClothes.Models.Pedido", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<string>("Direccion")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Estado")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<DateTime>("FechaPedido")
                    .HasColumnType("datetime2");

                b.Property<string>("NombreCliente")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Telefono")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<double>("Total")
                    .HasColumnType("float");

                b.HasKey("Id");

                b.ToTable("Pedidos");
            });

            modelBuilder.Entity("LorenisClothes.Models.Producto", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<string>("Color")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Descripcion")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("ImagenUrl")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Nombre")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<double>("Precio")
                    .HasColumnType("float");

                b.Property<string>("Talla")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.ToTable("Productos");
            });

            modelBuilder.Entity("LorenisClothes.Models.DetallePedido", b =>
            {
                b.HasOne("LorenisClothes.Models.Pedido", "Pedido")
                    .WithMany()
                    .HasForeignKey("PedidoId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("LorenisClothes.Models.Producto", "Producto")
                    .WithMany()
                    .HasForeignKey("ProductoId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Pedido");

                b.Navigation("Producto");
            });
        }
    }
}