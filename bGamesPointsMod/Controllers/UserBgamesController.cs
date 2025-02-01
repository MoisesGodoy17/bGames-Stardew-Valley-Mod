
using StardewModdingAPI;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json; // Necesitarás instalar este paquete para trabajar con JSON.
using bGamesPointsMod.Models;
using System.Text;

namespace bGamesPointsMod.Controllers
{
    public class UserBgamesController
    {
        private readonly IMonitor Monitor;
        private readonly IModHelper Helper;
        private readonly HttpClient httpClient; // Cliente HTTP para hacer las peticiones a la API externa
        private readonly string credentialsFilePath; // Ruta de acceso al archivo de credenciales


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
            this.credentialsFilePath = Path.Combine(helper.DirectoryPath, "users.json");
        }

        public async Task<int> UserCheck(string email, string password) {
            string apiUrl = $"http://localhost:3010/player_by_email/{email}";
            try {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var userFromApi = JsonConvert.DeserializeObject<UserBgamesModel>(jsonResponse);

                if (response.IsSuccessStatusCode && (userFromApi.Password == password)) {
                    Monitor.Log($"Usuario {userFromApi.Name} encontrado en la API.", LogLevel.Info);
                    return 1; // Usuario encontrado
                }
                else {
                    Monitor.Log("Usuario no encontrado en la API.", LogLevel.Info);
                    return 0; // Usuario no encontrado
                }
            }
            catch (Exception ex) {
                Monitor.Log($"Error al consultar la API: {ex.Message}", LogLevel.Error);
                return 0; // Error durante la llamada a la API
            }
        }

        public async void SaveUserBgames(string email) {
            string apiUrl = $"http://localhost:3010/player_by_email/{email}";
            try {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                string jsonResponse = await response.Content.ReadAsStringAsync();
                UserBgamesModel userFromApi = JsonConvert.DeserializeObject<UserBgamesModel>(jsonResponse);

                if (response.IsSuccessStatusCode && userFromApi != null) {
                    // Actualiza la instancia original
                    UserBgamesModel.Name = userFromApi.Name;
                    UserBgamesModel.Email = userFromApi.Email;
                    UserBgamesModel.Password = userFromApi.Password;
                    UserBgamesModel.Id_players = userFromApi.Id_players;
                    UserBgamesModel.Age = userFromApi.Age;
                    Monitor.Log($"Usuario {userFromApi.Name} guardado correctamente.", LogLevel.Info);

                    // Guarda los puntos del jugador en el objeto UserBgamesModel
                    SaveUserPoints(int.Parse(userFromApi.Id_players));
                    SaveCredentials(userFromApi.Email, userFromApi.Password);
                }
                else {
                    Monitor.Log("No se pudo guardar el usuario porque no hay datos disponibles.", LogLevel.Warn);
                }
            }
            catch (Exception ex) {
                Monitor.Log($"Error al guardar el usuario: {ex.Message}", LogLevel.Error);
            }
        }

        public bool CredentialsFileExists() {
            return File.Exists(this.credentialsFilePath);
        }

        // Esta funcion guarda las credenciales en un archivo JSON y al moment ode cargar dichos datos ocupa la de usercheck
        public void SaveCredentials(string username, string password) {
            try {
                string jsonContent = $"{{ \"username\": \"{username}\", \"password\": \"{password}\" }}";
                File.WriteAllText(this.credentialsFilePath, jsonContent);
                this.Monitor.Log("Credenciales guardadas exitosamente.", LogLevel.Info);
            }
            catch (Exception ex) {
                this.Monitor.Log($"Error al guardar las credenciales: {ex.Message}", LogLevel.Error);
            }
        }

        // Cargar credenciales desde el archivo
        public async Task<int> LoadCredentials() {
            try {
                // Verificar si el archivo existe antes de leerlo
                if (!CredentialsFileExists()) {
                    this.Monitor.Log("No se encontraron credenciales para cargar.", LogLevel.Warn);
                    return 0;
                }
                // Leer y deserializar las credenciales
                string jsonContent = File.ReadAllText(this.credentialsFilePath);
                var credentials = JsonConvert.DeserializeObject<dynamic>(jsonContent);

                string username = credentials.username.ToString();
                string password = credentials.password.ToString();

                this.Monitor.Log($"Credenciales cargadas: Usuario: {username}", LogLevel.Info);

                // Usar await para obtener el resultado de UserCheck
                int userCheckResult = await UserCheck(username, password);

                if (userCheckResult == 1) {
                    SaveUserBgames(username);
                    return 1;
                }
                return 0; // Credenciales incorrectas
            }
            catch (Exception ex) {
                this.Monitor.Log($"Error al cargar las credenciales: {ex.Message}", LogLevel.Error);
                return 0;
            }
        }

