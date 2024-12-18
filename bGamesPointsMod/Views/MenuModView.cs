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

        // Menu backgound
        public Texture2D menuBg;
        public Rectangle positionMenuBg;

        // Controlador de Buffs
        public BuffController buffController;

        // Usuario
        public UserBgamesModel userBgamesModel;

        // Controller del usaurio
        public UserBgamesController userBgamesController;

        // Controlador level up
        public LevelUpController levelUpController;

        // Botones inferiores
        private ClickableComponent buttonBuff;
        private ClickableComponent buttonLevelUp;

        // Control para mostrar/ocultar el men�
        private bool isMenuVisible = false;

        // Menu de buff y skills
        MenuBuffView menuBuff;
        MenuSkillsView menuSkills;


        public MenuModView(
            IModHelper helper, 
            BuffController buffController, 
            IMonitor monitor, 
            UserBgamesModel userBgamesModel,
            UserBgamesController userBgamesController,
            LevelUpController levelUpController)
        {
            // Inicializar Buffs y Helper
            this.Helper = helper ?? throw new ArgumentNullException(nameof(helper));
            this.buffController = buffController ?? throw new ArgumentNullException(nameof(buffController));
            this.Monitor = monitor;
            this.userBgamesModel = userBgamesModel;
            this.userBgamesController = userBgamesController;
            this.levelUpController = levelUpController;


            // Cargar el fondo del men�
            menuBg = helper.ModContent.Load<Texture2D>("assets/menubg.png");

            // Configuraci�n de tama�o y posici�n del men�
            int menuWidth = 640; // Ancho del fondo del men�
            int menuHeight = 480; // Alto del fondo del men�

            // Centrando el men� en la pantalla
            positionMenuBg = new Rectangle(
                (Game1.viewport.Width - menuWidth) / 2, // X centrado
                (Game1.viewport.Height - menuHeight) / 2, // Y centrado
                menuWidth,
                menuHeight
            );
            // Inicializar botones y colocarlos uno al lado del otro en la parte inferior del men�
            int buttonWidth = 100;
            int buttonHeight = 40;
            int buttonSpacing = 10; // Espacio entre los botones

            // Coordenadas para los botones en la parte inferior
            int buttonY = positionMenuBg.Y + menuHeight - buttonHeight - buttonSpacing;

            buttonBuff = new ClickableComponent(
                new Rectangle(
                    positionMenuBg.X + (menuWidth - 2 * buttonWidth - buttonSpacing) / 2, // X para el primer bot�n
                    buttonY,
                    buttonWidth,
                    buttonHeight
                ), "Buff"
            );
            buttonLevelUp = new ClickableComponent(
                new Rectangle(
                    buttonBuff.bounds.X + buttonWidth + buttonSpacing, // X para el segundo bot�n a la derecha del primero
                    buttonY,
                    buttonWidth,
                    buttonHeight
                ), "Level Up"
            );
            Game1.mouseCursor = 0;
            this.userBgamesModel = userBgamesModel;

            // Instancia del menu de buff
            menuBuff = new MenuBuffView(
                helper, 
                buffController, 
                Monitor, 
                userBgamesModel,
                userBgamesController);

            menuSkills = new MenuSkillsView(
                helper,
                Monitor,
                userBgamesModel,
                userBgamesController,
                levelUpController);
        }

        public void ToggleMenu()
        {
            isMenuVisible = !isMenuVisible;            
        }

        public void RenderMenu(SpriteBatch spriteBatch)
        {
            if (isMenuVisible)
            {
                // Dibujar fondo del men� solo si est� visible
                spriteBatch.Draw(menuBg, positionMenuBg, Color.White);

                // Dibujar botones inferiores
                Utility.drawTextWithShadow(spriteBatch, buttonBuff.name, Game1.dialogueFont, new Vector2(buttonBuff.bounds.X, buttonBuff.bounds.Y), Color.White);
                Utility.drawTextWithShadow(spriteBatch, buttonLevelUp.name, Game1.dialogueFont, new Vector2(buttonLevelUp.bounds.X, buttonLevelUp.bounds.Y), Color.White);
            }
            if (userBgamesModel != null) // Si el usuario no es nulo, muestra la informaci�n del usuario
            {
                string userInfo = $"Name: {userBgamesModel.Name}\nEmail: {userBgamesModel.Email}\nAge: {userBgamesModel.Age}";

                // Coordenadas para dibujar el texto en la parte superior del men�
                float textX = positionMenuBg.X + 20; // Margen desde la izquierda
                float textY = positionMenuBg.Y + 20; // Margen desde la parte superior

                spriteBatch.DrawString(Game1.smallFont, userInfo, new Vector2(textX, textY), Color.Black);
            }

            if (userBgamesModel.Points != null)
            {
                string userPointsInfo = "";
                foreach (var points in userBgamesModel.Points)
                {
                    userPointsInfo += $"{points.Name}: {points.Data}\n";
                }
                // Coordenadas para dibujar el texto en la parte superior del men�
                float textX = positionMenuBg.X + 400; // Margen desde la izquierda
                float textY = positionMenuBg.Y + 20; // Margen desde la parte superior

                spriteBatch.DrawString(Game1.smallFont, userPointsInfo, new Vector2(textX, textY), Color.Black);
            }
            if (menuBuff != null)
            {
                menuBuff.RenderMenu(spriteBatch);
            }
            if (menuSkills != null)
            {
                menuSkills.RenderMenu(spriteBatch);
            }
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