using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

namespace Dur_CraftedWeaponBalance
{
    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class CraftedWeaponBalancePlugin : BaseUnityPlugin
    {
        private const string MyGUID = "durandal.CraftedWeaponBalance";
        private const string PluginName = "CraftedWeaponBalance";
        private const string VersionString = "1.0.0";

        //public static ConfigEntry<int> NumberOfComponentsRequired;
        //public static ConfigEntry<int> NumberOfComponentsOnScrapMinimum;
        //public static ConfigEntry<int> NumberOfComponentsOnScrapMaximum;

        //private const int idArdonianComponent = 46;
        //private const int idImprovedKit = 48;
        //private const int idLegendaryComponent = 80000;
        //private const int idLegendaryKit = 80001;

        private static readonly Harmony Harmony = new Harmony(MyGUID);
        public static ManualLogSource Log = new ManualLogSource(PluginName);

        private void Awake()
        {
            //NumberOfComponentsRequired = Config.Bind("General Settings", "NumberOfComponentsRequired", 8, "The number of components required to craft a kit.");
            //NumberOfComponentsOnScrapMinimum = Config.Bind("General Settings", "NumberOfComponentsOnScrapMinimum", 1, "When scrapping, recieve at least this many components.");
            //NumberOfComponentsOnScrapMaximum = Config.Bind("General Settings", "NumberOfComponentsOnScrapMaximum", 1, "When scrapping, recieve at most this many components.");

            Harmony.CreateAndPatchAll(typeof(CraftedWeaponBalancePlugin));
            Log = Logger;
        }


    }
}
