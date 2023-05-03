using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testtttt
{

    public class Loot
    {
        private string name { get; set; }
        private decimal value { get; set; } //decimal потому что он точнее
        private decimal weight { get; set; }

        public Loot(string name, decimal value, decimal weight)
        {
            this.value = value;
            this.weight = weight;
            this.name = name;
        }
        public string Name { get { return name; } }
        public decimal Value { get { return value; } }
        public decimal Weight { get { return weight; } }
        public override string ToString() //перегруженный метод для вывода
        {
            return $"{name} value: {value} weight: {weight}";
        }
    }
}
