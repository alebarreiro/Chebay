﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Entities;

namespace DataAccessLayer
{
    interface IDALAdministrador
    {
        void AgregarAdministrador(Administrador a);

        void ModificarAdministrador(Administrador a);

        Administrador ObtenerAdministrador(string username);

        Administrador ObtenerAdministradorTienda(int idTienda);

        bool AutenticarAdministrador(string user, string password);
        //Devuelve true si es el password correcto para el usuario.
    }
}