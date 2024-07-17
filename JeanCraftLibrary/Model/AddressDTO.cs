﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Model
{
    public class AddressDTO
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string? Type { get; set; }
        public string? Detail { get; set; }
    }
}
