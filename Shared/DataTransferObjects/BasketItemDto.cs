using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public class BasketItemDto
    {
        public int Id { get; set; }

        public string ProductName { get; set; } = default!;

        public string PictureUrl { get; set; } = default!;
        [Range(minimum: 1, maximum: double.MaxValue)]

        public decimal Price { get; set; }
        [Range(minimum: 1 , 100)]

        public int Quantity { get; set; }
    }
}
