namespace AddressStructurationApi.Models
{
    public class NoStructured
    {

        public required string name { get; set; }
        public required string rue_numero { get; set; }
        public required string npa_localite { get; set; }
        public required Boolean ISO20022 { get; set; }
    }
}
