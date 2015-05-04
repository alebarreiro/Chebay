using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using DataAccessLayer;
using System.Diagnostics;
using Shared.Entities;
using System.Collections.Generic;

namespace Chebay.DataAccessLayerTests
{
    [TestClass]
    public class DALTiendaTest
    {
        [TestMethod]
        public void Test0Inicial()
        {
            ChebayDBPublic cdbp = new ChebayDBPublic();
            using (var schema = ChebayDBPublic.CreatePublic())
            {
                IDALTienda it = new DALTiendaEF();
                Debug.WriteLine("INICIO");
                Debug.WriteLine("0.1. Elimino TestAdmin.");
                it.EliminarAdministrador("TestAdmin");
                Debug.WriteLine("\n0.2. Elimino www.TestURL.com.");
                it.EliminarTienda("TestURL");
                Debug.WriteLine("\n0.3. Chequear que no existe TestAdmin.");
                Assert.AreEqual(it.ObtenerAdministrador("TestAdmin"), null);
                Debug.WriteLine("\n0.4. Chequear que no existe www.TestURL.com.");
                Assert.AreEqual(it.ObtenerTienda("TestURL"), null);
            }
        }

        [TestMethod]
        public void AgregarAdministrador()
        {
            IDALTienda it = new DALTiendaEF();
            Debug.WriteLine("\n1. AgregarAdministrador");
            Debug.WriteLine("1.1. Crea un Administrador nuevo TestAdmin con pass: pass123.");
            Administrador a = new Administrador();
            a.AdministradorID = "TestAdmin";
            a.password = "pass123";
            a.tiendas = new HashSet<Tienda>();
            it.AgregarAdministrador(a);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AgregarAdministrador_Existente()
        {
            IDALTienda it = new DALTiendaEF();
            Debug.WriteLine("\n1.2. Vuelve a crear un Administrador nuevo TestAdmin.");
            Administrador a = new Administrador();
            a.AdministradorID = "TestAdmin";
            a.password = "pass123123123";
            it.AgregarAdministrador(a);
        }

        [TestMethod]
        public void AutenticarAdministrador()
        {
            IDALTienda it = new DALTiendaEF();
            Debug.WriteLine("\n2. AutenticarAdministrador");
            Debug.WriteLine("2.1. Autentica TestAdmin con pass123.");
            Assert.AreEqual(it.AutenticarAdministrador("TestAdmin", "pass123"), true);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AutenticarAdministrador_PassIncorrecta()
        {
            IDALTienda it = new DALTiendaEF();
            Debug.WriteLine("\n2.2. Autentica TestAdmin con pass incorrecta.");
            Assert.AreEqual(it.AutenticarAdministrador("TestAdmin", "pass12"), false);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AutenticarAdministrador_UserIncorrecto()
        {
            IDALTienda it = new DALTiendaEF();
            Debug.WriteLine("\n2.3. Autentica con usuario inexistente.");
            Assert.AreEqual(it.AutenticarAdministrador("TestAdmin123", "pass12"), false);
        }

        [TestMethod]
        public void AgregarTienda()
        {
            IDALTienda it = new DALTiendaEF();
            Debug.WriteLine("\n3. AgregarTienda");
            Debug.WriteLine("3.1. Crea tienda TestURL.com.");

            Administrador a = it.ObtenerAdministrador("TestAdmin");
            Tienda t = new Tienda();
            t.TiendaID = "TestURL";
            t.nombre = "NombreTest";
            t.descripcion = "DescTest";
            //t.administradores = new HashSet<Administrador>();
            //t.administradores.Add(a);
            Debug.WriteLine("A");
            it.AgregarTienda(t, a.AdministradorID);
            Debug.WriteLine("B");
            Tienda ret = it.ObtenerTienda("TestURL");
            Assert.IsNotNull(ret);
            Assert.AreEqual(ret.TiendaID, "TestURL");
            Assert.AreEqual(ret.nombre, "NombreTest");
            Assert.AreEqual(ret.descripcion, "DescTest");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AgregarTienda_Existente()
        {
            IDALTienda it = new DALTiendaEF();
            Debug.WriteLine("\n3.2. Vuelve a crear tienda TestURL.com.");
            Tienda t = new Tienda();
            t.TiendaID = "TestURL";
            t.nombre = "NombreTest";
            t.descripcion = "DescTest";
            t.administradores = new List<Administrador>();
            Administrador a = it.ObtenerAdministrador("TestAdmin");
            t.administradores.Add(a);
            it.AgregarTienda(t, a.AdministradorID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AgregarTienda_AdminInexistente()
        {
            IDALTienda it = new DALTiendaEF();
            Debug.WriteLine("\n3.3. Crea tienda con Administrador inexistente.");
            Tienda t = new Tienda();
            t.TiendaID = "TestURL";
            t.nombre = "NombreTest";
            t.descripcion = "DescTest";
            t.administradores = new HashSet<Administrador>();
            Administrador a = it.ObtenerAdministrador("TestAdmin");
            t.administradores.Add(a);
            it.AgregarTienda(t, "TestAdmin123");
        }

        [TestMethod]
        public void ActualizarTienda()
        {
            IDALTienda it = new DALTiendaEF();
            Debug.WriteLine("\n4. Actualizar Tienda");
            Debug.WriteLine("4.1. Actualizar TestURL.");
            Tienda t = new Tienda();
            t.TiendaID = "TestURL";
            t.nombre = "NombreTestNuevo";
            t.descripcion = "DescTestNueva";
            it.ActualizarTienda(t);
            Tienda nuevaT = it.ObtenerTienda("TestURL");
            Assert.IsNotNull(nuevaT);
            Assert.AreEqual(nuevaT.TiendaID, "TestURL");
            Assert.AreEqual(nuevaT.nombre, "NombreTestNuevo");
            Assert.AreEqual(nuevaT.descripcion, "DescTestNueva");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ActualizarTienda_Inexistente()
        {
            IDALTienda it = new DALTiendaEF();
            Debug.WriteLine("\n4.2. Actualizar tienda inexistente.");
            Tienda t = new Tienda();
            t.TiendaID = "TestURL123";
            t.nombre = "NombreTest";
            t.descripcion = "DescTest";
            it.ActualizarTienda(t);
        }

        [TestMethod]
        public void AgregarCategoriaCompuesta()
        {
            using (var schema = ChebayDBContext.CreateTenant("TestURL"))
            {
                IDALTienda it = new DALTiendaEF();

                CategoriaCompuesta father = (CategoriaCompuesta)it.ObtenerCategoria("TestURL",1);
                System.Console.WriteLine(father.CategoriaID + father.Nombre);

                Categoria c = new CategoriaCompuesta { Nombre = "CatCompuestaPrueba", padre = father };

                it.AgregarCategoria(c, "TestURL");

                CategoriaCompuesta cc = (CategoriaCompuesta)it.ObtenerCategoria("TestURL", 2);
                Assert.AreEqual(cc.CategoriaID, 2);
                Assert.AreEqual(cc.Nombre, "CatCompuestaPrueba");
                //Assert.AreEqual(cc.padre.CategoriaID, 1);
            }
        }

        [TestMethod]
        public void AgregarCategoriaSimple()
        {
            using (var schema = ChebayDBContext.CreateTenant("TestURL"))
            {
                IDALTienda it = new DALTiendaEF();

                CategoriaCompuesta father = (CategoriaCompuesta)it.ObtenerCategoria("TestURL", 2);
                System.Console.WriteLine(father.CategoriaID + father.Nombre);

                Categoria c = new CategoriaSimple { Nombre = "CatSimplePrueba", padre = father };

                it.AgregarCategoria(c, "TestURL");

                CategoriaSimple cc = (CategoriaSimple)it.ObtenerCategoria("TestURL", 3);
                Assert.AreEqual(cc.CategoriaID, 3);
                Assert.AreEqual(cc.Nombre, "CatSimplePrueba");
                //Assert.AreEqual(cc.padre.CategoriaID, 2);
            }
        }
            
        [TestMethod]
        public void AgregarCategorias()
        {
            IDALTienda it = new DALTiendaEF();
            List<Categoria> lc = new List<Categoria>();
            CategoriaCompuesta father = (CategoriaCompuesta)it.ObtenerCategoria("TestURL", 1);
            for (int i = 1; i < 10; i++ )
            {
                CategoriaCompuesta cc = new CategoriaCompuesta { Nombre = "CatPrueba" + i.ToString(), padre=father };
                lc.Add(cc);
            }
            for (int i = 1; i < 10; i++)
            {
                CategoriaSimple cc = new CategoriaSimple { Nombre = "CatPruebaSimple" + i.ToString(), padre=father };
                lc.Add(cc);
            }
            it.AgregarCategorias(lc, "TestURL");

            for (int i = 3; i<10; i++)
            {
                CategoriaCompuesta c = (CategoriaCompuesta)it.ObtenerCategoria("TestURL", i+1);
                Assert.AreEqual(c.CategoriaID, i+1);
                Assert.AreEqual(c.Nombre, "CatPrueba"+(i-2).ToString());
             //   Assert.AreEqual(c.padre.CategoriaID, 1);
            }
        }
        
        [TestMethod]
        public void ListarCategorias()
        {
            IDALTienda it = new DALTiendaEF();
            List<Categoria> lc = it.ListarCategorias("TestURL");
            Assert.AreEqual(21, lc.Count);
        }

        [TestMethod]
        public void AgregarAtributo()
        {
            IDALTienda it = new DALTiendaEF();
            List<Atributo> lAtributos = new List<Atributo>();
            Categoria c = it.ObtenerCategoria("TestURL", 3);
            Debug.WriteLine(c.CategoriaID);
            Atributo a = new Atributo { categoria = c, etiqueta = "pulgadas" , valor = "23" };
            it.AgregarAtributo(a, "TestURL");
        }

        [TestMethod]
        public void AgregarAtributos()
        {
            IDALTienda it = new DALTiendaEF();
            List<Atributo> lAtributos = new List<Atributo>();
            Categoria c = it.ObtenerCategoria("TestURL",2);
            Atributo a = new Atributo { categoria = c, etiqueta = "Conectividad", valor = "LTE" };
            lAtributos.Add(a);
            a = new Atributo { categoria = c, etiqueta = "Conectividad", valor = "3G" };
            lAtributos.Add(a);
            a = new Atributo { categoria = c, etiqueta = "Conectividad", valor = "GSM" };
            lAtributos.Add(a);
            it.AgregarAtributos(lAtributos, "TestURL");
        }

        [TestMethod]
        public void ObtenerAtributos()
        {
            IDALTienda it = new DALTiendaEF();
            List<Atributo> la = it.ObtenerAtributos(3, "TestURL");
            Assert.AreEqual(4, la.Count);
        }
    }
}
