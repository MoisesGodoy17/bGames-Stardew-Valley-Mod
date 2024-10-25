namespace bGamesPointsMod.Models
{
    public class UserBgamesModel
    {
        public string Name { get; set; }              // Nombre del buff
        
        public string Email { get; set; }             // Email del usuario

        public string Password { get; set; }          // Contraseña del usuario

        public List<Array> Points { get; set; }    // Lista de puntos del usuario

        // Constructor para inicializar el buff
        public UserBgamesModel(string name, string email, string password, List<Array> points)
        {
            Name = name;
            Email = email;
            Password = password;
            Points = points;
        }
    }
}