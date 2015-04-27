using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DALTiendaEF : IDALTienda
    {
        void AgregarAdministrador(Administrador a)
        {
            using (var context = new ChebayDBContext())
            {
                try
                {
                    context.administradores.Add(a);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }
            }
        }

        /*void ModificarAdministrador(Administrador a)
        {
            using (var context = new ChebayDBContext())
            {
                try
                {
                    var query = from admin in context.administradores
                                where admin.AdministradorID == a.AdministradorID
                                select admin;
                    Administrador adm = query.FirstOrDefault();
                    adm.password = a.password;
                    adm.TiendaID = a.TiendaID;
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }
            }
        }*/

        Administrador ObtenerAdministrador(string idAdministrador)
        {
            using (var context = new ChebayDBContext())
            {
                try
                {
                    var query = from admin in context.administradores
                                where admin.AdministradorID == idAdministrador
                                select admin;
                    return query.FirstOrDefault();
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    return null;
                }
            }
        }

        Administrador ObtenerAdministradorTienda(string idTienda)
        {
            using (var context = new ChebayDBContext())
            {
                try
                {
                    var query = from admin in context.administradores
                                where admin.TiendaID == idTienda
                                select admin;
                    return query.FirstOrDefault();
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    return null;
                }
            }
        }

        bool AutenticarAdministrador(string idAdministrador, string passwd)
        //Devuelve true si es el password correcto para el usuario.
        {
            using (var context = new ChebayDBContext())
            {
                try
                {
                    var query = from admin in context.administradores
                                where admin.AdministradorID == idAdministrador
                                select admin;
                    Administrador adm = query.FirstOrDefault();
                    if (adm != null && adm.password == passwd)
                        return true;
                    else
                        return false;
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    return false;
                }
            }
        }

        bool CambiarPassAdministrador(string idAdministrador, string passwdVieja, string passwdNueva)
        //Devuelve true si passwdVieja es la password del idAdministrador y pudo cambiarla por passwdNueva.
        {
            using (var context = new ChebayDBContext())
            {
                try
                {
                    var query = from admin in context.administradores
                                where admin.AdministradorID == idAdministrador
                                select admin;
                    Administrador adm = query.FirstOrDefault();
                    if (adm != null && adm.password == passwdVieja)
                    {
                        adm.password = passwdNueva;
                        context.SaveChanges();
                        return true;
                    }
                    else
                        return false;
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    return false;
                }
            }
        }
    
    }
}
