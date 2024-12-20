using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Tools;
using bGamesPointsMod.Models;

namespace bGamesPointsMod.Controllers
{
    public class MenuModView : IClickableMenu
    {
        private readonly IMonitor Monitor;
        private readonly IModHelper Helper;

        // Fondo del men�
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

        // Control para mostrar/ocultar el men�
        private bool isMenuVisible = false;

        // Men�s de Buff y Skills
        private MenuBuffView menuBuff;
        private MenuSkillsView menuSkills;

        public MenuModView(
            IModHelper helper,
            BuffController buffController,
            IMonitor monitor,
            UserBgamesModel userBgamesModel,
            UserBgamesController userBgamesController,
            LevelUpController levelUpController)
        {
            // Inicializaci�n
            this.Helper = helper ?? throw new ArgumentNullException(nameof(helper));
            this.buffController = buffController ?? throw new ArgumentNullException(nameof(buffController));
            this.Monitor = monitor;
            this.userBgamesModel = userBgamesModel;
            this.userBgamesController = userBgamesController;
            this.levelUpController = levelUpController;

            // Cargar el fondo del men�
            menuBg = helper.ModContent.Load<Texture2D>("assets/menubg.png");

            // Configuraci�n del tama�o y posici�n del men�
            int menuWidth = 640, menuHeight = 480;
            positionMenuBg = new Rectangle(
                (Game1.viewport.Width - menuWidth) / 2,
                (Game1.viewport.Height - menuHeight) / 2,
                menuWidth,
                menuHeight
            );

            // Configuraci�n de botones
            int buttonWidth = 150, buttonHeight = 40, buttonSpacing = 10;
            int buttonY = positionMenuBg.Y + menuHeight - buttonHeight - buttonSpacing;

            buttonBuff = new ClickableComponent(
                new Rectangle(positionMenuBg.X + (menuWidth - 2 * buttonWidth - buttonSpacing) / 2,
                              buttonY, buttonWidth, buttonHeight), "Buff");

            buttonLevelUp = new ClickableComponent(
                new Rectangle(buttonBuff.bounds.X + buttonWidth + buttonSpacing,
                              buttonY, buttonWidth, buttonHeight), "Level Up");

            // Instancia de los men�s
            menuBuff = new MenuBuffView(helper, buffController, monitor, userBgamesModel, userBgamesController);
            menuSkills = new MenuSkillsView(helper, monitor, userBgamesModel, userBgamesController, levelUpController);
        }

        public void ToggleMenu()
        {
            isMenuVisible = !isMenuVisible;            
        }

        public void RenderMenu(SpriteBatch spriteBatch)
        {
            if (isMenuVisible)
            {
                // Dibujar el fondo del men�
                spriteBatch.Draw(menuBg, positionMenuBg, Color.White);

                // Dibujar botones inferiores
                DrawButton(spriteBatch, buttonBuff.bounds, buttonBuff.name, Color.Khaki, Color.White, Color.Brown);
                DrawButton(spriteBatch, buttonLevelUp.bounds, buttonLevelUp.name, Color.Khaki, Color.White, Color.Brown);
                

                // Dibujar informaci�n del usuario
                if (userBgamesModel != null)
                {
                    string userInfo = $"Name: {userBgamesModel.Name}\nEmail: {userBgamesModel.Email}\nAge: {userBgamesModel.Age}";
                    spriteBatch.DrawString(Game1.smallFont, userInfo, new Vector2(positionMenuBg.X + 20, positionMenuBg.Y + 20), Color.Black);
                }

                // Renderizar men�s Buff y Skills si est�n visibles
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

        public void DrawButton(SpriteBatch spriteBatch, Rectangle bounds, string text, Color buttonColor, Color borderColor, Color textColor)
        {
            int borderThickness = 2;

            // Dibujar el fondo del bot�n
            spriteBatch.Draw(Game1.staminaRect, bounds, buttonColor);

            // Dibujar bordes del bot�n
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

        public void OnOpenMenuBuff(ButtonPressedEventArgs e, BuffController buffController, LevelUpController levelUpController)
        {
            // Verificar si el bot�n de Buffs fue clickeado
            if (e.Button == SButton.MouseLeft && buttonBuff.bounds.Contains(Game1.getMouseX(), Game1.getMouseY()))
            {
                this.Monitor.Log("Bot�n de Buffs clickeado.", LogLevel.Info);
                menuBuff.ToggleMenu();
            }
            // Verificar si el bot�n de Level Up fue clickeado
            else if (e.Button == SButton.MouseLeft && buttonLevelUp.bounds.Contains(Game1.getMouseX(), Game1.getMouseY()))
            {
                this.Monitor.Log("Bot�n de Level up clickeado.", LogLevel.Info);
                menuSkills.ToggleMenu(); // Alternar la visibilidad del men� de habilidades
            }
            else
            {
                // Manejar clics en los botones del men� Buff
                menuBuff.HandleButtonClick(e, buffController);
            }

            // Manejar clics en los botones del men� Skills
            if (menuSkills.IsMenuVisible) // Asegurarse de que el men� est� visible
            {
                menuSkills.HandleButtonClick(e, levelUpController);
            }
        }

        public void RenderMenus(SpriteBatch spriteBatch)
        {
            // Renderizar el men� de Buffs si est� visible
            if (menuBuff.IsMenuVisible)
            {
                menuBuff.RenderMenu(spriteBatch);
            }

            // Renderizar el men� de Skills si est� visible
            if (menuSkills.IsMenuVisible)
            {
                menuSkills.RenderMenu(spriteBatch);
            }
        }
    }
}