using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_HospitalDatabase.Data.Models
{
    public class Patient
    {
        public Patient()
        {
            this.Diagnoses = new HashSet<Diagnose>();
            this.Visitations = new HashSet<Visitation>();
            this.Prescriptions = new HashSet<PatientMedicament>();
        }
        [Key]
        public int PatientId { get; set; }
        [MaxLength(50)]
        [Unicode]
        public string FirstName { get; set; } = null!;
        [MaxLength(50)]
        [Unicode]
        public string LastName { get; set; } = null!;
        [MaxLength(250)]
        [Unicode]
        public string Address { get; set; } = null!;
        [MaxLength(80)]
        public string Email { get; set; } = null!;
        public bool HasInsurance { get; set; }
        public virtual ICollection<Diagnose> Diagnoses { get; set; } = null!;
        public virtual ICollection<Visitation> Visitations { get; set; } = null!;
        public virtual ICollection<PatientMedicament> Prescriptions { get; set; } = null!;
      
    }
}
