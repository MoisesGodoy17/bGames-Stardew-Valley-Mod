
using StardewModdingAPI;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json; // Necesitarás instalar este paquete para trabajar con JSON.
using bGamesPointsMod.Models;

namespace bGamesPointsMod.Controllers
{
    public class UserBgamesController
    {
        private UserBgamesModel userBgamesModel;
        private PointsBgamesModel pointsBgamesModel;
        private readonly IMonitor Monitor;
        private readonly IModHelper Helper;
        private readonly HttpClient httpClient; // Cliente HTTP para hacer las peticiones a la API externa

        public UserBgamesController(
            IMonitor monitor,
            IModHelper helper,  //Helper
            UserBgamesModel userBgamesModel,
            PointsBgamesModel pointsBgamesModel)
        {
            this.Monitor = monitor;
            this.Helper = helper;
            this.userBgamesModel = userBgamesModel;
            this.pointsBgamesModel = pointsBgamesModel;
            this.httpClient = new HttpClient(); // Inicializa el cliente HTTP
        }

        public async Task<int> UserCheck(string email, string password)
        {
            // URL de la API externa que se usará para consultar por email
            string apiUrl = $"http://localhost:3010/player_by_email/{email}";
            try
            {
                // Realiza la llamada a la API para verificar si el usuario existe
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                string jsonResponse = await response.Content.ReadAsStringAsync();
                userBgamesModel = JsonConvert.DeserializeObject<UserBgamesModel>(jsonResponse);

                if (response.IsSuccessStatusCode && (userBgamesModel.Password == password))
                {
                    Monitor.Log($"Usuario {password} guardado correctamente.", LogLevel.Info);
                    Monitor.Log($"Usuario {userBgamesModel.Password} guardado correctamente.", LogLevel.Info);
                    // Si la respuesta es exitosa, obtenemos el JSON y lo convertimos en el modelo
                    Monitor.Log("Usuario encontrado en la API.", LogLevel.Info);
                    return 1; // Usuario encontrado
                }
                else
                {
                    Monitor.Log("Usuario no encontrado en la API.", LogLevel.Info);
                    return 0; // Usuario no encontrado
                }
            }
            catch (Exception ex)
            {
                Monitor.Log($"Error al consultar la API: {ex.Message}", LogLevel.Error);
                return 0; // Error durante la llamada a la API
            }
        }

        public async void SaveUserBgames(string email, string password)
        {
            string apiUrl = $"http://localhost:3010/player_by_email/{email}";
            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
            string jsonResponse = await response.Content.ReadAsStringAsync();
            if (response != null)
            {
                userBgamesModel = JsonConvert.DeserializeObject<UserBgamesModel>(jsonResponse);
                Monitor.Log($"Usuario {userBgamesModel.Name} guardado correctamente.", LogLevel.Info);
            }
            else
            {
                Monitor.Log("No se pudo guardar el usuario porque no hay datos disponibles.", LogLevel.Warn);
            }
        }

        public async void SaveUserPoints()
        {
            string apiUrl = $"http://localhost:3001/player_all_attributes/{userBgamesModel.Id_players}";
            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
            string jsonResponse = await response.Content.ReadAsStringAsync();
            if (response != null)
            {
                List<PointsBgamesModel> pointsBgames = JsonConvert.DeserializeObject<List<PointsBgamesModel>>(jsonResponse);
                Monitor.Log($"Puntos del usuario guardados correctamente.", LogLevel.Info);
                userBgamesModel.Points = pointsBgames;
                if (userBgamesModel.Points == null)
                {
                    Monitor.Log("No se pudo guardar el puntaje del usuario porque no hay datos disponibles.", LogLevel.Warn);
                }
                foreach (var points in userBgamesModel.Points)
                {
                    Console.WriteLine($"  - {points.Name}: {points.Data}");
                }
            }
            else
            {
                Monitor.Log("No se pudo guardar el puntaje del usuario porque no hay datos disponibles.", LogLevel.Warn);
            }
        }

        public UserBgamesModel GetUser()
        {
            return userBgamesModel;
        }
    }
}


