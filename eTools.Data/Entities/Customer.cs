namespace eTools.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            Rentals = new HashSet<Rental>();
        }
        [Key]
        public int CustomerID { get; set; }


        [Required(ErrorMessage = "LastName is required")]
        [StringLength(50, ErrorMessage = "CouponIDValue accepts a maximum of 50 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        [StringLength(50, ErrorMessage = "FirstName accepts a maximum of 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(50, ErrorMessage = "Address accepts a maximum of 50 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(75, ErrorMessage = "City accepts a maximum of 75 characters")]
        public string City { get; set; }


        [Required(ErrorMessage = "Province is required")]
        [StringLength(2, ErrorMessage = "Province accepts a maximum of 2 characters")]
        public string Province { get; set; }


        [Required(ErrorMessage = "PostalCode is required")]
        [StringLength(6, ErrorMessage = "PostalCode accepts a maximum of 6 characters")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "ContactPhone is required")]
        [StringLength(12, ErrorMessage = "ContactPhone accepts a maximum of 12 characters")]
        public string ContactPhone { get; set; }


        [Required(ErrorMessage = "EmailAddress is required")]
        [StringLength(15, ErrorMessage = "EmailAddress accepts a maximum of 15 characters")]
        public string EmailAddress { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rental> Rentals { get; set; }
    }
}
