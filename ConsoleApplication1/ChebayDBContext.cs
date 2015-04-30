using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Shared.Entities;
using System.Data.Common;
using System.Collections.Concurrent;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Configuration;
using Shared;
using System.Diagnostics;

namespace DataAccessLayer
{

    class ChebayDBContext : DbContext
    {
        public ChebayDBContext()//(string connection): base(connection)
        {
            Database.SetInitializer<ChebayDBContext>(null);
            string con = ConfigurationManager.ConnectionStrings["ChebayDBContext"].ToString();
            base.Database.Connection.ConnectionString = con;
        }

        public DbSet<Tienda> tiendas { get; set; }
        public DbSet<Producto> productos { get; set; }
        public DbSet<Categoria> categorias { get; set; }
        public DbSet<Usuario> usuarios { get; set; }
        public DbSet<Compra> compras { get; set; }
        public DbSet<Oferta> ofertas { get; set; }
        public DbSet<Atributo> atributos { get; set; }
        public DbSet<Calificacion> calificaciones { get; set; }
        public DbSet<Comentario> comentarios { get; set; }
        public DbSet<Conversacion> conversaciones { get; set; }
        public DbSet<Mensaje> mensajes { get; set; }
        public DbSet<Visita> visitas { get; set; }
        public DbSet<Favorito> favoritos { get; set; }
        public DbSet<Administrador> administradores { get; set; } //nueva



        private ChebayDBContext(string connection, DbCompiledModel model)
            : base(connection, model)
        {
            Database.SetInitializer<ChebayDBContext>(null);
        }

        //: base(connection, model, contextOwnsConnection: false) { }

        private static ConcurrentDictionary<string, DbCompiledModel> modelCache = new ConcurrentDictionary<string, DbCompiledModel>();

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

        public static ChebayDBContext CreateTenant(string schemaName, DbConnection connection)
        {
            var builder = new DbModelBuilder();
            builder.Entity<Atributo>().ToTable("Atributos", schemaName);
            builder.Entity<Calificacion>().ToTable("Calificaciones", schemaName);
            builder.Entity<Categoria>().ToTable("Categorias", schemaName);
            builder.Entity<Comentario>().ToTable("Comentarios", schemaName);
            builder.Entity<Compra>().ToTable("Compras", schemaName);
            builder.Entity<Conversacion>().ToTable("Conversaciones", schemaName);
            builder.Entity<Mensaje>().ToTable("Mensajes", schemaName);
            builder.Entity<Oferta>().ToTable("Ofertas", schemaName);
            builder.Entity<Producto>().ToTable("Productos", schemaName);
            builder.Entity<Usuario>().ToTable("Usuarios", schemaName);
            builder.Entity<Visita>().ToTable("Visitas", schemaName);
            builder.Entity<Favorito>().ToTable("Favoritos", schemaName);

            var model = builder.Build(connection);
            DbCompiledModel compModel = model.Compile();
            var compiledModel = modelCache.GetOrAdd(schemaName, compModel);
            ChebayDBContext ret = new ChebayDBContext(connection.ConnectionString, compiledModel);
            return ret;
        }

        public static void ProvisionTenant(string tenantSchema, DbConnection connection)
        {
            try
            {

                using (var ctx = CreateTenant(tenantSchema, connection))
                {
                    if (!ctx.Database.Exists())
                    {
                        ctx.Database.Create();
                    }
                    else
                    {
                        var createScript = ((IObjectContextAdapter)ctx).ObjectContext.CreateDatabaseScript();
                        ctx.Database.ExecuteSqlCommand(createScript);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public void seed()
        {
            Usuario[] users = { new Usuario{UsuarioID="Dexter" },
                                new Usuario{UsuarioID="Newton"}
                              };
            foreach (var u in users)
            {
                usuarios.Add(u);
            }

            Categoria[] cats = { new CategoriaSimple { CategoriaID = "Bestias" } };
            foreach (var c in cats)
            {
                categorias.Add(c);
            }

            Producto[] products = { new Producto{ProductoID=1, UsuarioID="Dexter", nombre="Samsung S6", descripcion="bestia", CategoriaID="Bestias", fecha_cierre= DateTime.Now},
                                    new Producto{ProductoID=2, UsuarioID="Newton", nombre="Samsung S5", descripcion="bestia", CategoriaID="Bestias", fecha_cierre= DateTime.Now }
                                  };

            foreach (var p in products)
            {
                productos.Add(p);
            }

            SaveChanges();

        }
    }


    public class ChebayDBPublic : DbContext
    {
        public ChebayDBPublic()
        {
            Database.SetInitializer<ChebayDBPublic>(null);
            string con = ConfigurationManager.ConnectionStrings["ChebayDBContext"].ToString();
            base.Database.Connection.ConnectionString = con;
        }

        public DbSet<Administrador> administradores { get; set; }
        public DbSet<Tienda> tiendas { get; set; }


        private ChebayDBPublic(string connection, DbCompiledModel model)
            : base(connection, model)
        {
            Database.SetInitializer<ChebayDBPublic>(null);
        }


        private static ConcurrentDictionary<string, DbCompiledModel> modelCache = new ConcurrentDictionary<string, DbCompiledModel>();

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }


        public static ChebayDBPublic CreatePublic(DbConnection connection)
        {
            string schema = "global";
            var builder = new DbModelBuilder();
            builder.Entity<Administrador>().ToTable("Administradores", schema);
            builder.Entity<Tienda>().ToTable("Tiendas", schema);

            var model = builder.Build(connection);
            DbCompiledModel compModel = model.Compile();
            var compiledModel = modelCache.GetOrAdd(schema, compModel);
            return new ChebayDBPublic(connection.ConnectionString, compiledModel);
        }


        public static void ProvidePublicSchema(DbConnection connection)
        {
            try
            {
                using (var ctx = CreatePublic(connection))
                {
                    if (!ctx.Database.Exists())
                    {
                        ctx.Database.Create();
                    }
                    else
                    {
                        var createScript = ((IObjectContextAdapter)ctx).ObjectContext.CreateDatabaseScript();
                        ctx.Database.ExecuteSqlCommand(createScript);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Seed()
        {
            Administrador[] admins = { new Administrador {AdministradorID = "Admin1", TiendaID="mytienda1", password= "admin1"},
                                       new Administrador {AdministradorID= "Admin2", TiendaID="mytienda1", password= "admin2"},
                                       new Administrador {AdministradorID = "Admin3", TiendaID="mytienda2", password= "admin3"},
                                       new Administrador {AdministradorID= "Admin4", TiendaID="mytienda2", password= "admin4"}
                                     
                                     };
            Tienda[] tiendasarray = {   new Tienda{ TiendaID="mytienda1", nombre="SuperTienda", descripcion="Dale" },
                                        new Tienda{TiendaID="mytienda2", nombre= "MegaTienda", descripcion= "Ok"}
                                    };

            foreach (var a in admins)
            {
                administradores.Add(a);
            }

            foreach (var t in tiendasarray)
            {
                tiendas.Add(t);
            }

            SaveChanges();
        }
    }
}