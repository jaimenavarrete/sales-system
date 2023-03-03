using SalesSystem.DataAccess.Data;
using System;
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

        public void CreateUnitType(UnitTypes unitType)
        {
            unitType.Created = DateTime.UtcNow;
            unitType.CreatedBy = Guid.NewGuid().ToString();

            _context.UnitTypes.Add(unitType);
            _context.SaveChanges();
        }
    }
}
