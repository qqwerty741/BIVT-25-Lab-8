using System.Reflection.Metadata.Ecma335;
using static Lab8.Purple.Task1;

namespace Lab8.Purple
{
    public class Task1
    {
        public class Participant
        {
            private string _name;
            private string _surname;
            private double[] _coefs;
            private int[,] _marks;
            private int _jump;

            public string Name => _name;
            public string Surname => _surname;
            public double[] Coefs
            {
                get
                {
                    if (_coefs == null) return new double[0];
                    return (from i in _coefs select i).ToArray();
                }
            }
            public int[,] Marks
            {
                get
                {
                    if (_marks == null) return new int[0, 0];
                    int[,] clone = new int[4, 7];
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            clone[i, j] = _marks[i, j];
                        }
                    }
                    return clone;
                }
            }
            public double TotalScore
            {
                get
                {
                    if (_marks == null) return 0;
                    double sum = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        int localSum = 0;
                        int localMin = 7, localMax = -1;
                        for (int j = 0; j < 7; j++)
                        {
                            localSum += _marks[i, j];
                            localMin = Math.Min(_marks[i, j], localMin);
                            localMax = Math.Max(_marks[i, j], localMax);
                        }
                        localSum -= localMin;
                        localSum -= localMax;
                        sum += (double)localSum * _coefs[i];
                    }
                    return sum;
                }
            }

            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _coefs = new double[4];
                for (int i = 0; i < 4; i++) _coefs[i] = 2.5;
                _marks = new int[4, 7];
                for (int i = 0; i < 4 * 7; i++) _marks[i % 4, i / 4] = 0;
                _jump = 0;
            }

            public void SetCriterias(double[] coefs)
            {
                if (_coefs == null) return;
                for (int i = 0; i < 4; i++) _coefs[i] = coefs[i];
            }
            public void Jump(int[] marks)
            {
                if (_marks == null) return;
                for (int i = 0; i < 7; i++) _marks[_jump, i] = marks[i];
                _jump++;
            }
            public static void Sort(Participant[] array)
            {
                var NewArray = (from i in array
                                orderby -i.TotalScore
                                select i).ToArray();
                for (int i = 0; i < NewArray.Length; i++)
                {
                    array[i] = NewArray[i];
                }

            }
            public void Print()
            {
                Console.Write($"{_name,10} {_surname,10} - {Math.Round(TotalScore, 3),7}");
                return;
            }

        }
        public class Judge
        {
            private string _name;
            private int[] _mass;
            private int _mark;

            public string Name => _name;
            //public int[] Mass => _mass.ToArray();

            public Judge(string name, int[] mass) { 
                _name = name;
                _mass = mass.ToArray();
                _mark = 0;
            }

            public int CreateMark()
            {
                if (_mark == _mass.Length) _mark = 0;
                return _mass[_mark++];
            }

            public void Print()
            {
                Console.WriteLine($"{Name} -> NextMark: {_mass[_mark]}");
                for (int i = 0; i < _mass.Length; i++)
                {
                    if(i == _mark) Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"{_mass[i]} ");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine();
            }
        }
        public class Competition
        {
            private Participant[] _participants;
            private Judge[] _judges;

            public Participant[] Participants => _participants.ToArray();
            public Judge[] Judges => _judges.ToArray();

            public Competition(Judge[] judges)
            {
                _judges = judges.ToArray();
                _participants = new Participant[0];
            }
            
            public void Evaluate(Participant p)
            {
                int[] marks = new int[7];
                for(int i = 0; i < 7; i++)
                {
                    marks[i] = Judges[i].CreateMark();
                }
                p.Jump(marks);
            }
            
            public void Add(Participant p)
            {
                Array.Resize(ref _participants, _participants.Length + 1);
                Evaluate(p);
                _participants[^1] = p;
            }
            public void Add(Participant[] ps)
            {
                for(int i = 0; i < ps.Length; i++)
                {
                    Add(ps[i]);
                }
            }

            public void Sort()
            {
                Participant.Sort(_participants);
            }
        }
    }

}