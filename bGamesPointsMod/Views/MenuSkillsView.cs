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
using bGamesPointsMod.Buffs;
using System.Security.Cryptography.X509Certificates;

namespace bGamesPointsMod.Controllers
{
    public class MenuSkillsView : IClickableMenu
    {
        private readonly IMonitor Monitor;
        private readonly IModHelper Helper;

        // Menu backgound
        private Texture2D menuBg;
        private Rectangle positionMenuBg;

        // Usuario
        public UserBgamesModel userBgamesModel;

        // Controller del level up y usuario
        public UserBgamesController userBgamesController;
        public LevelUpController levelUpController;
        public BuffController buffController;

        // Botones inferiores
        private ClickableComponent miningLevelUp;
        private ClickableComponent foragingLevelUp;
        private ClickableComponent fishingLevelUp;
        private ClickableComponent combatLevelUp;
        private ClickableComponent farmingLevelUp;
        private ClickableComponent closeButton;

        // Control para mostrar/ocultar el menú
        private bool isMenuVisible = false;
        public bool IsMenuVisible => isMenuVisible;

        // Guarda el ancho y alto de la ventana para detectar cambios
        private int lastViewportWidth;
        private int lastViewportHeight;

        public MenuSkillsView(
        IModHelper helper,
        IMonitor monitor,
        UserBgamesModel userBgamesModel,
        UserBgamesController userBgamesController,
        LevelUpController levelUpController,
        BuffController buffController)
        {
            // Inicializar Buffs y Helper
            this.Helper = helper ?? throw new ArgumentNullException(nameof(helper));
            this.Monitor = monitor;
            this.userBgamesModel = userBgamesModel;
            this.userBgamesController = userBgamesController;
            this.buffController = buffController;
            this.levelUpController = levelUpController ?? throw new ArgumentNullException(nameof(levelUpController));

            menuBg = helper.ModContent.Load<Texture2D>("assets/menubg.png");

            this.Helper.Events.Input.ButtonPressed += OnButtonPressedMenuSkill;
            this.Helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
            // Configurar el layout inicial
            UpdateLayout();
        }

        public void UpdateLayout()
        {
            // Tamaño fijo del menú y botones
            int menuWidth = 500; // Ancho fijo del menú
            int menuHeight = 550; // Alto fijo del menú

            // Posición centrada del menú
            positionMenuBg = new Rectangle(
                (Game1.viewport.Width - menuWidth) / 2,
                (Game1.viewport.Height - menuHeight) / 2,
                menuWidth,
                menuHeight
            );

            // Tamaño fijo de los botones
            int buttonWidth = 170; // Ancho fijo de los botones
            int buttonHeight = 70; // Alto fijo de los botones
            int buttonSpacing = 55; // Espaciado entre botones
            int buttonY = positionMenuBg.Y + 40; // Margen superior de los botones respecto al menú

            // Primera fila de botones
            miningLevelUp = new ClickableComponent(
                new Rectangle(
                    positionMenuBg.X + 50, // Margen izquierdo del menú
                    buttonY,
                    buttonWidth,
                    buttonHeight
                ), "Mining\nlevel up"
            );

            foragingLevelUp = new ClickableComponent(
                new Rectangle(
                    positionMenuBg.X + 50 + buttonWidth + buttonSpacing, // Posición a la derecha del primer botón
                    buttonY,
                    buttonWidth,
                    buttonHeight
                ), "Foraging\nlevel up"
            );

            // Segunda fila de botones
            fishingLevelUp = new ClickableComponent(
                new Rectangle(
                    positionMenuBg.X + 50, // Margen izquierdo del menú
                    buttonY + buttonHeight + buttonSpacing,
                    buttonWidth,
                    buttonHeight
                ), "Fishing\nlevel up"
            );

            combatLevelUp = new ClickableComponent(
                new Rectangle(
                    positionMenuBg.X + 50 + buttonWidth + buttonSpacing, // Posición a la derecha del botón anterior
                    buttonY + buttonHeight + buttonSpacing,
                    buttonWidth,
                    buttonHeight
                ), "Combat\nlevel up"
            );

            // Tercera fila de botones
            farmingLevelUp = new ClickableComponent(
                new Rectangle(
                    positionMenuBg.X + 50, // Margen izquierdo del menú
                    buttonY + 2 * (buttonHeight + buttonSpacing),
                    buttonWidth,
                    buttonHeight
                ), "Farming\nlevel up"
            );

            // Botón de cierre
            closeButton = new ClickableComponent(
                new Rectangle(
                    positionMenuBg.Right - 40, // Ajustado para estar al borde derecho del menú
                    positionMenuBg.Top + 10,  // Espaciado desde la parte superior del menú
                    30,  // Ancho fijo
                    30   // Alto fijo
                ), "X"
            );

            // Actualizar las dimensiones guardadas
            lastViewportWidth = Game1.viewport.Width;
            lastViewportHeight = Game1.viewport.Height;
        }



