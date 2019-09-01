using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using UnityEngine;
using VRCModLoader;
using VRCTools;
using Harmony;

namespace DisablePortals
{
    public static class ModInfo
    {
        public const string Name = "DisablePortals";
        public const string Author = "Herp Derpinstine, yoshifan";
        public const string Company = "NanoNuke @ nanonuke.net";
        public const string Version = "1.0.0";
    }
    [VRCModInfo(ModInfo.Name, ModInfo.Version, ModInfo.Author)]

    public class DisablePortals : VRCMod
    {
        void OnApplicationStart()
        {
            VRCTools.ModPrefs.RegisterCategory("disableportals", ModInfo.Name);
            VRCTools.ModPrefs.RegisterPrefBool("disableportals", "disabled", false, "Disabled");
            VRCTools.ModPrefs.SetBool("disableportals", "disabled", false);

            HarmonyInstance harmonyInstance = HarmonyInstance.Create("portalpatch");
            harmonyInstance.Patch(typeof(PortalInternal).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).First<MethodInfo>(x => ("EnterWorld".Equals(x.Name) && (x.GetParameters().Length == 0))), new HarmonyMethod(typeof(DisablePortals).GetMethod("EnterWorld", BindingFlags.Static | BindingFlags.NonPublic)), null, null);
            harmonyInstance.Patch(typeof(PortalInternal).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).First<MethodInfo>(x => ("EnterWorld".Equals(x.Name) && (x.GetParameters().Length == 2))), new HarmonyMethod(typeof(DisablePortals).GetMethod("EnterWorld", BindingFlags.Static | BindingFlags.NonPublic)), null, null);
        }

        private static bool EnterWorld()
        {
            if (VRCTools.ModPrefs.GetBool("disableportals", "disabled"))
                return false;
            return true;
        }
    }
}
