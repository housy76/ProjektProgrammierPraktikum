// Erstellt durch Daniel Hornauer
namespace AppData.Models
{
    /// <summary>
    /// Data Model for RessourceBookedTime
    /// </summary>
    public class RessourceBookedTime
    {
        public int Id { get; set; }
        public int BookedTimeId { get; set; }
        public BookedTime BookedTime { get; set; }

        public int RessourceId { get; set; }
        public Ressource Ressource { get; set; }
    }
}