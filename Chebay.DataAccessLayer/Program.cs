using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Shared.Entities;
using System.Data.Entity.Core.EntityClient;
using System.Data.Common;
using System.Diagnostics;
using Shared.DataTypes;

namespace DataAccessLayer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Utilizar en caso de pruebas minimas...");
            //DALMercadoLibreREST ml = new DALMercadoLibreREST();
            //ml.test();
            //Console.Read();
        }
       

            //ChebayDBPublic.ProvidePublicSchema();
            //using (var bd = ChebayDBPublic.CreatePublic())
            //{
            //    bd.Seed();
            //}
            /*
            IDALSubasta isub = new DALSubastaEF();
            IDALUsuario iusr = new DALUsuarioEF();
            IDALTienda itie = new DALTiendaEF();

            var lista = itie.ListarTodosTipoAtributo(7, "HardShop");
            foreach (var ta in lista)
            {
                System.Console.WriteLine(ta.TipoAtributoID);
            }
            */
            /*ChebayDBContext.ProvisionTenant("MobileCenter");
            using (var db = ChebayDBContext.CreateTenant("MobileCenter"))
            {
                db.seed();
                #region algoritmo first
                /*
                // ALGORITMO!!!

                var query = from p in db.productos.Include("categoria")
                            where p.nombre == "Testing"
                            select p;



                Producto prod = query.First();


                var t1 = from c in db.categorias.Include("tipoatributos").Include("padre")
                         where c.CategoriaID == prod.CategoriaID
                         select c;
                Categoria hijo = t1.First();
                System.Console.WriteLine("primer categoria"+ hijo.CategoriaID);
                foreach (var ta in hijo.tipoatributos)
                {
                    System.Console.WriteLine(ta.TipoAtributoID);
                }


                var t2 = from c in db.categorias.Include("tipoatributos")
                         where c.CategoriaID == hijo.padre.CategoriaID
                         select c;
                Categoria padre = t2.First();

                System.Console.WriteLine("segunda categoria"+ padre.CategoriaID);
                foreach (var ta in padre.tipoatributos)
                {
                    System.Console.WriteLine(ta.TipoAtributoID);
                }
                System.Console.WriteLine("fin");

                List<TipoAtributo> ac = new List<TipoAtributo>();

                Categoria itcat = prod.categoria;
                bool finish = false;
                do
                {
                    System.Console.WriteLine(itcat.CategoriaID);
                    var itquery = from c in db.categorias.Include("tipoatributos").Include("padre")
                                  where c.CategoriaID == itcat.CategoriaID
                                  select c;

                    itcat = itquery.First();
                    foreach (var a in itcat.tipoatributos)
                    {
                        ac.Add(a);
                    }
                    if (itcat.CategoriaID != 1)
                        itcat = itcat.padre;
                    else
                        finish = true;
                } while (!finish);

                foreach (var a in ac)
                {
                    System.Console.WriteLine(a.TipoAtributoID);
                }
                
#endregion

                #region test algoritmo obtener todos tipo atributos
                /* TEST para atributos de padre
                var query = from a in db.categorias
                            where a.CategoriaID == 1
                            select a;
                CategoriaCompuesta raiz = (CategoriaCompuesta)query.First();

                CategoriaCompuesta padre = new CategoriaCompuesta { Nombre = "padre", padre = raiz };
                db.categorias.Add(padre);

                //guardo padre
                db.SaveChanges();
                var query1 = from a in db.categorias
                            where a.Nombre=="padre"
                            select a;
                


                CategoriaSimple hijo = new CategoriaSimple { Nombre = "hijo", padre=(CategoriaCompuesta)query1.First() };
                db.categorias.Add(hijo);
                db.SaveChanges(); //guardo hijo

                var query2 = from a in db.categorias
                             where a.Nombre == "hijo"
                             select a;

                TipoAtributo[] tipoatributos = {
                                                   //new TipoAtributo{TipoAtributoID="Tipoatributoabuelo", tipodato=TipoDato.STRING},
                                                   new TipoAtributo{TipoAtributoID="Tipoatributopadre", tipodato=TipoDato.STRING, categorias=new List<Categoria>()},
                                                   new TipoAtributo{TipoAtributoID="Tipoatributohijo", tipodato=TipoDato.STRING, categorias=new List<Categoria>()}
                                                };
                tipoatributos[0].categorias.Add(query1.First());
                tipoatributos[1].categorias.Add(query2.First());
                foreach (var a in tipoatributos)
                {
                    db.tipoatributos.Add(a);
                }

                db.SaveChanges(); 
                
                //agrego taa a raiz
                                TipoAtributo taa = new TipoAtributo { TipoAtributoID = "raizzz", tipodato = TipoDato.BOOL, categorias= new List<Categoria>()};
                var quer = from c in db.categorias
                           where c.CategoriaID == 1
                           select c;
                taa.categorias.Add(quer.First());
                db.tipoatributos.Add(taa);
                db.SaveChanges();

                 
                ///hasta aqui funciono
                Atributo[] atributos = { 
                                            //new Atributo{etiqueta="Atributoabuelo", TipoAtributoID="Tipoatributoabuelo"},
                                            new Atributo{etiqueta="Atributopadre", valor="valorpadre", TipoAtributoID="Tipoatributopadre", },
                                            new Atributo{etiqueta="Atributohijo", valor="valorhijo", TipoAtributoID="Tipoatributohijo"}
                                       };

                foreach (var a in atributos)
                {
                    db.atributos.Add(a);
                }
                db.SaveChanges();
                //
                
                var query3 = from a in db.categorias
                             where a.Nombre == "hijo"
                             select a;


                Producto p = new Producto { fecha_cierre=DateTime.Now, nombre="Testing", UsuarioID="Gauss", categoria=(CategoriaSimple)query3.First()}; //revisar cate
                db.productos.Add(p);
                db.SaveChanges();
                
                
                #endregion

                /*
                List<Producto>prods = isub.ObtenerTodosProductos("TestURL");
                DataRecomendacion data = new DataRecomendacion {UsuarioID="unos", productos=new List<DataProducto>() };
                foreach (var p in prods)
                {
                    data.productos.Add(new DataProducto(p));
                }
                iusr.AgregarRecomendacionesUsuario("TestURL", data);
                DataRecomendacion d= iusr.ObtenerRecomendacionesUsuario("TestURL", data);
                Console.Read();
                foreach (var p in d.productos)
                {
                    System.Console.WriteLine(p.ProductoID);
                }

              */
            }

            //cargar algoritmo
            //IDALTienda tdal = new DALTiendaEF();
            //byte[] bytes = System.IO.File.ReadAllBytes(Environment.CurrentDirectory+@"..\..\..\Data\Chebay.AlgorithmDLLInfiniteLoop.dll");
            //Personalizacion pers = new Personalizacion { PersonalizacionID="HardShop", algoritmo=bytes };
            //tdal.ActualizarAlgoritmoPersonalizacion(pers);

            //DataRecomendacion d = new DataRecomendacion{UsuarioID="Gauss"};
            //DataRecomendacion dr = iusr.ObtenerRecomendacionesUsuario("HardShop", d);
            //foreach (var p in dr.productos)
            //{
            //    System.Console.WriteLine(p.ProductoID+p.nombre);
            //}

            
            //string currentpath = Environment.CurrentDirectory;
           
            //System.IO.Directory.GetParent();
            //ChebayDBPublic.ProvidePublicSchema();

            /*using (var context = ChebayDBContext.CreateTenant("DB4.3"))
            {
                //context.seed(); 
                Comentario[] com = { new Comentario { fecha = DateTime.Now, ProductoID = 1, texto = "comentario1 de dexter", UsuarioID = "Dexter" },
                                   new Comentario { fecha = DateTime.Now, ProductoID = 1, texto = "comentario2 de dexter", UsuarioID = "Dexter" }};
                foreach (var c in com)
                {
                    context.comentarios.Add(c);
                }
                context.SaveChanges();
            }*/
            //using (var context = ChebayDBPublic.CreatePublic())
            //    {
            //       context.Seed();
            //    }
      /*      AtributoSesion a = new AtributoSesion { AdministradorID="test@chebay.com", AtributoSesionID="sesion", Datos="esta nueva"};
            IDALTienda dal = new DALTiendaEF();
            dal.AgregarAtributoSesion(a);
            List<AtributoSesion> list = dal.ObtenerAtributosSesion("test@chebay.com");
            foreach (var l in list)
            {
                Console.WriteLine(l.AtributoSesionID+" "+l.AdministradorID+ " "+ l.Datos);
            }*/
            //Console.Read();
            //Ejemplo para crear schema
            //ChebayDBContext.ProvisionTenant("Tienda1");
            //ChebayDBPublic.ProvidePublicSchema();
        
            //Ejemplo utilizar schema
            //using (var context = ChebayDBContext.CreateTenant("Tienda1"))
            //{
            //}
     
}
