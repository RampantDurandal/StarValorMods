using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

namespace Dur_LegendaryUpgradeKits
{
    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class LegendaryUpgradeKitsPlugin : BaseUnityPlugin
    {
        private const string MyGUID = "durandal.LegendaryUpgradeKits";
        private const string PluginName = "Legendary Upgrade Kits";
        private const string VersionString = "1.0.0";

        public static ConfigEntry<int> NumberOfComponentsRequired;
        public static ConfigEntry<int> NumberOfComponentsOnScrapMinimum;
        public static ConfigEntry<int> NumberOfComponentsOnScrapMaximum;

        private const int idArdonianComponent = 46;
        private const int idImprovedKit = 48;
        private const int idLegendaryComponent = 80000;
        private const int idLegendaryKit = 80001;

        private const string nameLegendaryComponent = "Legendary Component";
        private const string nameLegendaryKit = "Legendary Upgrade Kit";
        private const string descLegendaryKit = "Used to upgrade equipment up to gold tier.\nSelect an equipment and the upgrade button will appear. This item is consumed on usage.";

        private static readonly Harmony Harmony = new Harmony(MyGUID);
        public static ManualLogSource Log = new ManualLogSource(PluginName);

        private void Awake()
        {
            NumberOfComponentsRequired = Config.Bind("General Settings", "NumberOfComponentsRequired", 8, "The number of components required to craft a kit.");
            NumberOfComponentsOnScrapMinimum = Config.Bind("General Settings", "NumberOfComponentsOnScrapMinimum", 1, "When scrapping, recieve at least this many components.");
            NumberOfComponentsOnScrapMaximum = Config.Bind("General Settings", "NumberOfComponentsOnScrapMaximum", 1, "When scrapping, recieve at most this many components.");

            Harmony.CreateAndPatchAll(typeof(LegendaryUpgradeKitsPlugin));
            Log = Logger;
        }

        [HarmonyPatch(typeof(ItemDB), "LoadDatabaseForce")]
        [HarmonyPostfix]
        private static void ItemDBLoadDBForce_Post()
        {
            FieldInfo fieldInfo = AccessTools.Field(typeof(ItemDB), "items");
            List<Item> list = (List<Item>)fieldInfo.GetValue(null);

            // Create and add the new component item
            Item originalComponentItem = ItemDB.GetItem(idArdonianComponent);
            Item copiedComponentItem = UnityEngine.Object.Instantiate<Item>(originalComponentItem);
            copiedComponentItem.id = idLegendaryComponent;
            copiedComponentItem.name = idLegendaryComponent + "." + nameLegendaryComponent;
            copiedComponentItem.refName = nameLegendaryComponent;
            copiedComponentItem.itemName = nameLegendaryComponent;
            copiedComponentItem.rarity = 5;
            copiedComponentItem.basePrice = 2000f;
            copiedComponentItem.randomDrop = false;
            copiedComponentItem.craftable = false;
            copiedComponentItem.craftingYield = 0;
            copiedComponentItem.teachItemBlueprints = new int[]{idLegendaryKit};
            list.Add(copiedComponentItem);

            // Create and add the new upgrade kit item
            Item originalKitItem = ItemDB.GetItem(idImprovedKit);
            Item copiedKitItem = UnityEngine.Object.Instantiate<Item>(originalKitItem);
            copiedKitItem.id = idLegendaryKit;
            copiedKitItem.name = idLegendaryKit + "." + nameLegendaryKit;
            copiedKitItem.refName = nameLegendaryKit;
            copiedKitItem.itemName = nameLegendaryKit;
            copiedKitItem.description = descLegendaryKit;
            copiedKitItem.rarity = 5;
            copiedKitItem.levelPlus = -150;
            copiedKitItem.canUpgradeToTier = ItemRarity.Legendary_5;
            copiedKitItem.basePrice = 20000f;
            copiedKitItem.randomDrop = false;
            copiedKitItem.craftingMaterials = new List<CraftMaterial>
            {
                new CraftMaterial
                {
                    itemID = idLegendaryComponent,
                    quantity = NumberOfComponentsRequired.Value
                }
            };
            list.Add(copiedKitItem);
        }

        [HarmonyPatch(typeof(Inventory), nameof(Inventory.ScrapItem))]
        [HarmonyPrefix]
        private static void ScrapItem_Pre(Inventory __instance)
        {
            int selectedItem = (int)typeof(Inventory).GetField("selectedItem", AccessTools.all).GetValue(__instance);
            if (selectedItem == -1)
                return;

            CargoSystem cargoSystem = PlayerControl.inst.GetCargoSystem;
            CargoItem cargoItem = cargoSystem.cargo[selectedItem];
            if (cargoItem.itemType > 2)
                return;

            if (cargoItem.rarity == (int)ItemRarity.Legendary_5)
            {
                int quantity = Random.Range(NumberOfComponentsOnScrapMinimum.Value, NumberOfComponentsOnScrapMaximum.Value);
                Store(cargoSystem, idLegendaryComponent, quantity);
            }
        }

        private static void Store(CargoSystem cs, int id, int qnt)
        {
            cs.StoreItem(3, id, 1, qnt, 0, -1, -1);
            string str = ((int)qnt > 1) ? ("(" + (int)qnt + ") ") : "";
            SideInfo.AddMsg(Lang.Get(6, 18, str + ItemDB.GetItemNameModified(id, 2)));
        }
    }
}