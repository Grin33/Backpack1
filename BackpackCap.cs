using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testtttt
{
    class BackpackCap
    {
        private decimal capacity { get; set; }
        public decimal Capacity { get { return capacity; } }
        public BackpackCap(decimal capacity) { this.capacity = capacity; }
        public BackpackCap(string Capacity)
        {
            if (Capacity == null) throw new ArgumentNullException(nameof(Capacity));
            else
                if ((Capacity == "default") || (Capacity == "Default")) { this.capacity = 100; }
        }

    }
}
