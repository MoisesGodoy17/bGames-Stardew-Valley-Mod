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

        // Botones inferiores
        private ClickableComponent miningLevelUp;
        private ClickableComponent foragingLevelUp;
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
        LevelUpController levelUpController)
        {
            // Inicializar Buffs y Helper
            this.Helper = helper ?? throw new ArgumentNullException(nameof(helper));
            this.Monitor = monitor;
            this.userBgamesModel = userBgamesModel;
            this.userBgamesController = userBgamesController;
            this.levelUpController = levelUpController ?? throw new ArgumentNullException(nameof(levelUpController));

            menuBg = helper.ModContent.Load<Texture2D>("assets/menubg.png");

            this.Helper.Events.Input.ButtonPressed += OnButtonPressedMenuSkill;
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

            miningLevelUp = new ClickableComponent(new Rectangle(
                    positionMenuBg.X + 70,
                    buttonY,
                    buttonWidth,
                    buttonHeight
                ), "Mining level up"

            );
            foragingLevelUp = new ClickableComponent(
                new Rectangle(
                    positionMenuBg.X + 70 + buttonWidth + buttonSpacing,
                    buttonY,
                    buttonWidth,
                    buttonHeight
                ), "Foraging level up"
            );

            closeButton = new ClickableComponent(
                new Rectangle(positionMenuBg.X + menuWidth - 40, 
                positionMenuBg.Y, 
                30, 
                30
                ), "X");
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

                // Dibujar botones inferiores
                DrawButtonWithCost(spriteBatch, miningLevelUp, "10 pts");
                DrawButtonWithCost(spriteBatch, foragingLevelUp, "10 pts");
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

        public void HandleButtonClick(ButtonPressedEventArgs e, LevelUpController levelUpController)
        {
            // Si el menú no está visible, ignorar los clics
            if (!isMenuVisible)
            {
                return;
            }
            if (e.Button == SButton.MouseLeft && miningLevelUp.bounds.Contains(Game1.getMouseX(), Game1.getMouseY()))
            {
                this.Monitor.Log("Botón miningLevelUp clickeado.", LogLevel.Info);
                levelUpController.SkillMining();
            }
            else if (e.Button == SButton.MouseLeft && foragingLevelUp.bounds.Contains(Game1.getMouseX(), Game1.getMouseY()))
            {
                this.Monitor.Log("Botón foragingLevelUp clickeado.", LogLevel.Info);
                levelUpController.SkillForaging();
            }
            if (e.Button == SButton.MouseLeft && closeButton.bounds.Contains(Game1.getMouseX(), Game1.getMouseY()))
            {
                Monitor.Log("Cerrar menú.", LogLevel.Info);
                Game1.activeClickableMenu = null; // Cierra el menú
                ToggleMenu(); // Cierra el menu
            }
        }

        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            // Detectar cambio en el tamaño de la ventana
            if (Game1.viewport.Width != lastViewportWidth || Game1.viewport.Height != lastViewportHeight){
                UpdateLayout();
            }
        }

        private void OnButtonPressedMenuSkill(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button == SButton.F2)
            {
                ToggleMenu();
            }
        }
    }
}
