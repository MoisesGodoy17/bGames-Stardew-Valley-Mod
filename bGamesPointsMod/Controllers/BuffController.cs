using bGamesPointsMod.Models;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Tools;


namespace bGamesPointsMod.Controllers;
public class BuffController
{
    private readonly IMonitor Monitor;
    private readonly IModHelper Helper;
    private BuffModel miningBuff;
    private BuffModel foraningBuff;
    private BuffModel speedBuff;
    public int flagBuff = 0;
    public int originalSpeed;

    public BuffController(
             IMonitor monitor,
             IModHelper helper,  //Helper
             BuffModel miningBuff,
             BuffModel foraningBuff,
             BuffModel speedBuff)
    {
        this.Monitor = monitor;
        this.Helper = helper;  //helper
        this.miningBuff = miningBuff;
        this.foraningBuff = foraningBuff;
        this.speedBuff = speedBuff;
    }

    public void OnButtonPressedMiningSpeed(object sender, ButtonPressedEventArgs e)
    {
        if (!Context.IsWorldReady && flagBuff == 1) return;

        if (flagBuff == 0 && miningBuff.TickBuff == -1) // si se preciona la tecla, no hay ningun buff usandose y el tick no esta corriendo, entoces se pued activar el buff
        {
            miningBuff.TickBuff = 900;
            flagBuff = 1; // Activar flag
            Monitor.Log("Habilidad activada: Velocidad de minado aumentada.", LogLevel.Info);
        }
    }

    public void OnUpdateTickedMiningSpeed(object sender, UpdateTickedEventArgs e)
    {
         if (miningBuff.TickBuff > 0 && flagBuff == 1)
         {
            if (Game1.player.CurrentTool is Pickaxe pickaxe)
            {
                pickaxe.AnimationSpeedModifier = 0.75f;
            }
            miningBuff.TickBuff--;
         }
         if (miningBuff.TickBuff == 0)
         {
            if (Game1.player.CurrentTool is Pickaxe pickaxeTool)
            {
                pickaxeTool.AnimationSpeedModifier = 1.0f;
            }
            foreach (Item item in Game1.player.Items)
            {
                if (item is Pickaxe pickaxeItem)
                {
                    pickaxeItem.AnimationSpeedModifier = 1.0f;
                }
            }
            flagBuff = 0; // Restablecer flag
            miningBuff.TickBuff = -1;

            SleepBuff(); // Desvincula el evento de tick cuando el buff termina
            Monitor.Log("Habilidad desactivada: La velocidad de minado ha vuelto a la normalidad.", LogLevel.Info);
         }
    }

    public void OnButtonPressedForagingSpeed(object sender, ButtonPressedEventArgs e)
    {
        if (!Context.IsWorldReady || flagBuff == 1) return;

        if (flagBuff == 0 && foraningBuff.TickBuff == -1)
        {
            foraningBuff.TickBuff = 900;
            flagBuff = 1;
            Monitor.Log("Habilidad activada: Velocidad de tala aumentada.", LogLevel.Info);
        }
    }

    public void OnUpdateTickedForagingSpeed(object sender, UpdateTickedEventArgs e)
    {
        if (foraningBuff.TickBuff > 0)
        {
            if (Game1.player.CurrentTool is Axe axe)
            {
                axe.AnimationSpeedModifier = 0.5f; // La velocidad va desde 0.0 hasta 1.0. 0.0 muy rapido y 1.0 normal, mas de 1.0 es lento, menos que 0.0 se mentiene igual
            }
            foraningBuff.TickBuff--;
        }
        if (foraningBuff.TickBuff == 0 && flagBuff == 1)
        {
           if (Game1.player.CurrentTool is Axe axe)
           {
                axe.AnimationSpeedModifier = 1.0f;
           }
            flagBuff = 0;
            foraningBuff.TickBuff = -1;
            SleepBuff();
            Monitor.Log("Habilidad desactivada: La velocidad de tala ha vuelto a la normalidad.", LogLevel.Info);
        }
    }

    public void OnButtonPressedSpeed(object sender, ButtonPressedEventArgs e)
    {
        if (!Context.IsWorldReady || flagBuff == 1) return;

        if (flagBuff== 0 && speedBuff.TickBuff == -1)
        {
            speedBuff.TickBuff = 900;
            flagBuff = 1;
            originalSpeed = Game1.player.speed;
            Monitor.Log("Habilidad activada: Velocidad incrementada por 10 segundos.", LogLevel.Info);
        }
    }

    public void OnUpdateTickedSpeed(object sender, UpdateTickedEventArgs e)
    {
        if (speedBuff.TickBuff > 0)
        {
            Game1.player.speed = 10;
            speedBuff.TickBuff--;
        }
        if (speedBuff.TickBuff == 0)
        {
            Game1.player.speed = originalSpeed;
            speedBuff.TickBuff = -1;
            flagBuff = 0;
            SleepBuff();
            Monitor.Log("Habilidad desactivada. La velocidad a vuelto a la normalidad.", LogLevel.Info);
        }
    }
    public void SleepBuff() // funcion que desvincula los buff que se han clickado mientras un buff esta activo
    {
        this.Helper.Events.Input.ButtonPressed -= OnButtonPressedMiningSpeed;
        this.Helper.Events.GameLoop.UpdateTicked -= OnUpdateTickedMiningSpeed;
        this.Helper.Events.Input.ButtonPressed -= OnButtonPressedForagingSpeed;
        this.Helper.Events.GameLoop.UpdateTicked -= OnUpdateTickedForagingSpeed;
        this.Helper.Events.Input.ButtonPressed -= OnButtonPressedSpeed;
        this.Helper.Events.GameLoop.UpdateTicked -= OnUpdateTickedSpeed;
    }
}