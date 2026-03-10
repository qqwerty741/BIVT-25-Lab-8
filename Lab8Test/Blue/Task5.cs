using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lab8Test.Blue
{
    [TestClass]
    public sealed class Task5
    {
        record InputSportsman(string Name, string Surname, int Place);
        record InputTeam(string Name, InputSportsman[] Sportsmen);
        record OutputTeam(string Name, int TotalScore, int TopPlace);

        private InputTeam[] _inputManTeams;
        private InputTeam[] _inputWomanTeams;
        private OutputTeam[] _outputManTeams;
        private OutputTeam[] _outputWomanTeams;

        private Lab8.Blue.Task5.Sportsman[] _manSportsmen;
        private Lab8.Blue.Task5.Sportsman[] _womanSportsmen;
        private Lab8.Blue.Task5.Team[] _manTeams;
        private Lab8.Blue.Task5.Team[] _womanTeams;

        [TestInitialize]
        public void LoadData()
        {
            var folder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            folder = Path.Combine(folder, "Lab8Test", "Blue");

            var input = JsonSerializer.Deserialize<JsonElement>(
                File.ReadAllText(Path.Combine(folder, "input.json")))!;
            var output = JsonSerializer.Deserialize<JsonElement>(
                File.ReadAllText(Path.Combine(folder, "output.json")))!;

            _inputManTeams = input.GetProperty("Task5Man").Deserialize<InputTeam[]>()!;
            _inputWomanTeams = input.GetProperty("Task5Woman").Deserialize<InputTeam[]>()!;

            _outputManTeams = output.GetProperty("Task5Man").Deserialize<OutputTeam[]>()!;
            _outputWomanTeams = output.GetProperty("Task5Woman").Deserialize<OutputTeam[]>()!;

            _manSportsmen = _inputManTeams.SelectMany(t => t.Sportsmen)
                                          .Select(s => new Lab8.Blue.Task5.Sportsman(s.Name, s.Surname))
                                          .ToArray();

            _womanSportsmen = _inputWomanTeams.SelectMany(t => t.Sportsmen)
                                              .Select(s => new Lab8.Blue.Task5.Sportsman(s.Name, s.Surname))
                                              .ToArray();
        }

        [TestMethod]
        public void Test_00_OOP()
        {
            var type = typeof(Lab8.Blue.Task5.Sportsman);
            Assert.IsFalse(type.IsValueType, "Sportsman должен быть классом");
            Assert.IsTrue(type.IsClass);
            Assert.AreEqual(0, type.GetFields(BindingFlags.Public | BindingFlags.Instance).Length);
            Assert.IsTrue(type.GetProperty("Name")?.CanRead ?? false, "Нет свойства Name");
            Assert.IsTrue(type.GetProperty("Surname")?.CanRead ?? false, "Нет свойства Surname");
            Assert.IsTrue(type.GetProperty("Place")?.CanRead ?? false, "Нет свойства Place");
            Assert.IsFalse(type.GetProperty("Name")?.CanWrite ?? false, "Свойство Name должно быть только для чтения");
            Assert.IsFalse(type.GetProperty("Surname")?.CanWrite ?? false, "Свойство Surname должно быть только для чтения");
            Assert.IsFalse(type.GetProperty("Place")?.CanWrite ?? false, "Свойство Place должно быть только для чтения");
            Assert.IsNotNull(type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(string), typeof(string) }, null), "Нет публичного конструктора Sportsman(string name, string surname)");
            Assert.IsNotNull(type.GetMethod("SetPlace", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(int) }, null), "Нет публичного метода SetPlace(int place)");
            Assert.IsNotNull(type.GetMethod("Print", BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null), "Нет публичного метода Print()");
            Assert.AreEqual(0, type.GetFields().Count(f => f.IsPublic));
            Assert.AreEqual(3, type.GetProperties().Count(f => f.PropertyType.IsPublic));
            Assert.AreEqual(1, type.GetConstructors().Count(f => f.IsPublic));
            Assert.AreEqual(9, type.GetMethods().Count(f => f.IsPublic));

            type = typeof(Lab8.Blue.Task5.Team);
            Assert.IsTrue(type.IsAbstract, "Team должен быть абстрактным классом");
            Assert.IsTrue(type.IsClass);
            Assert.AreEqual(0, type.GetFields(BindingFlags.Public | BindingFlags.Instance).Length);
            Assert.IsTrue(type.GetProperty("Name")?.CanRead ?? false, "Нет свойства Name");
            Assert.IsTrue(type.GetProperty("Sportsmen")?.CanRead ?? false, "Нет свойства Sportsmen");
            Assert.IsTrue(type.GetProperty("TotalScore")?.CanRead ?? false, "Нет свойства TotalScore");
            Assert.IsTrue(type.GetProperty("TopPlace")?.CanRead ?? false, "Нет свойства TopPlace");
            Assert.IsFalse(type.GetProperty("Name")?.CanWrite ?? false, "Свойство Name должно быть только для чтения");
            Assert.IsFalse(type.GetProperty("Sportsmen")?.CanWrite ?? false, "Свойство Sportsmen должно быть только для чтения");
            Assert.IsFalse(type.GetProperty("TotalScore")?.CanWrite ?? false, "Свойство TotalScore должно быть только для чтения");
            Assert.IsFalse(type.GetProperty("TopPlace")?.CanWrite ?? false, "Свойство TopPlace должно быть только для чтения");
            Assert.IsNotNull(type.GetMethod("Add", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(Lab8.Blue.Task5.Sportsman) }, null), "Нет публичного метода Add(Sportsman sportsman)");
            Assert.IsNotNull(type.GetMethod("Add", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(Lab8.Blue.Task5.Sportsman[]) }, null), "Нет публичного метода Add(Sportsman[] sportsmen)");
            Assert.IsNotNull(type.GetMethod("Sort", BindingFlags.Static | BindingFlags.Public, null, new[] { typeof(Lab8.Blue.Task5.Team[]) }, null), "Нет публичного статического метода Sort(Team[] array)");
            Assert.IsNotNull(type.GetMethod("GetChampion", BindingFlags.Static | BindingFlags.Public, null, new[] { typeof(Lab8.Blue.Task5.Team[]) }, null), "Нет публичного статического метода GetChampion(Team[] teams)");
            Assert.IsNotNull(type.GetMethod("Print", BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null), "Нет публичного метода Print()");
            var strengthMethod = type.GetMethod("GetTeamStrength", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            Assert.IsNotNull(strengthMethod, "Нет метода GetTeamStrength()");
            Assert.IsTrue(strengthMethod.IsFamily || strengthMethod.IsFamilyOrAssembly, "Метод GetTeamStrength должен быть protected");
            Assert.IsTrue(strengthMethod.IsAbstract, "Метод GetTeamStrength должен быть abstract");
            Assert.AreEqual(0, type.GetFields().Count(f => f.IsPublic));
            Assert.AreEqual(4, type.GetProperties().Count(f => f.PropertyType.IsPublic));
            Assert.AreEqual(13, type.GetMethods().Count(f => f.IsPublic));

            var manType = typeof(Lab8.Blue.Task5.ManTeam);
            Assert.IsTrue(manType.IsClass);
            Assert.AreEqual(type, manType.BaseType);
            Assert.IsNotNull(manType.GetConstructor(new[] { typeof(string) }));
            var manStrength = manType.GetMethod("GetTeamStrength", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.AreEqual(manType, manStrength.DeclaringType);
            Assert.AreEqual(0, manType.GetFields().Count(f => f.IsPublic));
            Assert.AreEqual(4, manType.GetProperties().Count(f => f.PropertyType.IsPublic));
            Assert.AreEqual(1, manType.GetConstructors().Count(f => f.IsPublic));
            Assert.AreEqual(11, manType.GetMethods().Count(f => f.IsPublic));

            var womanType = typeof(Lab8.Blue.Task5.WomanTeam);
            Assert.IsTrue(womanType.IsClass);
            Assert.AreEqual(type, womanType.BaseType);
            Assert.IsNotNull(womanType.GetConstructor(new[] { typeof(string) }));
            var womanStrength = womanType.GetMethod("GetTeamStrength", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.AreEqual(womanType, womanStrength.DeclaringType);
            Assert.AreEqual(0, womanType.GetFields().Count(f => f.IsPublic));
            Assert.AreEqual(4, womanType.GetProperties().Count(f => f.PropertyType.IsPublic));
            Assert.AreEqual(1, womanType.GetConstructors().Count(f => f.IsPublic));
            Assert.AreEqual(11, womanType.GetMethods().Count(f => f.IsPublic));
        }

        [TestMethod]
        public void Test_01_CreateManSportsmen()
        {
            Assert.AreEqual(_manSportsmen.Length, _inputManTeams.Sum(t => t.Sportsmen.Length));
        }

        [TestMethod]
        public void Test_01_CreateWomanSportsmen()
        {
            Assert.AreEqual(_womanSportsmen.Length, _inputWomanTeams.Sum(t => t.Sportsmen.Length));
        }

        [TestMethod]
        public void Test_02_InitManSportsmen()
        {
            CheckManSportsmen(placeExpected: false);
        }

        [TestMethod]
        public void Test_02_InitWomanSportsmen()
        {
            CheckWomanSportsmen(placeExpected: false);
        }

        [TestMethod]
        public void Test_03_SetManPlaces()
        {
            SetManPlaces();
            CheckManSportsmen(placeExpected: true);
        }

        [TestMethod]
        public void Test_03_SetWomanPlaces()
        {
            SetWomanPlaces();
            CheckWomanSportsmen(placeExpected: true);
        }

        [TestMethod]
        public void Test_04_CreateManTeams()
        {
            InitManTeams();
            CheckManTeams(filled: false);
        }

        [TestMethod]
        public void Test_04_CreateWomanTeams()
        {
            InitWomanTeams();
            CheckWomanTeams(filled: false);
        }

        [TestMethod]
        public void Test_05_FillManTeams()
        {
            SetManPlaces();
            InitManTeams();
            FillManTeams();
            CheckManTeams(filled: true);
        }

        [TestMethod]
        public void Test_05_FillWomanTeams()
        {
            SetWomanPlaces();
            InitWomanTeams();
            FillWomanTeams();
            CheckWomanTeams(filled: true);
        }

        [TestMethod]
        public void Test_06_FillManyManTeams()
        {
            SetManPlaces();
            InitManTeams();
            FillManyManTeams();
            CheckManTeams(filled: true);
        }

        [TestMethod]
        public void Test_06_FillManyWomanTeams()
        {
            SetWomanPlaces();
            InitWomanTeams();
            FillManyWomanTeams();
            CheckWomanTeams(filled: true);
        }

        [TestMethod]
        public void Test_07_SortMan()
        {
            SetManPlaces();
            InitManTeams();
            FillManTeams();
            Lab8.Blue.Task5.Team.Sort(_manTeams);
            CheckManTeamsSorted();
        }

        [TestMethod]
        public void Test_07_SortWoman()
        {
            SetWomanPlaces();
            InitWomanTeams();
            FillWomanTeams();
            Lab8.Blue.Task5.Team.Sort(_womanTeams);
            CheckWomanTeamsSorted();
        }

        [TestMethod]
        public void Test_08_GetTeamStrength()
        {
            SetManPlaces();
            InitManTeams();
            FillManTeams();

            SetWomanPlaces();
            InitWomanTeams();
            FillWomanTeams();

            double delta = 0.0001;

            // ManTeam 0
            var strength0 = (double)_manTeams[0].GetType().GetMethod("GetTeamStrength", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(_manTeams[0], null);
            Assert.AreEqual(11.32075, strength0, delta);

            // WomanTeam 0
            var strengthWoman = (double)_womanTeams[0].GetType().GetMethod("GetTeamStrength", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(_womanTeams[0], null);
            Assert.AreEqual(1.160714, strengthWoman, delta);

            // ManTeam 1
            var strength1 = (double)_manTeams[1].GetType().GetMethod("GetTeamStrength", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(_manTeams[1], null);
            Assert.AreEqual(7.594937, strength1, delta);
        }

        [TestMethod]
        public void Test_09_GetManChampion()
        {
            SetManPlaces();
            InitManTeams();
            FillManTeams();

            var champion = Lab8.Blue.Task5.Team.GetChampion(_manTeams);
            Assert.AreEqual("Локомотив", champion.Name);
        }

        [TestMethod]
        public void Test_09_GetWomanChampion()
        {
            SetWomanPlaces();
            InitWomanTeams();
            FillWomanTeams();

            var champion = Lab8.Blue.Task5.Team.GetChampion(_womanTeams);
            Assert.AreEqual("Динамо", champion.Name);
        }

        private void SetManPlaces()
        {
            int idx = 0;
            foreach (var t in _inputManTeams)
            {
                foreach (var s in t.Sportsmen)
                {
                    _manSportsmen[idx].SetPlace(s.Place);
                    idx++;
                }
            }
        }

        private void SetWomanPlaces()
        {
            int idx = 0;
            foreach (var t in _inputWomanTeams)
            {
                foreach (var s in t.Sportsmen)
                {
                    _womanSportsmen[idx].SetPlace(s.Place);
                    idx++;
                }
            }
        }

        private void InitManTeams()
        {
            _manTeams = _inputManTeams.Select(t => (Lab8.Blue.Task5.Team)new Lab8.Blue.Task5.ManTeam(t.Name)).ToArray();
        }

        private void InitWomanTeams()
        {
            _womanTeams = _inputWomanTeams.Select(t => (Lab8.Blue.Task5.Team)new Lab8.Blue.Task5.WomanTeam(t.Name)).ToArray();
        }

        private void FillManTeams()
        {
            int idx = 0;
            for (int i = 0; i < _manTeams.Length; i++)
            {
                foreach (var s in _inputManTeams[i].Sportsmen)
                {
                    _manTeams[i].Add(_manSportsmen[idx]);
                    idx++;
                }
            }
        }

        private void FillWomanTeams()
        {
            int idx = 0;
            for (int i = 0; i < _womanTeams.Length; i++)
            {
                foreach (var s in _inputWomanTeams[i].Sportsmen)
                {
                    _womanTeams[i].Add(_womanSportsmen[idx]);
                    idx++;
                }
            }
        }

        private void FillManyManTeams()
        {
            int idx = 0;
            for (int i = 0; i < _manTeams.Length; i++)
            {
                var sportsmenToAdd = _manSportsmen.Skip(idx).Take(_inputManTeams[i].Sportsmen.Length).ToArray();
                _manTeams[i].Add(sportsmenToAdd);
                idx += _inputManTeams[i].Sportsmen.Length;
            }
        }

        private void FillManyWomanTeams()
        {
            int idx = 0;
            for (int i = 0; i < _womanTeams.Length; i++)
            {
                var sportsmenToAdd = _womanSportsmen.Skip(idx).Take(_inputWomanTeams[i].Sportsmen.Length).ToArray();
                _womanTeams[i].Add(sportsmenToAdd);
                idx += _inputWomanTeams[i].Sportsmen.Length;
            }
        }

        private void CheckManSportsmen(bool placeExpected)
        {
            int idx = 0;
            foreach (var t in _inputManTeams)
            {
                foreach (var s in t.Sportsmen)
                {
                    var sp = _manSportsmen[idx];
                    Assert.AreEqual(s.Name, sp.Name);
                    Assert.AreEqual(s.Surname, sp.Surname);
                    if (placeExpected)
                        Assert.AreEqual(s.Place, sp.Place);
                    else
                        Assert.AreEqual(0, sp.Place);
                    idx++;
                }
            }
        }

        private void CheckWomanSportsmen(bool placeExpected)
        {
            int idx = 0;
            foreach (var t in _inputWomanTeams)
            {
                foreach (var s in t.Sportsmen)
                {
                    var sp = _womanSportsmen[idx];
                    Assert.AreEqual(s.Name, sp.Name);
                    Assert.AreEqual(s.Surname, sp.Surname);
                    if (placeExpected)
                        Assert.AreEqual(s.Place, sp.Place);
                    else
                        Assert.AreEqual(0, sp.Place);
                    idx++;
                }
            }
        }

        private void CheckManTeams(bool filled)
        {
            for (int i = 0; i < _manTeams.Length; i++)
            {
                var team = _manTeams[i];
                Assert.AreEqual(_inputManTeams[i].Name, team.Name);
                if (filled)
                {
                    Assert.AreEqual(6, team.Sportsmen.Length);
                    int startIdx = _inputManTeams.Take(i).Sum(x => x.Sportsmen.Length);
                    for (int j = 0; j < 6; j++)
                    {
                        var exp = _inputManTeams[i].Sportsmen[j];
                        var act = team.Sportsmen[j];
                        Assert.AreEqual(exp.Name, act.Name);
                        Assert.AreEqual(exp.Surname, act.Surname);
                        Assert.AreEqual(exp.Place, act.Place);
                    }
                    int expSum = team.Sportsmen.Sum(s => s.Place > 0 && s.Place <= 5 ? 6 - s.Place : 0);
                    Assert.AreEqual(expSum, team.TotalScore);
                    int expTop = team.Sportsmen.Where(s => s.Place > 0).Select(s => s.Place).DefaultIfEmpty(int.MaxValue).Min();
                    Assert.AreEqual(expTop == int.MaxValue ? 0 : expTop, team.TopPlace);
                }
                else
                {
                    Assert.AreEqual(0, team.TotalScore);
                    Assert.AreEqual(18, team.TopPlace);
                }
            }
        }

        private void CheckWomanTeams(bool filled)
        {
            for (int i = 0; i < _womanTeams.Length; i++)
            {
                var team = _womanTeams[i];
                Assert.AreEqual(_inputWomanTeams[i].Name, team.Name);
                if (filled)
                {
                    Assert.AreEqual(6, team.Sportsmen.Length);
                    int startIdx = _inputWomanTeams.Take(i).Sum(x => x.Sportsmen.Length);
                    for (int j = 0; j < 6; j++)
                    {
                        var exp = _inputWomanTeams[i].Sportsmen[j];
                        var act = team.Sportsmen[j];
                        Assert.AreEqual(exp.Name, act.Name);
                        Assert.AreEqual(exp.Surname, act.Surname);
                        Assert.AreEqual(exp.Place, act.Place);
                    }
                    int expSum = team.Sportsmen.Sum(s => s.Place > 0 && s.Place <= 5 ? 6 - s.Place : 0);
                    Assert.AreEqual(expSum, team.TotalScore);
                    int expTop = team.Sportsmen.Where(s => s.Place > 0).Select(s => s.Place).DefaultIfEmpty(int.MaxValue).Min();
                    Assert.AreEqual(expTop == int.MaxValue ? 0 : expTop, team.TopPlace);
                }
                else
                {
                    Assert.AreEqual(0, team.TotalScore);
                    Assert.AreEqual(18, team.TopPlace);
                }
            }
        }

        private void CheckManTeamsSorted()
        {
            for (int i = 0; i < _manTeams.Length; i++)
            {
                Assert.AreEqual(_outputManTeams[i].Name, _manTeams[i].Name);
                Assert.AreEqual(_outputManTeams[i].TotalScore, _manTeams[i].TotalScore);
                Assert.AreEqual(_outputManTeams[i].TopPlace, _manTeams[i].TopPlace);
            }
        }

        private void CheckWomanTeamsSorted()
        {
            for (int i = 0; i < _womanTeams.Length; i++)
            {
                Assert.AreEqual(_outputWomanTeams[i].Name, _womanTeams[i].Name);
                Assert.AreEqual(_outputWomanTeams[i].TotalScore, _womanTeams[i].TotalScore);
                Assert.AreEqual(_outputWomanTeams[i].TopPlace, _womanTeams[i].TopPlace);
            }
        }
    }
}