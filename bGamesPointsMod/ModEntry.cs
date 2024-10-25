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

namespace bGamesPointsMod
{
    internal sealed class ModEntry : Mod
    {
        // Modelos de Buffs
        private BuffModel miningBuff;
        private BuffModel foraningBuff;
        private BuffModel speedBuff;
        private UserBgamesModel userBgamesModel;

        // Controlador de Buffs
        public BuffController buffController;

        private UserBgamesController userController;

        // Boton de menu de mod
        private Texture2D bTMenuMod;
        private Rectangle bBMenuMod;

        private Menu menu;  // Instancia de la clase Menu

        public override void Entry(IModHelper helper) // Función principal
        {
            // Inicializar Buffs
            miningBuff = new BuffModel("Velocidad de minado", 0.5f, 0, -1);
            foraningBuff = new BuffModel("Velocidad de tala", 0.5f, 0, -1);
            speedBuff = new BuffModel("Velocidad de caminata", 0.5f, 0, -1);

            // Mostrar boton en pantalla del menu
            bTMenuMod = helper.ModContent.Load<Texture2D>("assets/menu.png");
            bBMenuMod = new Rectangle(10, 10, 20, 20);

            // Crear instancia de BuffController
            buffController = new BuffController(this.Monitor, this.Helper, miningBuff, foraningBuff, speedBuff);
            userController = new UserBgamesController(this.Monitor, helper, userBgamesModel);

            // Crear instancia de Menu
            menu = new Menu(helper, buffController);

            // Mostrar boton en pantalla
            helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            helper.Events.Display.RenderedHud += OnRenderedHud;

            //Evento login
            helper.Events.Input.ButtonPressed += OnButtonPressedLogin;
        }
        private void OnDayStarted(object sender, DayStartedEventArgs e) // Funcion que restaura el buff en caso de irse a dormir con un buff activo
        {
            // Verifica si el jugador tiene un pico equipado
            if (Game1.player.CurrentTool is Pickaxe pickaxe)
            {
                // Restauramos la velocidad de animación del pico a su valor por defecto (1.0)
                pickaxe.AnimationSpeedModifier = 1.0f;
                this.Monitor.Log("Nuevo día: Se restauró la velocidad de animación del pico a 0.0.", LogLevel.Info);
            }
        }

        private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady) return;

            this.Monitor.Log($"{Game1.player.Name} pressed {e.Button}.", LogLevel.Debug);

            if (e.Button == SButton.MouseLeft && bBMenuMod.Contains(Game1.getMouseX(), Game1.getMouseY()))
            {
                Monitor.Log("Botón de menú clickeado.", LogLevel.Info);
                menu.ToggleMenu(); // Mostrar/Ocultar 
            }
            else
            {
                menu.HandleButtonClick(e, buffController); // Manejar clic en los botones del menú
            }
        }

        private void OnRenderedHud(object sender, RenderedHudEventArgs e)
        {
            var spriteBatch = e.SpriteBatch;

            // Dibujar el botón del menú
            spriteBatch.Draw(bTMenuMod, bBMenuMod, Color.White);

            // Renderizar el menú si está visible
            menu.RenderMenu(spriteBatch);
        }

        // Método de login que se llama al presionar una tecla específica
        private async void OnButtonPressedLogin(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button == SButton.F5) // Por ejemplo, F5 para activar el login
            {
                // Simulación de credenciales (podrías pedirlas de otro modo)
                string email = "test@test.cl";
                string password = "asd123";

                int loginResult = await userController.UserCheck(email, password);

                if (loginResult == 1)
                {
                    Monitor.Log("Login exitoso.", LogLevel.Info);
                    var user = userController.GetUser();
                    Monitor.Log($"Bienvenido, {user.Name}!", LogLevel.Info);
                }
                else
                {
                    Monitor.Log("Login fallido. Usuario no encontrado o error en la API.", LogLevel.Warn);
                }
            }
        }
    }
}
