using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Buffs;

namespace bGamesPointsMod.Controllers;
public class BuffController
{
    private readonly IMonitor Monitor;
    private readonly IModHelper Helper;
    public Buff miningBuff;
    public Buff foraningBuff;
    public Buff speedBuff;
    public Buff reducedEnergyBuff;
    public Buff luckLevelBuff;
    public Buff fishingBuff;
    public Buff farmingBuff;
    private float lastStamina; // Para rastrear cambios en la energía
    private float originalZoomLevel; // Almacena el nivel de zoom original

    public BuffController(
             IMonitor monitor,
             IModHelper helper,  //Helper
             Buff miningBuff,
             Buff foragingBuff,
             Buff speedBuff,
             Buff reducedEnergyBuff,
             Buff luckLevelBuff,
             Buff fishingBuff,
             Buff farmingBuff)
    {
        this.Monitor = monitor;
        this.Helper = helper;  //helper
        this.miningBuff = miningBuff;
        this.foraningBuff = foragingBuff;
        this.speedBuff = speedBuff;
        this.reducedEnergyBuff = reducedEnergyBuff;
        this.luckLevelBuff = luckLevelBuff;
        this.fishingBuff = fishingBuff;
        this.farmingBuff = farmingBuff;
    }

    public void BuffSpeed()
    {
        Buff speedBuff = new Buff(id: "Speedbuff",
                displayName: "Speed foraning buff",
                iconTexture: null,
                iconSheetIndex: 0,
                duration: 10_000, // 10 segundos
                effects: new BuffEffects()
                {
                    Speed = { 5 }
                }
                );
        Game1.player.applyBuff(speedBuff);
    }

    public void BuffForaging()
    {
        Buff foraningBuff = new Buff(id: "Foraningspeed",
                displayName: "Speed foraning buff",
                iconTexture: null,
                iconSheetIndex: 0,
                duration: 10_000, // 10 segundos
                effects: new BuffEffects()
                {
                    ForagingLevel = { 5 }
                });
        Game1.player.applyBuff(foraningBuff);
    }

    public void BuffMining()
    {
        Buff miningBuff = new Buff(id: "Miningspeed",
                displayName: "Speed mining buff",
                iconTexture: null,
                iconSheetIndex: 0,
                duration: 10_000, // 10 segundos
                effects: new BuffEffects()
                {
                    MiningLevel = { 5 }
                });
        Game1.player.applyBuff(miningBuff);
    }

    public void ReducedEnergyBuff()
    {
        Buff reducedEnergyBuff = new Buff(id: "ReducedEnergy",
            displayName: "Speed mining buff",
            iconTexture: null,
            iconSheetIndex: 0,
            duration: 10_000 // 10 segundos
        );

        Game1.player.applyBuff(reducedEnergyBuff);

        this.Monitor.Log("Buff aplicado: Consumo de energía reducido a la mitad.", LogLevel.Info);
    }

    public void BuffLuckLevel()
    {
        Buff luckLevelBuff = new Buff(id: "Lucklevelup",
                displayName: "Luck level up",
                iconTexture: null,
                iconSheetIndex: 0,
                duration: 10_000, // 10 segundos
                effects: new BuffEffects()
                {
                    LuckLevel = { 2 }
                });
        Game1.player.applyBuff(luckLevelBuff);
    }

    public void BuffFishing()
    {
        Buff fishingBuff = new Buff(id: "Fishingspeed",
                displayName: "Speed Fishing buff",
                iconTexture: null,
                iconSheetIndex: 0,
                duration: 10_000, // 10 segundos
                effects: new BuffEffects()
                {
                    FishingLevel = { 5 }
                });
        Game1.player.applyBuff(fishingBuff);
    }

    public void BuffFarming()
    {
        Buff farmingBuff = new Buff(id: "Farmingspeed",
                displayName: "Speed farming buff",
                iconTexture: null,
                iconSheetIndex: 0,
                duration: 10_000, // 10 segundos
                effects: new BuffEffects()
                {
                    FarmingLevel = { 5 }
                });
        Game1.player.applyBuff(farmingBuff);
    }

    public void OnUpdateTickedReducedEnergyBuff(object sender, UpdateTickedEventArgs e)
    {
        float currentStamina = Game1.player.Stamina;
        // Verificar si la energía ha disminuido (indica una acción)
        if (currentStamina < lastStamina)
        {
            float energyUsed = lastStamina - currentStamina;
            // Reducir el consumo de energía a la mitad
            float reducedEnergy = energyUsed / 2;
            // Ajustar la energía del jugador
            Game1.player.Stamina += reducedEnergy;
            this.Monitor.Log($"Acción realizada: Consumo de energía reducido de {energyUsed} a {energyUsed - reducedEnergy}.", LogLevel.Debug);
        }
        lastStamina = currentStamina; // Actualizar la energía anterior para el siguiente tick
        // Verificar si el buff ha expirado
        if (IsActiveReducedEnergyBuff == false) // Si el buff ha expirado desvincula el evento
        {
            this.Helper.Events.GameLoop.UpdateTicked -= OnUpdateTickedReducedEnergyBuff;
        }
    }

    public bool IsActiveFarmingBuff
    {
        get
        {
            return Game1.player.hasBuff("Farmingspeed");
        }
    }

    public bool IsActiveFishingBuff
    {
        get
        {
            return Game1.player.hasBuff("Fishingspeed");
        }
    }

    public bool IsActiveLuckLevelBuff
    {
        get
        {
            return Game1.player.hasBuff("Lucklevelup");
        }
    }

    public bool IsActiveReducedEnergyBuff
    {
        get
        {
            return Game1.player.hasBuff("ReducedEnergy");
        }
    }
    public bool IsActiveMiningBuff
    {
        get
        {
            return Game1.player.hasBuff("Miningspeed");
        }
    }
    public bool IsActiveForaningBuff
    {
        get
        {
            return Game1.player.hasBuff("Foraningspeed");
        }
    }

    public bool IsActiveSpeedBuff
    {
        get
        {
            return Game1.player.hasBuff("Speedbuff");
        }
    }
}