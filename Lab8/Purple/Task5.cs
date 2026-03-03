namespace Lab8.Purple
{
    public class Task5
    {
        private static int Find(string[] arr, string elem)
        {
            if (elem == null || elem == "") return -1;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == elem) return i;
            }
            return -1;
        }
        private static int Find((string, double)[] arr, string elem)
        {
            if (elem == null) return -1;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].Item1 == elem) return i;
            }
            return -1;
        }
        private static void AddTo(ref (string, double)[] mass, string elem, int num = 1)
        {
            if (Find(mass, elem) == -1) AddM(ref mass, (elem, num));
            else mass[Find(mass, elem)].Item2 += num;
        }
        private static void AddM<T>(ref T[] arr, T elem)
        {
            Array.Resize(ref arr, arr.Length + 1);
            arr[arr.Length - 1] = elem;
        }
        private static void AddM<T>(ref T[] arr, T[] elem)
        {
            for (int i = 0; i < elem.Length; i++) AddM(ref arr, elem[i]);
        }
        public struct Response
        {
            private string _animal;
            private string _characterTrait;
            private string _concept;

            public string Animal => _animal;
            public string CharacterTrait => _characterTrait;
            public string Concept => _concept;

            public Response(string a1 = "", string a2 = "", string a3 = "")
            {
                _animal = a1;
                _characterTrait = a2;
                _concept = a3;
            }

            public int CountVotes(Response[] responses, int qNum)
            {
                int ans = 0;

                for (int i = 0; i < responses.Length; i++)
                {
                    if (qNum == 1) if (Animal == responses[i].Animal) ans++;
                    if (qNum == 2) if (CharacterTrait == responses[i].CharacterTrait) ans++;
                    if (qNum == 3) if (Concept == responses[i].Concept) ans++;
                }
                return ans;
            }

            public void Print()
            {
                Console.WriteLine($"{Animal,20}, {CharacterTrait,20}, {Concept,20}");
            }
        }

        public struct Research
        {
            private string _name;
            private Response[] _responses;

            public string Name => _name;
            public Response[] Responses => _responses;

            public Research(string name)
            {
                _name = name;
                _responses = new Response[0];
            }

            public void Add(string[] answers)
            {
                Array.Resize(ref _responses, _responses.Length + 1);
                _responses[^1] = new Response(answers[0], answers[1], answers[2]);
            }
            internal (string, int)[] TResp(int num)
            {
                var checks = new string[0];
                var top = new (string, int)[0];
                for (var i = 0; i < Responses.Length; i++)
                {
                    if (num == 1)
                    {
                        if (Find(checks, Responses[i].Animal) != -1 || Responses[i].Animal == null) continue;
                        AddM(ref checks, Responses[i].Animal);
                        AddM(ref top, (Responses[i].Animal, Responses[i].CountVotes(Responses, num)));
                    }
                    if (num == 2)
                    {
                        if (Find(checks, Responses[i].CharacterTrait) != -1 || Responses[i].CharacterTrait == null) continue;
                        AddM(ref checks, Responses[i].CharacterTrait);
                        AddM(ref top, (Responses[i].CharacterTrait, Responses[i].CountVotes(Responses, num)));
                    }
                    if (num == 3)
                    {
                        if (Find(checks, Responses[i].Concept) != -1 || Responses[i].Concept == null) continue;
                        AddM(ref checks, Responses[i].Concept);
                        AddM(ref top, (Responses[i].Concept, Responses[i].CountVotes(Responses, num)));
                    }
                }
                top = (from i in top
                       orderby i.Item2 descending
                       select i).ToArray();
                return top;
            }
            public string[] GetTopResponses(int num)
            {
                int elems = 5;
                var checks = new string[0];
                var top = TResp(num);
                for (int i = 0; i < elems && i < top.Length; i++)
                {
                    if (top[i].Item1 == null) continue;
                    AddM(ref checks, top[i].Item1);
                }
                return checks;
            }
            public void Print()
            {
                for (int i = 0; i < Responses.Length; i++)
                {
                    Console.Write($"{i,3}: ");
                    Responses[i].Print();
                }
            }
        }
        
        public class Report
        {
            private Research[] _researches;
            private static int _num;

            public Research[] Researches => _researches;

            static Report()
            {
                _num = 1;
            }
            public Report()
            {
                _researches = new Research[0];
            }

            public Research MakeResearch()
            {
                Research New = new Research($"No_{_num++}_{DateTime.Now.Month}_{DateTime.Now.Year}");
                AddM(ref _researches, New);
                return New;
            }
            public (string, double)[] GetGeneralReport(int question)
            {
                var ans = new (string, double)[0];
                int allAns = 0;
                for(int i = 0; i < _researches.Length; i++)
                {
                    var temp = _researches[i].TResp(question);
                    for(int j = 0; j < temp.Length; j++)
                    {
                        AddTo(ref ans, temp[j].Item1, temp[j].Item2);
                        allAns += temp[j].Item2;
                    }
                }
                var ans2 = (from i in ans
                            select (i.Item1, i.Item2 / allAns * 100)).ToArray();
                for(int i = 0; i < ans2.Length; i++)
                {
                    Console.WriteLine($"{ans2[i].Item1} - {ans2[i].Item2}");
                }
                Console.WriteLine();
                return ans2;
            }
        }
    }
}