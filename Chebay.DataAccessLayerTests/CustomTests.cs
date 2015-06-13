using DataAccessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared.Entities;
using System;

namespace Chebay.DataAccessLayerTests
{   [TestClass]
    public class CustomTests
    {
        [TestMethod]
        public void CierreSubastaNoCompras()
        {
            Console.WriteLine("Cierre subasta sin compradores");
            IDALSubasta sdal = new DALSubastaEF();
            Producto p = new Producto { nombre = "test", UsuarioID = "bob_rmselzb_seligstein@tfbnw.net", fecha_cierre = DateTime.UtcNow.AddMinutes(1), CategoriaID = 5 };
            sdal.AgregarProducto(p, "MobileCenter");
            Console.WriteLine("Se espera que se le notifique al vendedor que no fue exitosa la venta.");
        }

        [TestMethod]
        public void CierreSubastaConOferta()
        {
            Console.WriteLine("Cierre subasta con una oferta");
            string tienda = "MobileCenter";
            IDALSubasta sdal = new DALSubastaEF();
            Producto p = new Producto { nombre = "test1", UsuarioID = "bob_rmselzb_seligstein@tfbnw.net", fecha_cierre = DateTime.UtcNow.AddMinutes(1), CategoriaID = 5 };
            long prod = sdal.AgregarProducto(p, tienda);

            Oferta o = new Oferta { monto = 500, ProductoID = prod, UsuarioID = "open_pirsaoz_user@tfbnw.net" };
            sdal.OfertarProducto(o, tienda);
            Console.WriteLine("Se ha creado un producto y ofertado::" + prod);
            Console.WriteLine("Se espera el envio de un mail al vendedor como comprador.");

        }

        [TestMethod]
        public void CierreSubastaCompraDirecta()
        {
            Console.WriteLine("Cierre subasta con compra directa");
            string tienda = "MobileCenter";
            IDALSubasta sdal = new DALSubastaEF();
            Producto p = new Producto { nombre = "test2", UsuarioID = "bob_rmselzb_seligstein@tfbnw.net", fecha_cierre = DateTime.UtcNow.AddMinutes(1), CategoriaID = 5 };
            long prod = sdal.AgregarProducto(p, tienda);
            Compra compra = new Compra { monto = 200, ProductoID = prod, UsuarioID = "open_pirsaoz_user@tfbnw.net", fecha_compra = DateTime.UtcNow };
            sdal.AgregarCompra(compra, tienda);
            Console.WriteLine("Se espera el envio de un mail al vendedor como comprador.");

        }

    }
}
