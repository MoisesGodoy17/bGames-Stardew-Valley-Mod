using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Tools;
using bGamesPointsMod.Models;
using bGamesPointsMod.Controllers;
using Microsoft.Xna.Framework.Graphics;
using bGamesPointsMod.Views;

namespace bGamesPointsMod
{
    internal sealed class ModEntry : Mod
    {
        // Modelos de Buffs
        private BuffModel miningBuff;
        private BuffModel foraningBuff;
        private BuffModel speedBuff;
        public UserBgamesModel userBgamesModel;
        public PointsBgamesModel pointsBgamesModel;

        // Controlador de Buffs
        public BuffController buffController;
        public UserBgamesController userController;

        // Boton de menu de mod
        private Texture2D bTMenuMod;
        private Rectangle bBMenuMod;
        private MenuMod menu;// Instancia de la clase Menu

        public override void Entry(IModHelper helper)
        {
            // Inicializar Buffs
            miningBuff = new BuffModel("Velocidad de minado", 0.5f, 0, -1);
            foraningBuff = new BuffModel("Velocidad de tala", 0.5f, 0, -1);
            speedBuff = new BuffModel("Velocidad de caminata", 0.5f, 0, -1);

            // Mostrar boton en pantalla del menu
            bTMenuMod = helper.ModContent.Load<Texture2D>("assets/menu.png");
            bBMenuMod = new Rectangle(10, 10, 20, 20);


            // Crear instancia de BuffController
            pointsBgamesModel = new PointsBgamesModel("", "", "");
            userBgamesModel = new UserBgamesModel("","","","","", null);

            buffController = new BuffController(this.Monitor, this.Helper, miningBuff, foraningBuff, speedBuff);
            userController = new UserBgamesController(this.Monitor, helper, userBgamesModel, pointsBgamesModel);

            // Crear instancia de Menu
            menu = new MenuMod(helper, buffController, Monitor, userBgamesModel, userController);

            // Mostrar boton en pantalla
            helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            helper.Events.Display.RenderedHud += OnRenderedHud;
            helper.Events.Input.ButtonPressed += OnButtonPressedLogin;
        }

        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            // Verifica si el jugador tiene un pico equipado
            if (Game1.player.CurrentTool is Pickaxe pickaxe)
            {
                // Restauramos la velocidad de animación del pico a su valor por defecto (1.0)
                pickaxe.AnimationSpeedModifier = 1.0f;
                this.Monitor.Log("Nuevo día: Se restauró la velocidad de animación del pico a 1.0.", LogLevel.Info);
            }
        }

        private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady) return;

            if (e.Button == SButton.MouseLeft && bBMenuMod.Contains(Game1.getMouseX(), Game1.getMouseY()))
            {
                Monitor.Log("Botón de menú clickeado.", LogLevel.Info);
                menu.ToggleMenu();
            }
            else
            {
                menu.OnOpenMenuBuff(e, buffController);
            }
        }

        private void OnRenderedHud(object sender, RenderedHudEventArgs e)
        {
            var spriteBatch = e.SpriteBatch;

            spriteBatch.Draw(bTMenuMod, bBMenuMod, Color.White);
            menu.RenderMenu(spriteBatch);
        }

        private void OnButtonPressedLogin(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button == SButton.F5)
            {
                // Solo muestra LoginView en pantalla cuando se presiona F5
                Game1.activeClickableMenu = new LoginView(userController, this.Monitor);
            }
        }
    }
}
