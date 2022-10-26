using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2

{
    [HotfixesSchema]
    public class CreatureSoundData
    {
        public int Id { get; set; } = 0;
        public uint SoundExertionId { get; set; } = 0;
        public uint SoundExertionCriticalId { get; set; } = 0;
        public uint SoundInjuryId { get; set; } = 0;
        public uint SoundInjuryCriticalId { get; set; } = 0;
        public uint SoundInjuryCrushingBlowId { get; set; } = 0;
        public uint SoundDeathId { get; set; } = 0;
        public uint SoundStunId { get; set; } = 0;
        public uint SoundStandId { get; set; } = 0;
        public uint SoundFootstepId { get; set; } = 0;
        public uint SoundAggroId { get; set; } = 0;
        public uint SoundWingFlapId { get; set; } = 0;
        public uint SoundWingGlideId { get; set; } = 0;
        public uint SoundAlertId { get; set; } = 0;
        public uint SoundJumpStartId { get; set; } = 0;
        public uint SoundJumpEndId { get; set; } = 0;
        public uint SoundPetAttackId { get; set; } = 0;
        public uint SoundPetOrderId { get; set; } = 0;
        public uint SoundPetDismissId { get; set; } = 0;
        public uint LoopSoundId { get; set; } = 0;
        public uint BirthSoundId { get; set; } = 0;
        public uint SpellCastDirectedSoundId { get; set; } = 0;
        public uint SubmergeSoundId { get; set; } = 0;
        public uint SubmergedSoundId { get; set; } = 0;
        public uint WindupSoundId { get; set; } = 0;
        public uint WindupCriticalSoundId { get; set; } = 0;
        public uint ChargeSoundId { get; set; } = 0;
        public uint ChargeCriticalSoundId { get; set; } = 0;
        public uint BattleShoutSoundId { get; set; } = 0;
        public uint BattleShoutCriticalSoundId { get; set; } = 0;
        public uint TauntSoundId { get; set; } = 0;
        public uint CreatureSoundDataIDPet { get; set; } = 0;
        public decimal FidgetDelaySecondsMin { get; set; } = 0;
        public decimal FidgetDelaySecondsMax { get; set; } = 0;
        public byte CreatureImpactType { get; set; } = 0;
        public uint NPCSoundId { get; set; } = 0;
        public uint SoundFidget1 { get; set; } = 0;
        public uint SoundFidget2 { get; set; } = 0;
        public uint SoundFidget3 { get; set; } = 0;
        public uint SoundFidget4 { get; set; } = 0;
        public uint SoundFidget5 { get; set; } = 0;
        public uint CustomAttack1 { get; set; } = 0;
        public uint CustomAttack2 { get; set; } = 0;
        public uint CustomAttack3 { get; set; } = 0;
        public uint CustomAttack4 { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
