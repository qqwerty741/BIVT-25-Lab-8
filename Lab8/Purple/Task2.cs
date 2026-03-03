namespace Lab8.Purple
{
    public class Task2
    {
        public struct Participant
        {
            private string _name;
            private string _surname;
            private int _distance;
            private int[] _marks;

            private int _target;

            public string Name => _name;
            public string Surname => _surname;
            public int Distance => _distance;
            public int[] Marks
            {
                get
                {
                    if (_marks == null) return new int[0];
                    return (from i in _marks select i).ToArray();
                }
            }
            public int Result
            {
                get
                {
                    if (_marks == null) return 0;
                    if (_distance == 0) return 0;
                    int ans;
                    ans = (_distance >= _target ? 60 + (_distance - _target) * 2 : 60 + (_distance - _target) * 2);
                    int minMark = 21, maxMark = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        minMark = Math.Min(_marks[i], minMark);
                        maxMark = Math.Max(_marks[i], maxMark);
                        ans += _marks[i];
                    }
                    return Math.Max(ans - minMark - maxMark, 0);
                }
            }

            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _distance = 0;
                _marks = new int[5];
            }

            public void Jump(int dist, int[] marks, int target)
            {
                if (_marks == null) return;
                _distance = dist;
                _target = target;
                for (int i = 0; i < 5; i++) _marks[i] = marks[i];
            }

            public static void Sort(Participant[] arr)
            {
                var copy = (from i in arr
                            orderby -i.Result
                            select i).ToArray();
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = copy[i];
                }
            }
            public void Print()
            {
                Console.Write($"{_name,10} {_surname,10} - {Result}");
                return;
            }

        }
        public abstract class SkiJumping
        {
            protected string _name;
            protected int _standard;
            protected Participant[] _participants;
            protected int _lastJump;

            public string Name => _name;
            public int Standard => _standard;
            public Participant[] Participants => _participants;

            public SkiJumping(string name, int standart)
            {
                _name = name;
                _standard = standart;
                _participants = new Participant[0];
                _lastJump = 0;
            }

            public void Add(Participant p)
            {
                Array.Resize(ref _participants, _participants.Length + 1);
                _participants[^1] = p;
            }
            public void Add(Participant[] ps)
            {
                foreach (var item in ps)
                {
                    Add(item);
                }
            }

            public void Jump(int dist, int[] marks)
            {
                _participants[_lastJump++].Jump(dist, marks, _standard);
            }

            public void Print()
            {
                foreach(var item in _participants)
                {
                    item.Print();
                }
            }
        }
        public class JuniorSkiJumping : SkiJumping
        {
            public JuniorSkiJumping() : base("100m", 100) { }
        }
        public class ProSkiJumping : SkiJumping
        {
            public ProSkiJumping() : base("150m", 150) { }
        }
    }
}