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
        void AgregarUsuario(Usuario u)
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
                    Debug.WriteLine("Error: "+e.Message);
                }
            }
        }

      /*  void ActualizarUsuario(Usuario u)
        {
            using (var context = new ChebayDBContext())
            {
                try
                {
                    var query = from usr in context.usuarios
                                where usr.UsuarioID == u .UsuarioID
                                select usr;

                    Usuario user = query.FirstOrDefault();
                    user.token
                    context.SaveChanges();
                }

                catch (Exception e)
                {
                    Debug.WriteLine("Error: "+e.Message);
                }
            }
        }*/

        Usuario ObtenerUsuario(string id)
        {
            using (var context = new ChebayDBContext())
            {
                try
                {
                    var query = from usr in context.usuarios
                                where usr.UsuarioID == id
                                select usr;
                    if (query.Count() > 0)
                        return query.First();
                    else
                        return null;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error: " + e.Message);
                    return null;
                }
            }
        }

        List<Usuario> ObtenerTodosUsuarios()
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
                    Debug.WriteLine("Error: " + e.Message);
                    return null;
                }
            }
        }
    }
} 
        
