using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Entities;
using Shared.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;

namespace DataAccessLayer
{
    public class UsuariosEFContext : DbContext
    {
        public UsuariosEFContext()
        {
            var ddlcopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
        
        public DbSet<Usuario> Usuarios { get; set; }

    }

    public class DALUsuarioEF : IDALUsuario
    {
        void AgregarUsuario(Usuario u)
        {
            using (var context = new UsuariosEFContext())
            {
                try
                {
                    context.Usuarios.Add(u);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error: "+e.Message);
                }
            }
        }

        void ActualizarUsuario(Usuario u)
        {
            using (var context = new UsuariosEFContext())
            {
                try
                {
                    var query = from usr in context.Usuarios
                                where usr.Id == u.Id
                                select usr;
                    
                    foreach (Usuario usr in query)
                    {
                        //Actualiza a usr.
                    }
                    context.SaveChanges();
                }

                catch (Exception e)
                {
                    Debug.WriteLine("Error: "+e.Message);
                }
            }
        }

        Usuario ObtenerUsuario(int id)
        {
            throw new NotImplementedException();
        }

        List<Usuario> ObtenerTodosUsuarios()
        {
            throw new NotImplementedException();
        }

        Usuario ObtenerUsuarioMail(string email)
        //Obtener Usuario por Mail.
        {
            throw new NotImplementedException();
        }
    }
} 
        
