using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using StardewModdingAPI;
using bGamesPointsMod.Controllers;
using StardewModdingAPI.Events;

namespace bGamesPointsMod.Views
{
    public class LoginView : IClickableMenu
    {
        private readonly UserBgamesController userController;
        private readonly IMonitor Monitor;
        private readonly IModHelper Helper;

        private TextBox emailTextBox;
        private TextBox passwordTextBox;
        private ClickableComponent loginButton;
        private string message = "";

        private int lastViewportWidth;
        private int lastViewportHeight;

        public LoginView(UserBgamesController userController, IMonitor monitor, IModHelper helper)
            : base(0, 0, Game1.viewport.Width, Game1.viewport.Height) {
            this.userController = userController;
            this.Monitor = monitor;
            Helper = helper;

            lastViewportWidth = Game1.viewport.Width;
            lastViewportHeight = Game1.viewport.Height;

            // Inicializar campos de texto y botón
            UpdateLayout();

            // Suscribirse al evento de redimensionamiento
            this.Helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
            Game1.keyboardDispatcher.Subscriber = null; // Asegurarte de no tener suscriptores previos
            this.Helper.Events.Input.ButtonPressed += OnButtonPressed;
        }

        private void UpdateLayout() {
            // Tamaño fijo del cuadro de diálogo
            int dialogWidth = 600; // Ancho fijo
            int dialogHeight = 400; // Alto fijo

            // Posición centrada del cuadro de diálogo
            int dialogX = (Game1.viewport.Width - dialogWidth) / 2;
            int dialogY = (Game1.viewport.Height - dialogHeight) / 2;

            // Tamaño fijo para los campos de texto
            int textBoxWidth = 420;
            int textBoxHeight = 40; // Alto fijo de los TextBox
            int textBoxSpacing = 20; // Espaciado entre los TextBox

            // Configurar el campo de texto para el email
            emailTextBox = new TextBox(Game1.content.Load<Texture2D>("LooseSprites\\textBox"), Game1.mouseCursors, Game1.smallFont, Game1.textColor) {
                X = dialogX + (dialogWidth - textBoxWidth) / 2,
                Y = dialogY + 150,
                Width = textBoxWidth,
                Height = textBoxHeight,
                textLimit = 50,
                Selected = true
            };
            emailTextBox.Text = "Email";

            // Configurar el campo de texto para la contraseña
            passwordTextBox = new TextBox(Game1.content.Load<Texture2D>("LooseSprites\\textBox"), Game1.mouseCursors, Game1.smallFont, Game1.textColor) {
                X = dialogX + (dialogWidth - textBoxWidth) / 2,
                Y = emailTextBox.Y + textBoxHeight + textBoxSpacing,
                Width = textBoxWidth,
                Height = textBoxHeight,
                textLimit = 50,
                PasswordBox = true
            };
            passwordTextBox.Text = "Password";

            // Tamaño y posición fija para el botón de login
            int buttonWidth = 150; // Ancho fijo del botón
            int buttonHeight = 40; // Alto fijo del botón
            loginButton = new ClickableComponent(
                new Rectangle(
                    dialogX + 30 + (dialogWidth - buttonWidth) / 2 ,
                    passwordTextBox.Y + textBoxHeight + textBoxSpacing,
                    buttonWidth,
                    buttonHeight
                ), "Login"
            );
        }


        public override void draw(SpriteBatch b) {
            // Tamaño fijo del cuadro de diálogo
            int dialogWidth = 600; // Ancho fijo
            int dialogHeight = 400; // Alto fijo

            // Posición centrada del cuadro de diálogo
            int dialogX = (Game1.viewport.Width - dialogWidth) / 2;
            int dialogY = (Game1.viewport.Height - dialogHeight) / 2;

            // Dibujar cuadro de diálogo
            Game1.drawDialogueBox(dialogX, dialogY, dialogWidth, dialogHeight, false, true);

            // Dibujar campos de texto
            emailTextBox.Draw(b);
            passwordTextBox.Draw(b);

            // Dibujar botón de login
            Utility.drawTextWithShadow(b, loginButton.name, Game1.dialogueFont, new Vector2(loginButton.bounds.X, loginButton.bounds.Y), Color.White);

            // Dibujar mensaje de resultado
            if (!string.IsNullOrEmpty(message)) {
                Utility.drawTextWithShadow(b, message, Game1.smallFont, new Vector2(dialogX + 20, dialogY + dialogHeight - 50), Color.Yellow);
            }

            // Dibujar el cursor del mouse
            this.drawMouse(b);
            base.draw(b);
        }

        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e) {
            // Detectar si el tamaño de la ventana ha cambiado
            if (Game1.viewport.Width != lastViewportWidth || Game1.viewport.Height != lastViewportHeight) {
                lastViewportWidth = Game1.viewport.Width;
                lastViewportHeight = Game1.viewport.Height;

                // Recalcular las posiciones de los elementos
                UpdateLayout();
            }
        }

        

        public override void receiveLeftClick(int x, int y, bool playSound = true) {
            base.receiveLeftClick(x, y, playSound);

            // Seleccionar el TextBox adecuado en función del clic
            if (new Rectangle(emailTextBox.X, emailTextBox.Y, emailTextBox.Width, emailTextBox.Height).Contains(x, y)) {
                Game1.keyboardDispatcher.Subscriber = emailTextBox;
                emailTextBox.Selected = true;
                passwordTextBox.Selected = false;
            }
            else if (new Rectangle(passwordTextBox.X, passwordTextBox.Y, passwordTextBox.Width, passwordTextBox.Height).Contains(x, y)) {
                Game1.keyboardDispatcher.Subscriber = passwordTextBox;
                passwordTextBox.Selected = true;
                emailTextBox.Selected = false;
            }

            // Verificar si se ha hecho clic en el botón de login
            if (loginButton.bounds.Contains(x, y)) {
                PerformLogin();
            }
        }

        private async void PerformLogin() {
            string email = emailTextBox.Text;
            string password = passwordTextBox.Text;
            int loginResult = await userController.UserCheck(email, password);

            if (loginResult == 1) {
                userController.SaveUserBgames(email);
                //Game1.addHUDMessage(new HUDMessage("Inicio de sesion exitoso!", 2));
                Game1.addHUDMessage(new HUDMessage("Login successful!", 2));
            }
            else {
                //Game1.addHUDMessage(new HUDMessage("Error al iniciar sesion, la contraseña o el correo son incorrectos!", 2));
                Game1.addHUDMessage(new HUDMessage("Error logging in, the password or email is incorrect!", 2));
            }
        }

        private void OnButtonPressed(object sender, ButtonPressedEventArgs e) {
            // Verifica si estamos en un campo de texto activo
            if (emailTextBox.Selected || passwordTextBox.Selected) {
                // Cancelar la acción de la tecla "e" mientras se escribe
                if (e.Button == SButton.E) {
                    Helper.Input.Suppress(SButton.E); // Cancela la acción predeterminada para "e"
                    Monitor.Log("La tecla 'e' fue presionada mientras se escribía. Acción cancelada.", LogLevel.Debug);
                }
            }
        }
    }
}