        public async void SaveUserPoints(int idPlayer) {
            string apiUrl = $"http://localhost:3001/player_all_attributes/{idPlayer}";
            try {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                string jsonResponse = await response.Content.ReadAsStringAsync();
                
                if (response.IsSuccessStatusCode && response != null) {
                    List<PointsBgamesModel> pointsFromApi = JsonConvert.DeserializeObject<List<PointsBgamesModel>>(jsonResponse);
                    UserBgamesModel.Points = pointsFromApi; // Actualiza la lista de puntos en la instancia original
                    Monitor.Log("Puntos del usuario guardados correctamente.", LogLevel.Info);
                }
                else {
                    Monitor.Log("No se pudo guardar el puntaje del usuario porque no hay datos disponibles.", LogLevel.Warn);
                }
            }
            catch (Exception ex) {
                Monitor.Log($"Error al guardar los puntos: {ex.Message}", LogLevel.Error);
            }
        }

        public UserBgamesModel GetUser() {
            return UserBgamesModel;
        }

        // funcion para gastar puntos en las habilidades
        public int SpendPoints(int spendPoints) {//Cada habilidad va a costar 5 puntos bGames o mas
            int i = spendPoints;
            if (UserBgamesModel.Points != null) {
                while (i > 0) {
                    foreach (var points in UserBgamesModel.Points) { // recorre el puntaje de cada tributo
                        if (0 < Int32.Parse(points.Data) && spendPoints > 0) {
                            spendPoints--; //le se resta uno a los puntos a gastar
                            points.Data = (Int32.Parse(points.Data) - 1).ToString(); // tambien resto uno a los puntos ctuales
                        }
                    }
                    if (spendPoints == 0) {
                        return 1;
                    }
                    i--;
                }
                if (spendPoints > 0) {
                    return 0; // si no se gasto todo lo que se queria gastar se sale de la funcion
                }
            }
            return 0;
        }

        public async Task SavePointsBgames() {
            try{
                string userId = UserBgamesModel.Id_players;

                foreach (var points in UserBgamesModel.Points) {
                    // Crear el cuerpo del JSON
                    var payload = new {
                        id_player = userId,
                        id_attributes = points.Id_attributes,
                        new_data = points.Data
                    };

                    // Serializar el JSON
                    string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(payload);

                    // Configurar la solicitud HTTP
                    using (var content = new StringContent(jsonContent, Encoding.UTF8, "application/json")) {
                        // Realizar la solicitud PUT
                        HttpResponseMessage response = await httpClient.PutAsync("http://localhost:3002/player_attributes_single", content);

                        // Verificar la respuesta
                        if (response.IsSuccessStatusCode) {
                            Monitor.Log($"Puntos actualizados correctamente: {points.Name} ({points.Data})", LogLevel.Info);
                        }
                        else {
                            string error = await response.Content.ReadAsStringAsync();
                            Monitor.Log($"Error al actualizar los puntos para {points.Name}: {error}", LogLevel.Error);
                        }
                    }
                }
            }
            catch (Exception ex) {
                Monitor.Log($"Error al guardar los puntos: {ex.Message}", LogLevel.Error);
            }
        }


        public async Task<int> CheckConnectionBgames() {
            string apiUrl = $"http://localhost:3010/";
            try {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode && response != null) {
                    Monitor.Log("Conexion con bGames establecida.", LogLevel.Info);
                    return 1;
                }
                else {
                    Monitor.Log("No se puedo establecer la conexion.", LogLevel.Warn);
                    return 0;
                }
            }
            catch (Exception ex) {
                Monitor.Log($"Error: {ex.Message}", LogLevel.Error);
                return 0;
            }
        }
    }
}


