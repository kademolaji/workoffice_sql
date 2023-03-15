using System;
using WorkOffice.Contracts.Models;
using WorkOffice.Domain.Entities;

namespace WorkOffice.Contracts.Mappings
{
    public static class StructureDefinitionMappings
    {
        public static T ToModel<T>(this StructureDefinition entity) where T : StructureDefinitionModel, new()
        {
            return new T
            {
                StructureDefinitionId = entity.StructureDefinitionId.ToString(),
                Definition = entity.Definition,
                Description = entity.Description,
                Level = entity.Level,
                ClientId = entity.ClientId
            };
        }

        public static T ToModel<T>(this StructureDefinitionModel entity) where T : StructureDefinition, new()
        {
            return new T
            {
                Definition = entity.Definition,
                Description = entity.Description,
                Level = entity.Level,
                ClientId = entity.ClientId
            };
        }
    }
}
