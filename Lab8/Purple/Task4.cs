namespace Lab8.Purple
{
    public class Task4
    {
        private static void AddM<T>(ref T[] arr, T elem)
        {
            Array.Resize(ref arr, arr.Length + 1);
            arr[arr.Length - 1] = elem;
        }

        public class Sportsman
        {
            private string _name;
            private string _surname;
            private double _time;

            public string Name => _name;
            public string Surname => _surname;
            public double Time => _time;

            public Sportsman(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _time = 0;
            }
            public void Run(double time)
            {
                if (_time == 0) _time = time;
            }
            public void Print()
            {
                Console.WriteLine($"{Name} {Surname} - {Time} сек.");
            }

            public static void Sort(Sportsman[] mass)
            {
                var sMass = (from i in mass
                             orderby i._time
                             select i).ToArray();
                for (int i = 0; i < mass.Length; i++) mass[i] = sMass[i];
            }
        }

        public class SkiMan : Sportsman
        {
            public SkiMan(string name, string surname) : base(name, surname)
            {
            }
            public SkiMan(string name, string surname, double time) : base(name, surname)
            {
                Run(time);
            }
        }

        public class SkiWoman : Sportsman
        {
            public SkiWoman(string name, string surname) : base(name, surname)
            {
            }
            public SkiWoman(string name, string surname, double time) : base(name, surname)
            {
                Run(time);
            }
        }


        public class Group
        {
            private string _name;
            private Sportsman[] _sportsmen;

            public string Name => _name;
            public Sportsman[] Sportsmen => _sportsmen;

            public Group(string name)
            {
                _name = name;
                _sportsmen = new Sportsman[0];
            }
            public Group(Group other)
            {
                _name = other.Name;
                _sportsmen = other.Sportsmen.ToArray();
            }

            public void Add(Sportsman sportsman)
            {
                Array.Resize(ref _sportsmen, _sportsmen.Length + 1);
                _sportsmen[^1] = sportsman;
            }
            public void Add(Sportsman[] sportsmen)
            {
                for (int i = 0; i < sportsmen.Length; i++)
                {
                    Add(sportsmen[i]);
                }
            }
            public void Add(Group group)
            {
                Add(group.Sportsmen);
            }
            public void Sort()
            {
                _sportsmen = (from i in _sportsmen
                              orderby i.Time
                              select i).ToArray();
            }
            public static Group Merge(Group g1, Group g2)
            {
                var ans = new Group("Финалисты");
                ans.Add(g1);
                ans.Add(g2);
                ans.Sort();
                return ans;
            }
            public void Print()
            {
                Console.WriteLine($"{Name}");
                foreach (var i in _sportsmen)
                {
                    i.Print();
                }
            }

            public void Split(out Sportsman[] men, out Sportsman[] women)
            {
                men = new Sportsman[0];
                women = new Sportsman[0];
                for(int i = 0; i < _sportsmen.Length; i++)
                {
                    if (_sportsmen[i].Name[^1] == 'а' || _sportsmen[i].Name[^1] == 'я') if (_sportsmen[i].Name != "Никита")
                        {
                            AddM(ref women, _sportsmen[i]);
                            continue;
                        }
                    AddM(ref men, _sportsmen[i]);
                }
            }
            
            private Sportsman[] Merge(Sportsman[] g1, Sportsman[] g2)
            {
                int i = 0;
                int j = 0;
                Sportsman[] ans = new Sportsman[0];
                bool flag = false;
                while (i < g1.Length && j < g2.Length)
                {
                    if (!flag) AddM(ref ans, g1[i++]);
                    if (flag) AddM(ref ans, g2[j++]);
                    flag = !flag;
                }
                while (i < g1.Length)
                {
                    AddM(ref ans, g1[i++]);
                }
                while (j < g2.Length)
                {
                    AddM(ref ans, g2[j++]);
                }
                return ans;
            }
            public void Shuffle()
            {
                Sportsman[] men;
                Sportsman[] women;
                Sort();
                Split(out men, out women);
                Sportsman[] ans;
                if (men[0].Time < women[0].Time)
                {
                    _sportsmen = Merge(men, women);
                }
                else
                {
                    _sportsmen = Merge(women, men);
                }
                for (int i = 0; i < _sportsmen.Length; i++) { _sportsmen[i].Print(); }
            }
        }

        
    }
}