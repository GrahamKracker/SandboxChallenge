using SandboxChallenge;

[assembly: MelonInfo(typeof(SandboxChallenge.Main), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace SandboxChallenge;

[HarmonyPatch]
public class Main : BloonsTD6Mod
{
    internal static MelonLogger.Instance Logger;

    public override void OnInitialize()
    {
        Logger = LoggerInstance;
    }
}