using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2

{
    [HotfixesSchema]
    public class CreatureSoundData
    {
        public uint ID { get; set; } = 0;
        public uint SoundExertionID { get; set; } = 0;
        public uint SoundExertionCriticalID { get; set; } = 0;
        public uint SoundInjuryID { get; set; } = 0;
        public uint SoundInjuryCriticalID { get; set; } = 0;
        public uint SoundInjuryCrushingBlowID { get; set; } = 0;
        public uint SoundDeathID { get; set; } = 0;
        public uint SoundStunID { get; set; } = 0;
        public uint SoundStandID { get; set; } = 0;
        public uint SoundFootstepID { get; set; } = 0;
        public uint SoundAggroID { get; set; } = 0;
        public uint SoundWingFlapID { get; set; } = 0;
        public uint SoundWingGlideID { get; set; } = 0;
        public uint SoundAlertID { get; set; } = 0;
        public uint SoundJumpStartID { get; set; } = 0;
        public uint SoundJumpEndID { get; set; } = 0;
        public uint SoundPetAttackID { get; set; } = 0;
        public uint SoundPetOrderID { get; set; } = 0;
        public uint SoundPetDismissID { get; set; } = 0;
        public uint LoopSoundID { get; set; } = 0;
        public uint BirthSoundID { get; set; } = 0;
        public uint SpellCastDirectedSoundID { get; set; } = 0;
        public uint SubmergeSoundID { get; set; } = 0;
        public uint SubmergedSoundID { get; set; } = 0;
        public uint WindupSoundID { get; set; } = 0;
        public uint WindupCriticalSoundID { get; set; } = 0;
        public uint ChargeSoundID { get; set; } = 0;
        public uint ChargeCriticalSoundID { get; set; } = 0;
        public uint BattleShoutSoundID { get; set; } = 0;
        public uint BattleShoutCriticalSoundID { get; set; } = 0;
        public uint TauntSoundID { get; set; } = 0;
        public uint CreatureSoundDataIDPet { get; set; } = 0;
        public decimal FidgetDelaySecondsMin { get; set; } = 0;
        public decimal FidgetDelaySecondsMax { get; set; } = 0;
        public byte CreatureImpactType { get; set; } = 0;
        public uint NPCSoundID { get; set; } = 0;
        public uint SoundFidget0 { get; set; } = 0;
        public uint SoundFidget1 { get; set; } = 0;
        public uint SoundFidget2 { get; set; } = 0;
        public uint SoundFidget3 { get; set; } = 0;
        public uint SoundFidget4 { get; set; } = 0;
        public uint CustomAttack0 { get; set; } = 0;
        public uint CustomAttack1 { get; set; } = 0;
        public uint CustomAttack2 { get; set; } = 0;
        public uint CustomAttack3 { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
