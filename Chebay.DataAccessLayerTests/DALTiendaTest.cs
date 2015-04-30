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
        public void TestMethod1()
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
                Assert.AreEqual(it.ObtenerAdministrador("TestAdmin"),null);
                Debug.WriteLine("\n0.4. Chequear que no existe www.TestURL.com.");
                Assert.AreEqual(it.ObtenerTienda("www.TestURL.com"),null);
                Debug.WriteLine("");

                Debug.WriteLine("1. AgregarAdministrador");
                Debug.WriteLine("1.1. Crea un Administrador nuevo TestAdmin con pass: pass123.");
                it.AgregarAdministrador("TestAdmin", "pass123");
                Debug.WriteLine("");

                Debug.WriteLine("1.2. Vuelve a crear un Administrador nuevo TestAdmin.");
                it.AgregarAdministrador("TestAdmin", "pass12341234234");
                Debug.WriteLine("");

                Debug.WriteLine("2. AutenticarAdministrador");
                Debug.WriteLine("2.1. Autentica TestAdmin con pass123.");
                Assert.AreEqual(it.AutenticarAdministrador("TestAdmin", "pass123"), true);
                Debug.WriteLine("");

                Debug.WriteLine("2.2. Autentica TestAdmin con pass incorrecta.");
                Assert.AreEqual(it.AutenticarAdministrador("TestAdmin", "pass12"), false);
                Debug.WriteLine("");

                Debug.WriteLine("2.3. Autentica con usuario inexistente.");
                Assert.AreEqual(it.AutenticarAdministrador("TestAdmin123", "pass12"), false);
                Debug.WriteLine("");

                Debug.WriteLine("3. AgregarTienda");
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

                Debug.WriteLine("\nFIN: Volver al estado original.");
                it.EliminarAdministrador("TestAdmin");
                it.EliminarTienda("www.TestURL.com");
                /*
                it.AutenticarAdministrador("idAdmin", "pass1234");
                System.Console.WriteLine(asdf);
                it.AgregarTienda("Amazon", "desc1", "www.amazon.com", "idAdmin");


                System.Console.WriteLine("");
                System.Console.WriteLine("url    nombre      desc    admin");

                foreach (var a in schema.tiendas)
                {
                    System.Console.Write(a.TiendaID +
                        "     " + a.nombre +
                        "   " + a.descripcion);
                    foreach (var adm in a.administradores)
                    {
                        System.Console.WriteLine("    " + adm.AdministradorID);
                    }

                }*/
            }
        }
    }
}
