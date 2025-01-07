using bGamesPointsMod.Models;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.Tools;
using StardewValley;

namespace bGamesPointsMod.Controllers
{
    public class LevelUpController
    {
        private readonly IMonitor Monitor;
        private readonly IModHelper Helper;
        private LevelUpModel miningSkill;
        private LevelUpModel foraningSkill;
        private LevelUpModel fishingSkill;
        private LevelUpModel combatSkill;
        private LevelUpModel luckSkill;

        public LevelUpController(
            IMonitor monitor, 
            IModHelper helper,
            LevelUpModel miningSkill, 
            LevelUpModel foraningSkill,
            LevelUpModel fishingSkill,
            LevelUpModel combatSkill,
            LevelUpModel luckSkill
            )
        {
            Monitor = monitor;
            Helper = helper;
            this.miningSkill = miningSkill;
            this.foraningSkill = foraningSkill;
            this.fishingSkill = fishingSkill;
            this.combatSkill = combatSkill;
            this.luckSkill = luckSkill;
        }

        public void SkillMining()
        {
            if (!Context.IsWorldReady || Game1.player == null)
                return;
            else
            {
                // Incrementa el nivel de minería del jugador en 200 puntos de experiencia.
                Game1.player.gainExperience(Farmer.miningSkill, miningSkill.Experience); // Se le suma la experiencia del objeto levelup

                // Muestra un mensaje en la consola del mod y en el chat del juego.
                Monitor.Log("¡Has ganado 200 puntos de experiencia en minería!", LogLevel.Info);
                Game1.addHUDMessage(new HUDMessage("¡Tu habilidad de miner@ ha incrementado!", 2));
            }
        }
        public void SkillForaging()
        {
            if (!Context.IsWorldReady || Game1.player == null)
                return;
            else
            {
                // Incrementa el nivel de forrajeo del jugador en 200 puntos de experiencia.
                Game1.player.gainExperience(Farmer.foragingSkill, foraningSkill.Experience); // Se le suma la experiencia del objeto levelup

                // Muestra un mensaje en la consola del mod y en el chat del juego.
                Monitor.Log("¡Has ganado 200 puntos de experiencia en tala!", LogLevel.Info);
                Game1.addHUDMessage(new HUDMessage("¡Tu habilidad de talad@r ha incrementado!", 2));
            }
        }
        public void SkillFishing()
        {
            if (!Context.IsWorldReady || Game1.player == null)
                return;
            else
            {
                // Incrementa el nivel de pesca del jugador en 200 puntos de experiencia.
                Game1.player.gainExperience(Farmer.fishingSkill, fishingSkill.Experience); // Se le suma la experiencia del objeto levelup

                // Muestra un mensaje en la consola del mod y en el chat del juego.
                Monitor.Log("¡Has ganado 200 puntos de experiencia en pesca!", LogLevel.Info);
                Game1.addHUDMessage(new HUDMessage("¡Tu habilidad de pesc@ ha incrementado!", 2));
            }
        }

        public void SkillCombat()
        {
            if (!Context.IsWorldReady || Game1.player == null)
                return;
            else
            {
                // Incrementa el nivel de combate del jugador en 200 puntos de experiencia.
                Game1.player.gainExperience(Farmer.combatSkill, combatSkill.Experience); // Se le suma la experiencia del objeto levelup

                // Muestra un mensaje en la consola del mod y en el chat del juego.
                Monitor.Log("¡Has ganado 200 puntos de experiencia en combate!", LogLevel.Info);
                Game1.addHUDMessage(new HUDMessage("¡Tu habilidad de combat@ ha incrementado!", 2));
            }
        }

        public void SkillLuck()
        {
            if (!Context.IsWorldReady || Game1.player == null)
                return;
            else
            {
                // Incrementa el nivel de suerte del jugador en 200 puntos de experiencia.
                Game1.player.gainExperience(Farmer.luckSkill, combatSkill.Experience); // Se le suma la experiencia del objeto levelup

                // Muestra un mensaje en la consola del mod y en el chat del juego.
                Monitor.Log("¡Has ganado 200 puntos de experiencia en suerte!", LogLevel.Info);
                Game1.addHUDMessage(new HUDMessage("¡Tu habilidad de suert@ ha incrementado!", 2));
            }
        }
    }
}
