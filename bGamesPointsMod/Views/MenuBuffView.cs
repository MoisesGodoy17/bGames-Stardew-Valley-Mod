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
        private ClickableComponent foragingBuff;
        private ClickableComponent reducedEnergyBuff;
        private ClickableComponent luckLevelBuff;
        private ClickableComponent fishingBuff;
        private ClickableComponent farmingBuff;
        private ClickableComponent closeButton;

        // Control para mostrar/ocultar el menú
        private bool isMenuVisible = false;
        public bool IsMenuVisible => isMenuVisible;

        // Control de buff
        private int isBuffActive = 0;

        private int lastViewportWidth;
        private int lastViewportHeight;

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

            // Suscribirse a eventos de entrada y actualización
            this.Helper.Events.Input.ButtonPressed += OnButtonPressedMenuBuff;
            this.Helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;

            // Configurar el layout inicial
            UpdateLayout();
        }

        public void UpdateLayout()
        {
            int menuWidth = Game1.viewport.Width / 3; // 1/3 del ancho de la ventana
            int menuHeight = Game1.viewport.Height / 2; // 1/2 de la altura de la ventana

            positionMenuBg = new Rectangle(
                (Game1.viewport.Width - menuWidth) / 2,
                (Game1.viewport.Height - menuHeight) / 2,
                menuWidth,
                menuHeight
            );

            int buttonWidth = menuWidth / 4;
            int buttonHeight = menuHeight / 8;
            int buttonSpacing = 10;
            int buttonY = positionMenuBg.Y + 50;

            // Definición de botones
            speedBuff = new ClickableComponent(new Rectangle(positionMenuBg.X + 50, buttonY, buttonWidth, buttonHeight), "Speed Buff");
            miningBuff = new ClickableComponent(new Rectangle(positionMenuBg.X + 150 + buttonWidth + buttonSpacing, buttonY, buttonWidth, buttonHeight), "Mining Buff");
            foragingBuff = new ClickableComponent(new Rectangle(positionMenuBg.X + 50, buttonY + 100, buttonWidth, buttonHeight), "Foraging Buff");
            reducedEnergyBuff = new ClickableComponent(new Rectangle(positionMenuBg.X + 150 + buttonWidth + buttonSpacing, buttonY + 100, buttonWidth, buttonHeight), "Stamina Buff");
            luckLevelBuff = new ClickableComponent(new Rectangle(positionMenuBg.X + 50, buttonY + 200, buttonWidth, buttonHeight), "Luck Buff");
            fishingBuff = new ClickableComponent(new Rectangle(positionMenuBg.X + 150 + buttonWidth + buttonSpacing, buttonY + 200, buttonWidth, buttonHeight), "Fishing Buff");
            farmingBuff = new ClickableComponent(new Rectangle(positionMenuBg.X + 50, buttonY + 300, buttonWidth, buttonHeight), "Farming Buff");
            closeButton = new ClickableComponent(new Rectangle(positionMenuBg.X + menuWidth - 40, positionMenuBg.Y, 30, 30), "X");

            // Actualizar las dimensiones guardadas
            lastViewportWidth = Game1.viewport.Width;
            lastViewportHeight = Game1.viewport.Height;
        }

        public void ToggleMenu()
        {
            isMenuVisible = !isMenuVisible;
            Game1.activeClickableMenu = isMenuVisible ? this : null;
        }
        public void RenderMenu(SpriteBatch spriteBatch)
        {
            if (isMenuVisible)
            {
                // Dibujar fondo del menú solo si está visible
                spriteBatch.Draw(menuBg, positionMenuBg, Color.White);
                // Dibujar botones con el costo debajo
                DrawButtonWithCost(spriteBatch, speedBuff, "10 pts");
                DrawButtonWithCost(spriteBatch, miningBuff, "10 pts");
                DrawButtonWithCost(spriteBatch, foragingBuff, "10 pts");
                DrawButtonWithCost(spriteBatch, reducedEnergyBuff, "10 pts");
                DrawButtonWithCost(spriteBatch, luckLevelBuff, "10 pts");
                DrawButtonWithCost(spriteBatch, fishingBuff, "10 pts");
                DrawButtonWithCost(spriteBatch, farmingBuff, "10 pts");
                DrawCloseButton(spriteBatch);
                drawMouse(spriteBatch);
            }
        }

        private void DrawButtonWithCost(SpriteBatch spriteBatch, ClickableComponent button, string cost)
        {
            int borderThickness = 2;

            // Dibujar fondo del botón
            spriteBatch.Draw(Game1.staminaRect, button.bounds, Color.Khaki);

            // Dibujar bordes
            spriteBatch.Draw(Game1.staminaRect, new Rectangle(button.bounds.X, button.bounds.Y, button.bounds.Width, borderThickness), Color.Brown);
            spriteBatch.Draw(Game1.staminaRect, new Rectangle(button.bounds.X, button.bounds.Y, borderThickness, button.bounds.Height), Color.Brown);
            spriteBatch.Draw(Game1.staminaRect, new Rectangle(button.bounds.X + button.bounds.Width - borderThickness, button.bounds.Y, borderThickness, button.bounds.Height), Color.Brown);
            spriteBatch.Draw(Game1.staminaRect, new Rectangle(button.bounds.X, button.bounds.Y + button.bounds.Height - borderThickness, button.bounds.Width, borderThickness), Color.Brown);

            // Dibujar texto del botón
            Vector2 textSize = Game1.smallFont.MeasureString(button.name);
            Vector2 textPosition = new Vector2(
                button.bounds.X + (button.bounds.Width - textSize.X) / 2,
                button.bounds.Y + (button.bounds.Height - textSize.Y) / 2
            );
            Utility.drawTextWithShadow(spriteBatch, button.name, Game1.smallFont, textPosition, Color.Brown);

            // Dibujar costo debajo
            Vector2 costSize = Game1.smallFont.MeasureString(cost);
            Vector2 costPosition = new Vector2(
                button.bounds.X + (button.bounds.Width - costSize.X) / 2,
                button.bounds.Y + button.bounds.Height + 5
            );
            Utility.drawTextWithShadow(spriteBatch, cost, Game1.smallFont, costPosition, Color.Black);
        }

        private void DrawCloseButton(SpriteBatch spriteBatch)
        {
            int borderThickness = 2;

            // Dibujar el fondo del botón
            spriteBatch.Draw(Game1.staminaRect, closeButton.bounds, Color.Red);

            // Dibujar bordes del botón
            spriteBatch.Draw(Game1.staminaRect, new Rectangle(closeButton.bounds.X, closeButton.bounds.Y, closeButton.bounds.Width, borderThickness), Color.White);
            spriteBatch.Draw(Game1.staminaRect, new Rectangle(closeButton.bounds.X, closeButton.bounds.Y, borderThickness, closeButton.bounds.Height), Color.White);
            spriteBatch.Draw(Game1.staminaRect, new Rectangle(closeButton.bounds.X + closeButton.bounds.Width - borderThickness, closeButton.bounds.Y, borderThickness, closeButton.bounds.Height), Color.White);
            spriteBatch.Draw(Game1.staminaRect, new Rectangle(closeButton.bounds.X, closeButton.bounds.Y + closeButton.bounds.Height - borderThickness, closeButton.bounds.Width, borderThickness), Color.White);

            // Dibujar el texto "X"
            Vector2 textSize = Game1.smallFont.MeasureString(closeButton.name);
            Vector2 textPosition = new Vector2(
                closeButton.bounds.X + (closeButton.bounds.Width - textSize.X) / 2,
                closeButton.bounds.Y + (closeButton.bounds.Height - textSize.Y) / 2
            );
            Utility.drawTextWithShadow(spriteBatch, closeButton.name, Game1.smallFont, textPosition, Color.White);
        }

        public void HandleButtonClick(ButtonPressedEventArgs e, BuffController buffController){
            // Si el menú no está visible, ignorar los clics
            if (!isMenuVisible){
                return;
            }
            if (e.Button == SButton.MouseLeft && closeButton.bounds.Contains(Game1.getMouseX(), Game1.getMouseY())){
                Monitor.Log("Cerrar menú.", LogLevel.Info);
                Game1.activeClickableMenu = null; // Cierra el menú
                ToggleMenu(); // Cierra el menu
            }
            if (isBuffActive == 0) {
                this.Monitor.Log($"Hay un buff activo.{isBuffActive}", LogLevel.Debug);
                if (e.Button == SButton.MouseLeft && speedBuff.bounds.Contains(Game1.getMouseX(), Game1.getMouseY())) {
                    this.Monitor.Log("Botón de Speed Buff clickeado.", LogLevel.Info);
                    if (userBgamesController.SpendPoints(5) == 1) {
                        buffController.BuffSpeed();
                        userBgamesController.SavePointsBgames();
                        Game1.addHUDMessage(new HUDMessage("Si tiene los puntos necesarios!", 2));
                    }
                    else {
                        Game1.addHUDMessage(new HUDMessage("No tiene los puntos necesarios!", 2));
                    }
                }
                if (e.Button == SButton.MouseLeft && miningBuff.bounds.Contains(Game1.getMouseX(), Game1.getMouseY())) {
                    this.Monitor.Log("Botón de Stamin Buff clickeado.", LogLevel.Info);
                    buffController.BuffMining();
                }
                if (e.Button == SButton.MouseLeft && foragingBuff.bounds.Contains(Game1.getMouseX(), Game1.getMouseY())) {
                    this.Monitor.Log("Botón de Foraning Buff clickeado.", LogLevel.Info);
                    buffController.BuffForaging();
                }
                if (e.Button == SButton.MouseLeft && reducedEnergyBuff.bounds.Contains(Game1.getMouseX(), Game1.getMouseY())) {
                    this.Monitor.Log("Botón de Reduced Energy Buff clickeado.", LogLevel.Info);
                    buffController.ReducedEnergyBuff();
                    this.Helper.Events.GameLoop.UpdateTicked += buffController.OnUpdateTickedReducedEnergyBuff;
                }
                if (e.Button == SButton.MouseLeft && luckLevelBuff.bounds.Contains(Game1.getMouseX(), Game1.getMouseY())) {
                    this.Monitor.Log("Botón de Luck Level Buff clickeado.", LogLevel.Info);
                    buffController.BuffLuckLevel();
                }
                if (e.Button == SButton.MouseLeft && fishingBuff.bounds.Contains(Game1.getMouseX(), Game1.getMouseY())) {
                    this.Monitor.Log("Botón de Fishing Buff clickeado.", LogLevel.Info);
                    buffController.BuffFishing();
                }
                if (e.Button == SButton.MouseLeft && farmingBuff.bounds.Contains(Game1.getMouseX(), Game1.getMouseY())) {
                    this.Monitor.Log("Botón de Farming Buff clickeado.", LogLevel.Info);
                    buffController.BuffFarming();
                }
            } else {
                Game1.addHUDMessage(new HUDMessage("Hay un buff activado!", 2));
                this.Monitor.Log("No se puede activar un buff", LogLevel.Info);
            }
        }
        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            // Verificar si hay algún buff activo
            bool anyBuffActive = buffController.IsActiveMiningBuff ||
                                 buffController.IsActiveSpeedBuff ||
                                 buffController.IsActiveForaningBuff ||
                                 buffController.IsActiveReducedEnergyBuff ||
                                 buffController.IsActiveLuckLevelBuff ||
                                 buffController.IsActiveFishingBuff ||
                                 buffController.IsActiveFarmingBuff;

            // Actualizar el estado de isBuffActive solo si hay un cambio
            isBuffActive = anyBuffActive ? 1 : 0;

            // Detectar cambio en el tamaño de la ventana
            if (Game1.viewport.Width != lastViewportWidth || Game1.viewport.Height != lastViewportHeight)
            {
                UpdateLayout();
            }
        }

        private void OnButtonPressedMenuBuff(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button == SButton.F3)
            {
                ToggleMenu();
            }
        }
    }
}
