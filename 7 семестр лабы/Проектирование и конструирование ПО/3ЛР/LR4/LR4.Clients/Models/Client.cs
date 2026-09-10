using System.ComponentModel.DataAnnotations.Schema;

namespace LR4.Clients.Models
{
    [Table("client")]
    public class Client
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column ("registeredObjects")]
        public int RegisteredObjects { get; set; }
    }
}
