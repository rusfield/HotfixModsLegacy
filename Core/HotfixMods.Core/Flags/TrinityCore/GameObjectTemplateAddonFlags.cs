namespace HotfixMods.Core.Flags.TrinityCore
{
    [Flags]
    public enum GameObjectTemplateAddonFlags : long
    {
        NONE = 0,
        IN_USE = 1,
        LOCKED = 2,
        INTERACT_COND = 4,
        TRANSPORT = 8,
        NOT_SELECTABLE = 16,
        NO_DESPAWN = 32,
        TRIGGERED = 64,
        DAMAGED = 512,
        DESTROYED = 1024
    }
}
