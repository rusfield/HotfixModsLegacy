using HotfixMods.Core.Models.Db2;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class SpellVisualKitDto : BaseDto
    {
        public SpellVisualKitDto() : base(nameof(SpellVisualKit)) { }

        public SpellVisualKit SpellVisualKit { get; set; } = new();
        public List<EffectGroup> EffectGroups { get; set; } = new();

        public class EffectGroup
        {
            public SpellVisualKitEffect SpellVisualKitEffect { get; set; } = new();
            public SpellVisualEffectName SpellVisualEffectName { get; set; } = new();

            // Effect 1
            public SpellProceduralEffect SpellProceduralEffect { get; set; } = new();
            // Effect 2
            public SpellVisualKitModelAttach SpellVisualKitModelAttach { get; set; } = new();
            // Effect 3 and 4
            public CameraEffect CameraEffect { get; set; } = new();
            // Effect 6
            public SpellVisualAnim SpellVisualAnim { get; set; } = new();
            // Effect 7
            public ShadowyEffect ShadowyEffect { get; set; } = new();
            // Effect 8
            public SpellEffectEmission SpellEffectEmission { get; set; } = new();
            // Effect 9
            public OutlineEffect OutlineEffect { get; set; } = new();
            // Effect 11
            public DissolveEffect DissolveEffect { get; set; } = new();
            // Effect 12
            public EdgeGlowEffect EdgeGlowEffect { get; set; } = new();
            // Effect 13
            public BeamEffect BeamEffect { get; set; } = new();
            // Effect 14
            public ClientSceneEffect ClientSceneEffect { get; set; } = new();
            // Effect 15
            public CloneEffect CloneEffect { get; set; } = new();
            // Effect 16
            public GradientEffect GradientEffect { get; set; } = new();
            // Effect 17
            public BarrageEffect BarrageEffect { get; set; } = new();
            // Effect 18
            public RopeEffect RopeEffect { get; set; } = new();
            // Effect 19
            public SpellVisualScreenEffect SpellVisualScreenEffect { get; set; } = new();
        }
    }
}
