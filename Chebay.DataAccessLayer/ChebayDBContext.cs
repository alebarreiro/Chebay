﻿using System;
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
        static string con = @"Data Source=SLAVE-PC\SQLEXPRESS;Database=chebay;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
        //static string con = @"Server=tcp:tb5xxtzdlj.database.windows.net,1433;Database=chebay;User ID=master@tb5xxtzdlj;Password=#!Chebay1;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
        static DbConnection connection = new SqlConnection(con);

        public ChebayDBContext()//(string connection): base(connection)
        {
            Database.SetInitializer<ChebayDBContext>(null);
            this.Configuration.LazyLoadingEnabled = false;
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
        public DbSet<TipoAtributo> tipoatributos { get; set; }
        public DbSet<ImagenProducto> imagenesproducto { get; set; }
        public DbSet<ImagenUsuario> imagenesusuario { get; set; }

        private ChebayDBContext(DbCompiledModel model)
            : base(con, model)
        {
            Database.SetInitializer<ChebayDBContext>(null);
            this.Configuration.LazyLoadingEnabled = false;
        }

        public static ConcurrentDictionary<string, DbCompiledModel> modelCache = new ConcurrentDictionary<string, DbCompiledModel>();

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
            builder.Entity<TipoAtributo>().ToTable("TiposAtributo", schemaName);
            builder.Entity<ImagenProducto>().ToTable("ImagenesProducto", schemaName);
            builder.Entity<ImagenUsuario>().ToTable("ImagenUsuario", schemaName);

            //relaciones
            builder.Entity<Usuario>().HasMany<Producto>(s => s.visitas)
                .WithMany(s => s.visitas)
                .Map(ps =>
                {
                    ps.MapLeftKey("UsuarioID");
                    ps.MapRightKey("ProductoID");
                    ps.ToTable("Visitas", schemaName);
                });

            builder.Entity<Usuario>().HasMany<Producto>(s => s.favoritos)
                .WithMany(s => s.favoritos)
                .Map(ps =>
                {
                    ps.MapLeftKey("UsuarioID");
                    ps.MapRightKey("ProductoID");
                    ps.ToTable("Favoritos", schemaName);
                });

            builder.Entity<Producto>().HasMany<Oferta>(p => p.ofertas);
            builder.Entity<Producto>().HasMany<Comentario>(p => p.comentarios);
            //builder.Entity<Producto>().HasMany<Compra>(p=> p.compras);
            builder.Entity<Producto>().HasMany<Atributo>(s => s.atributos)
                .WithMany(s => s.productos)
                .Map(ps =>
                {
                    ps.MapLeftKey("ProductoID");
                    ps.MapRightKey("AtributoID");
                    ps.ToTable("ProductoAtributo", schemaName);
                });

            builder.Entity<CategoriaSimple>().HasMany<Producto>(s => s.productos);

            builder.Entity<Usuario>().HasMany<Oferta>(p => p.ofertas);
            builder.Entity<Usuario>().HasMany<Comentario>(p => p.comentarios);
            builder.Entity<Usuario>().HasMany<Compra>(p => p.compras);
            builder.Entity<Usuario>().HasMany<Producto>(p => p.publicados);
            builder.Entity<Usuario>().HasMany<Calificacion>(c => c.calificaciones);
            builder.Entity<Usuario>().HasMany<Calificacion>(c => c.calificacionesrecibidas);
            //builder.Entity<Categoria>().HasMany<Atributo>(s => s.atributos);

            builder.Entity<Categoria>().HasMany<TipoAtributo>(s => s.tipoatributos)
                .WithMany(s => s.categorias)
                .Map(ps =>
                {
                    ps.MapLeftKey("CategoriaID");
                    ps.MapRightKey("TipoAtributoID");
                    ps.ToTable("CategoriaTipoAtributo", schemaName);
                });



            builder.Entity<TipoAtributo>().HasMany<Atributo>(p => p.atributos);


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
            Usuario[] users = { new Usuario{ UsuarioID="Dexter", fecha_ingreso= DateTime.Now},
                                new Usuario{ UsuarioID="Newton", fecha_ingreso= DateTime.Now },
                                new Usuario{ UsuarioID="Cantor" , fecha_ingreso= DateTime.Now},
                                new Usuario{ UsuarioID="Arquimedes", fecha_ingreso= DateTime.Now},
                                new Usuario{ UsuarioID="Gauss", fecha_ingreso= DateTime.Now},
                                new Usuario{ UsuarioID="Euler", fecha_ingreso= DateTime.Now}
                              };
            foreach (var u in users)
            {
                usuarios.Add(u);
            }
            SaveChanges();

            Categoria[] cats = {new CategoriaCompuesta {Nombre = "Raiz", padre=null},
                                new CategoriaSimple { Nombre = "Samsung" },
                                new CategoriaSimple { Nombre = "Apple"},
                                new CategoriaSimple { Nombre = "LG"},
                                new CategoriaSimple { Nombre = "Sony"}
                               };
            foreach (var c in cats)
            {
                categorias.Add(c);
            }
            SaveChanges();

            TipoAtributo[] tatrs = { 
                                     new TipoAtributo{TipoAtributoID="CamaraRes", tipodato=TipoDato.INTEGER},
                                     new TipoAtributo{TipoAtributoID="Pantalla", tipodato=TipoDato.INTEGER},
                                     new TipoAtributo{TipoAtributoID="FechaRelease", tipodato=TipoDato.STRING},
                                     new TipoAtributo{TipoAtributoID="Storage", tipodato=TipoDato.INTEGER}
                                   };

            foreach (var a in tatrs)
            {
                a.categorias = new List<Categoria>();
                a.categorias.Add(categorias.Find(2)); //agrego a categoria samsung
                tipoatributos.Add(a);
            }

            SaveChanges();

            //Samsung sample
            Atributo[] atrs = { 
                                new Atributo{ etiqueta="CamaraRes", valor="X82", TipoAtributoID="CamaraRes" },
                                new Atributo{ etiqueta="Screen Size", valor="5", TipoAtributoID="Pantalla" },
                                new Atributo{ etiqueta="InternaMemory", valor="16", TipoAtributoID="Storage" },
                                new Atributo{ etiqueta= "Camera resolution", valor="13", TipoAtributoID="CamaraRes" },
                                new Atributo{ etiqueta="InternaMemory", valor="64", TipoAtributoID="Storage" }                           
                              };

            foreach (var a in atrs)
            {
                atributos.Add(a);
            }
            SaveChanges();

            Producto[] products = { 
                                    new Producto{ UsuarioID="Dexter", CategoriaID=2, nombre="Samsung S6", descripcion="bestia", fecha_cierre= DateTime.Now },
                                    new Producto{ UsuarioID="Newton", CategoriaID=2, nombre="Samsung S5", descripcion="bestia", fecha_cierre= DateTime.Now }
                                  };
            foreach (var p in products)
            {
                p.atributos = new List<Atributo>();
                var atrib = from a in atributos
                            where a.AtributoID == 1
                            select a;
                p.atributos.Add(atrib.First());
                productos.Add(p);
            }

            SaveChanges();

            System.Console.WriteLine("Lista de Usuarios:");
            foreach (var u in usuarios)
            {
                System.Console.WriteLine(u.UsuarioID);
            }

            System.Console.WriteLine("Lista de Productos");
            foreach (var p in productos)
            {
                System.Console.WriteLine(p.ProductoID + " "  + " " + p.nombre + "" + p.descripcion + " " + p.UsuarioID );
            }

            System.Console.WriteLine("Lista de Categorias");
            foreach(var c in categorias){
                System.Console.WriteLine(c.CategoriaID + " " + c.Nombre);
            }

            System.Console.WriteLine("Lista de Atributos");
            foreach (var a in atributos)
            {
                System.Console.WriteLine(a.AtributoID + " " + a.etiqueta + " " + a.valor);
            }
        }
    }


    public class ChebayDBPublic : DbContext
    {
        //static string con = ConfigurationManager.ConnectionStrings["ChebayDBContext"].ToString();
        static string con = @"Data Source=SLAVE-PC\SQLEXPRESS;Database=chebay;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
        //static string con = @"Server=tcp:tb5xxtzdlj.database.windows.net,1433;Database=chebay;User ID=master@tb5xxtzdlj;Password=#!Chebay1;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
        //@"Server=qln8u7yf2c.database.windows.net,1433;Database=chebaytesting;User ID=chebaydb@qln8u7yf2c;Password=#!Chebay;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
        static DbConnection connection = new SqlConnection(con);

        public ChebayDBPublic()
        {
            Database.SetInitializer<ChebayDBPublic>(null);
            base.Database.Connection.ConnectionString = con;
        }

        public DbSet<Administrador> administradores { get; set; }
        public DbSet<Tienda> tiendas { get; set; }
        public DbSet<AtributoSesion> atributossesion { get; set; }
        public DbSet<Personalizacion> personalizaciones { get; set; }

        private ChebayDBPublic(DbCompiledModel model)
            : base(con, model)
        {
            Database.SetInitializer<ChebayDBPublic>(null);
        }


        public static ConcurrentDictionary<string, DbCompiledModel> modelCache = new ConcurrentDictionary<string, DbCompiledModel>();

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
            builder.Entity<AtributoSesion>().ToTable("AtributoSesion", schema);
            builder.Entity<Personalizacion>().ToTable("Personalizaciones", schema);


            builder.Entity<Tienda>().HasMany<Administrador>(s => s.administradores)
                .WithMany(s => s.tiendas)
                .Map(ps =>
                {
                    ps.MapLeftKey("TiendaID");
                    ps.MapRightKey("AdministradorID");
                    ps.ToTable("AdminTienda", schema);
                });

            builder.Entity<Administrador>().HasMany<AtributoSesion>(p => p.atributosSesion);
            
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
            Tienda[] tiendasarray = {   
                                        //new Tienda { TiendaID="LaTienda", nombre= "LaTienda", descripcion= "LaTienda.com", administradores=new List<Administrador>() },
                                        //new Tienda { TiendaID="TestURL", nombre= "Tienda testing", descripcion= "Productos", administradores=new List<Administrador>() },
                                        //new Tienda { TiendaID="uruFutbol", nombre= "Fobal-CAP", descripcion= "Fobal uruguayo", administradores=new List<Administrador>() },
                                        new Tienda { TiendaID="MobileCenter", nombre= "MobileCenter", descripcion= "Productos", administradores=new List<Administrador>() }
                                        //new Tienda { TiendaID="HardShop", nombre= "HardShop", descripcion= "Hardware pc", administradores=new List<Administrador>() }

                                    };
            Administrador[] admins = { 
                                       //new Administrador { AdministradorID= "test@chebay.com", password= "#!Chebay1", tiendas = new List<Tienda>() },
                                       //new Administrador { AdministradorID= "adminTestURL", password= "pass123", tiendas = new List<Tienda>() },
                                       //new Administrador { AdministradorID= "adminuruFutbol", password= "pass123", tiendas = new List<Tienda>() },
                                       new Administrador { AdministradorID= "adminMobileCenter", password= "pass123", tiendas = new List<Tienda>() }                               
                                       //new Administrador { AdministradorID= "adminHardShop", password= "pass123", tiendas = new List<Tienda>() }                                     
                                     };

            for (int i = 0; i < admins.Count(); i++ )
            {
                admins[i].tiendas.Add(tiendasarray[i]);
            }

            foreach (var a in admins)
            {
                administradores.Add(a);
            }

            foreach (var t in tiendasarray)
            {
                tiendas.Add(t);
            }

            SaveChanges();

            AtributoSesion[] atrs = { 
                                        //new AtributoSesion{AdministradorID="adminTestURL", AtributoSesionID="Algo", Datos="unvalordealgo"},
                                        //new AtributoSesion{AdministradorID="adminTestURL", AtributoSesionID="estadointermedio", Datos="unjsonporejemplo"}
                                    };
            foreach (var a in atrs)
            {
                atributossesion.Add(a);
            }
            SaveChanges();

            Personalizacion[] pers = {  
                                        //new Personalizacion{PersonalizacionID="uruFutbol", datos="Blue"},
                                        //new Personalizacion{PersonalizacionID="TestURL", datos="Black"},
                                        //new Personalizacion{PersonalizacionID="LaTienda", datos="Black"},                 
                                        new Personalizacion{PersonalizacionID="MobileCenter", datos="Black"}   
                                        //new Personalizacion{PersonalizacionID="HardShop", datos="Black"}                             
                                     };
            foreach (var p in pers)
            {
                personalizaciones.Add(p);
            }
            SaveChanges();

        }
    }
}