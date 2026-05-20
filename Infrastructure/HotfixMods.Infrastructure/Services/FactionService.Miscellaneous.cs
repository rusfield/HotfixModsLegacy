using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public partial class FactionService
    {
        async Task SetIdAndVerifiedBuild(FactionDto dto)
        {
            var factionId = await GetIdByConditionsAsync<Faction>(dto.Faction.ID, dto.IsUpdate);
            var factionTemplateId = dto.IsUpdate && dto.FactionTemplate.ID > 0
                ? dto.FactionTemplate.ID
                : factionId;
            var hotfixModsEntityId = await GetIdByConditionsAsync<HotfixModsEntity>(dto.HotfixModsEntity.ID, dto.IsUpdate);

            dto.HotfixModsEntity.ID = hotfixModsEntityId;
            dto.HotfixModsEntity.RecordID = factionId;
            dto.HotfixModsEntity.VerifiedBuild = VerifiedBuild;
            if (string.IsNullOrWhiteSpace(dto.HotfixModsEntity.Name))
                dto.HotfixModsEntity.Name = string.IsNullOrWhiteSpace(dto.Faction.Name) ? $"Faction {factionId}" : dto.Faction.Name;

            dto.Faction.ID = factionId;
            dto.Faction.VerifiedBuild = VerifiedBuild;

            dto.FactionTemplate.ID = factionTemplateId;
            dto.FactionTemplate.Faction = (ushort)factionId;
            dto.FactionTemplate.VerifiedBuild = VerifiedBuild;
        }
    }
}
