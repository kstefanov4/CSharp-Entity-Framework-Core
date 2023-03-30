using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trucks.Data.Models
{
    public class Despatcher
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(40)]
        public string Name { get; set; } = null!;
        [Required]
        public string Position { get; set; } = null!;

        [Required]
        public virtual ICollection<Truck> Trucks { get; set; } = new HashSet<Truck>();
    }
}
