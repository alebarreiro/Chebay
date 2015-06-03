using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using DataAccessLayer;
using System.Diagnostics;
using Shared.Entities;
using System.Collections.Generic;
using System.IO;

namespace Chebay.DataAccessLayerTests
{
    [TestClass]
    public class DALTiendaTests
    {

        public DALTiendaTests()
        {

        }

        private static string urlTest = "template1";
        private static string adminTest = "adminMobileCenter";
        private static IDALTienda it = new DALTiendaEF();

        [TestMethod]
        public void SuperTest()
        {
            /*Tienda template1 = new Tienda 
            {
                TiendaID = "template2",
                descripcion = "Tienda con el Template 2.",
                nombre = "template2",
                administradores = new HashSet<Administrador>()
            };
            it.AgregarTienda(template1, adminTest);
            */
            
            string path = @"C:\\Users\\Alejandro\\template1.css";
            string readText = File.ReadAllText(path);
            Debug.Write(readText);

            Personalizacion pr = new Personalizacion
            {
                PersonalizacionID = urlTest,
                datos = null,
                template = 1,
                backgroud_image = null,
                css = readText
            };
            it.PersonalizarTienda(pr, "template1");

            /*Test0Inicial();
            AgregarAdministrador();
            AgregarTienda();
            AgregarVariasCategorias();
            AgregarVariosTipoAtributo();*/
        }

        [TestMethod]
        public void PersonalizarTienda()
        {
            //it.PersonalizarTienda("63589F", urlTest);
        }

        [TestMethod]
        public void Test0Inicial()
        {
            ChebayDBPublic cdbp = new ChebayDBPublic();
            using (var schema = ChebayDBPublic.CreatePublic())
            {
                Debug.WriteLine("INICIO");
                Debug.WriteLine("0.1. Elimino TestAdmin.");
                it.EliminarAdministrador(adminTest);

                it.EliminarPersonalizacion(urlTest);
                Debug.WriteLine("\n0.2. Elimino TestURL.");
                it.EliminarTienda(urlTest);
                /*
                Debug.WriteLine("\n0.3. Chequear que no existe TestAdmin.");
                Assert.AreEqual(it.ObtenerAdministrador(adminTest), null);
                Debug.WriteLine("\n0.4. Chequear que no existe www.TestURL.com.");
                Assert.AreEqual(it.ObtenerTienda(urlTest), null);*/
            }
        }

        [TestMethod]
        public void AgregarAdministrador()
        {
            Debug.WriteLine("\n1. AgregarAdministrador");
            Debug.WriteLine("1.1. Crea un Administrador nuevo TestAdmin con pass: pass123.");
            Administrador a = new Administrador();
            a.AdministradorID = adminTest;
            a.password = "pass123";
            a.tiendas = new HashSet<Tienda>();
            it.AgregarAdministrador(a);
        }

        [TestMethod]
        public void AutenticarAdministrador()
        {
            Debug.WriteLine("\n2. AutenticarAdministrador");
            Debug.WriteLine("2.1. Autentica TestAdmin con pass123.");
            Assert.AreEqual(it.AutenticarAdministrador(adminTest, "pass123"), true);
        }

        [TestMethod]
        public void AgregarTienda()
        {
            Debug.WriteLine("\n3. AgregarTienda");
            Debug.WriteLine("3.1. Crea tienda");

            Tienda t = new Tienda
            {
                TiendaID = urlTest,
                nombre = urlTest,
                descripcion = "Tienda de Celulares",
                administradores = new HashSet<Administrador>()
            };

            it.AgregarTienda(t, adminTest);
            Tienda ret = it.ObtenerTienda(urlTest);

            Assert.IsNotNull(ret);
            Assert.AreEqual(ret.TiendaID, urlTest);
            Assert.AreEqual(ret.nombre, urlTest);
            Assert.AreEqual(ret.descripcion, "Tienda de Celulares");
        }

