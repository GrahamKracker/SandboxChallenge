using System.Collections.Generic;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api.Scenarios;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Difficulty;
using Il2CppAssets.Scripts.Models.Gameplay.Mods;
using Il2CppAssets.Scripts.Models.Rounds;
using UnityEngine;

namespace SandboxChallenge;

public class SandboxChallengeGameMode : ModGameMode
{
    public override string DisplayName => "Sandbox Challenge";

    public override void ModifyBaseGameModeModel(ModModel gameModeModel)
    {        
        gameModeModel.AddMutator(new ChimpsModModel("ChimpsModModel_"));
        gameModeModel.AddMutator(new EndRoundModModel("EndRoundModModel_", 100));
        gameModeModel.AddMutator(new ModifyAllCashModModel("ModifyAllCashModModel_", 0));
        gameModeModel.AddMutator(new LockTowerModModel("LockTowerModModel_", "BananaFarm"));
    }

    public string[] Bosses = {
        "DreadbloonElite5",
        "LychElite5",
        "PhayzeElite5", 
        "VortexElite5",
        "BloonariusElite5"
    };
    
    public override void ModifyGameModel(GameModel gameModel)
    {
        foreach(var upgrade in gameModel.upgrades)
        {
            upgrade.cost *= 5;
        }
        
        foreach (var bloonModel in gameModel.bloons)
        {
            if (bloonModel.IsRock || bloonModel.isInvulnerable || bloonModel.baseId == "MiniLych" || bloonModel.baseId == "MiniLychElite")
            {
                continue;
            }

            if (bloonModel.isBoss)
            {
                bloonModel.leakDamage = gameModel.maxHealth + gameModel.maxShield;
                bloonModel.totalLeakDamage = bloonModel.leakDamage.ToIl2Cpp().Cast<Il2CppSystem.Nullable<float>>();
                continue;
            }

            bloonModel.speed *= 7.5f;
            bloonModel.distributeDamageToChildren = false;
            bloonModel.maxHealth *= 100;
            bloonModel.leakDamage *= 10_000;
            bloonModel.totalLeakDamage = bloonModel.leakDamage.ToIl2Cpp().Cast<Il2CppSystem.Nullable<float>>();
        }

        for (var i = 0; i < gameModel.roundSet.rounds.Count; i++)
        {
            var roundModel = gameModel.roundSet.rounds[i];
            roundModel.groups.ForEach(groupModel =>
            {
                const int groupMultiplier = 10;
                groupModel.count *= groupMultiplier;
            });
            
            List<string> completedBosses = new List<string>();
            
            if (i > 0 && i % 20 == 0)
            {
                var boss = Bosses[Random.Range(0, Bosses.Length)];
                while (completedBosses.Contains(boss))
                {
                    boss = Bosses[Random.Range(0, Bosses.Length)];
                }
                roundModel.groups = roundModel.groups.AddTo(new BloonGroupModel("BloonGroupModel_",
                    boss, 0, 0, 1));
            }          
            roundModel.emissions_ = null;
        }
    }

    public override string Icon => VanillaSprites.SandboxBtn;

    public override string Difficulty => DifficultyType.Medium;
    public override string BaseGameMode => GameModeType.Sandbox;
}