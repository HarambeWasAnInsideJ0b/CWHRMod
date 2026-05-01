using BepInEx;
using HarmonyLib;
using UnityEngine;

[BepInPlugin("gorilla.carrierwarheadrestoration", "Carrier Warhead Restoration", "1.0.0")]
public class Plugin : BaseUnityPlugin
{
    private void Awake()
    {
        Harmony harmony = new Harmony("gorilla.carrierwarheadrestoration");
        Logger.LogInfo("Carrier Warhead Restoration loaded");
        new Harmony("gorilla.carrierwarheadrestoration").PatchAll();
    }
}

[HarmonyPatch(typeof(Airbase), "OnStartServer")]
class Patch_Airbase_OnStartServer
{
    static void Postfix(Airbase __instance)
    {
        FixStorage(__instance);
    }

    static void FixStorage(Airbase airbase)
    {
        if (!airbase.AttachedAirbase)
            return;

        if (!airbase.TryGetAttachedUnit(out Unit unit))
            return;

        var storages = unit.GetComponentsInChildren<WarheadStorage>(true);

        foreach (var storage in storages)
        {
            airbase.AddStorage(storage);
        }
    }
}

[HarmonyPatch(typeof(Airbase), "OnStartClientOnly")]
class Patch_Airbase_OnStartClientOnly
{
    static void Postfix(Airbase __instance)
    {
        if (!__instance.AttachedAirbase)
            return;

        if (!__instance.TryGetAttachedUnit(out Unit unit))
            return;

        var storages = unit.GetComponentsInChildren<WarheadStorage>(true);

        foreach (var storage in storages)
        {
            __instance.AddStorage(storage);
        }
    }
}