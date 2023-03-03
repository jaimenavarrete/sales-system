using SalesSystem.Business.Exceptions;
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

        public UnitTypes GetUnitTypeById(int id)
        {
            var unitType = _context.UnitTypes.Find(id);

            return unitType;
        }

        public void CreateUnitType(UnitTypes unitType)
        {
            unitType.Created = DateTime.UtcNow;
            unitType.CreatedBy = Guid.NewGuid().ToString();

            _context.UnitTypes.Add(unitType);
            _context.SaveChanges();
        }

        public void EditUnitType(UnitTypes unitTypes)
        {
            var currentUnitType = _context.UnitTypes.Find(unitTypes.Id);

            if(currentUnitType is null)
            {
                throw new BusinessException("La unidad de medida que intenta editar, no existe.");
            }

            currentUnitType.Name = unitTypes.Name;
            currentUnitType.Description = unitTypes.Description;
            currentUnitType.Modified = DateTime.UtcNow;
            currentUnitType.ModifiedBy = Guid.NewGuid().ToString();

            _context.SaveChanges();
        }

        public void DeleteUnitType(int id)
        {
            var unitType = _context.UnitTypes.Find(id);

            if (unitType is null)
            {
                throw new BusinessException("La unidad de medida que intenta borrar, no existe.");
            }

            var hasProducts = _context.Products.Any(product => product.UnitTypeId == id);

            if(hasProducts)
            {
                throw new BusinessException("La unidad de medida no se puede borrar, porque está siendo utilizada en uno o más productos.");
            }

            _context.UnitTypes.Remove(unitType);
            _context.SaveChanges();
        }
    }
}
