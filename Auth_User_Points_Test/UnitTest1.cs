using bGamesPointsMod.Controllers;
using bGamesPointsMod.Models;
using Newtonsoft.Json;
using System.Net.Http;


namespace Auth_User_Points_Test
{
    [TestClass]

    public class Auth_User_Points_Test
    {
        private readonly HttpClient httpClient = new HttpClient();
        UserBgamesModel userBgamesModel =
                new UserBgamesModel("", "", "", "", "", null);
        PointsBgamesModel pointsBgamesModel =
            new PointsBgamesModel("", "", "");

        public UserBgamesModel UserBgamesModel { get; private set; }
        public PointsBgamesModel PointsBgamesModel { get; private set; }

        public async Task<int> UserCheck(string email, string password)
        {
            if ((email=="" || email==" " || email==null) || 
                (password=="" || password=="" || password==null))
            {
                return 3; // Datos vacíos
            }
            string apiUrl = $"http://localhost:3010/player_by_email/{email}";
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var userFromApi = JsonConvert.DeserializeObject<UserBgamesModel>(jsonResponse);

                if (response.IsSuccessStatusCode && (userFromApi.Password == password))
                {
                    return 1; // Usuario encontrado
                }
                else
                {
                    return 2; // Contraseña incorrecta
                }
            }
            catch (Exception ex)
            {
                return 0; // Error durante la llamada a la API
            }
        }

        public async Task<int> SaveUserBgames(string email)
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
                    userBgamesModel.Name = userFromApi.Name;
                    userBgamesModel.Email = userFromApi.Email;
                    userBgamesModel.Password = userFromApi.Password;
                    userBgamesModel.Id_players = userFromApi.Id_players;
                    userBgamesModel.Age = userFromApi.Age;

                    // Guarda los puntos del jugador en el objeto UserBgamesModel
                    SaveUserPoints(int.Parse(userFromApi.Id_players));
                    //SaveCredentials(userFromApi.Email, userFromApi.Password);
                    return 1;
                }
                else
                {
                    Console.WriteLine("Error al obtener los datos del jugador");
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener los datos del jugador");
                return 0;
            }
        }
        public async Task<int> SaveUserPoints(int idPlayer)
        {
            string apiUrl = $"http://localhost:3001/player_all_attributes/{idPlayer}";
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode && response != null)
                {
                    List<PointsBgamesModel> pointsFromApi = JsonConvert.DeserializeObject<List<PointsBgamesModel>>(jsonResponse);
                    userBgamesModel.Points = pointsFromApi; // Actualiza la lista de puntos en la instancia original
                    return 1;
                }
                else
                {
                    Console.WriteLine("Error al obtener los puntos del jugador");
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener los puntos del jugador");
                return 0;
            }
        }

        public async Task<int> CheckConnectionBgames()
        {
            string apiUrl = $"http://localhost:3010/";
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode && response != null)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int SpendPoints(int spendPoints)
        {//Cada habilidad va a costar 5 puntos bGames o mas
            int i = spendPoints;
            List<PointsBgamesModel> p = new List<PointsBgamesModel>
            {
            new PointsBgamesModel("0", "Social", "100"),
            new PointsBgamesModel("1", "Fisico", "50"),
            new PointsBgamesModel("2", "Afectivo", "75")
            };
            if (p != null)
            {
                while (i > 0)
                {
                    foreach (var points in p)
                    { // recorre el puntaje de cada tributo
                        if (0 < Int32.Parse(points.Data) && spendPoints > 0)
                        {
                            spendPoints--; //le se resta uno a los puntos a gastar
                            points.Data = (Int32.Parse(points.Data) - 1).ToString(); // tambien resto uno a los puntos ctuales
                        }
                    }
                    if (spendPoints == 0)
                    {
                        return 1;
                    }
                    i--;
                }
                if (spendPoints > 0)
                {
                    return 0; // si no se gasto todo lo que se queria gastar se sale de la funcion
                }
            }
            return 0;
        }


        [TestMethod]
        public void TestCheckUser()
        {
            Task<int> result;
            result = UserCheck("test@test.cl","");
            Assert.AreEqual(3, result.Result);

            result = UserCheck("test@test.cl", "");
            Assert.AreEqual(3, result.Result);

            result = UserCheck("", "123");
            Assert.AreEqual(3, result.Result);
        }

        [TestMethod]
        public void TestSaveUserBgames()
        {
            Task<int> result;
            result = SaveUserBgames("test@test.cl");
            Assert.AreEqual(1, result.Result);

            result = SaveUserBgames("");
            Assert.AreEqual(0, result.Result);
        }

        [TestMethod]
        public void TestSavePoints()
        {
            Task<int> result;

            result = SaveUserPoints(0);
            Assert.AreEqual(1, result.Result);
        }

        [TestMethod]
        public void TestCheckConnection()
        {
            Task<int> result = CheckConnectionBgames();
            Assert.AreEqual(1, result.Result);
        }

        [TestMethod]
        public void TestSpendPoints()
        {
            int result;

            result = SpendPoints(226);
            Assert.AreEqual(0, result);

            result = SpendPoints(10);
            Assert.AreEqual(1, result);
        }
    }
}