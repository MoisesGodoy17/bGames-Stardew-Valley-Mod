using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Tools;
using bGamesPointsMod.Models;
using System.Threading;

namespace bGamesPointsMod.Controllers
{
    public class MenuModView : IClickableMenu
    {
        private readonly IMonitor Monitor;
        private readonly IModHelper Helper;

        // Fondo del menú
        public Texture2D menuBg;
        public Rectangle positionMenuBg;

        // Controladores
        public BuffController buffController;
        public UserBgamesModel userBgamesModel;
        public UserBgamesController userBgamesController;
        public LevelUpController levelUpController;

        // Botones inferiores
        private ClickableComponent buttonBuff;
        private ClickableComponent buttonLevelUp;

        // Control para mostrar/ocultar el menú
        private bool isMenuVisible = false;

        // Menús de Buff y Skills
        private MenuBuffView menuBuff;
        private MenuSkillsView menuSkills;
        private ClickableComponent closeButton;

        // Guarda el ancho y alto de la ventana para detectar cambios
        private int lastViewportWidth;
        private int lastViewportHeight;

        public MenuModView(
            IModHelper helper,
            BuffController buffController,
            IMonitor monitor,
            UserBgamesModel userBgamesModel,
            UserBgamesController userBgamesController,
            LevelUpController levelUpController)
        {
            // Inicialización
            this.Helper = helper ?? throw new ArgumentNullException(nameof(helper));
            this.buffController = buffController ?? throw new ArgumentNullException(nameof(buffController));
            this.Monitor = monitor;
            this.userBgamesModel = userBgamesModel;
            this.userBgamesController = userBgamesController;
            this.levelUpController = levelUpController;

            // Cargar el fondo del menú
            menuBg = helper.ModContent.Load<Texture2D>("assets/menubg.png");

            // Instancia de los menús
            menuBuff = new MenuBuffView(helper, buffController, monitor, userBgamesModel, userBgamesController);
            menuSkills = new MenuSkillsView(helper, monitor, userBgamesModel, userBgamesController, levelUpController);
            this.Helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
            UpdateLayout();

        }
        public void UpdateLayout() {
            // Tamaño fijo del menú
            int menuWidth = 600; // Ancho fijo
            int menuHeight = 400; // Alto fijo

            // Posición centrada del menú
            positionMenuBg = new Rectangle(
                (Game1.viewport.Width - menuWidth) / 2,
                (Game1.viewport.Height - menuHeight) / 2,
                menuWidth,
                menuHeight
            );

            // Tamaño fijo de los botones
            int buttonWidth = 150;
            int buttonHeight = 40;
            int buttonSpacing = 10;

            // Posición dinámica de los botones
            int buttonY = positionMenuBg.Y + menuHeight - buttonHeight - buttonSpacing;

            buttonBuff = new ClickableComponent(
                new Rectangle(
                    positionMenuBg.X + (menuWidth - 2 * buttonWidth - buttonSpacing) / 2,
                    buttonY,
                    buttonWidth,
                    buttonHeight
                ), "Buff");

            buttonLevelUp = new ClickableComponent(
                new Rectangle(
                    buttonBuff.bounds.X + buttonWidth + buttonSpacing,
                    buttonY,
                    buttonWidth,
                    buttonHeight
                ), "Level Up");

            // Botón de cierre fijo en la esquina superior derecha del menú
            closeButton = new ClickableComponent(
                new Rectangle(
                    positionMenuBg.Right - 40, // Ajustado para estar al borde derecho del menú
                    positionMenuBg.Top + 10,  // Espaciado desde la parte superior
                    30,  // Ancho fijo
                    30   // Alto fijo
                ), "X");
        }


        public void ToggleMenu() {
            isMenuVisible = !isMenuVisible;
            Game1.activeClickableMenu = isMenuVisible ? this : null;
        }

