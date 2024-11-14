using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Tools;
using bGamesPointsMod.Models;
using bGamesPointsMod.Views;
using Microsoft.Xna.Framework.Content;
using StardewValley.Buffs;

namespace bGamesPointsMod.Controllers
{
    public class MenuBuffAndSkill : IClickableMenu
    {
        private readonly IMonitor Monitor;
        private readonly IModHelper Helper;
        // Menu backgound

        private Texture2D menuBg;
        private Rectangle positionMenuBg;

        // Controlador de Buffs
        public BuffController buffController;

        // Usuario
        public UserBgamesModel userBgamesModel;

        // Controller del usaurio
        public UserBgamesController userBgamesController;

        // Botones inferiores
        private ClickableComponent speedBuff;
        private ClickableComponent miningBuff;
        private ClickableComponent foranmingBuff;

        // Control para mostrar/ocultar el menú
        private bool isMenuVisible = false;

        public bool IsMenuVisible => isMenuVisible;

        public MenuBuffAndSkill(
        IModHelper helper,
        BuffController buffController,
        IMonitor monitor,
        UserBgamesModel userBgamesModel,
        UserBgamesController userBgamesController)
        {
            this.Helper = helper ?? throw new ArgumentNullException(nameof(helper));
            this.Monitor = monitor;
            this.userBgamesModel = userBgamesModel;
            this.buffController = buffController;
            this.userBgamesController = userBgamesController;

            menuBg = helper.ModContent.Load<Texture2D>("assets/menubg.png");

            int menuWidth = 640;
            int menuHeight = 480;

            positionMenuBg = new Rectangle(
                (Game1.viewport.Width - menuWidth) / 2,
                (Game1.viewport.Height - menuHeight) / 2,
                menuWidth,
                menuHeight
            );

            int buttonWidth = 100;
            int buttonHeight = 40;
            int buttonSpacing = 150;
            int buttonY = positionMenuBg.Y + 20;

            speedBuff = new ClickableComponent(
                new Rectangle(
                    positionMenuBg.X + 50,
                    buttonY,
                    buttonWidth,
                    buttonHeight
                ), "Speed Buff"
            );


            miningBuff = new ClickableComponent(
                new Rectangle(
                    positionMenuBg.X + 50 + buttonWidth + buttonSpacing,
                    buttonY,
                    buttonWidth,
                    buttonHeight
                ), "Mining Buff"
            );

            foranmingBuff = new ClickableComponent(
                new Rectangle(
                    positionMenuBg.X + 50,
                    buttonY + 50,
                    buttonWidth,
                    buttonHeight
                ), "Foraning Buff"
            );

            Game1.mouseCursor = 0;
            // Suscribir al evento ButtonPressed para escuchar la tecla F3
            this.Helper.Events.Input.ButtonPressed += OnButtonPressedLogin;
            // Subscribirse al evento UpdateTicked para verificar el estado del buff
            helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
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

                // Dibujar botones inferiores
                Utility.drawTextWithShadow(spriteBatch, speedBuff.name, Game1.dialogueFont, new Vector2(speedBuff.bounds.X, speedBuff.bounds.Y), Color.White);
                Utility.drawTextWithShadow(spriteBatch, miningBuff.name, Game1.dialogueFont, new Vector2(miningBuff.bounds.X, miningBuff.bounds.Y), Color.White);
                Utility.drawTextWithShadow(spriteBatch, foranmingBuff.name, Game1.dialogueFont, new Vector2(foranmingBuff.bounds.X, foranmingBuff.bounds.Y), Color.White);
            }
        }
        public void HandleButtonClick(ButtonPressedEventArgs e, BuffController buffController)
        {
            
            if (e.Button == SButton.MouseLeft && speedBuff.bounds.Contains(Game1.getMouseX(), Game1.getMouseY()))
            {
                if (userBgamesController.SpendPoints(10,0) == 1)
                {
                    this.Monitor.Log("Botón de Speed Buff clickeado.", LogLevel.Info);
                    buffController.BuffSpeed();
                }
                else {this.Monitor.Log("No tienes los puntos necesarios para activar este buff.", LogLevel.Info);}
            }
            if (e.Button == SButton.MouseLeft && miningBuff.bounds.Contains(Game1.getMouseX(), Game1.getMouseY()))
            {
                this.Monitor.Log("Botón de Stamin Buff clickeado.", LogLevel.Info);
                buffController.BuffMining();
            }
            if (e.Button == SButton.MouseLeft && foranmingBuff.bounds.Contains(Game1.getMouseX(), Game1.getMouseY()))
            {
                this.Monitor.Log("Botón de Foraning Buff clickeado.", LogLevel.Info);
                buffController.BuffForaning();
            }
        }
        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            // Verificar si el buff está activo llamando a la propiedad IsActive
            if (buffController.IsActiveMiningBuff || buffController.IsActiveSpeedBuff || buffController.IsActiveForaningBuff)
            {
                this.Monitor.Log("El buff de minería está activo.", LogLevel.Debug);
            }
        }
        private void OnButtonPressedLogin(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button == SButton.F3)
            {
                ToggleMenu();
            }
        }
    }
}
