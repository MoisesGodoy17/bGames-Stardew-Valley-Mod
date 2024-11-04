
using StardewModdingAPI;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json; // Necesitarás instalar este paquete para trabajar con JSON.
using bGamesPointsMod.Models;

namespace bGamesPointsMod.Controllers
{
    public class UserBgamesController
    {
        private readonly IMonitor Monitor;
        private readonly IModHelper Helper;
        private readonly HttpClient httpClient; // Cliente HTTP para hacer las peticiones a la API externa

        public UserBgamesModel UserBgamesModel { get; private set; }
        public PointsBgamesModel PointsBgamesModel { get; private set; }

        public UserBgamesController(
            IMonitor monitor,
            IModHelper helper,
            UserBgamesModel userBgamesModel,
            PointsBgamesModel pointsBgamesModel)
        {
            this.Monitor = monitor;
            this.Helper = helper;
            this.UserBgamesModel = userBgamesModel; // Usa la instancia original pasada
            this.PointsBgamesModel = pointsBgamesModel;
            this.httpClient = new HttpClient(); // Inicializa el cliente HTTP
        }

        public async Task<int> UserCheck(string email, string password)
        {
            string apiUrl = $"http://localhost:3010/player_by_email/{email}";
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var userFromApi = JsonConvert.DeserializeObject<UserBgamesModel>(jsonResponse);

                if (response.IsSuccessStatusCode && (userFromApi.Password == password))
                {
                    Monitor.Log($"Usuario {userFromApi.Name} encontrado en la API.", LogLevel.Info);
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

        public async void SaveUserBgames(string email)
        {
            string apiUrl = $"http://localhost:3010/player_by_email/{email}";
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                string jsonResponse = await response.Content.ReadAsStringAsync();
                UserBgamesModel userFromApi = JsonConvert.DeserializeObject<UserBgamesModel>(jsonResponse);

                if (response.IsSuccessStatusCode && userFromApi != null)
                {
                    // Actualiza la instancia original
                    UserBgamesModel.Name = userFromApi.Name;
                    UserBgamesModel.Email = userFromApi.Email;
                    UserBgamesModel.Password = userFromApi.Password;
                    UserBgamesModel.Id_players = userFromApi.Id_players;
                    UserBgamesModel.Age = userFromApi.Age;
                    Monitor.Log($"Usuario {userFromApi.Name} guardado correctamente.", LogLevel.Info);

                    // Guarda los puntos del jugador en el objeto UserBgamesModel
                    SaveUserPoints(int.Parse(userFromApi.Id_players));
                }
                else
                {
                    Monitor.Log("No se pudo guardar el usuario porque no hay datos disponibles.", LogLevel.Warn);
                }
            }
            catch (Exception ex)
            {
                Monitor.Log($"Error al guardar el usuario: {ex.Message}", LogLevel.Error);
            }
        }

        public async void SaveUserPoints(int idPlayer)
        {
            string apiUrl = $"http://localhost:3001/player_all_attributes/{idPlayer}";
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                string jsonResponse = await response.Content.ReadAsStringAsync();
                
                if (response.IsSuccessStatusCode && response != null)
                {
                    List<PointsBgamesModel> pointsFromApi = JsonConvert.DeserializeObject<List<PointsBgamesModel>>(jsonResponse);
                    UserBgamesModel.Points = pointsFromApi; // Actualiza la lista de puntos en la instancia original
                    Monitor.Log("Puntos del usuario guardados correctamente.", LogLevel.Info);
                }
                else
                {
                    Monitor.Log("No se pudo guardar el puntaje del usuario porque no hay datos disponibles.", LogLevel.Warn);
                }
            }
            catch (Exception ex)
            {
                Monitor.Log($"Error al guardar los puntos: {ex.Message}", LogLevel.Error);
            }
        }

        public UserBgamesModel GetUser()
        {
            return UserBgamesModel;
        }
    }
}


