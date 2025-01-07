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
using StardewValley.Buffs;

namespace bGamesPointsMod
{
    internal sealed class ModEntry : Mod
    {
        // Modelos de Buffs
        public UserBgamesModel userBgamesModel;
        public PointsBgamesModel pointsBgamesModel;
        public LevelUpModel miningSkill;
        public LevelUpModel foraningSkill;
        public LevelUpModel fishingSkill;
        public LevelUpModel combatSkill;
        public LevelUpModel luckSkill;
        int CheckConnection;

        // Controlador de Buffs
        public BuffController buffController;
        public UserBgamesController userController;
        public LevelUpController levelUpController;

        // Boton de menu de mod
        private Texture2D bTMenuMod;
        private Rectangle bBMenuMod;
        private MenuModView menu;// Instancia de la clase Menu

        public override void Entry(IModHelper helper)
        {
            // Inicializar Buffs
            Buff miningBuff = new Buff(id: "Mining speed",
                displayName: "Speed mining buff",
                iconTexture: null,
                iconSheetIndex: 0,
                duration: 10_000, // 10 segundos
                effects: new BuffEffects()
                {MiningLevel = { 10 }});

            Buff foragingBuff = new Buff(
                id: "Foraging speed",
                displayName: "Speed foraging buff",
                iconTexture: null,
                iconSheetIndex: 0,
                duration: 10_000, // 10 segundos
                effects: new BuffEffects()
                {ForagingLevel = { 10 }});

            Buff speedBuff = new Buff(
                id: "walking speed",
                displayName: "Walking speed buff",
                iconTexture: null,
                iconSheetIndex: 0,
                duration: 10_000, // 10 segundos
                effects: new BuffEffects()
                {Speed = { 10 }});

            Buff reducedEnergyBuff = new Buff(
                id: "Reduced Energy",
                displayName: "Speed mining buff",
                iconTexture: null,
                iconSheetIndex: 0,
                duration: 30_000); // 10 segundos

            Buff luckLevelBuff = new Buff(id: "Luck level up",
                displayName: "Luck level up",
                iconTexture: null,
                iconSheetIndex: 0,
                duration: 10_000, // 10 segundos
                effects: new BuffEffects()
                {LuckLevel = { 1 }});

            Buff fishingBuff = new Buff(id: "Fishing speed",
                displayName: "Fishing speed buff",
                iconTexture: null,
                iconSheetIndex: 0,
                duration: 10_000, // 10 segundos
                effects: new BuffEffects()
                {FishingLevel = { 5 }});

            Buff farmingBuff = new Buff(id: "Farming speed",
                displayName: "Speed farming buff",
                iconTexture: null,
                iconSheetIndex: 0,
                duration: 10_000, // 10 segundos
                effects: new BuffEffects()
                {FarmingLevel = { 5 }});

            // Inicializar modelos de level up
            miningSkill = new LevelUpModel(100, "Mining");
            foraningSkill = new LevelUpModel(100, "Foraging");
            fishingSkill = new LevelUpModel(100, "Fishing");
            combatSkill = new LevelUpModel(100, "Combat");
            luckSkill = new LevelUpModel(100, "Luck");


            // Mostrar boton en pantalla del menu
            bTMenuMod = helper.ModContent.Load<Texture2D>("assets/menu.png");
            bBMenuMod = new Rectangle(10, 10, 20, 20);

            // Crear instancia de BuffController
            pointsBgamesModel = new PointsBgamesModel("", "", "");
            userBgamesModel = new UserBgamesModel("","","","","", null);
            buffController = new BuffController(
                this.Monitor, 
                this.Helper, 
                miningBuff, 
                foragingBuff, 
                speedBuff, 
                reducedEnergyBuff,
                luckLevelBuff,
                fishingBuff,
                farmingBuff);
            userController = new UserBgamesController(this.Monitor, helper, userBgamesModel, pointsBgamesModel);

            // Crear instancia de LevelUpController
            levelUpController = new LevelUpController(this.Monitor, this.Helper, miningSkill, foraningSkill, fishingSkill, combatSkill, luckSkill);

            // Crear instancia de Menu
            menu = new MenuModView(helper, buffController, Monitor, userBgamesModel, userController, levelUpController);

            

            // Mostrar boton en pantalla
            helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            helper.Events.Display.RenderedHud += OnRenderedHud;
            //helper.Events.Input.ButtonPressed += OnButtonPressedLogin;
        }
        private async void OnDayStarted(object sender, DayStartedEventArgs e) {
            CheckConnection = await userController.CheckConnectionBgames();

            // Verifica si el jugador tiene un pico equipado
            if (Game1.player.CurrentTool is Pickaxe pickaxe) {
                // Restauramos la velocidad de animación del pico a su valor por defecto (1.0)
                pickaxe.AnimationSpeedModifier = 1.0f;
                this.Monitor.Log("Nuevo día: Se restauró la velocidad de animación del pico a 1.0.", LogLevel.Info);
            }
            if (CheckConnection == 1) {
                this.Monitor.Log("Conexion extablecida!", LogLevel.Info);
                if (userController.CredentialsFileExists()) {
                    this.Monitor.Log("Ya existe un usario!", LogLevel.Info);
                }
                else {
                    this.Monitor.Log("No existe un usario!", LogLevel.Info);
                }
            }
            else {
                this.Monitor.Log("Sin conexion con bGames!", LogLevel.Info);
            }
        }
        private async void OnButtonPressed(object? sender, ButtonPressedEventArgs e) {
            CheckConnection = await userController.CheckConnectionBgames();

            if (!Context.IsWorldReady) return;

            if (e.Button == SButton.MouseLeft && bBMenuMod.Contains(Game1.getMouseX(), Game1.getMouseY())) {
                if (CheckConnection == 1) {
                    this.Monitor.Log("Conexion extablecida!", LogLevel.Info);
                    if (userController.CredentialsFileExists()) {
                        userController.LoadCredentials(); // Carga las credenciales guardadas en el json y las asigna al modelo bGames
                        this.Monitor.Log("Ya existe un usario!", LogLevel.Info);
                        Game1.addHUDMessage(new HUDMessage("Conexion con bGames establecida!", 2));
                        Monitor.Log("Botón de menú clickeado.", LogLevel.Info);
                        menu.ToggleMenu();
                    }
                    else {
                        // Solo muestra LoginView en pantalla cuando no existe un jugador
                        OnButtonPressedLogin(sender, e);
                        this.Monitor.Log("No existe un usario!", LogLevel.Info);
                    }
                }
                else {
                    this.Monitor.Log("Sin conexion con bGames!", LogLevel.Info);
                    Game1.addHUDMessage(new HUDMessage("Sin conexion con bGames!", 2));
                    Game1.addHUDMessage(new HUDMessage("Inicia los servicios de bGames para continuar!", 2));
                }
            }
            else {
                menu.OnOpenMenuBuff(e, buffController, levelUpController);
            }
        }
        private void OnRenderedHud(object sender, RenderedHudEventArgs e) {
            var spriteBatch = e.SpriteBatch;

            spriteBatch.Draw(bTMenuMod, bBMenuMod, Color.White);
            menu.RenderMenu(spriteBatch);
        }
        private void OnButtonPressedLogin(object sender, ButtonPressedEventArgs e) {
            // Solo muestra LoginView en pantalla cuando no existe un jugador
            Game1.activeClickableMenu = new LoginView(userController, this.Monitor, this.Helper);
        }
    }
}