        public void ToggleMenu() {
            isMenuVisible = !isMenuVisible;
            Game1.activeClickableMenu = isMenuVisible ? this : null;
        }
        public void RenderMenu(SpriteBatch spriteBatch) {
            if (isMenuVisible) {
                // Dibujar fondo del menú solo si está visible
                spriteBatch.Draw(menuBg, positionMenuBg, Color.White);

                // Dibujar botones inferiores
                DrawButtonWithCost(spriteBatch, miningLevelUp, "2 pts");
                DrawButtonWithCost(spriteBatch, foragingLevelUp, "2 pts");
                DrawButtonWithCost(spriteBatch, fishingLevelUp, "2 pts");
                DrawButtonWithCost(spriteBatch, combatLevelUp, "2 pts");
                DrawButtonWithCost(spriteBatch, farmingLevelUp, "2 pts");
                DrawCloseButton(spriteBatch);
                drawMouse(spriteBatch);
            }
        }

        private void DrawButtonWithCost(SpriteBatch spriteBatch, ClickableComponent button, string cost) {
            int borderThickness = 2;

            // Dibujar fondo del botón
            spriteBatch.Draw(Game1.staminaRect, button.bounds, Color.Brown);

            // Dibujar bordes
            spriteBatch.Draw(Game1.staminaRect, new Rectangle(button.bounds.X, button.bounds.Y, button.bounds.Width, borderThickness), Color.Khaki);
            spriteBatch.Draw(Game1.staminaRect, new Rectangle(button.bounds.X, button.bounds.Y, borderThickness, button.bounds.Height), Color.Khaki);
            spriteBatch.Draw(Game1.staminaRect, new Rectangle(button.bounds.X + button.bounds.Width - borderThickness, button.bounds.Y, borderThickness, button.bounds.Height), Color.Khaki);
            spriteBatch.Draw(Game1.staminaRect, new Rectangle(button.bounds.X, button.bounds.Y + button.bounds.Height - borderThickness, button.bounds.Width, borderThickness), Color.Khaki);

            // Dibujar texto del botón
            Vector2 textSize = Game1.smallFont.MeasureString(button.name);
            Vector2 textPosition = new Vector2(
                button.bounds.X + (button.bounds.Width - textSize.X) / 2,
                button.bounds.Y + (button.bounds.Height - textSize.Y) / 2
            );
            Utility.drawTextWithShadow(spriteBatch, button.name, Game1.smallFont, textPosition, Color.White);

            // Dibujar costo debajo
            Vector2 costSize = Game1.smallFont.MeasureString(cost);
            Vector2 costPosition = new Vector2(
                button.bounds.X + (button.bounds.Width - costSize.X) / 2,
                button.bounds.Y + button.bounds.Height + 5
            );
            Utility.drawTextWithShadow(spriteBatch, cost, Game1.smallFont, costPosition, Color.Black);
        }

