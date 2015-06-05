using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccessLayer;
using Shared.Entities;
using System.Collections.Generic;
using Shared.DataTypes;
using System.Diagnostics;

namespace Chebay.DataAccessLayerTests
{
    [TestClass]
    public class DALUsuarioTests
    {
        
        private static string urlTest = "MobileCenter";
        private static string user1 = "aleTest";
        private static string user2 = "Recoba";
        private static IDALUsuario iu = new DALUsuarioEF();

        [TestMethod]
        public void SuperTest()
        {
            ObtenerFactura();
        }

        [TestMethod]
        public void AgregarUsuario()
        {
            Usuario u = new Usuario
            {
                UsuarioID = user1
            };
            iu.AgregarUsuario(u, urlTest);
            Usuario usr = iu.ObtenerUsuario(user1, urlTest);
            Assert.AreEqual(usr.UsuarioID, u.UsuarioID);

            u = new Usuario
            {
                UsuarioID = user2
            };
            iu.AgregarUsuario(u, urlTest);
            usr = iu.ObtenerUsuario(user2, urlTest);
            Assert.AreEqual(usr.UsuarioID, u.UsuarioID);
        }

        [TestMethod]
        public void ActualizarUsuario()
        {
            Usuario u = new Usuario
            {
                UsuarioID = "aleTest",
                Apellido = "Añón",
                Nombre = "Alejandro",
                Ciudad = "Montevideo",
                Pais = "Uruguay",
                Direccion = "Acevedo Diaz esq. 18 de Julio",
                NumeroContacto = "098856438",
                CodigoPostal = 11200,
                Email = "alejandro.anon@fing.edu.uy"
            };
            iu.ActualizarUsuario(u, urlTest);

            Usuario usr = iu.ObtenerUsuario(user1, urlTest);

            Assert.AreEqual(usr.UsuarioID, u.UsuarioID);
            Assert.AreEqual(usr.Apellido, u.Apellido);
            Assert.AreEqual(usr.Nombre, u.Nombre);
            Assert.AreEqual(usr.Ciudad, u.Ciudad);
            Assert.AreEqual(usr.Pais, u.Pais);
            Assert.AreEqual(usr.Direccion, u.Direccion);
            Assert.AreEqual(usr.NumeroContacto, u.NumeroContacto);
            Assert.AreEqual(usr.CodigoPostal, u.CodigoPostal);
            Assert.AreEqual(usr.Email, u.Email);
        }

        [TestMethod]
        public void AgregarCalificacion()
        {
            Usuario u1 = iu.ObtenerUsuario(user1, urlTest);
            Usuario u2 = iu.ObtenerUsuario(user2, urlTest);

            Calificacion c = new Calificacion
            {
                ProductoID = 1,
                usuario_c = u1,
                usuario_e = u2,
                puntaje = 4
            };
            iu.AgregarCalificacion(c, urlTest);
        }

        [TestMethod]
        public void ObtenerFactura()
        {
            List<DataFactura> ldf = iu.ObtenerFactura("alejandroanonmallo@gmail.com", "MobileCenter");
            foreach (DataFactura df in ldf)
            {
                Debug.WriteLine("A");
                Debug.WriteLine(df.nombreProducto);
                Debug.WriteLine(df.fecha);
                Debug.WriteLine(df.monto);
                Debug.WriteLine(df.esCompra);
            }
        }


        /*
        //--CALIFICACIONES--
        void AgregarCalificacion(Calificacion c, string idTienda);
        Calificacion ObtenerCalificacion(long idCalificacion, string idTienda);
        List<Calificacion> ObtenerCalificaciones(string idTienda);
        void EliminarCalificacion(long idCalificacion, string idTienda);
        DataCalificacion ObtenerCalificacionUsuario(string idUsuario, string idTienda);

        //--IMAGENES--
        void AgregarImagenUsuario(ImagenUsuario iu, string idTienda);
        ImagenUsuario ObtenerImagenUsuario(string idUsuario, string idTienda);
        void EliminarImagenUsuario(string idUsuario, string idTienda);

        */
    }
}
