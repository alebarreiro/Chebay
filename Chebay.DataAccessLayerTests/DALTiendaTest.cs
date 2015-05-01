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
            var connection = @"Server=qln8u7yf2c.database.windows.net,1433;Database=chebaytesting;User ID=chebaydb@qln8u7yf2c;Password=#!Chebay;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
            using (var db = new SqlConnection(connection))
            {
                var schema = ChebayDBPublic.CreatePublic(db);
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
            Tienda t = new Tienda();
            t.TiendaID = "TestURL";
            t.nombre = "NombreTest";
            t.descripcion = "DescTest";
            t.administradores = new List<Administrador>();
            Administrador a = it.ObtenerAdministrador("TestAdmin");
            t.administradores.Add(a);
            it.AgregarTienda(t,a.AdministradorID);
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
            t.administradores = new List<Administrador>();
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
        public void AgregarCategoria()
        {
            var connection = @"Server=qln8u7yf2c.database.windows.net,1433;Database=chebaytesting;User ID=chebaydb@qln8u7yf2c;Password=#!Chebay;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
            using (var db = new SqlConnection(connection))
            {
                IDALTienda it = new DALTiendaEF();
                List<Categoria> lc = new List<Categoria>();
                for (int i = 1; i < 10; i++ )
                {
                    CategoriaCompuesta cc = new CategoriaCompuesta();
                    cc.CategoriaID = "CategoriaCompuesta" + i.ToString();
                    cc.PadreID = "/";
                    cc.hijas = new List<Categoria>();
                    lc.Add(cc);
                }
                it.AgregarCategorias(lc, "TestURL");

                CategoriaCompuesta c = (CategoriaCompuesta)it.ObtenerCategoria("TestURL", "CategoriaCompuesta3");
                Assert.AreEqual(c.CategoriaID,"CategoriaCompuesta3");
                Assert.AreEqual(c.PadreID,"/");
            }
        }
        /*
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AgregarCategoria_Existente()
        {
            Debug.WriteLine("5.2. Agregar Categoria Raiz de vuelta.");
            IDALTienda it = new DALTiendaEF();
            it.AgregarCategoriaCompuesta("CatPrueba", "/", "www.TestURL.com");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AgregarCategoria_ConPadreNULL()
        {
            IDALTienda it = new DALTiendaEF();
            it.AgregarCategoriaCompuesta("CatPrueba", null, "www.TestURL.com");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AgregarCategoria_TiendaInexistente()
        {
            IDALTienda it = new DALTiendaEF();
            it.AgregarCategoriaCompuesta("CatPrueba", "/", "www.TestURL.com123123123");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AgregarCategoria_ConPadreInexistente()
        {
            IDALTienda it = new DALTiendaEF();
            it.AgregarCategoriaCompuesta("CatPrueba", "SinPadre", "www.TestURL.com");
        }*/
    }
}
