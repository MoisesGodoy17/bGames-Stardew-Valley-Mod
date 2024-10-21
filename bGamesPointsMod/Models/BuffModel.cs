namespace bGamesPointsMod.Models
{
    public class BuffModel
    {
        public string Name { get; set; }              // Nombre del buff
        public float SpeedModifier { get; set; }       // Modificador de velocidad
        public int CostPoints { get; set; }            // Puntos necesarios para activar el buff

        public int TickBuff {  get; set; }

        // Constructor para inicializar el buff
        public BuffModel(string name, float speedModifier, int costPoints, int tickBuff)
        {
            Name = name;
            SpeedModifier = speedModifier;
            CostPoints = costPoints;
            TickBuff = tickBuff;
        }
    }
}