        public void RenderMenu(SpriteBatch spriteBatch)
        {
            if (isMenuVisible) {
                // Dibujar el fondo del menú
                spriteBatch.Draw(menuBg, positionMenuBg, Color.White);

                // Dibujar botones inferiores
                DrawButton(spriteBatch, buttonBuff.bounds, buttonBuff.name, Color.Brown, Color.Khaki, Color.White);
                DrawButton(spriteBatch, buttonLevelUp.bounds, buttonLevelUp.name, Color.Brown, Color.Khaki, Color.White);
                DrawCloseButton(spriteBatch);
                drawMouse(spriteBatch);

                // Dibujar información del usuario
                if (userBgamesModel != null)
                {
                    string userInfo = $"Name: {userBgamesModel.Name}\nEmail: {userBgamesModel.Email}\nAge: {userBgamesModel.Age}";
                    spriteBatch.DrawString(Game1.smallFont, userInfo, new Vector2(positionMenuBg.X + 20, positionMenuBg.Y + 40), Color.Black);
                }
                if (userBgamesModel.Points != null)
                {
                    string userPointsInfo = "";
                    foreach (var points in userBgamesModel.Points)
                    {
                        userPointsInfo += $"{points.Name}: {points.Data}\n";
                    }
                    // Coordenadas para dibujar el texto en la parte superior del menú
                    float textX = positionMenuBg.X + 400; // Margen desde la izquierda
                    float textY = positionMenuBg.Y + 20; // Margen desde la parte superior
                    spriteBatch.DrawString(Game1.smallFont, userPointsInfo, new Vector2(positionMenuBg.X + 400, positionMenuBg.Y + 40), Color.Black);
                }
                // Renderizar menús Buff y Skills si están visibles
                if (menuBuff.IsMenuVisible) menuBuff.RenderMenu(spriteBatch);
                if (menuSkills.IsMenuVisible) menuSkills.RenderMenu(spriteBatch);
            }

            // Dibujar cursor del mouse
            drawMouse(spriteBatch);
            if (menuBuff != null)
                menuBuff.RenderMenu(spriteBatch);
            
            if (menuSkills != null)
                menuSkills.RenderMenu(spriteBatch);
        }

        public void DrawButton(SpriteBatch spriteBatch, Rectangle bounds, string text, Color buttonColor, Color borderColor, Color textColor) {
            int borderThickness = 2;
            // Dibujar el fondo del botón
            spriteBatch.Draw(Game1.staminaRect, bounds, buttonColor);

            // Dibujar bordes del botón
            spriteBatch.Draw(Game1.staminaRect, new Rectangle(bounds.X, bounds.Y, bounds.Width, borderThickness), borderColor); // Top
            spriteBatch.Draw(Game1.staminaRect, new Rectangle(bounds.X, bounds.Y, borderThickness, bounds.Height), borderColor); // Left
            spriteBatch.Draw(Game1.staminaRect, new Rectangle(bounds.X + bounds.Width - borderThickness, bounds.Y, borderThickness, bounds.Height), borderColor); // Right
            spriteBatch.Draw(Game1.staminaRect, new Rectangle(bounds.X, bounds.Y + bounds.Height - borderThickness, bounds.Width, borderThickness), borderColor); // Bottom

            // Dibujar el texto centrado
            Vector2 textSize = Game1.smallFont.MeasureString(text);
            Vector2 textPosition = new Vector2(
                bounds.X + (bounds.Width - textSize.X) / 2,
                bounds.Y + (bounds.Height - textSize.Y) / 2
            );
            Utility.drawTextWithShadow(spriteBatch, text, Game1.smallFont, textPosition, textColor);
        }

        public void OnOpenMenuBuff(ButtonPressedEventArgs e, BuffController buffController, LevelUpController levelUpController) {
            // Verificar si el botón de Buffs fue clickeado
            if (e.Button == SButton.MouseLeft && buttonBuff.bounds.Contains(Game1.getMouseX(), Game1.getMouseY())) {
                this.Monitor.Log("Botón de Buffs clickeado.", LogLevel.Info);
                menuBuff.ToggleMenu();
            }
            // Verificar si el botón de Level Up fue clickeado
            else if (e.Button == SButton.MouseLeft && buttonLevelUp.bounds.Contains(Game1.getMouseX(), Game1.getMouseY())) {
                this.Monitor.Log("Botón de Level up clickeado.", LogLevel.Info);
                menuSkills.ToggleMenu(); // Alternar la visibilidad del menú de habilidades
            }
            else {
                // Manejar clics en los botones del menú Buff
                menuBuff.HandleButtonClick(e, buffController);
            }
            // Manejar clics en los botones del menú Skills
            if (menuSkills.IsMenuVisible) { // Asegurarse de que el menú esté visible
                menuSkills.HandleButtonClick(e, levelUpController);
            }
            if (e.Button == SButton.MouseLeft && closeButton.bounds.Contains(Game1.getMouseX(), Game1.getMouseY())) {
                Monitor.Log("Cerrar menú.", LogLevel.Info);
                Game1.activeClickableMenu = null; // Cierra el menú
                ToggleMenu(); // Cierra el menu
            }
        }

        public void RenderMenus(SpriteBatch spriteBatch) {
            // Renderizar el menú de Buffs si está visible
            if (menuBuff.IsMenuVisible) {
                menuBuff.RenderMenu(spriteBatch);
            }

            // Renderizar el menú de Skills si está visible
            if (menuSkills.IsMenuVisible) {
                menuSkills.RenderMenu(spriteBatch);
            }
        }

        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e) {
            // Detectar cambio en el tamaño de la ventana
            if (Game1.viewport.Width != lastViewportWidth || Game1.viewport.Height != lastViewportHeight) {
                UpdateLayout();
            }
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
    }
}