        [TestMethod]
        public void ActualizarTienda()
        {
            Debug.WriteLine("\n4. Actualizar Tienda");
            Debug.WriteLine("4.1. Actualizar TestURL.");
            Tienda t = new Tienda();
            t.TiendaID = urlTest;
            t.nombre = "NombreTestNuevo";
            t.descripcion = "DescTestNueva";
            it.ActualizarTienda(t);
            Tienda nuevaT = it.ObtenerTienda(urlTest);
            Assert.IsNotNull(nuevaT);
            Assert.AreEqual(nuevaT.TiendaID, urlTest);
            Assert.AreEqual(nuevaT.nombre, "NombreTestNuevo");
            Assert.AreEqual(nuevaT.descripcion, "DescTestNueva");
        }

        [TestMethod]
        public void AgregarCategoriaCompuesta()
        {
            using (var schema = ChebayDBContext.CreateTenant(urlTest))
            {
                CategoriaCompuesta father = (CategoriaCompuesta)it.ObtenerCategoria(urlTest, 1);
                System.Console.WriteLine(father.CategoriaID + father.Nombre);

                Categoria c = new CategoriaCompuesta
                {
                    Nombre = "CatCompuestaPrueba",
                    padre = father,
                    hijas = new HashSet<Categoria>(),
                    tipoatributos = new HashSet<TipoAtributo>()
                };
                it.AgregarCategoria(c, urlTest);

                CategoriaCompuesta cc = (CategoriaCompuesta)it.ObtenerCategoria(urlTest, 2);
                Assert.AreEqual(cc.CategoriaID, 2);
                Assert.AreEqual(cc.Nombre, "CatCompuestaPrueba");
                //Assert.AreEqual(cc.padre.CategoriaID, 1);
            }
        }

        [TestMethod]
        public void AgregarCategoriaSimple()
        {
            using (var schema = ChebayDBContext.CreateTenant(urlTest))
            {
                CategoriaCompuesta father = (CategoriaCompuesta)it.ObtenerCategoria(urlTest, 2);
                System.Console.WriteLine(father.CategoriaID + father.Nombre);

                Categoria c = new CategoriaSimple { Nombre = "CatSimplePrueba", padre = father };

                it.AgregarCategoria(c, urlTest);

                CategoriaSimple cc = (CategoriaSimple)it.ObtenerCategoria(urlTest, 3);
                Assert.AreEqual(cc.CategoriaID, 3);
                Assert.AreEqual(cc.Nombre, "CatSimplePrueba");
                //Assert.AreEqual(cc.padre.CategoriaID, 2);
            }
        }

        [TestMethod]
        public void AgregarCategorias()
        {
            List<Categoria> lc = new List<Categoria>();
            CategoriaCompuesta father = (CategoriaCompuesta)it.ObtenerCategoria(urlTest, 1);
            for (int i = 1; i < 10; i++)
            {
                CategoriaCompuesta cc = new CategoriaCompuesta { Nombre = "CatPrueba" + i.ToString(), padre = father };
                lc.Add(cc);
            }
            for (int i = 1; i < 10; i++)
            {
                CategoriaSimple cc = new CategoriaSimple { Nombre = "CatPruebaSimple" + i.ToString(), padre = father };
                lc.Add(cc);
            }
            it.AgregarCategorias(lc, urlTest);

            for (int i = 3; i < 10; i++)
            {
                CategoriaCompuesta c = (CategoriaCompuesta)it.ObtenerCategoria(urlTest, i + 1);
                Assert.AreEqual(c.CategoriaID, i + 1);
                Assert.AreEqual(c.Nombre, "CatPrueba" + (i - 2).ToString());
                //   Assert.AreEqual(c.padre.CategoriaID, 1);
            }
        }

        [TestMethod]
        public void ListarCategorias()
        {
            List<Categoria> lc = it.ListarCategorias(urlTest);
            Assert.AreEqual(21, lc.Count);
        }

