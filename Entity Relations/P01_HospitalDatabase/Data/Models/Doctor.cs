using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_HospitalDatabase.Data.Models
{
    public class Doctor
    {
        public Doctor()
        {
            this.Visitations = new HashSet<Visitation>();
        }
        [Key]
        public int DoctorId { get; set; }
        [MaxLength(100)]
        [Unicode]
        public string Name { get; set; } = null!;
        [MaxLength(100)]
        [Unicode]
        public string Specialty { get; set; } = null!;
        public virtual ICollection<Visitation> Visitations { get; set; } = null!;
    }
}
