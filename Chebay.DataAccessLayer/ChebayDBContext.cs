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
using System.Data.SqlClient;


namespace DataAccessLayer
{

    public class ChebayDBContext : DbContext
    {
        //static string con = ConfigurationManager.ConnectionStrings["ChebayDBContext"].ToString();
        static string con = @"Server=qln8u7yf2c.database.windows.net,1433;Database=chebaytesting;User ID=chebaydb@qln8u7yf2c;Password=#!Chebay;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
        static DbConnection connection = new SqlConnection(con);

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
        
        private ChebayDBContext(DbCompiledModel model)
            : base(con, model)
        {
            Database.SetInitializer<ChebayDBContext>(null);
        }

        private static ConcurrentDictionary<string, DbCompiledModel> modelCache = new ConcurrentDictionary<string, DbCompiledModel>();

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

        public static ChebayDBContext CreateTenant(string schemaName)
        {
            var builder = new DbModelBuilder();
            builder.Entity<Atributo>().ToTable("Atributos", schemaName);
            builder.Entity<Calificacion>().ToTable("Calificaciones", schemaName);
            builder.Entity<Categoria>()
                .Map<CategoriaCompuesta>(m => m.Requires("TipoCategoria").HasValue(0))
                .Map<CategoriaSimple>(m => m.Requires("TipoCategoria").HasValue(1))
                .ToTable("Categorias", schemaName);  
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
            ChebayDBContext ret = new ChebayDBContext(compiledModel);
            return ret;
        }

        public static void ProvisionTenant(string tenantSchema)
        {
            try
            {

                using (var ctx = CreateTenant(tenantSchema))
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
                                new Usuario{UsuarioID="Newton" }
                              };
            foreach (var u in users)
            {
                usuarios.Add(u);
            }

            Categoria[] cats = { new CategoriaSimple { Nombre = "Bestias" } };
            foreach (var c in cats)
            {
                categorias.Add(c);
            }

            Producto[] products = { new Producto{UsuarioID="Dexter", nombre="Samsung S6", descripcion="bestia", CategoriaID=1, fecha_cierre= DateTime.Now},
                                    new Producto{UsuarioID="Newton", nombre="Samsung S5", descripcion="bestia", CategoriaID=1, fecha_cierre= DateTime.Now }
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
        //static string con = ConfigurationManager.ConnectionStrings["ChebayDBContext"].ToString();
        static string con = @"Server=qln8u7yf2c.database.windows.net,1433;Database=chebaytesting;User ID=chebaydb@qln8u7yf2c;Password=#!Chebay;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
        static DbConnection connection = new SqlConnection(con);

        public ChebayDBPublic()
        {
            Database.SetInitializer<ChebayDBPublic>(null);
            base.Database.Connection.ConnectionString = con;
        }

        public DbSet<Administrador> administradores { get; set; }
        public DbSet<Tienda> tiendas { get; set; }


        private ChebayDBPublic(DbCompiledModel model)
            : base(con, model)
        {
            Database.SetInitializer<ChebayDBPublic>(null);
        }


        private static ConcurrentDictionary<string, DbCompiledModel> modelCache = new ConcurrentDictionary<string, DbCompiledModel>();

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }


        public static ChebayDBPublic CreatePublic()
        {
            string schema = "global";
            var builder = new DbModelBuilder();
            builder.Entity<Administrador>().ToTable("Administradores", schema);
            builder.Entity<Tienda>().ToTable("Tiendas", schema);
            var model = builder.Build(connection);
            DbCompiledModel compModel = model.Compile();
            var compiledModel = modelCache.GetOrAdd(schema, compModel);
            return new ChebayDBPublic(compiledModel);
        }


        public static void ProvidePublicSchema()
        {
            try
            {
                using (var ctx = CreatePublic())
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
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        public void Seed()
        {
            Administrador[] admins = { new Administrador { AdministradorID= "Admin1", TiendaID="mytienda1", password= "admin1"},
                                       new Administrador { AdministradorID= "Admin2", TiendaID="mytienda1", password= "admin2"},
                                       new Administrador { AdministradorID= "Admin3", TiendaID="mytienda2", password= "admin3"},
                                       new Administrador { AdministradorID= "Admin4", TiendaID="mytienda2", password= "admin4"}
                                     
                                     };
            Tienda[] tiendasarray = {   new Tienda{ TiendaID="mytienda1", nombre="SuperTienda", descripcion="Dale" },
                                        new Tienda{ TiendaID="mytienda2", nombre= "MegaTienda", descripcion= "Ok"}
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