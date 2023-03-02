using SalesSystem.DataAccess.Data;
using System.Collections.Generic;
using System.Linq;

namespace SalesSystem.Business.Services
{
    public class UnitTypesService
    {
        private readonly SalesSystemEntities _context = new SalesSystemEntities();

        public List<UnitTypes> GetUnitTypes()
        {
            var unitTypes = _context.UnitTypes.ToList();

            return unitTypes;
        }
    }
}
