using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace CraftedWeaponSize
{
    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class CraftedWeaponSizePlugin : BaseUnityPlugin
    {
        private const string MyGUID = "durandal.CraftedWeaponSize";
        private const string PluginName = "Crafted Weapon Size";
        private const string VersionString = "1.0.0";

        private static readonly Harmony Harmony = new Harmony(MyGUID);
        public static ManualLogSource Log = new ManualLogSource(PluginName);

        private void Awake()
        {
            Harmony.CreateAndPatchAll(typeof(CraftedWeaponSizePlugin));
            Log = Logger;
        }
        
        [HarmonyPatch(typeof(Weapon), "Load")]
        [HarmonyPostfix]
        public static void EnlargeCraftedWeapon(Weapon __instance)
        {
            if (__instance.wRef.isCrafted && __instance.wRef.space >= 4f)
            {
                //Log.LogInfo("Postfix Weapon::Load for" + __instance.name);
                //Log.LogInfo("Weapon was size " + __instance.sizeMod);
                int multiplesOfFour = (int)__instance.wRef.space / 4;
                __instance.sizeMod *= 1f + (float)multiplesOfFour * 0.5f;
                //Log.LogInfo("Weapon is now size " + __instance.sizeMod);
            }
        }

        [HarmonyPatch(typeof(BeamWeapon), "Load")]
        [HarmonyPostfix]
        public static void EnlargeCraftedBeamWeapon(BeamWeapon __instance, ref float ___beamWidthMin, ref float ___beamWidthMax, ref float ___flashSize)
        {
            if (__instance.wRef.isCrafted && __instance.wRef.space >= 4f)
            {
                //Log.LogInfo("Postfix BeamWeapon::Load for" + __instance.name);
                //Log.LogInfo("BeamWeapon was size " + __instance.sizeMod);
                //Log.LogInfo("BeamWeapon widthMin was size " + ___beamWidthMin);
                //Log.LogInfo("BeamWeapon widthMax was size " + ___beamWidthMax);
                //Log.LogInfo("BeamWeapon flash was size " + ___flashSize);
                float mult = 1.25f;
                if (__instance.wRef.space >= 8f)
                {
                    mult += 0.2f;
                }
                if (__instance.wRef.space >= 12f)
                {
                    mult += 0.15f;
                }
                __instance.sizeMod *= mult;
                ___beamWidthMin *= mult;
                ___beamWidthMax *= mult;
                ___flashSize *= mult;
                if (___flashSize > 10f)
                {
                    //Log.LogInfo("BeamWeapon flash hit the cap");
                    ___flashSize = 10f;
                }
                //Log.LogInfo("BeamWeapon is now size " + __instance.sizeMod);
                //Log.LogInfo("BeamWeapon widthMin is now size " + ___beamWidthMin);
                //Log.LogInfo("BeamWeapon widthMax is now size " + ___beamWidthMax);
                //Log.LogInfo("BeamWeapon flash is now size " + ___flashSize);
            }
        }
    }
}
