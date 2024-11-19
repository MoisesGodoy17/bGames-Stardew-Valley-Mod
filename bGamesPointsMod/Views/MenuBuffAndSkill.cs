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
    public class MenuBuffView : IClickableMenu
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
        private ClickableComponent reducedEnergyBuff;
        private ClickableComponent luckLevelBuff;
        private ClickableComponent fishingBuff;
        private ClickableComponent farmingBuff;

        // Control para mostrar/ocultar el menú
        private bool isMenuVisible = false;

        public bool IsMenuVisible => isMenuVisible;

        public MenuBuffView(
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
                    positionMenuBg.X + 70,
                    buttonY,
                    buttonWidth,
                    buttonHeight
                ), "Speed Buff\n10 pts"
                
            );
            miningBuff = new ClickableComponent(
                new Rectangle(
                    positionMenuBg.X + 70 + buttonWidth + buttonSpacing,
                    buttonY,
                    buttonWidth,
                    buttonHeight
                ), "Mining Buff\n10pts"
            );
            foranmingBuff = new ClickableComponent(
                new Rectangle(
                    positionMenuBg.X + 70,
                    buttonY + 90,
                    buttonWidth,
                    buttonHeight
                ), "Foraning Buff\n10 pts"
            );
            reducedEnergyBuff = new ClickableComponent(
                new Rectangle(
                    positionMenuBg.X + 70 + buttonWidth + buttonSpacing,
                    buttonY + 90,
                    buttonWidth,
                    buttonHeight
                ), "Stemine Buff\n10 pts"
            );
            luckLevelBuff = new ClickableComponent(
                new Rectangle(
                    positionMenuBg.X + 70,
                    buttonY + 180,
                    buttonWidth,
                    buttonHeight
                ), "Luck Buff\n10 pts"
            );
            fishingBuff = new ClickableComponent(
                new Rectangle(
                    positionMenuBg.X + 70 + buttonWidth + buttonSpacing,
                    buttonY + 180,
                    buttonWidth,
                    buttonHeight
                ), "Fishing Buff\n10 pts"
            );
            farmingBuff = new ClickableComponent(
                new Rectangle(
                    positionMenuBg.X + 70,
                    buttonY + 270,
                    buttonWidth,
                    buttonHeight
                ), "Farming Buff\n10 pts"
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
                Utility.drawTextWithShadow(spriteBatch, reducedEnergyBuff.name, Game1.dialogueFont, new Vector2(reducedEnergyBuff.bounds.X, reducedEnergyBuff.bounds.Y), Color.White);
                Utility.drawTextWithShadow(spriteBatch, luckLevelBuff.name, Game1.dialogueFont, new Vector2(luckLevelBuff.bounds.X, luckLevelBuff.bounds.Y), Color.White);
                Utility.drawTextWithShadow(spriteBatch, fishingBuff.name, Game1.dialogueFont, new Vector2(fishingBuff.bounds.X, fishingBuff.bounds.Y), Color.White);
                Utility.drawTextWithShadow(spriteBatch, farmingBuff.name, Game1.dialogueFont, new Vector2(farmingBuff.bounds.X, farmingBuff.bounds.Y), Color.White);
            }
        }
        public void HandleButtonClick(ButtonPressedEventArgs e, BuffController buffController)
        {
            // Si el menú no está visible, ignorar los clics
            if (!isMenuVisible)
            {
                return;
            }

            if (e.Button == SButton.MouseLeft && speedBuff.bounds.Contains(Game1.getMouseX(), Game1.getMouseY()))
            {
                //if (userBgamesController.SpendPoints(10,0) == 1)
                //{
                    this.Monitor.Log("Botón de Speed Buff clickeado.", LogLevel.Info);
                    buffController.BuffSpeed();
                //}
                //else {this.Monitor.Log("No tienes los puntos necesarios para activar este buff.", LogLevel.Info);}
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
            if (e.Button == SButton.MouseLeft && reducedEnergyBuff.bounds.Contains(Game1.getMouseX(), Game1.getMouseY()))
            {
                this.Monitor.Log("Botón de Reduced Energy Buff clickeado.", LogLevel.Info);
                buffController.ReducedEnergyBuff();
                this.Helper.Events.GameLoop.UpdateTicked += buffController.OnUpdateTickedReducedEnergyBuff;
            }
            if (e.Button == SButton.MouseLeft && luckLevelBuff.bounds.Contains(Game1.getMouseX(), Game1.getMouseY()))
            {
                this.Monitor.Log("Botón de Luck Level Buff clickeado.", LogLevel.Info);
                buffController.BuffLuckLevel();
            }
            if (e.Button == SButton.MouseLeft && fishingBuff.bounds.Contains(Game1.getMouseX(), Game1.getMouseY()))
            {
                this.Monitor.Log("Botón de Fishing Buff clickeado.", LogLevel.Info);
                buffController.BuffFishing();
            }
            if (e.Button == SButton.MouseLeft && farmingBuff.bounds.Contains(Game1.getMouseX(), Game1.getMouseY()))
            {
                this.Monitor.Log("Botón de Farming Buff clickeado.", LogLevel.Info);
                buffController.BuffFarming();
            }
        }
        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            // Verificar si el buff está activo llamando a la propiedad IsActive
            if (buffController.IsActiveMiningBuff || 
                buffController.IsActiveSpeedBuff || 
                buffController.IsActiveForaningBuff ||
                buffController.IsActiveReducedEnergyBuff ||
                buffController.IsActiveLuckLevelBuff ||
                buffController.IsActiveFishingBuff ||
                buffController.IsActiveFarmingBuff)
            {
                this.Monitor.Log("Hay un buff activo.", LogLevel.Debug);
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
