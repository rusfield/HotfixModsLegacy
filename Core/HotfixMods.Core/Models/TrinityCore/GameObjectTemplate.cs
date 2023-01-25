using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models
{
    [WorldSchema]
    public class GameobjectTemplate
    {
        [IndexField]
        public uint Entry { get; set; } = 0;
        public byte Type { get; set; } = 0;
        public uint DisplayId { get; set; } = 0;
        public string Name { get; set; } = "";
        public string IconName { get; set; } = "";
        public string CastBarCaption { get; set; } = "";
        public string Unk1 { get; set; } = "";
        public decimal Size { get; set; } = 1;
        public int Data0 { get; set; } = 0;
        public int Data1 { get; set; } = 0;
        public int Data2 { get; set; } = 0;
        public int Data3 { get; set; } = 0;
        public int Data4 { get; set; } = 0;
        public int Data5 { get; set; } = 0;
        public int Data6 { get; set; } = 0; 
        public int Data7 { get; set; } = 0;
        public int Data8 { get; set; } = 0;
        public int Data9 { get; set; } = 0;
        public int Data10 { get; set; } = 0;
        public int Data11 { get; set; } = 0;
        public int Data12 { get; set; } = 0;
        public int Data13 { get; set; } = 0;
        public int Data14 { get; set; } = 0;
        public int Data15 { get; set; } = 0;
        public int Data16 { get; set; } = 0;
        public int Data17 { get; set; } = 0;
        public int Data18 { get; set; } = 0;
        public int Data19 { get; set; } = 0;
        public int Data20 { get; set; } = 0;
        public int Data21 { get; set; } = 0;
        public int Data22 { get; set; } = 0;
        public int Data23 { get; set; } = 0;
        public int Data24 { get; set; } = 0;
        public int Data25 { get; set; } = 0;
        public int Data26 { get; set; } = 0;
        public int Data27 { get; set; } = 0;
        public int Data28 { get; set; } = 0;
        public int Data29 { get; set; } = 0;
        public int Data30 { get; set; } = 0;
        public int Data31 { get; set; } = 0;
        public int Data32 { get; set; } = 0;
        public int Data33 { get; set; } = 0;
        public int Data34 { get; set; } = 0;
        public int ContentTuningId { get; set; } = 0;
        public string AIName { get; set; } = "";
        public string ScriptName { get; set; } = "";
        public int VerifiedBuild { get; set; } = -1;
    }

}