        private void DrawCloseButton(SpriteBatch spriteBatch) {
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

        public void HandleButtonClick(ButtonPressedEventArgs e, LevelUpController levelUpController) {
            // Si el menú no está visible, ignorar los clics
            if (!isMenuVisible) {
                return;
            }
            if (e.Button == SButton.MouseLeft && miningLevelUp.bounds.Contains(Game1.getMouseX(), Game1.getMouseY())) {
                this.Monitor.Log("Botón Mining levelUp clickeado.", LogLevel.Info);
                if (userBgamesController.SpendPoints(2) == 1){
                    levelUpController.SkillMining();
                    userBgamesController.SavePointsBgames();
                    Game1.addHUDMessage(new HUDMessage("Mining attribute increased!", 2));
                    //Game1.addHUDMessage(new HUDMessage("Atributo de Mineria aumentada!", 2));
                } else {
                    Game1.addHUDMessage(new HUDMessage("You don't have enough points!", 2));
                    //Game1.addHUDMessage(new HUDMessage("No tiene los puntos necesarios!", 2));
                }
            }
            if (e.Button == SButton.MouseLeft && foragingLevelUp.bounds.Contains(Game1.getMouseX(), Game1.getMouseY())) {
                this.Monitor.Log("Botón Foraging levelUp clickeado.", LogLevel.Info);
                if (userBgamesController.SpendPoints(2) == 1) {
                    levelUpController.SkillForaging();
                    userBgamesController.SavePointsBgames();
                    Game1.addHUDMessage(new HUDMessage("Foraging attribute increased!", 2));
                    //Game1.addHUDMessage(new HUDMessage("Atributo de Recoleccion aumentada!", 2));
                } else {
                    //Game1.addHUDMessage(new HUDMessage("You don't have enough points!", 2));
                    Game1.addHUDMessage(new HUDMessage("No tiene los puntos necesarios!", 2));
                }  
            }
            if (e.Button == SButton.MouseLeft && fishingLevelUp.bounds.Contains(Game1.getMouseX(), Game1.getMouseY())) {
                this.Monitor.Log("Botón Fishing levelUp clickeado.", LogLevel.Info);
                if (userBgamesController.SpendPoints(2) == 1) {
                    levelUpController.SkillFishing();
                    userBgamesController.SavePointsBgames();
                    Game1.addHUDMessage(new HUDMessage("Fishing attribute increased!", 2));
                    //Game1.addHUDMessage(new HUDMessage("Atributo de Pesca aumentada!", 2));
                } else {
                    Game1.addHUDMessage(new HUDMessage("You don't have enough points!", 2));
                    //Game1.addHUDMessage(new HUDMessage("No tiene los puntos necesarios!", 2));
                }
            }
            if (e.Button == SButton.MouseLeft && combatLevelUp.bounds.Contains(Game1.getMouseX(), Game1.getMouseY())) {
                this.Monitor.Log("Botón Combat levelUp clickeado.", LogLevel.Info);
                if (userBgamesController.SpendPoints(2) == 1){
                    levelUpController.SkillCombat();
                    userBgamesController.SavePointsBgames();
                    Game1.addHUDMessage(new HUDMessage("Combat attribute increased!", 2));
                    //Game1.addHUDMessage(new HUDMessage("Atributo de Combate aumentado!", 2));
                } else {
                    Game1.addHUDMessage(new HUDMessage("You don't have enough points!", 2));
                    //Game1.addHUDMessage(new HUDMessage("No tiene los puntos necesarios!", 2));
                }
            }
            if (e.Button == SButton.MouseLeft && farmingLevelUp.bounds.Contains(Game1.getMouseX(), Game1.getMouseY())) {
                this.Monitor.Log("Botón Luck levelUp clickeado.", LogLevel.Info);
                if (userBgamesController.SpendPoints(2) == 1) {
                    levelUpController.SkillFarming();
                    userBgamesController.SavePointsBgames();
                    Game1.addHUDMessage(new HUDMessage("Farming attribute increased!", 2));
                    //Game1.addHUDMessage(new HUDMessage("Atributo de Agricultura aumentado!", 2));
                } else {
                    Game1.addHUDMessage(new HUDMessage("You don't have enough points!", 2));
                    //Game1.addHUDMessage(new HUDMessage("No tiene los puntos necesarios!", 2));
                }
            }
            if (e.Button == SButton.MouseLeft && closeButton.bounds.Contains(Game1.getMouseX(), Game1.getMouseY())) {
                Monitor.Log("Cerrar menú.", LogLevel.Info);
                Game1.activeClickableMenu = null; // Cierra el menú
                ToggleMenu(); // Cierra el menu
            }
        }

        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e) {
            // Detectar cambio en el tamaño de la ventana
            if (Game1.viewport.Width != lastViewportWidth || Game1.viewport.Height != lastViewportHeight){
                UpdateLayout();
            }
        }

        private void OnButtonPressedMenuSkill(object sender, ButtonPressedEventArgs e) {
            if (e.Button == SButton.F2) {
                ToggleMenu();
            }
        }
    }
}
