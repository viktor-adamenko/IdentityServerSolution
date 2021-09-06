using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyApi.Models
{
    public class Pill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Pharmacy { get; set; }
        public decimal Price { get; set; }
    }
}
