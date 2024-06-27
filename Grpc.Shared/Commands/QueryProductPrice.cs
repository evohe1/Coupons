using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grpc.Shared.Commands
{
    public class QueryProductPrice
    {
        public int ProductId { get; set; }
        public Double Price { get; set; }
        public string Url { get; set; }
        public int Status { get; set; }
    }
}
