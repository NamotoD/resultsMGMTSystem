//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Assignment2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Net.Http;
    using System.Web.Mvc;

    public partial class unit
    {
        private unit c;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public unit()
        {
            this.results = new HashSet<result>();
        }

        public unit(unit c)
        {
            this.c = c;
        }

        [Required]
        [RegularExpression(@"^[a-zA-Z]{3}[0-9]{4}$", ErrorMessage = "Unit code should be in this format 'aaa1111'.")]
        [Remote("IsUnitCodeUsed", "units", HttpMethod = "POST", ErrorMessage = "Unit code already exists in database.")]
        public string unitCode { get; set; }
        [Required]
        [RegularExpression(@"[a-zA-Z0-9 ]{1,30}", ErrorMessage = "Unit title cannot be longer than 30 characters.")]
        public string unitTitle { get; set; }
        [Required]
        [RegularExpression(@"[a-zA-Z ]{1,30}", ErrorMessage = "Type in up to 30 upper or lowercase letters or spaces")]
        public string unitCoordinator { get; set; }
        public string unitOutline { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<result> results { get; set; }
    }
}