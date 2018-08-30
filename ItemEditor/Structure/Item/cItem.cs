namespace ItemEditor.Structure.Item
{
    internal class cItem
    {
        public string[] Name, Description;

        public int[] NUM,
            SET,
            ORIGIN,
            RAREID,
            RAREPROB;

        public string SMC,
            QuestTriggerIDs,
            EffectNormal, EffectAttack, EffectDamage;

        public ulong ItemFlag, 
            JobFlag;

        public int ID,
            Enable,
            Price,
            QuestTriggerCount,
            ItemType, ItemSubType,
            Wearing,
            LevelMin, LevelMax,
            Grade,
            Durability,
            Set,
            Fame,
            RVRGrade, RVRValue,
            ZoneFlag,
            MaxUse,
            TexID, TexROW, TexCOL,
            Weight,
            CastleWar;

        public string GetNameLang(Language lang)
        {
            return Name[(int)lang];
        }

        public string GetDescrLang(Language lang)
        {
            return Description[(int)lang];
        }

        public bool changesWasHere = false;

    }

    public enum ICLASS
    {
        JOB_TITAN,
        JOB_KNIGHT,
        JOB_HEALER,
        JOB_MAGE,
        JOB_ROGUE,
        JOB_SORCERER,
        JOB_NIGHTSHADOW,
        JOB_EX_ROGUE,
        JOB_EX_MAGE,
        JOB_UNDEFINED,
        JOB_PET,
        JOB_APET,
    }
}
