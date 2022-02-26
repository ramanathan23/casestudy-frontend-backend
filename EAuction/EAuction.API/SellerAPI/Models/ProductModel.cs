using EAuction.Core.Common.Validations;
using EAuction.Seller.Core.Validations;
using System;
using System.ComponentModel.DataAnnotations;

namespace EAuction.Seller.API.Models
{
    public class ProductModel
    {
        [Required]
        [MinLength(5)]
        [MaxLength(30)]
        public string ProductName { get; set; }

        [Required]
        public string ShortDescription { get; set; }

        [Required]
        [IsValidCategory]
        public string Category { get; set; }

        [Required]
        [Range(Double.Epsilon, Double.MaxValue)]
        public decimal StartingPrice { get; set; }

        [Required]
        [FutureDate]
        public DateTime BidEndDate { get; set; }
    }
}