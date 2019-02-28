using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.WebApi.Models
{
    public class ProducerDto
    {
        public int ProducerId { get; set; }

        public string ProducerName { get; set; }

        public string ProducerAddress { get; set; }
    }
}