        [TestMethod]
        public void ListarCategoriasHijas()
        {
            CategoriaCompuesta c = (CategoriaCompuesta)it.ObtenerCategoria(urlTest, 2);
            Assert.AreEqual(1, c.hijas.Count);
        }
/*
        [TestMethod]
        public void AgregarAtributo()
        {
            List<Atributo> lAtributos = new List<Atributo>();
            Categoria c = it.ObtenerCategoria(urlTest, 3);
            Debug.WriteLine(c.CategoriaID);
            Atributo a = new Atributo { categoria = c, etiqueta = "pulgadas", valor = "23" };
            it.AgregarAtributo(a, urlTest);
        }

        [TestMethod]
        public void AgregarAtributos()
        {
            List<Atributo> lAtributos = new List<Atributo>();
            Categoria c = it.ObtenerCategoria(urlTest, 2);
            Atributo a = new Atributo { categoria = c, etiqueta = "Conectividad", valor = "LTE" };
            lAtributos.Add(a);
            a = new Atributo { categoria = c, etiqueta = "Conectividad", valor = "3G" };
            lAtributos.Add(a);
            a = new Atributo { categoria = c, etiqueta = "Conectividad", valor = "GSM" };
            lAtributos.Add(a);
            it.AgregarAtributos(lAtributos, urlTest);
        }

        [TestMethod]
        public void ObtenerAtributos()
        {
            List<Atributo> la = it.ObtenerAtributos(3, urlTest);
            Assert.AreEqual(4, la.Count);
        }
        */
        [TestMethod]
        public void SessionAtributesTest()
        {
            using (var schema = ChebayDBPublic.CreatePublic())
            {
                IDALTienda handler = new DALTiendaEF();
                AtributoSesion[] atr = {   
													new AtributoSesion { AdministradorID="Admin1", AtributoSesionID="Cache", Datos="algo" },
													new AtributoSesion { AdministradorID = "Admin1", AtributoSesionID = "Cache2", Datos = "algo2" },                  
													new AtributoSesion { AdministradorID = "Admin1", AtributoSesionID = "Cache2", Datos = "distinto" }
												};
                foreach (var a in atr)
                {
                    handler.AgregarAtributoSesion(a);
                }

                Console.WriteLine("Atributos del usuario admin.");
                foreach (var a in handler.ObtenerAtributosSesion("Admin1"))
                {
                    Console.WriteLine(a.AtributoSesionID + " " + a.Datos);
                }
            }
        }

        [TestMethod]
        public void AgregarTipoAtributo()
        {
            TipoAtributo ta = new TipoAtributo
            {
                TipoAtributoID = "Tamaño pantalla",
                tipodato = TipoDato.STRING
            };
            it.AgregarTipoAtributo(ta, 3, urlTest);

            ta = new TipoAtributo
            {
                TipoAtributoID = "Otro TipoAtributo",
                tipodato = TipoDato.STRING
            };
            it.AgregarTipoAtributo(ta, 2, urlTest);
        }

        [TestMethod]
        public void ListarTipoAtributo()
        {
            List<TipoAtributo> test = it.ListarTipoAtributo(urlTest);
            Assert.AreEqual(2, test.Count);
        }

