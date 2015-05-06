﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccessLayer;
using Shared.Entities;
using Shared.DataTypes;
using System.Diagnostics;

namespace Chebay.DataAccessLayerTests
{
    [TestClass]
    public class DALSubastaTests
    {
        public DALSubastaTests()
        {

        }

        private IDALSubasta idal = new DALSubastaEF();

        [TestMethod]
        public void SuperTestSubasta()
        {
            TestInicialSubasta();
            AgregarProducto();
            AgregarComentario();
            OfertarProducto();
        }

        [TestMethod]
        public void TestInicialSubasta()
        {
            IDALUsuario iu = new DALUsuarioEF();
            Usuario u = new Usuario { UsuarioID = "userPrueba" };
            iu.AgregarUsuario(u, "TestURL");
            u = new Usuario { UsuarioID = "otroUserPrueba" };
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

            p = new Producto
            {
                nombre = "Laptop",
                UsuarioID = "userPrueba",
                descripcion = "Es una Toshiba",
                precio_base_subasta = 1000,
                precio_compra = 3000,
                fecha_cierre = new DateTime(2016, 3, 27),
                CategoriaID = 3
            };
            idal.AgregarProducto(p, "TestURL");

            p = new Producto
            {
                nombre = "Televisión",
                UsuarioID = "userPrueba",
                descripcion = "Es una Samsung",
                precio_base_subasta = 750,
                precio_compra = 3000,
                fecha_cierre = new DateTime(2016, 9, 7),
                CategoriaID = 3
            };
            idal.AgregarProducto(p, "TestURL");

            p = new Producto
            {
                nombre = "Caballo",
                UsuarioID = "userPrueba",
                descripcion = "Es una yegua",
                precio_base_subasta = 1000,
                precio_compra = 20000,
                fecha_cierre = new DateTime(2015, 5, 10),
                CategoriaID = 3
            };
            idal.AgregarProducto(p, "TestURL");

            p = new Producto
            {
                nombre = "Perro",
                UsuarioID = "userPrueba",
                descripcion = "Es una cocker",
                precio_base_subasta = 30,
                precio_compra = 2000,
                fecha_cierre = new DateTime(2015, 5, 6),
                CategoriaID = 3
            };
            idal.AgregarProducto(p, "TestURL");

            p = new Producto
            {
                nombre = "Iguana",
                UsuarioID = "userPrueba",
                descripcion = "Es una iguana",
                precio_base_subasta = 500,
                precio_compra = 2000,
                fecha_cierre = new DateTime(2015, 5, 12),
                CategoriaID = 3
            };
            idal.AgregarProducto(p, "TestURL");

            p = new Producto
            {
                nombre = "Gato",
                UsuarioID = "userPrueba",
                descripcion = "Es una siames",
                precio_base_subasta = 70,
                precio_compra = 2000,
                fecha_cierre = new DateTime(2015, 5, 14),
                CategoriaID = 3
            };
            idal.AgregarProducto(p, "TestURL");

            p = new Producto
            {
                nombre = "Ratón",
                UsuarioID = "userPrueba",
                descripcion = "Es un ratón",
                precio_base_subasta = 10,
                precio_compra = 2000,
                fecha_cierre = new DateTime(2015, 5, 6),
                CategoriaID = 3
            };
            idal.AgregarProducto(p, "TestURL");

            p = new Producto
            {
                nombre = "Vaca",
                UsuarioID = "userPrueba",
                descripcion = "Es un hereford",
                precio_base_subasta = 1200,
                precio_compra = 2000,
                fecha_cierre = new DateTime(2015, 5, 16),
                CategoriaID = 3
            };
            idal.AgregarProducto(p, "TestURL");

            p = new Producto
            {
                nombre = "Toro",
                UsuarioID = "userPrueba",
                descripcion = "Es un toro",
                precio_base_subasta = 140,
                precio_compra = 2000,
                fecha_cierre = new DateTime(2015, 5, 16),
                CategoriaID = 3
            };
            idal.AgregarProducto(p, "TestURL");
 
            /*Producto ret = idal.ObtenerProducto(1, "TestURL");
            Assert.AreEqual("Celular", p.nombre);
            Assert.AreEqual("userPrueba", p.UsuarioID);
            Assert.AreEqual("Es un celular", p.descripcion);
            Assert.AreEqual(100, p.precio_base_subasta);
            Assert.AreEqual(2000, p.precio_compra);
            Assert.AreEqual(new DateTime(1993, 3, 27), p.fecha_cierre);
            Assert.AreEqual(3, p.CategoriaID);*/
        }

        [TestMethod]
        public void AgregarComentario()
        {
            
            Comentario c = new Comentario
            {
                texto = "comentario",
                fecha = DateTime.Today,
                ProductoID = 1,
                UsuarioID = "otroUserPrueba",
            };

            idal.AgregarComentario(c, "TestURL");
        }

        [TestMethod]
        public void OfertarProducto()
        {
            Oferta o = new Oferta
            {
                esFinal = false,
                monto = 120,
                ProductoID = 1,
                UsuarioID = "userPrueba"
            };
            idal.OfertarProducto(o, "TestURL");
            o = new Oferta
            {
                esFinal = false,
                monto = 160,
                ProductoID = 1,
                UsuarioID = "otroUserPrueba"
            };
            idal.OfertarProducto(o, "TestURL");
            o = new Oferta
            {
                esFinal = false,
                monto = 130,
                ProductoID = 1,
                UsuarioID = "userPrueba"
            };
            idal.OfertarProducto(o, "TestURL");

            List<DataProducto> ldp = idal.ObtenerProductosPersonalizados("TestURL");
            foreach (DataProducto dp in ldp)
            {
                Assert.AreEqual(160,dp.precio_actual);
                Assert.AreEqual("otroUserPrueba", dp.idOfertante);
            }
        }

    }
}