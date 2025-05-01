﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class CustomerBasket
    {
        public String Id { get; set; } // Guid : Created From Client [FrontEnd]
        public ICollection<BasketItem> Items { get; set; } = [];
    }
}
