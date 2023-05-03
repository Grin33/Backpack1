using testtttt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace testtttt
{
    public class Backpack
    {
        static object locker = new(); //замок
        public List<Loot> Most_valuable { get; private set; } = null;
        private decimal max_Weight { get; set; }
        private decimal best_value { get; set; }
        private decimal final_weight { get; set; }
        public decimal Best_value { get { return best_value; } }
        public decimal Final_weight { get { return final_weight; } }
        public Backpack(decimal max_Weight)
        {
            this.max_Weight = max_Weight;
        }


        public List<List<bool>> Test1 = new List<List<bool>>();
        public int iternum = 0;
        #region Straight_Methods

        public void print(List<int> m)
        {
            foreach(int i in m)
            {
                Console.Write(i + ",");
            }
            Console.WriteLine();
        }
        public void print(List<List<int>> m)
        {
            foreach(List<int> k in m)
            {
                foreach(int i in k)
                {
                    Console.Write(i + ",");
                }
                Console.WriteLine();
            }
        }
        public void init_shuffle(ref List<Loot> loots)
        {
            var templist = new List<bool>(loots.Count);
            for (int i = 0; i < loots.Count; i++)
            {
                templist.Add(true);
            }
            //print(templist);
            int t = 0;
            all_mask(ref templist, ref t);
            Mask_Work(ref loots);
            Test1.Clear();
            //print(Test1);
        }

        public void Mask_Work(ref List<Loot> loots)
        {
            foreach(List<bool> mask in Test1)
            {
                var vals = new List<Loot>() { };
                for (int i = 0; i < loots.Count; i++)
                {
                    if (mask[i] == true)
                    {
                        vals.Add(loots[i]);
                    }
                }
                decimal req_weight = 0m;
                decimal req_value = 0m;
                Is_Requirement(ref vals, ref req_weight, ref req_value);
                if ((Most_valuable == null) && (req_weight <= max_Weight))
                {
                    Most_valuable = vals;
                    best_value = req_value;
                    final_weight = req_weight;
                }
                else
                {
                    if (req_weight <= max_Weight && req_value > best_value)
                    {
                        Most_valuable = vals;
                        best_value = req_value;
                        final_weight = req_weight;
                    }
                }
            }
        }
        public void all_mask (ref List<bool> templist,ref int i)
        {
            //iternum++;
            if (i == templist.Count)
            {
                //print(templist);
                Test1.Add(templist);
            }
            else
            {
                var j = i + 1;
                var templist2 = new List<bool>(templist);
                all_mask(ref templist,ref j);
                templist2[i] = false;
                all_mask(ref templist2,ref j);
            }
            
        }
        private void Is_Requirement(ref List<Loot> loots, ref decimal req_Weight, ref decimal req_Value)
        {
            foreach (Loot i in loots)
            {
                req_Weight += i.Weight;
                req_Value += i.Value;
            }

        }
        #endregion
        
        #region Parallel_Methods
        public void Is_Requirement_1(ref List<Loot> thread_loot, ref decimal iteration_value,ref decimal iteration_weight,
            ref decimal thread_weight, ref decimal thread_value, ref ThreadLocal<List<Loot>> thread_best)
        {
            foreach (var loot in thread_loot)
            {
                iteration_value += loot.Value;
                iteration_weight += loot.Weight;
            }
            if (thread_best.Value != null)
            {
                foreach (var loot2 in thread_best.Value)
                {
                    thread_weight += loot2.Weight;
                    thread_value += loot2.Value;
                }
            }
                
        }

        public void checkin(ref List<Loot> thread_loot, ref ThreadLocal<List<Loot>> thread_best)
        {
            decimal iteration_weight = 0;
            decimal iteration_value = 0;
            decimal thread_weight = 0;
            decimal thread_value = 0;
            //if (thread_best.Value != null)
            {
                Is_Requirement_1(ref thread_loot, ref iteration_value, ref iteration_weight, ref thread_weight, ref thread_value, ref thread_best);
                if ((thread_loot != null))
                {
                    if ((thread_best.Value == null) && (iteration_weight <= max_Weight))
                    {
                        thread_best.Value = thread_loot;
                    }
                    if ((thread_value < iteration_value) && (iteration_weight <= max_Weight))
                    {
                        thread_best.Value = thread_loot;
                    }
                }
            }
            
        }

        public void init_shuffle_Parallel(ref List<Loot> loots)
        {
            var templist = new List<bool>(loots.Count);
            for (int i = 0; i < loots.Count; i++)
            {
                templist.Add(true);
            }
            //print(templist);
            int t = 0;
            all_mask(ref templist, ref t);
            Mask_Work_Parallel(loots);
            //Console.WriteLine(Marshal.SizeOf(Test1));
            Test1.Clear();
            //print(Test1);
        }

        //public void Checks(List<Loot> iter_loots, )
        public void Mask_Work_Parallel_1(List<Loot> loots)
        {
            int capa = loots.Count;
            var tloc_loot = new ConcurrentDictionary<List<Loot>, int>();
            Parallel.For(0, Test1.Count, i =>
            {
                var iter_loots = new List<Loot>();
                for (int f = 0; f < capa; f++)
                {
                    var temp = new List<bool>(Test1[i]);
                    if (temp[f] == true)
                    {
                        iter_loots.Add(loots[f]);
                    }
                }
                //checks();
            }
            );
        }
        public void Mask_Work_Parallel(List<Loot> loots)
        {
            int capa = loots.Count;
            //var thread_weight = new ThreadLocal<decimal>();
            //var thread_value = new ThreadLocal<decimal>();
            //var tloc_loot = new ConcurrentDictionary<int, List<Loot>>();
            var thread_best = new ThreadLocal<List<Loot>>();
            Parallel.For(0, Test1.Count, () => Most_valuable, (i, loop, thread_loot) =>
            {
                //thread_value.Value = 0m;
                thread_loot = new List<Loot>() { };
                for (int f = 0; f < capa; f++)
                {
                    var temp = new List<bool>(Test1[i]);
                    if (temp[f] == true)
                    {
                        thread_loot.Add(loots[f]);
                    }
                }
                checkin(ref thread_loot, ref thread_best);

                return thread_best.Value;
            },
            (x) =>
            {
                if (x != null) {
                    var v = 0m; var w = 0m;
                    var v1 = 0m;
                    foreach (var i in x)
                    {
                        v += i.Value;
                        w += i.Weight;
                    }
                    lock(locker)
                    {
                        Interlocked.Add(ref iternum, 1);
                        if (Most_valuable != null)
                            foreach (var j in Most_valuable)
                            {
                                v1 += j.Value;

                            }
                        if (v > v1) { Most_valuable = x; best_value = v; final_weight = w; }
                    }
                }
                
            }
            );

        }

        #endregion


        /// <summary>
        /// Возвращает лучший набор вещей
        /// </summary>
        /// <returns> Возвращает экземпляр List<Loot> </returns>
        public List<Loot> Get_Most_Valuable() => Most_valuable;
    }
}