        [TestMethod]
        public void AgregarVariasCategorias()
        {
            
            CategoriaCompuesta father = (CategoriaCompuesta)it.ObtenerCategoria(urlTest, 1);
            
            //PRIMER NIVEL
            Categoria c = new CategoriaCompuesta
            {
                Nombre = "Samsung",
                padre = father,
                hijas = new HashSet<Categoria>(),
                tipoatributos = new HashSet<TipoAtributo>()
            };
            it.AgregarCategoria(c, urlTest);
            CategoriaCompuesta cc = (CategoriaCompuesta)it.ObtenerCategoria(urlTest, 2);
            Assert.AreEqual(cc.CategoriaID, 2);
            Assert.AreEqual(cc.Nombre, "Samsung");

            Categoria c2 = new CategoriaCompuesta
            {
                Nombre = "Windows",
                padre = father,
                hijas = new HashSet<Categoria>(),
                tipoatributos = new HashSet<TipoAtributo>()
            };
            it.AgregarCategoria(c2, urlTest);

            Categoria c3 = new CategoriaCompuesta
            {
                Nombre = "Apple",
                padre = father,
                hijas = new HashSet<Categoria>(),
                tipoatributos = new HashSet<TipoAtributo>()
            };
            it.AgregarCategoria(c3, urlTest);

            //SEGUNDO NIVEL
                //WINDOWS
            Categoria c21 = new CategoriaSimple
            {
                Nombre = "Lumia",
                padre = (CategoriaCompuesta)c2,
                tipoatributos = new HashSet<TipoAtributo>()
            };
            it.AgregarCategoria(c21, urlTest);

            Categoria c22 = new CategoriaSimple
            {
                Nombre = "Asha",
                padre = (CategoriaCompuesta)c2,
                tipoatributos = new HashSet<TipoAtributo>()
            };
            it.AgregarCategoria(c22, urlTest);

                //SAMSUNG
            Categoria c11 = new CategoriaCompuesta
            {
                Nombre = "Galaxy",
                padre = (CategoriaCompuesta)c,
                hijas = new HashSet<Categoria>(),
                tipoatributos = new HashSet<TipoAtributo>()
            };
            it.AgregarCategoria(c11, urlTest);

            Categoria c111 = new CategoriaSimple
            {
                Nombre = "S",
                padre = (CategoriaCompuesta)c11,
                tipoatributos = new HashSet<TipoAtributo>()
            };
            it.AgregarCategoria(c111, urlTest);

            Categoria c112 = new CategoriaSimple
            {
                Nombre = "Note",
                padre = (CategoriaCompuesta)c11,
                tipoatributos = new HashSet<TipoAtributo>()
            };
            it.AgregarCategoria(c112, urlTest);

            Categoria c12 = new CategoriaSimple
            {
                Nombre = "Ace",
                padre = (CategoriaCompuesta)c,
                tipoatributos = new HashSet<TipoAtributo>()
            };
            it.AgregarCategoria(c12, urlTest);

                //APPLE
            Categoria c31 = new CategoriaCompuesta
            {
                Nombre = "iPhone",
                padre = (CategoriaCompuesta)c3,
                hijas = new HashSet<Categoria>(),
                tipoatributos = new HashSet<TipoAtributo>()
            };
            it.AgregarCategoria(c31, urlTest);

            Categoria c311 = new CategoriaSimple
            {
                Nombre = "6",
                padre = (CategoriaCompuesta)c31,
                tipoatributos = new HashSet<TipoAtributo>()
            };
            it.AgregarCategoria(c311, urlTest);

            Categoria c312 = new CategoriaSimple
            {
                Nombre = "5",
                padre = (CategoriaCompuesta)c31,
                tipoatributos = new HashSet<TipoAtributo>()
            };
            it.AgregarCategoria(c312, urlTest);

            Categoria c313 = new CategoriaSimple
            {
                Nombre = "4",
                padre = (CategoriaCompuesta)c31,
                tipoatributos = new HashSet<TipoAtributo>()
            };
            it.AgregarCategoria(c313, urlTest);
        }

        [TestMethod]
        public void AgregarVariosTipoAtributo()
        {
            TipoAtributo ta = new TipoAtributo
            {
                TipoAtributoID = "Apple Pay",
                tipodato = TipoDato.BOOL
            };
            it.AgregarTipoAtributo(ta, 4, urlTest);

            ta = new TipoAtributo
            {
                TipoAtributoID = "Tamaño de la pantalla",
                tipodato = TipoDato.FLOAT
            };
            it.AgregarTipoAtributo(ta, 1, urlTest);

            ta = new TipoAtributo
            {
                TipoAtributoID = "Peso",
                tipodato = TipoDato.INTEGER
            };
            it.AgregarTipoAtributo(ta, 1, urlTest);

            ta = new TipoAtributo
            {
                TipoAtributoID = "Memoria de disco",
                tipodato = TipoDato.INTEGER
            };
            it.AgregarTipoAtributo(ta, 1, urlTest);

            ta = new TipoAtributo
            {
                TipoAtributoID = "Memoria RAM",
                tipodato = TipoDato.INTEGER
            };
            it.AgregarTipoAtributo(ta, 1, urlTest);

            ta = new TipoAtributo
            {
                TipoAtributoID = "Procesador",
                tipodato = TipoDato.STRING
            };
            it.AgregarTipoAtributo(ta, 1, urlTest);

            ta = new TipoAtributo
            {
                TipoAtributoID = "Sistema Operativo",
                tipodato = TipoDato.INTEGER
            };
            it.AgregarTipoAtributo(ta, 1, urlTest);
        }

        [TestMethod]
        public void ListarVariosTipoAtributo()
        {
            List<TipoAtributo> test = it.ListarTipoAtributo(4,urlTest);
            Assert.AreEqual(7, test.Count);
        }


    }
}
