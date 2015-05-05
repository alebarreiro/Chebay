using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccessLayer;
using Shared.Entities;

namespace Chebay.DataAccessLayerTests
{
    [TestClass]
    public class DALSubastaTests
    {
        public DALSubastaTests()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
        }

        private IDALSubasta idal = new DALSubastaEF();

        [TestMethod]
        public void TestInicialSubasta()
        {
            IDALUsuario iu = new DALUsuarioEF();
            Usuario u = new Usuario { UsuarioID = "userPrueba" };
            iu.AgregarUsuario(u, "TestURL");
        }

        [TestMethod]
        public void AgregarProducto()
        {
            Producto p = new Producto
            {
                nombre = "Celular",
                UsuarioID = "userPrueba",
                descripcion = "Es un celular",
                precio_base_subasta = 100,
                precio_compra = 2000,
                fecha_cierre = new DateTime(1993, 3, 27),
                CategoriaID = 3
            };
            idal.AgregarProducto(p, "TestURL");

            Producto ret = idal.ObtenerProducto(1, "TestURL");
            Assert.AreEqual("Celular", p.nombre);
            Assert.AreEqual("userPrueba", p.UsuarioID);
            Assert.AreEqual("Es un celular", p.descripcion);
            Assert.AreEqual(100, p.precio_base_subasta);
            Assert.AreEqual(2000, p.precio_compra);
            Assert.AreEqual(new DateTime(1993, 3, 27), p.fecha_cierre);
            Assert.AreEqual(3, p.CategoriaID);
        }

        [TestMethod]
        public void AgregarComentario()
        {
            
            Comentario c = new Comentario
            {
                texto = "comentario",
                fecha = DateTime.Today,
                ProductoID = 1,
                UsuarioID = "userPrueba",
            };

            idal.AgregarComentario(c, "TestURL");
        }


    }
}