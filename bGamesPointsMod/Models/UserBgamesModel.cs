using bGamesPointsMod.Models;
namespace bGamesPointsMod.Models
{
    public class UserBgamesModel
    {
        public string Name { get; set; }              // Nombre del buff
        
        public string Email { get; set; }             // Email del usuario

        public string Password { get; set; }          // Contraseña del usuario
        
        public string Id_players { get; set; }

        public string Age { get; set; }

        public List<PointsBgamesModel> Points { get; set; } // Lista de puntos


        // Constructor para inicializar el buff
        public UserBgamesModel(string name, string email, string password, string id_players
            ,string age, List<PointsBgamesModel> points)
        {
            Name = name;
            Email = email;
            Password = password;
            Id_players = id_players;
            Age = age;
            Points = points ?? new List<PointsBgamesModel>();
        }
    }
}