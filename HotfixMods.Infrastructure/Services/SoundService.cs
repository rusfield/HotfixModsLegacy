﻿using HotfixMods.Core.Models;
using HotfixMods.Core.Providers;
using HotfixMods.Infrastructure.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Services
{
    public partial class SoundService : Service
    {
        public SoundService(IDb2Provider db2Provider, IMySqlProvider mySqlProvider) : base(db2Provider, mySqlProvider)  {   }

        public async Task SaveSoundAsync(SoundDto sound)
        {
            if(sound.FileDataIds.Count > 10)
            {
                /*
                 * Adding more than 10 SoundKitEntries will cause conflicts with the next SoundKit.
                 * If this number is increased, you need to make appropriate changes everywhere in code.
                 */
                throw new Exception($"SoundKit should not have more than 10 SoundKitEntries (aka FileDataIds).");
            }
                
            var hotfixId = await GetNextHotfixIdAsync();
            sound.InitHotfixes(hotfixId, VerifiedBuild);

            if (sound.IsUpdate)
            {
                // TODO
            }
            await _mySql.AddAsync(BuildSoundKit(sound));
            await _mySql.AddManyAsync(BuildSoundKitEntry(sound));

            await _mySql.AddManyAsync(sound.GetHotfixes());
        }
    }
}
