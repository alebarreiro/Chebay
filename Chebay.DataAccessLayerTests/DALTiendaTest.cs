using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using DataAccessLayer;
using System.Diagnostics;
using Shared.Entities;

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
                it.EliminarTienda("www.TestURL.com");
                Debug.WriteLine("\n0.3. Chequear que no existe TestAdmin.");
                Assert.AreEqual(it.ObtenerAdministrador("TestAdmin"), null);
                Debug.WriteLine("\n0.4. Chequear que no existe www.TestURL.com.");
                Assert.AreEqual(it.ObtenerTienda("www.TestURL.com"), null);
            }
        }

        [TestMethod]
        public void Test1AgregarAdministrador()
        {
            var connection = @"Server=qln8u7yf2c.database.windows.net,1433;Database=chebaytesting;User ID=chebaydb@qln8u7yf2c;Password=#!Chebay;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
            using (var db = new SqlConnection(connection))
            {
                var schema = ChebayDBPublic.CreatePublic(db);
                IDALTienda it = new DALTiendaEF();
                Debug.WriteLine("\n1. AgregarAdministrador");
                Debug.WriteLine("1.1. Crea un Administrador nuevo TestAdmin con pass: pass123.");
                it.AgregarAdministrador("TestAdmin", "pass123");

                Debug.WriteLine("\n1.2. Vuelve a crear un Administrador nuevo TestAdmin.");
                it.AgregarAdministrador("TestAdmin", "pass12341234234");
            }
        }

        [TestMethod]
        public void Test2AutenticarAdministrador()
        {
            var connection = @"Server=qln8u7yf2c.database.windows.net,1433;Database=chebaytesting;User ID=chebaydb@qln8u7yf2c;Password=#!Chebay;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
            using (var db = new SqlConnection(connection))
            {
                var schema = ChebayDBPublic.CreatePublic(db);
                IDALTienda it = new DALTiendaEF();
                Debug.WriteLine("\n2. AutenticarAdministrador");
                Debug.WriteLine("2.1. Autentica TestAdmin con pass123.");
                Assert.AreEqual(it.AutenticarAdministrador("TestAdmin", "pass123"), true);
                
                Debug.WriteLine("\n2.2. Autentica TestAdmin con pass incorrecta.");
                Assert.AreEqual(it.AutenticarAdministrador("TestAdmin", "pass12"), false);
                
                Debug.WriteLine("\n2.3. Autentica con usuario inexistente.");
                Assert.AreEqual(it.AutenticarAdministrador("TestAdmin123", "pass12"), false);
            }
        }

        [TestMethod]
        public void Test3AgregarTienda()
        {
            var connection = @"Server=qln8u7yf2c.database.windows.net,1433;Database=chebaytesting;User ID=chebaydb@qln8u7yf2c;Password=#!Chebay;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
            using (var db = new SqlConnection(connection))
            {
                var schema = ChebayDBPublic.CreatePublic(db);
                IDALTienda it = new DALTiendaEF();
                Debug.WriteLine("\n3. AgregarTienda");
                Debug.WriteLine("3.1. Crea tienda TestURL.com.");
                it.AgregarTienda("NombreTest", "DescTest", "www.TestURL.com", "TestAdmin");
                Tienda t = it.ObtenerTienda("www.TestURL.com");
                Assert.IsNotNull(t);
                Assert.AreEqual(t.TiendaID, "www.TestURL.com");
                Assert.AreEqual(t.nombre, "NombreTest");
                Assert.AreEqual(t.descripcion, "DescTest");

                Debug.WriteLine("\n3.2. Vuelve a crear tienda TestURL.com.");
                it.AgregarTienda("NombreTest2", "DescTest2", "www.TestURL.com", "TestAdmin");

                Debug.WriteLine("\n3.3. Crea tienda con Administrador inexistente.");
                it.AgregarTienda("NombreTest2", "DescTest2", "www.TestURL2.com", "TestAdmin123");
            }
        }

        [TestMethod]
        public void Test4ActualizarTienda()
        {
            var connection = @"Server=qln8u7yf2c.database.windows.net,1433;Database=chebaytesting;User ID=chebaydb@qln8u7yf2c;Password=#!Chebay;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
            using (var db = new SqlConnection(connection))
            {
                var schema = ChebayDBPublic.CreatePublic(db);
                IDALTienda it = new DALTiendaEF();
                Debug.WriteLine("\n4. Actualizar Tienda");
                Debug.WriteLine("4.1. Actualizar TestURL.com.");
                it.ActualizarTienda("NombreTestNuevo", "DescTestNueva", "www.TestURL.com");
                Tienda nuevaT = it.ObtenerTienda("www.TestURL.com");
                Assert.IsNotNull(nuevaT);
                Assert.AreEqual(nuevaT.TiendaID, "www.TestURL.com");
                Assert.AreEqual(nuevaT.nombre, "NombreTestNuevo");
                Assert.AreEqual(nuevaT.descripcion, "DescTestNueva");

                Debug.WriteLine("\n4.2. Actualizar tienda inexistente.");
                it.ActualizarTienda("NombreTestNuevo", "DescTestNueva", "www.TestURL.com1asdf23");
            }
        }

        [TestMethod]
        public void Test5AgregarCategoria()
        {
            var connection = @"Server=qln8u7yf2c.database.windows.net,1433;Database=chebaytesting;User ID=chebaydb@qln8u7yf2c;Password=#!Chebay;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
            using (var db = new SqlConnection(connection))
            {
                var schema = ChebayDBPublic.CreatePublic(db);
                IDALTienda it = new DALTiendaEF();
                Debug.WriteLine("\n5. Agregar Categoria.");
                Debug.WriteLine("5.1. Agregar Categoria Raiz.");
                it.AgregarCategoriaCompuesta("CatRaiz", null, "www.TestURL.com");
            }
        }
    }
}
