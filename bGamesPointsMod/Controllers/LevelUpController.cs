using bGamesPointsMod.Models;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.Tools;
using StardewValley;

namespace bGamesPointsMod.Controllers
{
    internal class LevelUpController
    {
        private readonly IMonitor Monitor;
        private LevelUpModel miningSkill;
        private LevelUpModel foraningSkill;

        public LevelUpController(IMonitor monitor, LevelUpModel miningSkill, LevelUpModel foraningSkill)
        {
            Monitor = monitor;
            this.miningSkill = miningSkill;
            this.foraningSkill = foraningSkill;
        }

        private void SkillMining()
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
        private void SkillForaging()
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
    }
}
