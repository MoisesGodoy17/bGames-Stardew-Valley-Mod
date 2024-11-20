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

        // Control para mostrar/ocultar el menú
        private bool isMenuVisible = false;
        public bool IsMenuVisible => isMenuVisible;

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

            miningLevelUp = new ClickableComponent(
                new Rectangle(
                    positionMenuBg.X + 70,
                    buttonY,
                    buttonWidth,
                    buttonHeight
                ), "Mininglevel up\n10 pts"

            );
            foragingLevelUp = new ClickableComponent(
                new Rectangle(
                    positionMenuBg.X + 70 + buttonWidth + buttonSpacing,
                    buttonY,
                    buttonWidth,
                    buttonHeight
                ), "Foraging\nlevel up\n10pts"
            );
            // Suscribir al evento ButtonPressed para escuchar la tecla F3
            this.Helper.Events.Input.ButtonPressed += OnButtonPressedMenuSkill;
            Game1.mouseCursor = 0;
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
                Utility.drawTextWithShadow(spriteBatch, miningLevelUp.name, Game1.dialogueFont, new Vector2(miningLevelUp.bounds.X, miningLevelUp.bounds.Y), Color.White);
                Utility.drawTextWithShadow(spriteBatch, foragingLevelUp.name, Game1.dialogueFont, new Vector2(foragingLevelUp.bounds.X, foragingLevelUp.bounds.Y), Color.White);
            }
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
