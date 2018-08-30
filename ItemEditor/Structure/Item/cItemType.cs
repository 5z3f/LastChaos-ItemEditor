using System.ComponentModel;

namespace ItemEditor.Structure.Item
{
    public enum ITYPE
    {
        ITYPE_WEAPON,
        ITYPE_WEAR,
        ITYPE_ONCE,
        ITYPE_SHOT,
        ITYPE_ETC,
        ITYPE_ACCESSORY,
        ITYPE_POTION
    }

    public enum IWEARING
    {
        // STARTS FROM -1, CALCULATIONS IN frmMain
        IWEARING_NONE,          // NO SLOT
        IWEARING_HELMET,        // HELMET
        IWEARING_ARMOR_UP,      // ARMOR
        IWEARING_WEAPON,        // WEAPON
        IWEARING_ARMOR_DOWN,    // PANTS
        IWEARING_SHIELD,        // SHIELD
        IWEARING_GLOVE,         // GLOVES
        IWEARING_BOOTS,         // BOOTS
        IWEARING_ACCESSORY1,    // ACC SLOT 1
        IWEARING_ACCESSORY2,    // ACC SLOT 2
        IWEARING_ACCESSORY3,    // ACC SLOT 3
        IWEARING_PET,           // PET SLOT
        IWEARING_BACKWING       // WINGS 
    }
    
    public enum IWEAPON
    {
        IWEAPON_NIGHT,
        IWEAPON_CROSSBOW,
        IWEAPON_STAFF,
        IWEAPON_BIGSWORD,
        IWEAPON_AXE,
        IWEAPON_SHORTSTAFF,
        IWEAPON_BOW,
        IWEAPON_SHORTGUM,
        IWEAPON_MINING,
        IWEAPON_GATHERING,
        IWEAPON_CHARGE,
        IWEAPON_TWOSWORD,
        IWEAPON_WAND,
        IWEAPON_SCYTHE,
        IWEAPON_POLEARM,
        IWEAPON_SOUL
    }
    
    public enum IWEAR
    {
        IWEAR_HELMET,
        IWEAR_ARMOR,
        IWEAR_PANTS,
        IWEAR_GLOVE,
        IWEAR_SHOES,
        IWEAR_SHIELD,
        IWEAR_BACKWING,
        IWEAR_SUIT
    }

    public enum IONCE
    {
        IONCE_WARP,
        IONCE_PROCESS_DOC,
        IONCE_MAKE_TYPE_DOC,
        IONCE_BOX,
        IONCE_MAKE_POTION_DOC,
        IONCE_CHANGE_DOC,
        IONCE_QUEST_SCROLL,
        IONCE_CASH,
        IONCE_SUMMON,
        IONCE_ETC,
        IONCE_TARGET,
        IONCE_TITLE,
        IONCE_REWARD_PACKAGE,
        IONCE_JUMPING_POTION,
        IONCE_EXTEND_CHARACTER_SLOT,
        IONCE_SERVER_TRANS,
        IONCE_REMOTE_EXPRESS,
        IONCE_JEWEL_POCKET,
        IONCE_CHAOS_JEWEL_POCKET,
        IONCE_CASH_INVENTORY,
        IONCE_PET_STASH,
        IONCE_GPS,
        IONCE_HOLY_WATER,
        IONCE_PROTECT_PVP
    }

    public enum ISHOT
    {
        ISHOT_ATKBULLET,
        ISHOT_MPBULLET,
        ISHOT_ARROW
    }

    public enum IETC
    {
        IETC_QUEST,
        IETC_EVENT,
        IETC_SKILLUP,
        IETC_UPGRADE,
        IETC_MATERIAL,
        IETC_MONEY,
        IETC_PRODUCT,
        IETC_PROCESS,
        IETC_OPTION,
        IETC_SAMPLE,
        IETC_TEXTURE,
        IETC_MIX_TYPE1,
        IETC_MIX_TYPE2,
        IETC_MIX_TYPE3,
        IETC_PET_AI,
        IETC_QUEST_TRIGGER,
        IETC_JEWEL,
        IETC_STABILIZER,
        IETC_PROCESS_SCROLL,
        IETC_MONSTER_MERCENARY_CARD,
        IETC_GUILD_MARK,
        IETC_REFORMER,
        IETC_CHAOSJEWEL,
        IETC_FUNCTIONS,
        IETC_RVR_JEWEL
    }
    
    public enum IACCESSORY
    {
        IACCESSORY_CHARM,
        IACCESSORY_MAGICSTONE,
        IACCESSORY_LIGHTSTONE,
        IACCESSORY_EARING,
        IACCESSORY_RING,
        IACCESSORY_NECKLACE,
        IACCESSORY_PET,
        IACCESSORY_ATTACK_PET,
        IACCESSORY_ARTIFACT
    }

    public enum IPOTION
    {
        IPOTION_STATE,
        IPOTION_HP,
        IPOTION_MP,
        IPOTION_DUAL,
        IPOTION_STAT,
        IPOTION_ETC,
        IPOTION_UP,
        IPOTION_TEARS,
        IPOTION_CRYSTAL,
        IPOTION_NPC_PORTAL,
        IPOTION_HP_SPEEDUP,
        IPOTION_MP_SPEEDUP,
        IPOTION_PET_HP,
        IPOTION_PET_SPEEDUP,
        IPOTION_TOTEM,
        IPOTION_PET_MP
    }
}
