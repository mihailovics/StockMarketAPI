using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Stock
{
    public class UpdateStockRequestDTO
    {
        [Required]
        [MaxLength(10, ErrorMessage = "Symbol cannot be over 10 char")]
        public string Symbol { get; set; } = string.Empty;
        [Required]
        [MaxLength(10, ErrorMessage = "Company name cannot be over 10 char")]
        public string Company { get; set; } = string.Empty;
        [Required]
        [Range(1,100000000)]
        public decimal Purchase { get; set; }
        [Required]
        [Range(0.001,100)]
        public decimal Dividend { get; set; }
        [Required]
        [MaxLength(10, ErrorMessage = "Industry cannot be over 10")]
        public string Industry { get; set; } = string.Empty;
        [Required]
        [Range(1,5000000000)]
        public long MarketCap {get; set; }
    }
}