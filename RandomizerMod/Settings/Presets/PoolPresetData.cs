﻿namespace RandomizerMod.Settings.Presets
{
    public static class PoolPresetData
    {
        public static PoolSettings Standard;
        public static PoolSettings Super;
        public static PoolSettings LifeTotems;
        public static PoolSettings SpoilerDAB;
        public static PoolSettings Everything;
        public static PoolSettings Vanilla;
        public static Dictionary<string, PoolSettings> PoolPresets;

        static PoolPresetData()
        {
            Standard = new PoolSettings
            {
                Dreamers = true,
                Skills = true,
                Charms = true,
                Keys = true,
                MaskShards = true,
                VesselFragments = true,
                PaleOre = true,
                CharmNotches = true,
                GeoChests = true,
                Relics = true,
                RancidEggs = true,
                Stags = true,
                Maps = false,
                WhisperingRoots = false,
                Grubs = false,
                LifebloodCocoons = false,
                SoulTotems = false,
                GeoRocks = false,
                BossEssence = false,
                BossGeo = false,
                LoreTablets = false,
                JournalEntries = false,
            };

            Super = new PoolSettings
            {
                Dreamers = true,
                Skills = true,
                Charms = true,
                Keys = true,
                MaskShards = true,
                VesselFragments = true,
                PaleOre = true,
                CharmNotches = true,
                GeoChests = true,
                Relics = true,
                RancidEggs = true,
                Stags = true,
                Maps = true,
                WhisperingRoots = true,
                Grubs = true,
                LifebloodCocoons = false,
                SoulTotems = false,
                GeoRocks = false,
                BossEssence = false,
                BossGeo = false,
                LoreTablets = false,
                JournalEntries = false,
            };

            LifeTotems = new PoolSettings
            {
                Dreamers = true,
                Skills = true,
                Charms = true,
                Keys = true,
                MaskShards = true,
                VesselFragments = true,
                PaleOre = true,
                CharmNotches = true,
                GeoChests = true,
                Relics = true,
                RancidEggs = true,
                Stags = true,
                Maps = false,
                WhisperingRoots = false,
                Grubs = false,
                LifebloodCocoons = true,
                SoulTotems = true,
                GeoRocks = false,
                BossEssence = false,
                BossGeo = true,
                LoreTablets = false,
                JournalEntries = false,
            };

            SpoilerDAB = new PoolSettings
            {
                Dreamers = true,
                Skills = true,
                Charms = true,
                Keys = true,
                MaskShards = true,
                VesselFragments = true,
                PaleOre = true,
                CharmNotches = true,
                GeoChests = true,
                Relics = true,
                RancidEggs = true,
                Stags = true,
                Maps = true,
                WhisperingRoots = true,
                Grubs = false,
                LifebloodCocoons = true,
                SoulTotems = true,
                GeoRocks = false,
                BossEssence = false,
                BossGeo = false,
                LoreTablets = false,
                JournalEntries = false,
            };

            Everything = new PoolSettings
            {
                Dreamers = true,
                Skills = true,
                Charms = true,
                Keys = true,
                MaskShards = true,
                VesselFragments = true,
                PaleOre = true,
                CharmNotches = true,
                GeoChests = true,
                Relics = true,
                RancidEggs = true,
                Stags = true,
                Maps = true,
                WhisperingRoots = true,
                Grubs = true,
                LifebloodCocoons = true,
                SoulTotems = true,
                GeoRocks = true,
                BossEssence = true,
                BossGeo = true,
                LoreTablets = true,
                JournalEntries = true,
            };

            Vanilla = new PoolSettings
            {
                Dreamers = false,
                Skills = false,
                Charms = false,
                Keys = false,
                MaskShards = false,
                VesselFragments = false,
                PaleOre = false,
                CharmNotches = false,
                GeoChests = false,
                Relics = false,
                RancidEggs = false,
                Stags = false,
                Maps = false,
                WhisperingRoots = false,
                Grubs = false,
                LifebloodCocoons = false,
                SoulTotems = false,
                GeoRocks = false,
                BossEssence = false,
                BossGeo = false,
                LoreTablets = false,
                JournalEntries = false,
            };

            PoolPresets = new Dictionary<string, PoolSettings>
            {
                { "Standard", Standard },
                { "Super", Super },
                { "LifeTotems", LifeTotems },
                { "Spoiler DAB", SpoilerDAB },
                { "EVERYTHING", Everything },
                { "Vanilla", Vanilla },
            };
        }
    }
}
