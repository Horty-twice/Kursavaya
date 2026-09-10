using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR4.Aspire
{
    public class ResultMessage
    {
        public long CarId { get; set; }
        public string? UpdateTime { get; set; }
    }

    public class CarMessage
    {
        public long CarId { get; set; }
        public long ClientId { get; set; }
    }
}
