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
using System.Diagnostics;

namespace testtttt
{
    public class Backpack
    {
        static object locker = new(); static object locker1 = new();
        public List<Loot>? Most_Valuable { get; private set; } = null;
        public decimal max_weight { get; set; }
        public decimal best_value { get; set; } = 0;
        public decimal final_weight { get; set; }

        public Backpack(decimal max_weight)
        {
            this.max_weight = max_weight;
        }

        public void PrintLoot(List<Loot> lootss)
        {
            foreach (var loot in lootss)
            {
                Console.WriteLine(loot);
            }
            Console.WriteLine();
        }

        #region Straight_Methods
        public void Check(ref List<Loot> templist)
        {
            decimal iteration_weight = 0;
            decimal iteration_value = 0;
            foreach (Loot oneloot in templist)
            {
                iteration_value += oneloot.Value;
                iteration_weight += oneloot.Weight;
            }
            if (iteration_weight <= max_weight)
            {
                if (iteration_value > best_value)
                {
                    best_value = iteration_value;
                    Most_Valuable = templist;
                    final_weight = iteration_weight;
                    //return true;
                }
                //else
                    //return false;
            }
            //else
                //return false;
        }

        public void shuffle(ref List<Loot> loots)
        {
            for (int i = 0; i < loots.Count; i++)
            {
                var templist = new List<Loot>();
                templist.Add(loots[i]);
                //if (Check(ref templist))
                //    shuffle(ref loots, templist, i);
                Check(ref templist);
                shuffle(ref loots, templist, i);
            }

        }

        public void shuffle(ref List<Loot> loots, List<Loot> templist, int item)
        {
            var n = item + 1;
            for (int i = n; i < loots.Count; i++)
            {
                var temploots = new List<Loot>(templist);
                temploots.Add(loots[i]);
                //if (Check(ref temploots))
                //    shuffle(ref loots, temploots, i);
                Check(ref temploots);
                shuffle(ref loots, temploots, i);
            }
        }
        #endregion Straight_Methods

        #region Parallel
        public void Check_Parallel(ref List<Loot> thread_loots, ref List<Loot> thread_best)
        {
            decimal iteration_weight = 0; decimal thread_best_weight = 0;
            decimal iteration_value = 0; decimal thread_best_value = 0;
            foreach (Loot oneloot in thread_loots)
            {
                iteration_value += oneloot.Value;
                iteration_weight += oneloot.Weight;
            }
            foreach (Loot oneloot in thread_best)
            {
                thread_best_weight += oneloot.Weight;
                thread_best_value += oneloot.Value;
            }
            //if (thread_best_weight > max_weight) { thread_best = new List<Loot>(); }
            if (iteration_weight <= max_weight)
            {
                if ((iteration_value > thread_best_value))
                {
                    thread_best = thread_loots;
                    //return true;
                }
                //else
                    //return false;
            }
            //else
                //return false;
        }

        public void nested_shuffle(ref List<Loot> loots, List<Loot> thread_iteration_loots, ref List<Loot> thread_best, int item)
        {
            var n = item + 1;
            for (int i = n; i < loots.Count; i++)
            {
                var thread_iteration_loots1 = new List<Loot>(thread_iteration_loots);
                thread_iteration_loots1.Add(loots[i]);
                //if (Check_Parallel(ref thread_iteration_loots1, ref thread_best) == true)
                //{
                //    nested_shuffle(ref loots, thread_iteration_loots1, ref thread_best, i);
                //}
                Check_Parallel(ref thread_iteration_loots1, ref thread_best);
                nested_shuffle(ref loots, thread_iteration_loots1, ref thread_best, i);
            }
        }
        public void parallel_shuffle(List<Loot> loots)
        {
            //var thread_best = new ThreadLocal<List<Loot>>();
            Parallel.For(0, loots.Count, () => Most_Valuable, (i, loop, thread_best) =>
            {
                thread_best = new List<Loot>() { };
                var thread_iteration = new List<Loot>() { };
                thread_iteration.Add(loots[i]);
                //if (Check_Parallel(ref thread_iteration, ref thread_best) == true)
                //{
                //    nested_shuffle(ref loots, thread_iteration, ref thread_best, i);
                //}
                Check_Parallel(ref thread_iteration, ref thread_best);
                nested_shuffle(ref loots, thread_iteration, ref thread_best, i);
                return thread_best;
            },
            (x) =>
            {
                decimal thread_best_value = 0;
                decimal thread_best_weight = 0;
                foreach (Loot oneloot in x) { thread_best_value += oneloot.Value; thread_best_weight += oneloot.Weight; }
                lock (locker)
                {
                    //PrintLoot(x);
                    if (best_value < thread_best_value)
                    {
                        Most_Valuable = x;
                        final_weight = thread_best_weight;
                        best_value = thread_best_value;
                    }

                }
            }
            );
        }

        #endregion Parallel

    }
}
