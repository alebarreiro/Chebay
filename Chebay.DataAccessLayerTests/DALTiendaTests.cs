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

        private static string urlTest = "HardPC";
        private static string adminTest = "adminMobileCenter";
        private static IDALTienda it = new DALTiendaEF();

        [TestMethod]
        public void SuperTest()
        {
            
            AgregarTienda();
            AgregarVariasCategorias();
            AgregarVariosTipoAtributo();
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
        public void AgregarTienda()
        {
            Debug.WriteLine("\n3. AgregarTienda");
            Debug.WriteLine("3.1. Crea tienda");

            Tienda t = new Tienda
            {
                TiendaID = urlTest,
                nombre = urlTest,
                descripcion = "Tu lugar de encuentro con la tecnología.",
                administradores = new HashSet<Administrador>()
            };

            it.AgregarTienda(t, adminTest);
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
                Nombre = "Computadoras",
                padre = father,
                hijas = new HashSet<Categoria>(),
                tipoatributos = new HashSet<TipoAtributo>()
            };
            it.AgregarCategoria(c, urlTest);
            CategoriaCompuesta cc = (CategoriaCompuesta)it.ObtenerCategoria(urlTest, 2);

            Categoria c2 = new CategoriaCompuesta
            {
                Nombre = "Hardware",
                padre = father,
                hijas = new HashSet<Categoria>(),
                tipoatributos = new HashSet<TipoAtributo>()
            };
            it.AgregarCategoria(c2, urlTest);

            Categoria c3 = new CategoriaSimple
            {
                Nombre = "Software",
                padre = father,
                productos = new HashSet<Producto>(),
                tipoatributos = new HashSet<TipoAtributo>()
            };
            it.AgregarCategoria(c3, urlTest);

            //SEGUNDO NIVEL
                //COMPUTADORAS
            Categoria c11 = new CategoriaCompuesta
            {
                Nombre = "Laptops",
                padre = (CategoriaCompuesta)c,
                hijas = new HashSet<Categoria>(),
                tipoatributos = new HashSet<TipoAtributo>()
            };
            it.AgregarCategoria(c11, urlTest);

            Categoria c12 = new CategoriaCompuesta
            {
                Nombre = "Torres",
                padre = (CategoriaCompuesta)c,
                hijas = new HashSet<Categoria>(),
                tipoatributos = new HashSet<TipoAtributo>()
            };
            it.AgregarCategoria(c12, urlTest);

                //HARDWARE
            Categoria c21 = new CategoriaSimple
            {
                Nombre = "Memoria RAM",
                padre = (CategoriaCompuesta)c2,
                tipoatributos = new HashSet<TipoAtributo>()
            };
            it.AgregarCategoria(c21, urlTest);

            Categoria c22 = new CategoriaSimple
            {
                Nombre = "Discos Duros",
                padre = (CategoriaCompuesta)c2,
                tipoatributos = new HashSet<TipoAtributo>()
            };
            it.AgregarCategoria(c22, urlTest);

            Categoria c23 = new CategoriaSimple
            {
                Nombre = "Motherboards",
                padre = (CategoriaCompuesta)c2,
                tipoatributos = new HashSet<TipoAtributo>()
            };
            it.AgregarCategoria(c23, urlTest);

            //TERCER NIVEL
                //Laptops
            Categoria c111 = new CategoriaSimple
            {
                Nombre = "Toshiba",
                padre = (CategoriaCompuesta)c11,
                tipoatributos = new HashSet<TipoAtributo>()
            };
            it.AgregarCategoria(c111, urlTest);

            Categoria c112 = new CategoriaSimple
            {
                Nombre = "Sony",
                padre = (CategoriaCompuesta)c11,
                tipoatributos = new HashSet<TipoAtributo>()
            };
            it.AgregarCategoria(c112, urlTest);

            Categoria c113 = new CategoriaSimple
            {
                Nombre = "Samsung",
                padre = (CategoriaCompuesta)c11,
                tipoatributos = new HashSet<TipoAtributo>()
            };
            it.AgregarCategoria(c113, urlTest);

            Categoria c114 = new CategoriaSimple
            {
                Nombre = "Apple",
                padre = (CategoriaCompuesta)c11,
                tipoatributos = new HashSet<TipoAtributo>()
            };
            it.AgregarCategoria(c114, urlTest);

            //TORRES
            Categoria c121 = new CategoriaSimple
            {
                Nombre = "AMD",
                padre = (CategoriaCompuesta)c12,
                tipoatributos = new HashSet<TipoAtributo>()
            };
            it.AgregarCategoria(c121, urlTest);

            Categoria c122 = new CategoriaSimple
            {
                Nombre = "INTEL",
                padre = (CategoriaCompuesta)c12,
                tipoatributos = new HashSet<TipoAtributo>()
            };
            it.AgregarCategoria(c122, urlTest);
        }

        [TestMethod]
        public void AgregarVariosTipoAtributo()
        {
            TipoAtributo ta = new TipoAtributo
            {
                TipoAtributoID = "Procesador",
                tipodato = TipoDato.STRING
            };
            it.AgregarTipoAtributo(ta, 2, urlTest);

            ta = new TipoAtributo
            {
                TipoAtributoID = "Disco duro",
                tipodato = TipoDato.STRING
            };
            it.AgregarTipoAtributo(ta, 2, urlTest);

            ta = new TipoAtributo
            {
                TipoAtributoID = "Memoria RAM",
                tipodato = TipoDato.INTEGER
            };
            it.AgregarTipoAtributo(ta, 2, urlTest);

            ta = new TipoAtributo
            {
                TipoAtributoID = "Pantalla",
                tipodato = TipoDato.STRING
            };
            it.AgregarTipoAtributo(ta, 2, urlTest);

            ta = new TipoAtributo
            {
                TipoAtributoID = "Garantía",
                tipodato = TipoDato.STRING
            };
            it.AgregarTipoAtributo(ta, 1, urlTest);

            ta = new TipoAtributo
            {
                TipoAtributoID = "¿Es un producto nuevo?",
                tipodato = TipoDato.BOOL
            };
            it.AgregarTipoAtributo(ta, 1, urlTest);

            ta = new TipoAtributo
            {
                TipoAtributoID = "Sistema Operativo",
                tipodato = TipoDato.STRING
            };
            it.AgregarTipoAtributo(ta, 2, urlTest);

            ta = new TipoAtributo
            {
                TipoAtributoID = "Tarjeta de video",
                tipodato = TipoDato.STRING
            };
            it.AgregarTipoAtributo(ta, 2, urlTest);

            ta = new TipoAtributo
            {
                TipoAtributoID = "Grabadora DVD",
                tipodato = TipoDato.BOOL
            };
            it.AgregarTipoAtributo(ta, 2, urlTest);

            ta = new TipoAtributo
            {
                TipoAtributoID = "Camara Web",
                tipodato = TipoDato.BOOL
            };
            it.AgregarTipoAtributo(ta, 2, urlTest);

            ta = new TipoAtributo
            {
                TipoAtributoID = "Peso",
                tipodato = TipoDato.STRING
            };
            it.AgregarTipoAtributo(ta, 5, urlTest);

            ta = new TipoAtributo
            {
                TipoAtributoID = "Duración de la batería",
                tipodato = TipoDato.STRING
            };
            it.AgregarTipoAtributo(ta, 5, urlTest);

            ta = new TipoAtributo
            {
                TipoAtributoID = "Capacidad",
                tipodato = TipoDato.STRING
            };
            it.AgregarTipoAtributo(ta, 7, urlTest);

            ta = new TipoAtributo
            {
                TipoAtributoID = "Tipo de memoria",
                tipodato = TipoDato.STRING
            };
            it.AgregarTipoAtributo(ta, 7, urlTest);

            //DISCO DURO
            ta = new TipoAtributo
            {
                TipoAtributoID = "Capacidad",
                tipodato = TipoDato.STRING
            };
            it.AgregarTipoAtributo(ta, 8, urlTest);

            ta = new TipoAtributo
            {
                TipoAtributoID = "Interfaz del disco duro",
                tipodato = TipoDato.STRING
            };
            it.AgregarTipoAtributo(ta, 8, urlTest);

            //Motherboards
            ta = new TipoAtributo
            {
                TipoAtributoID = "Modelo",
                tipodato = TipoDato.STRING
            };
            it.AgregarTipoAtributo(ta, 9, urlTest);

            ta = new TipoAtributo
            {
                TipoAtributoID = "Fabricante",
                tipodato = TipoDato.STRING
            };
            it.AgregarTipoAtributo(ta, 9, urlTest);

            ta = new TipoAtributo
            {
                TipoAtributoID = "Chipset",
                tipodato = TipoDato.STRING
            };
            it.AgregarTipoAtributo(ta, 9, urlTest);

            ta = new TipoAtributo
            {
                TipoAtributoID = "Memoria",
                tipodato = TipoDato.STRING
            };
            it.AgregarTipoAtributo(ta, 9, urlTest);

            ta = new TipoAtributo
            {
                TipoAtributoID = "BIOS",
                tipodato = TipoDato.STRING
            };
            it.AgregarTipoAtributo(ta, 9, urlTest);

        }

        [TestMethod]
        public void ListarVariosTipoAtributo()
        {
            List<TipoAtributo> test = it.ListarTipoAtributo(4,urlTest);
            Assert.AreEqual(7, test.Count);
        }


    }
}
