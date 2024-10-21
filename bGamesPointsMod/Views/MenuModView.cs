using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Tools;

namespace bGamesPointsMod.Controllers
{
    public class Menu
    {
        private readonly IMonitor Monitor;
        private readonly IModHelper Helper;
        private Texture2D bTMiningBuff;
        private Rectangle bBMiningBuff;
        private Texture2D bTForaningBuff;
        private Rectangle bBForaningBuff;
        private Texture2D bTSpeedBuff;
        private Rectangle bBSpeedBuff;
        public Texture2D bTMenuMod;
        public Rectangle bBMenuMod;

        // Menu backgound
        public Texture2D menuBg;
        public Rectangle positionMenuBg;

        // Controlador de Buffs
        public BuffController buffController;

        private bool isMenuVisible = false; // Control para mostrar/ocultar el menú

        public Menu(IModHelper helper, BuffController buffController)
        {
            // Inicializar Buffs y Helper
            this.Helper = helper ?? throw new ArgumentNullException(nameof(helper)); // Validar que Helper no sea null
            this.buffController = buffController ?? throw new ArgumentNullException(nameof(buffController)); // Validar que buffController no sea null

            // Menu backgound
            menuBg = helper.ModContent.Load<Texture2D>("assets/menubg.png");
            positionMenuBg = new Rectangle(100, 100, 300, 180); // posición y tamaño del menú

            // Inicializar botones
            // Ajustar las posiciones de los botones en relación al recuadro del menú
            bBMiningBuff = new Rectangle(positionMenuBg.X + 50, positionMenuBg.Y + 20, 20, 20);
            bTMiningBuff = helper.ModContent.Load<Texture2D>("assets/pickaxe.png");

            bBForaningBuff = new Rectangle(positionMenuBg.X + 50, positionMenuBg.Y + 60, 20, 20);
            bTForaningBuff = helper.ModContent.Load<Texture2D>("assets/axe.png");

            bBSpeedBuff = new Rectangle(positionMenuBg.X + 50, positionMenuBg.Y + 100, 20, 20);
            bTSpeedBuff = helper.ModContent.Load<Texture2D>("assets/run.png");
        }

        public void ToggleMenu()
        {
            isMenuVisible = !isMenuVisible;            
        }

        public void RenderMenu(SpriteBatch spriteBatch)
        {
            if (isMenuVisible)
            {
                // Dibujar fondo del menú solo si está visible
                spriteBatch.Draw(menuBg, positionMenuBg, Color.White);

                // Dibujar botones del menú solo si está visible
                spriteBatch.Draw(bTMiningBuff, bBMiningBuff, Color.White);
                spriteBatch.Draw(bTForaningBuff, bBForaningBuff, Color.White);
                spriteBatch.Draw(bTSpeedBuff, bBSpeedBuff, Color.White);
            }
        }

        public void HandleButtonClick(ButtonPressedEventArgs e, BuffController buffController)
        {
            if (e.Button == SButton.MouseLeft && bBMiningBuff.Contains(Game1.getMouseX(), Game1.getMouseY()))
            {
                this.Helper.Events.Input.ButtonPressed += buffController.OnButtonPressedMiningSpeed;
                this.Helper.Events.GameLoop.UpdateTicked += buffController.OnUpdateTickedMiningSpeed;
                    
            }
            if (e.Button == SButton.MouseLeft && bBForaningBuff.Contains(Game1.getMouseX(), Game1.getMouseY()))
            {
                this.Helper.Events.Input.ButtonPressed += buffController.OnButtonPressedForagingSpeed;
                this.Helper.Events.GameLoop.UpdateTicked += buffController.OnUpdateTickedForagingSpeed;
            }
            if (e.Button == SButton.MouseLeft && bBSpeedBuff.Contains(Game1.getMouseX(), Game1.getMouseY()))
            {
                this.Helper.Events.Input.ButtonPressed += buffController.OnButtonPressedSpeed;
                this.Helper.Events.GameLoop.UpdateTicked += buffController.OnUpdateTickedSpeed;
            }
        }
    }
}