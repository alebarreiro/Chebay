using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chebay.BusinessLogicLayer
{
    class Program
    {
        static void Main(string[] args)
        {
            BLNotificaciones bl = new BLNotificaciones();
            bl.sendEmailNotification();
        }
    }
}
