using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;

namespace DataAccessLayer
{
    public class DALUsuarioEF : IDALUsuario
    {
        public void AgregarUsuario(Usuario u)
        {
            using (var context = new ChebayDBContext())
            {
                try
                {
                    context.usuarios.Add(u);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }
            }
        }

        public void ActualizarUsuario(Usuario u)
        {
            using (var context = new ChebayDBContext())
            {
                try
                {
                    var query = from usr in context.usuarios
                                where usr.UsuarioID == u .UsuarioID
                                select usr;

                    Usuario user = query.FirstOrDefault();
                    context.SaveChanges();
                }

                catch (Exception e)
                {
                    Debug.WriteLine("Error: "+e.Message);
                }
            }
        }

        public Usuario ObtenerUsuario(string idUsuario)
        {
            using (var context = new ChebayDBContext())
            {
                try
                {
                    var query = from usr in context.usuarios
                                where usr.UsuarioID == idUsuario
                                select usr;
                    if (query.Count() > 0)
                        return query.First();
                    else
                        return null;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return null;
                }
            }
        }

        public List<Usuario> ObtenerTodosUsuarios()
        {
            using (var context = new ChebayDBContext())
            {
                try
                {
                    var query = from usr in context.usuarios
                                select usr;    
                    return query.ToList();
                }

                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    return null;
                }
            }
        }

        public void AgregarVisita(string idUsuario, Visita visita)
        {

        }

        public void AgregarFavorito(string idUsuario)
        {
           
        }
    }
} 
        
