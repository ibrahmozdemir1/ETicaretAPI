﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Dtos.Order
{
    public class CreateOrder
    {
        public string Description { get; set; }
        public string Adress { get; set; }
        public string BasketId { get; set; }
    }
}
