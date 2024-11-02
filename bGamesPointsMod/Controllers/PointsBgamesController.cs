using StardewModdingAPI;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json; // Necesitarás instalar este paquete para trabajar con JSON.
using bGamesPointsMod.Models;

namespace bGamesPointsMod.Controllers
{
    public class PointsBgamesController
    {
        private UserBgamesModel userBgamesModel;
        private readonly IMonitor Monitor;
        private readonly IModHelper Helper;
        private readonly HttpClient httpClient;

        public PointsBgamesController(
            IMonitor monitor,
            IModHelper helper,
            UserBgamesModel userBgamesModel)
        {
            this.Monitor = monitor;
            this.Helper = helper;
            this.userBgamesModel = userBgamesModel;
            this.httpClient = new HttpClient();
        }
    }
}
