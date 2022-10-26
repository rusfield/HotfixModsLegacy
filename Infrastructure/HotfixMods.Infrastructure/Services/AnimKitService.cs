﻿using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Infrastructure.Config;
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
        public AnimKitService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, AppConfig appConfig) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, appConfig) { }

        public async Task<AnimKitDto> GetNewAsync()
        {
            var result = new AnimKitDto();
            result.AnimKit.Id = await GetNextIdAsync<AnimKit>();

            return result;
        }

        public async Task<AnimKitDto?> GetByIdAsync(int id, Action<string, string, int>? callback = null)
        {
            var animKit = await GetSingleByIdAsync<AnimKit>(id);
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
            await SaveAsync(animKitDto.AnimKit);
            await SaveAsync(animKitDto.AnimKitSegments.ToArray());
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteAsync<AnimKit>(new DbParameter(nameof(AnimKit.Id), id));
            await DeleteAsync<AnimKitSegment>(new DbParameter(nameof(AnimKitSegment.ParentAnimKitId), id));
        }
    }
}
