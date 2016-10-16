﻿using System;

namespace MVC5Course.Models.ViewModels
{
    public class ClientContributionViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<decimal> OrderTotal { get; set; }
    }
}