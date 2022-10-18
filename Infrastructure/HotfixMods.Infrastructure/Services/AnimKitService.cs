using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Infrastructure.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Services
{
    public partial class AnimKitService : Service
    {
        public AnimKitService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider) { }

        public async Task<AnimKitDto> GetNewAsync()
        {
            var result = new AnimKitDto();
            result.AnimKit.Id = await GetNextHotfixEntityIdAsync<AnimKit>();

            return result;
        }

        public async Task<AnimKitDto?> GetById(int id)
        {
            var animKit = await GetSingleAsync<AnimKit>(new DbParameter(nameof(AnimKit.Id), id));
            if (null == animKit)
            {
                return null;
            }
            return new AnimKitDto()
            {
                AnimKit = animKit,
                AnimKitSegments = await GetAsync<AnimKitSegment>(new DbParameter(nameof(AnimKitSegment.ParentAnimKitId), id))
            };
        }

        public async Task SaveAsync(AnimKitDto animKitDto)
        {

        }
    }
}
