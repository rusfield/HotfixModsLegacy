using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2

{
    [HotfixesSchema]
    public class CreatureSoundData
    {
        public int Id { get; set; }
        public uint SoundExertionId { get; set; }
        public uint SoundExertionCriticalId { get; set; }
        public uint SoundInjuryId { get; set; }
        public uint SoundInjuryCriticalId { get; set; }
        public uint SoundInjuryCrushingBlowId { get; set; }
        public uint SoundDeathId { get; set; }
        public uint SoundStunId { get; set; }
        public uint SoundStandId { get; set; }
        public uint SoundFootstepId { get; set; }
        public uint SoundAggroId { get; set; }
        public uint SoundWingFlapId { get; set; }
        public uint SoundWingGlideId { get; set; }
        public uint SoundAlertId { get; set; }
        public uint SoundJumpStartId { get; set; }
        public uint SoundJumpEndId { get; set; }
        public uint SoundPetAttackId { get; set; }
        public uint SoundPetOrderId { get; set; }
        public uint SoundPetDismissId { get; set; }
        public uint LoopSoundId { get; set; }
        public uint BirthSoundId { get; set; }
        public uint SpellCastDirectedSoundId { get; set; }
        public uint SubmergeSoundId { get; set; }
        public uint SubmergedSoundId { get; set; }
        public uint WindupSoundId { get; set; }
        public uint WindupCriticalSoundId { get; set; }
        public uint ChargeSoundId { get; set; }
        public uint ChargeCriticalSoundId { get; set; }
        public uint BattleShoutSoundId { get; set; }
        public uint BattleShoutCriticalSoundId { get; set; }
        public uint TauntSoundId { get; set; }
        public uint CreatureSoundDataIDPet { get; set; }
        public decimal FidgetDelaySecondsMin { get; set; }
        public decimal FidgetDelaySecondsMax { get; set; }
        public byte CreatureImpactType { get; set; }
        public uint NPCSoundId { get; set; }
        public uint SoundFidget1 { get; set; }
        public uint SoundFidget2 { get; set; }
        public uint SoundFidget3 { get; set; }
        public uint SoundFidget4 { get; set; }
        public uint SoundFidget5 { get; set; }
        public uint CustomAttack1 { get; set; }
        public uint CustomAttack2 { get; set; }
        public uint CustomAttack3 { get; set; }
        public uint CustomAttack4 { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
