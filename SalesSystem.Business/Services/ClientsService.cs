using SalesSystem.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesSystem.Business.Services
{
    public class ClientsService
    {
        private readonly SalesSystemEntities _context = new SalesSystemEntities();

        public List<Clients> GetClients()
        {
            var clients = _context.Clients.ToList();

            return clients;
        }
    }
}
