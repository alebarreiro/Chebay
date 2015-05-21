using System;
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

        private static string urlTest = "MobileCenter";
        private IDALSubasta idal = new DALSubastaEF();

        [TestMethod]
        public void SuperTestSubasta()
        {

            Producto p = new Producto
            {
                nombre = "ProductoPorVencer",
                UsuarioID = "aleTest",
                CategoriaID = 5,
                descripcion = "Esta nuevo y esta bloqueado para ANTEL.",
                precio_base_subasta = 99,
                precio_compra = 2000,
                fecha_cierre = new DateTime(2015, 5, 21, 18, 22, 40)
            };
            idal.AgregarProducto(p, urlTest);

            Oferta o = new Oferta
            {
                esFinal = false,
                monto = 120,
                ProductoID = 29,
                UsuarioID = "Recoba"
            };
            idal.OfertarProducto(o, urlTest);
            
            //AgregarVariosProducto();
            
            /*TestInicialSubasta();
            AgregarProducto();
            AgregarComentario();
            OfertarProducto();
            ObtenerInfoProducto();
            ObtenerProductosCategoria();
            ObtenerProductosPorTerminar();*/
//            AgregarFavorito();
            //ObtenerCantFavoritos();
            //EliminarFavorito();
        }

        [TestMethod]
        public void TestInicialSubasta()
        {
            IDALUsuario iu = new DALUsuarioEF();
            Usuario u = new Usuario { UsuarioID = "userPrueba" };
            iu.AgregarUsuario(u, urlTest);
            u = new Usuario { UsuarioID = "otroUserPrueba" };
            iu.AgregarUsuario(u, urlTest);
        }

        [TestMethod]
        public void AgregarProducto()
        {
            Producto p = new Producto
            {
                nombre = "Celular",
                UsuarioID = "userPrueba",
                CategoriaID = 3,
                descripcion = "Es un celular",
                precio_base_subasta = 100,
                precio_compra = 2000,
                fecha_cierre = new DateTime(1993, 3, 27)
            };
            IDALTienda it = new DALTiendaEF();
            idal.AgregarProducto(p, urlTest);

            p = new Producto
            {
                nombre = "Laptop",
                UsuarioID = "userPrueba",
                CategoriaID = 3,
                descripcion = "Es una Toshiba",
                precio_base_subasta = 1000,
                precio_compra = 3000,
                fecha_cierre = new DateTime(2016, 3, 27)
            };
            idal.AgregarProducto(p, urlTest);

            p = new Producto
            {
                nombre = "Televisión",
                UsuarioID = "userPrueba",
                CategoriaID = 3,
                descripcion = "Es una Samsung",
                precio_base_subasta = 750,
                precio_compra = 3000,
                fecha_cierre = new DateTime(2016, 9, 7)
            };
            idal.AgregarProducto(p, urlTest);

            p = new Producto
            {
                nombre = "Caballo",
                UsuarioID = "userPrueba",
                CategoriaID = 3,
                descripcion = "Es una yegua",
                precio_base_subasta = 1000,
                precio_compra = 20000,
                fecha_cierre = new DateTime(2015, 5, 10)
            };
            idal.AgregarProducto(p, urlTest);

            p = new Producto
            {
                nombre = "Perro",
                UsuarioID = "userPrueba",
                CategoriaID = 3,
                descripcion = "Es una cocker",
                precio_base_subasta = 30,
                precio_compra = 2000,
                fecha_cierre = new DateTime(2015, 5, 6)
            };
            idal.AgregarProducto(p, urlTest);

            p = new Producto
            {
                nombre = "Iguana",
                UsuarioID = "userPrueba",
                CategoriaID = 3,
                descripcion = "Es una iguana",
                precio_base_subasta = 500,
                precio_compra = 2000,
                fecha_cierre = new DateTime(2015, 5, 12)
            };
            idal.AgregarProducto(p, urlTest);

            p = new Producto
            {
                nombre = "Gato",
                UsuarioID = "userPrueba",
                CategoriaID = 3,
                descripcion = "Es una siames",
                precio_base_subasta = 70,
                precio_compra = 2000,
                fecha_cierre = new DateTime(2015, 5, 14)
            };
            idal.AgregarProducto(p, urlTest);

            p = new Producto
            {
                nombre = "Ratón",
                UsuarioID = "userPrueba",
                CategoriaID = 3,
                descripcion = "Es un ratón",
                precio_base_subasta = 10,
                precio_compra = 2000,
                fecha_cierre = new DateTime(2015, 5, 6)
            };
            idal.AgregarProducto(p, urlTest);

            p = new Producto
            {
                nombre = "Vaca",
                UsuarioID = "userPrueba",
                CategoriaID = 3,
                descripcion = "Es un hereford",
                precio_base_subasta = 1200,
                precio_compra = 2000,
                fecha_cierre = new DateTime(2015, 5, 16)
            };
            idal.AgregarProducto(p, urlTest);

            p = new Producto
            {
                nombre = "Toro",
                UsuarioID = "userPrueba",
                CategoriaID = 3,
                descripcion = "Es un toro",
                precio_base_subasta = 140,
                precio_compra = 2000,
                fecha_cierre = new DateTime(2015, 5, 16)
            };
            idal.AgregarProducto(p, urlTest);
            /*Producto ret = idal.ObtenerProducto(1, urlTest);
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

            idal.AgregarComentario(c, urlTest);
            c = new Comentario
            {
                texto = "me anduvo bien",
                fecha = DateTime.Today,
                ProductoID = 1,
                UsuarioID = "otroUserPrueba",
            };

            idal.AgregarComentario(c, urlTest);
        }

        [TestMethod]
        public void OfertarProducto()
        {
            Oferta o = new Oferta
            {
                esFinal = false,
                monto = 120,
                ProductoID = 18,
                UsuarioID = "Recoba"
            };
            idal.OfertarProducto(o, urlTest);
/*            o = new Oferta
            {
                esFinal = false,
                monto = 160,
                ProductoID = 3,
                UsuarioID = "otroUserPrueba"
            };
            idal.OfertarProducto(o, urlTest);
            o = new Oferta
            {
                esFinal = false,
                monto = 130,
                ProductoID = 3,
                UsuarioID = "userPrueba"
            };
            idal.OfertarProducto(o, urlTest);*/
        }

        [TestMethod]
        public void ObtenerInfoProducto()
        {
            //CASO DE USO: VER INFO PRODUCTO FULL
            //--OPERACIÓN
            Producto p = idal.ObtenerInfoProducto(1, urlTest, "otroUserPrueba");

            //--CHEQUEOS
            Debug.WriteLine("--INFO PRODUCTO--");
            Debug.WriteLine("p.ProductoID: " + p.ProductoID);
            Assert.AreEqual(1, p.ProductoID);
            Debug.WriteLine("p.nombre: " + p.nombre);
            Assert.AreEqual("Celular", p.nombre);
            Debug.WriteLine("p.descripcion: " + p.descripcion);
            Assert.AreEqual("Es un celular", p.descripcion);
            Debug.WriteLine("p.UsuarioID: " + p.UsuarioID);
            Assert.AreEqual("userPrueba", p.UsuarioID);
            Debug.WriteLine("p.precio_base_subasta: " + p.precio_base_subasta);
            Assert.AreEqual(100, p.precio_base_subasta);
            Debug.WriteLine("p.precio_compra: " + p.precio_compra);
            Assert.AreEqual(2000, p.precio_compra);
            Debug.WriteLine("p.fecha_cierre: " + p.fecha_cierre);
            //Assert.AreEqual("1993-03-27 00:00:00.000", p.fecha_cierre);

            //--COMENTARIOS
            Debug.WriteLine("\n--COMENTARIOS--");
            foreach (Comentario com in p.comentarios)
            {
                Debug.WriteLine("com.ComentarioID: " + com.ComentarioID);
                Assert.AreEqual(1,com.ComentarioID);
                Debug.WriteLine("com.texto: " + com.texto);
                Assert.AreEqual("comentario",com.texto);
                Debug.WriteLine("com.ProductoID: " + com.ProductoID);
                Assert.AreEqual(1,com.ProductoID);
                Debug.WriteLine("com.UsuarioID: " + com.UsuarioID);
                Assert.AreEqual("otroUserPrueba",com.UsuarioID);
            }

            //--OFERTAS
            Debug.WriteLine("\n--OFERTAS--");
            foreach (Oferta of in p.ofertas)
            {
                Debug.WriteLine("of.OfertaID: " + of.OfertaID);
                Debug.WriteLine("of.ProductoID: " + of.ProductoID);
                Assert.AreEqual(1, of.ProductoID);
                Debug.WriteLine("of.UsuarioID: " + of.UsuarioID);
                Debug.WriteLine("of.monto: " + of.monto);
                Debug.WriteLine("");
            }

            //--CATEGORIAS
            Debug.WriteLine("--CATEGORIA--");
            Debug.WriteLine("c.CategoriaID: " + p.CategoriaID);
            Debug.WriteLine("c.Nombre: " + p.nombre);
            Debug.WriteLine("c.Tipo: " + p.GetType().ToString());
            Debug.WriteLine("");
            
            //--USUARIO
            Debug.WriteLine("--USUARIO--");
            Debug.WriteLine("c.CategoriaID: " + p.usuario.UsuarioID);
            Assert.AreEqual("userPrueba", p.usuario.UsuarioID);

            //--CATEGORIAS
            Debug.WriteLine("\n--ATRIBUTOS--");
            foreach (Atributo atr in p.atributos)
            {
                Debug.WriteLine("atr.AtributoID: " + atr.AtributoID);
                Debug.WriteLine("atr.TipoAtributoID: " + atr.TipoAtributoID);
                Debug.WriteLine("atr.valor: " + atr.valor);
                Debug.WriteLine("atr.etiqueta: " + atr.etiqueta);
            }

        }

        [TestMethod]
        public void ObtenerProductosCategoria()
        {
            List<Producto> prods = idal.ObtenerProductosCategoria(1, urlTest);
            foreach (Producto p in prods)
            {
                Debug.WriteLine(p.nombre);
            }
        }
       
        [TestMethod]
        public void ObtenerProductosPorTerminar()
        {
            List<DataProducto> prods = idal.ObtenerProductosPorTerminar(2,urlTest);
            foreach (DataProducto p in prods)
            {
                Debug.WriteLine(p.nombre);
            }
        }

        [TestMethod]
        public void AgregarFavorito()
        {
            idal.AgregarFavorito(1, "otroUserPrueba", urlTest);
        }

        [TestMethod]
        public void EliminarFavorito()
        {
            idal.EliminarFavorito(1, "userPrueba", urlTest);
        }

        [TestMethod]
        public void AgregarVariosProducto()
        {

            Producto p = new Producto
            {
                nombre = "Lumio 8000",
                UsuarioID = "aleTest",
                CategoriaID = 5,
                descripcion = "Esta nuevo y esta bloqueado para ANTEL.",
                precio_base_subasta = 99,
                precio_compra = 2000,
                fecha_cierre = new DateTime(2015, 5, 21, 17, 45, 00)
            };
            idal.AgregarProducto(p, urlTest);

            /*
            Producto p = new Producto
            {
                nombre = "Lumia 710",
                UsuarioID = "aleTest",
                CategoriaID = 5,
                descripcion = "Esta nuevo y esta bloqueado para ANTEL.",
                precio_base_subasta = 99,
                precio_compra = 2000,
                fecha_cierre = new DateTime(2015, 6, 2)
            };
            idal.AgregarProducto(p, urlTest);

            p = new Producto
            {
                nombre = "Lumia 640 XL Dual SIM",
                UsuarioID = "aleTest",
                CategoriaID = 5,
                descripcion = "Con una pantalla HD de 5,7 pulgadas y una cámara de 13 MP, tienes todo lo que necesitas que encontrar el equilibrio perfecto entre juego y trabajo.",
                precio_base_subasta = 1000,
                precio_compra = 3000,
                fecha_cierre = new DateTime(2015, 6, 2)
            };
            idal.AgregarProducto(p, urlTest);

            p = new Producto
            {
                nombre = "Lumia 640 XL",
                UsuarioID = "aleTest",
                CategoriaID = 5,
                descripcion = "Con una pantalla HD de 5,7 pulgadas y una cámara de 13 MP, tienes todo lo que necesitas que encontrar el equilibrio perfecto entre juego y trabajo.",
                precio_base_subasta = 1000,
                precio_compra = 3000,
                fecha_cierre = new DateTime(2015, 6, 2)
            };
            idal.AgregarProducto(p, urlTest);

            p = new Producto
            {
                nombre = "Lumia 532",
                UsuarioID = "aleTest",
                CategoriaID = 5,
                descripcion = "Es Lumia 532, es un potente smartphone con el software más reciente y las mejores características nuevas de Windows.",
                precio_base_subasta = 1000,
                precio_compra = 3000,
                fecha_cierre = new DateTime(2015, 6, 2)
            };
            idal.AgregarProducto(p, urlTest);

            p = new Producto
            {
                nombre = "Nokia Lumia 635",
                UsuarioID = "aleTest",
                CategoriaID = 5,
                descripcion = "Gran pantalla de 4,5\", pantalla táctil super sensible, alta velocidad para navegar.",
                precio_base_subasta = 250,
                precio_compra = 3000,
                fecha_cierre = new DateTime(2015, 6, 2)
            };
            idal.AgregarProducto(p, urlTest);

            //ASHA
            p = new Producto
            {
                nombre = "Nokia Asha 311",
                UsuarioID = "aleTest",
                CategoriaID = 6,
                descripcion = "Sofisticado celular, con excelentes prestaciones, Pantalla touchscreen, Cámara de foto 3.15 MP, MP3/MP4, Facebook, Correo electrónico, entre otras.",
                precio_base_subasta = 250,
                precio_compra = 3000,
                fecha_cierre = new DateTime(2015,6,10   )
            };
            idal.AgregarProducto(p, urlTest);

            p = new Producto
            {
                nombre = "Nokia Asha 503",
                UsuarioID = "aleTest",
                CategoriaID = 6,
                descripcion = "Un diseño que causa sensación, con excelentes funciones de cámara, redes sociales integradas y acceso rápido Fastlane. Haz fotos y compártelas más rápido que nunca. Desbloquea la pantalla sólo con deslizar el dedo.",
                precio_base_subasta = 250,
                precio_compra = 3000,
                fecha_cierre = new DateTime(2015, 6, 10)
            };
            idal.AgregarProducto(p, urlTest);

            //Samsung Galaxy S
            p = new Producto
            {
                nombre = "Samsung S5 Mini",
                UsuarioID = "aleTest",
                CategoriaID = 8,
                descripcion = "Galaxy S5 mini tiene una pantalla HD Super AMOLED de 4.5\". Cámara de 8MP. Posee sensor de huellas dactilares en su tecla de inicio desbloquear la pantalla. Resistente al polvo y al agua.",
                precio_base_subasta = 400,
                precio_compra = 3000,
                fecha_cierre = new DateTime(2015, 6, 10)
            };
            idal.AgregarProducto(p, urlTest);

            p = new Producto
            {
                nombre = "Samsung Galaxy S5",
                UsuarioID = "aleTest",
                CategoriaID = 8,
                descripcion = "Cuenta con la tecnología Download booster que permite que las descargas sean todavía más rápidas gracias a la suma de la conexión Wi-Fi que tengas más los 20 Mbps de velocidad LTE que te brinda Antel.",
                precio_base_subasta = 981,
                precio_compra = 3000,
                fecha_cierre = new DateTime(2015, 6, 10)
            };
            idal.AgregarProducto(p, urlTest);

            p = new Producto
            {
                nombre = "Samsung Galaxy S6",
                UsuarioID = "aleTest",
                CategoriaID = 8,
                descripcion = "Su elegante diseño de cuerpo metálico, más potente y con carga ultra-rápida de bateria. Cuenta con una pantalla Quad HD Super AMOLED de 5,1\", cámara de 16 MP, procesador Octa-Core y 3 GB de RAM.",
                precio_base_subasta = 1418,
                precio_compra = 30000,
                fecha_cierre = new DateTime(2015, 6, 10)
            };
            idal.AgregarProducto(p, urlTest);

            p = new Producto
            {
                nombre = "Samsung Galaxy S6 Edge",
                UsuarioID = "aleTest",
                CategoriaID = 8,
                descripcion = "El primer smartphone con doble borde curvo. Cuenta con una pantalla Quad HD Super AMOLED de 5,1\", cámara de 16 MP, procesador Octa-Core, 3 GB de RAM y carga ultra-rápida de batería.",
                precio_base_subasta = 1418,
                precio_compra = 30000,
                fecha_cierre = new DateTime(2015, 6, 10)
            };
            idal.AgregarProducto(p, urlTest);

            //SAMSUNG GALAXY NOTE
            p = new Producto
            {
                nombre = "Samsung Galaxy Note 4",
                UsuarioID = "aleTest",
                CategoriaID = 9,
                descripcion = "Descubrí el nuevo Galaxy Note, con una pantalla de 5,7? Super AMOLED Quad HD, cámara de 16MP con estabilizador óptico Smart OIS, batería de larga duración con carga rápida y un nuevo S PEN más preciso y funcional.",
                precio_base_subasta = 1418,
                precio_compra = 30000,
                fecha_cierre = new DateTime(2015, 6, 10)
            };
            idal.AgregarProducto(p, urlTest);

            p = new Producto
            {
                nombre = "Samsung Note 3",
                UsuarioID = "aleTest",
                CategoriaID = 9,
                descripcion = "Smartphone con sistema operativo Android, cámara de 13 megapíxeles, pantalla de 5,7 pulgadas, 3 gigas de RAM y hasta 64 gigas de almacenamiento. Además cuenta con las funcionalidades de \"Smart scroll\", \"Smart pause\" y \"Air gesture\".",
                precio_base_subasta = 1418,
                precio_compra = 30000,
                fecha_cierre = new DateTime(2015, 6, 10)
            };
            idal.AgregarProducto(p, urlTest);

            //SAMSUNG ACE
            p = new Producto
            {
                nombre = "Samsung Ace 4",
                UsuarioID = "aleTest",
                CategoriaID = 10,
                descripcion = "Smartphone con conexión LTE, sistema operativo Android 4.4 KitKat , cámara de 5 MP, pantalla de 4.3 pulgadas. Procesador de 1.2 GHz.",
                precio_base_subasta = 1418,
                precio_compra = 30000,
                fecha_cierre = new DateTime(2015, 6, 10)
            };
            idal.AgregarProducto(p, urlTest);

            //IPHONE 6
            p = new Producto
            {
                nombre = "iPhone 6",
                UsuarioID = "aleTest",
                CategoriaID = 12,
                descripcion = "El iPhone 6 no es simplemente más grande, es mejor en todo sentido. Más grande, pero asombrosamente delgado. Más poderoso, pero increíblemente eficiente.",
                precio_base_subasta = 1418,
                precio_compra = 30000,
                fecha_cierre = new DateTime(2015, 6, 10)
            };
            idal.AgregarProducto(p, urlTest);

            p = new Producto
            {
                nombre = "iPhone 6 Plus",
                UsuarioID = "aleTest",
                CategoriaID = 12,
                descripcion = "El iPhone 6 no es simplemente más grande, es mejor en todo sentido. Más grande, pero asombrosamente delgado. Más poderoso, pero increíblemente eficiente.",
                precio_base_subasta = 1418,
                precio_compra = 30000,
                fecha_cierre = new DateTime(2015, 6, 10)
            };
            idal.AgregarProducto(p, urlTest);

            //IPHONE 5
            p = new Producto
            {
                nombre = "Apple iPhone 5S",
                UsuarioID = "aleTest",
                CategoriaID = 12,
                descripcion = "El Iphone 5s es el primer smartphone que integra un chip con arquitectura de 64 bits, que combinado con el IOS 7, lo hacen muy rápido. Es un teléfono fino y liviano. Incluye un nuevo sensor de identidad por huella, denominado Touch ID.",
                precio_base_subasta = 1071,
                precio_compra = 30000,
                fecha_cierre = new DateTime(2015, 6, 10)
            };
            idal.AgregarProducto(p, urlTest);
            */
        }
    }
}