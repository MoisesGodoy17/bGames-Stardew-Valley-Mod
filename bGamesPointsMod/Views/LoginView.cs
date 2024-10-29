using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using StardewModdingAPI;
using bGamesPointsMod.Controllers;

namespace bGamesPointsMod.Views
{
    public class LoginView : IClickableMenu
    {
        private readonly UserBgamesController userController;
        private readonly IMonitor Monitor;

        private TextBox emailTextBox;
        private TextBox passwordTextBox;
        private ClickableComponent loginButton;
        private string message = "";

        public LoginView(UserBgamesController userController, IMonitor monitor)
            : base(0, 0, Game1.viewport.Width, Game1.viewport.Height)
        {
            this.userController = userController;
            this.Monitor = monitor;

            // Configuración de campos de texto
            emailTextBox = new TextBox(Game1.content.Load<Texture2D>("LooseSprites\\textBox"), Game1.mouseCursors, Game1.smallFont, Game1.textColor)
            {
                X = this.width / 2 - 100,
                Y = this.height / 2 - 50,
                Width = 200
            };
            emailTextBox.Text = "Email";
            Game1.mouseCursor = 0;

            passwordTextBox = new TextBox(Game1.content.Load<Texture2D>("LooseSprites\\textBox"), Game1.mouseCursors, Game1.smallFont, Game1.textColor)
            {
                X = this.width / 2 - 100,
                Y = this.height / 2,
                Width = 200
            };
            Game1.mouseCursor = 0;
            passwordTextBox.Text = "Password";

            loginButton = new ClickableComponent(
                new Rectangle(this.width / 2 - 50, this.height / 2 + 60, 100, 40),
                "Login");
            Game1.mouseCursor = 0;
        }

        public override void draw(SpriteBatch b)
        {
            Game1.mouseCursor = 0;
            // Dibuja el fondo y los componentes
            Game1.drawDialogueBox(this.width / 2 - 150, this.height / 2 - 100, 300, 200, false, true);
            emailTextBox.Draw(b);
            passwordTextBox.Draw(b);
            Game1.mouseCursor = 0;


            // Dibuja el botón de login
            Utility.drawTextWithShadow(b, loginButton.name, Game1.dialogueFont, new Vector2(loginButton.bounds.X, loginButton.bounds.Y), Color.White);
            Game1.mouseCursor = 0;
            // Mensaje de resultado
            if (!string.IsNullOrEmpty(message))
            {
                Utility.drawTextWithShadow(b, message, Game1.smallFont, new Vector2(this.width / 2 - 100, this.height / 2 + 100), Color.Yellow);
            }
            Game1.mouseCursor = 0;
            this.drawMouse(b);
            base.draw(b);
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            base.receiveLeftClick(x, y, playSound);

            // Seleccionar el TextBox adecuado en función del clic
            if (new Rectangle(emailTextBox.X, emailTextBox.Y, emailTextBox.Width, emailTextBox.Height).Contains(x, y))
            {
                Game1.keyboardDispatcher.Subscriber = emailTextBox;
                emailTextBox.Selected = true;
                passwordTextBox.Selected = false;
            }
            else if (new Rectangle(passwordTextBox.X, passwordTextBox.Y, passwordTextBox.Width, passwordTextBox.Height).Contains(x, y))
            {
                Game1.keyboardDispatcher.Subscriber = passwordTextBox;
                passwordTextBox.Selected = true;
                emailTextBox.Selected = false;
            }

            // Verificar si se ha hecho clic en el botón de login
            if (loginButton.bounds.Contains(x, y))
            {
                PerformLogin();
            }
        }

        private async void PerformLogin()
        {
            string email = emailTextBox.Text;
            string password = passwordTextBox.Text;
            int loginResult = await userController.UserCheck(email, password);

            if (loginResult == 1)
            {
                userController.SaveUserBgames(email, password);
                message = "Login exitoso! Bienvenido.";
            }
            else
            {
                message = "Login fallido. Intenta nuevamente.";
            }
        }
    }
}

