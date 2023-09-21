namespace RandomizerMod.Settings
{
    public class LongLocationSettings : SettingsModule
    {
        public enum WPSetting
        {
            Allowed,
            ExcludeWhitePalace
        }

        public enum BossEssenceSetting
        {
            All,
            ExcludeAllDreamBosses,
            ExcludeAllDreamWarriors
        }

        public enum CostItemHintSettings
        {
            CostAndName,
            CostOnly,
            NameOnly,
            None
        }

        public WPSetting WhitePalaceRando;
        public BossEssenceSetting BossEssenceRando;

        public bool ColosseumPreview;
        public bool KingFragmentPreview;

        public bool FlowerQuestPreview;

        public bool WhisperingRootPreview;
        public bool DreamerPreview;

        public bool AbyssShriekPreview;
        public bool VoidHeartPreview;

        public bool LoreTabletPreview;

        public bool BasinFountainPreview;
        public bool NailmasterPreview;

        public bool StagPreview;
        public bool MapPreview;

        public CostItemHintSettings GeoShopPreview;
        public CostItemHintSettings GrubfatherPreview;
        public CostItemHintSettings SeerPreview;
        public CostItemHintSettings EggShopPreview;
    }
}
