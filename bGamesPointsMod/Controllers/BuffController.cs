using bGamesPointsMod.Models;
using Force.DeepCloner;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Buffs;
using StardewValley.Tools;


namespace bGamesPointsMod.Controllers;
public class BuffController
{
    private readonly IMonitor Monitor;
    private readonly IModHelper Helper;
    public Buff miningBuff;
    public Buff foraningBuff;
    public Buff speedBuff;

    public BuffController(
             IMonitor monitor,
             IModHelper helper,  //Helper
             Buff miningBuff,
             Buff foraningBuff,
             Buff speedBuff)
    {
        this.Monitor = monitor;
        this.Helper = helper;  //helper
        this.miningBuff = miningBuff;
        this.foraningBuff = foraningBuff;
        this.speedBuff = speedBuff;
    }

    public bool IsActiveMiningBuff
    {
        get
        {
            return Game1.player.hasBuff(miningBuff.id);
        }
    }
    public bool IsActiveForaningBuff
    {
        get
        {
            return Game1.player.hasBuff(foraningBuff.id);
        }
    }

    public bool IsActiveSpeedBuff
    {
        get
        {
            return Game1.player.hasBuff(speedBuff.id);
        }
    }

    public void BuffSpeed()
    {
        Game1.player.applyBuff(speedBuff);
    }

    public void BuffForaning()
    {
        Game1.player.applyBuff(foraningBuff);
    }

    public void BuffMining()
    {
        Game1.player.applyBuff(miningBuff);
    }
}