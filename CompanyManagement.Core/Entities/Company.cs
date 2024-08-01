using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagement.Core.Entities
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }

        [Required, MaxLength(200)]
        public string CompanyName { get; set; }

        [Required, MaxLength(200)]
        public string CompanyAddress { get; set; }
        
        [MaxLength(200)]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z")]
        public string? Email { get; set; } //This will be applicable for clients with role == RoleA only

    }
}
