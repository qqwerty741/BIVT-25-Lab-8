using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lab8Test.Purple
{
    [TestClass]
    public sealed class Task3
    {
        record InputRow(string Name, string Surname, double[] Marks);
        record OutputRow(string Name, string Surname, int Score, int TopPlace, double TotalMark);

        private InputRow[] _input;
        private OutputRow[] _outputFigure;
        private OutputRow[] _outputIce;
        private Lab8.Purple.Task3.Participant[] _participantsFigure;
        private Lab8.Purple.Task3.Participant[] _participantsIce;
        private Lab8.Purple.Task3.Skating _skatingFigure;
        private Lab8.Purple.Task3.Skating _skatingIce;

        [TestInitialize]
        public void LoadData()
        {
            var folder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            folder = Path.Combine(folder, "Lab8Test", "Purple");

            var inputJson = JsonSerializer.Deserialize<JsonElement>(
                File.ReadAllText(Path.Combine(folder, "input.json")))!;
            var outputJson = JsonSerializer.Deserialize<JsonElement>(
                File.ReadAllText(Path.Combine(folder, "output.json")))!;

            _input = inputJson.GetProperty("Task3").Deserialize<InputRow[]>()!;

            _outputFigure = outputJson.GetProperty("Task3Figure").Deserialize<OutputRow[]>()!;
            _outputIce = outputJson.GetProperty("Task3Ice").Deserialize<OutputRow[]>()!;

            _participantsFigure = new Lab8.Purple.Task3.Participant[_input.Length];
            _participantsIce = new Lab8.Purple.Task3.Participant[_input.Length];
        }

        [TestMethod]
        public void Test_00_OOP()
        {
            var type = typeof(Lab8.Purple.Task3.Participant);
            Assert.IsTrue(type.IsValueType, "Participant должен быть структурой");
            Assert.AreEqual(0, type.GetFields(BindingFlags.Public | BindingFlags.Instance).Length);
            Assert.IsTrue(type.GetProperty("Name")?.CanRead ?? false, "Нет свойства Name");
            Assert.IsTrue(type.GetProperty("Surname")?.CanRead ?? false, "Нет свойства Surname");
            Assert.IsTrue(type.GetProperty("Marks")?.CanRead ?? false, "Нет свойства Marks");
            Assert.IsTrue(type.GetProperty("Score")?.CanRead ?? false, "Нет свойства Score");
            Assert.IsTrue(type.GetProperty("TopPlace")?.CanRead ?? false, "Нет свойства TopPlace");
            Assert.IsTrue(type.GetProperty("TotalMark")?.CanRead ?? false, "Нет свойства TotalMark");
            Assert.IsFalse(type.GetProperty("Name")?.CanWrite ?? false, "Свойство Name должно быть только для чтения");
            Assert.IsFalse(type.GetProperty("Surname")?.CanWrite ?? false, "Свойство Surname должно быть только для чтения");
            Assert.IsFalse(type.GetProperty("Marks")?.CanWrite ?? false, "Свойство Marks должно быть только для чтения");
            Assert.IsFalse(type.GetProperty("Score")?.CanWrite ?? false, "Свойство Score должно быть только для чтения");
            Assert.IsFalse(type.GetProperty("TopPlace")?.CanWrite ?? false, "Свойство TopPlace должно быть только для чтения");
            Assert.IsFalse(type.GetProperty("TotalMark")?.CanWrite ?? false, "Свойство TotalMark должно быть только для чтения");
            Assert.IsNotNull(type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(string), typeof(string) }, null), "Нет публичного конструктора Participant(string name, string surname)");
            Assert.IsNotNull(type.GetMethod("Evaluate", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(double) }, null), "Нет публичного метода Evaluate(double result)");
            Assert.IsNotNull(type.GetMethod("SetPlaces", BindingFlags.Static | BindingFlags.Public, null, new[] { typeof(Lab8.Purple.Task3.Participant[]) }, null), "Нет публичного статического метода SetPlaces(Participant[] array)");
            Assert.IsNotNull(type.GetMethod("Sort", BindingFlags.Static | BindingFlags.Public, null, new[] { typeof(Lab8.Purple.Task3.Participant[]) }, null), "Нет публичного статического метода Sort(Participant[] array)");
            Assert.IsNotNull(type.GetMethod("Print", BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null), "Нет публичного метода Print()");
            Assert.AreEqual(7, type.GetProperties().Count(f => f.PropertyType.IsPublic));
            Assert.AreEqual(1, type.GetConstructors().Count(f => f.IsPublic));
            Assert.AreEqual(15, type.GetMethods().Count(f => f.IsPublic));

            type = typeof(Lab8.Purple.Task3.Skating);
            Assert.IsTrue(type.IsAbstract, "Skating должен быть абстрактным классом");
            Assert.IsTrue(type.IsClass);
            Assert.AreEqual(0, type.GetFields(BindingFlags.Public | BindingFlags.Instance).Length);
            Assert.IsTrue(type.GetProperty("Participants")?.CanRead ?? false, "Нет свойства Participants");
            Assert.IsTrue(type.GetProperty("Moods")?.CanRead ?? false, "Нет свойства Moods");
            Assert.IsFalse(type.GetProperty("Participants")?.CanWrite ?? false, "Свойство Participants должно быть только для чтения");
            Assert.IsFalse(type.GetProperty("Moods")?.CanWrite ?? false, "Свойство Moods должно быть только для чтения");
            Assert.IsNotNull(type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(double[]) }, null), "Нет публичного конструктора Skating(double[] mood)");
            Assert.IsNotNull(type.GetMethod("Add", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(Lab8.Purple.Task3.Participant) }, null), "Нет публичного метода Add(Participant jumper)");
            Assert.IsNotNull(type.GetMethod("Add", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(Lab8.Purple.Task3.Participant[]) }, null), "Нет публичного метода Add(Participant[] jumpers)");
            Assert.IsNotNull(type.GetMethod("Evaluate", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(double[]) }, null), "Нет публичного метода Evaluate(double[] marks)");
            var modMood = type.GetMethod("ModificateMood", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(modMood, "Нет protected метода ModificateMood()");
            Assert.IsTrue(modMood.IsAbstract);
            Assert.AreEqual(0, type.GetFields().Count(f => f.IsPublic));
            Assert.AreEqual(2, type.GetProperties().Count(f => f.PropertyType.IsPublic));
            Assert.AreEqual(1, type.GetConstructors().Count(f => f.IsPublic));
            Assert.AreEqual(9, type.GetMethods().Count(f => f.IsPublic));

            var figType = typeof(Lab8.Purple.Task3.FigureSkating);
            Assert.IsTrue(figType.IsClass);
            Assert.AreEqual(type, figType.BaseType);
            Assert.IsNotNull(figType.GetConstructor(new[] { typeof(double[]) }));
            var figMod = figType.GetMethod("ModificateMood", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.AreEqual(figType, figMod.DeclaringType);
            Assert.AreEqual(0, figType.GetFields().Count(f => f.IsPublic));
            Assert.AreEqual(2, figType.GetProperties().Count(f => f.PropertyType.IsPublic));
            Assert.AreEqual(1, figType.GetConstructors().Count(f => f.IsPublic));
            Assert.AreEqual(9, figType.GetMethods().Count(f => f.IsPublic));

            var iceType = typeof(Lab8.Purple.Task3.IceSkating);
            Assert.IsTrue(iceType.IsClass);
            Assert.AreEqual(type, iceType.BaseType);
            Assert.IsNotNull(iceType.GetConstructor(new[] { typeof(double[]) }));
            var iceMod = iceType.GetMethod("ModificateMood", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.AreEqual(iceType, iceMod.DeclaringType);
            Assert.AreEqual(0, iceType.GetFields().Count(f => f.IsPublic));
            Assert.AreEqual(2, iceType.GetProperties().Count(f => f.PropertyType.IsPublic));
            Assert.AreEqual(1, iceType.GetConstructors().Count(f => f.IsPublic));
            Assert.AreEqual(9, iceType.GetMethods().Count(f => f.IsPublic));
        }

        [TestMethod]
        public void Test_01_Create()
        {
            InitFigure();
            CheckStructFigure(evaluated: false);
            InitIce();
            CheckStructIce(evaluated: false);
        }

        [TestMethod]
        public void Test_02_EvaluateFigure()
        {
            InitFigure();
            EvaluateFigure();
            CheckStructFigure(evaluated: true);
        }

        [TestMethod]
        public void Test_02_EvaluateIce()
        {
            InitIce();
            EvaluateIce();
            CheckStructIce(evaluated: true);
        }

        [TestMethod]
        public void Test_03_SetPlacesFigure()
        {
            InitFigure();
            EvaluateFigure();
            SetPlacesFigure();
            CheckStructFigure(evaluated: true);
        }

        [TestMethod]
        public void Test_03_SetPlacesIce()
        {
            InitIce();
            EvaluateIce();
            SetPlacesIce();
            CheckStructIce(evaluated: true);
        }

        [TestMethod]
        public void Test_04_SortFigure()
        {
            InitFigure();
            EvaluateFigure();
            SetPlacesFigure();

            Lab8.Purple.Task3.Participant.Sort(_skatingFigure.Participants);

            Assert.AreEqual(_outputFigure.Length, _skatingFigure.Participants.Length);
            for (int i = 0; i < _outputFigure.Length; i++)
            {
                Assert.AreEqual(_outputFigure[i].Name, _skatingFigure.Participants[i].Name);
                Assert.AreEqual(_outputFigure[i].Surname, _skatingFigure.Participants[i].Surname);
                Assert.AreEqual(_outputFigure[i].Score, _skatingFigure.Participants[i].Score);
                Assert.AreEqual(_outputFigure[i].TopPlace, _skatingFigure.Participants[i].TopPlace);
                Assert.AreEqual(_outputFigure[i].TotalMark, _skatingFigure.Participants[i].TotalMark, 0.0001);
            }
        }

        [TestMethod]
        public void Test_04_SortIce()
        {
            InitIce();
            EvaluateIce();
            SetPlacesIce();

            Lab8.Purple.Task3.Participant.Sort(_skatingIce.Participants);

            Assert.AreEqual(_outputIce.Length, _skatingIce.Participants.Length);
            for (int i = 0; i < _outputIce.Length; i++)
            {
                Assert.AreEqual(_outputIce[i].Name, _skatingIce.Participants[i].Name);
                Assert.AreEqual(_outputIce[i].Surname, _skatingIce.Participants[i].Surname);
                Assert.AreEqual(_outputIce[i].Score, _skatingIce.Participants[i].Score);
                Assert.AreEqual(_outputIce[i].TopPlace, _skatingIce.Participants[i].TopPlace);
                Assert.AreEqual(_outputIce[i].TotalMark, _skatingIce.Participants[i].TotalMark, 0.0001);
            }
        }

        [TestMethod]
        public void Test_05_ArrayLinqFigure()
        {
            InitFigure();
            EvaluateFigure();
            SetPlacesFigure();
            ArrayLinqFigure();
            CheckStructFigure(evaluated: true);
        }

        [TestMethod]
        public void Test_05_ArrayLinqIce()
        {
            InitIce();
            EvaluateIce();
            SetPlacesIce();
            ArrayLinqIce();
            CheckStructIce(evaluated: true);
        }

        [TestMethod]
        public void Test_06_MoodsModification()
        {
            double[] initial = new double[7] { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0 };

            var fig = new Lab8.Purple.Task3.FigureSkating(initial);
            var moodsFig = fig.Moods;
            for (int i = 0; i < 7; i++)
                Assert.AreEqual(1.0 + 0.1 * (i + 1), moodsFig[i], 0.0001);

            var ice = new Lab8.Purple.Task3.IceSkating(initial);
            var moodsIce = ice.Moods;
            for (int i = 0; i < 7; i++)
                Assert.AreEqual(1.0 * (1.0 + 0.01 * (i + 1)), moodsIce[i], 0.0001);
        }

        private void InitFigure()
        {
            for (int i = 0; i < _input.Length; i++)
                _participantsFigure[i] = new Lab8.Purple.Task3.Participant(_input[i].Name, _input[i].Surname);
        }

        private void InitIce()
        {
            for (int i = 0; i < _input.Length; i++)
                _participantsIce[i] = new Lab8.Purple.Task3.Participant(_input[i].Name, _input[i].Surname);
        }
        private void EvaluateFigure()
        {
            double[] initialMood = new double[7] { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0 };
            _skatingFigure = new Lab8.Purple.Task3.FigureSkating(initialMood);

            _skatingFigure.Add(_participantsFigure);

            for (int i = 0; i < _input.Length; i++)
            {
                _skatingFigure.Evaluate(_input[i].Marks);
            }
        }

        private void EvaluateIce()
        {
            double[] initialMood = new double[7] { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0 };
            _skatingIce = new Lab8.Purple.Task3.IceSkating(initialMood);

            _skatingIce.Add(_participantsIce);

            for (int i = 0; i < _input.Length; i++)
            {
                _skatingIce.Evaluate(_input[i].Marks);
            }
        }

        private void SetPlacesFigure()
        {
            Lab8.Purple.Task3.Participant.SetPlaces(_skatingFigure.Participants);
        }

        private void SetPlacesIce()
        {
            Lab8.Purple.Task3.Participant.SetPlaces(_skatingIce.Participants);
        }

        private void ArrayLinqFigure()
        {
            for (int i = 0; i < _participantsFigure.Length; i++)
            {
                var marks = _participantsFigure[i].Marks;
                if (marks == null) continue;
                for (int j = 0; j < marks.Length; j++)
                    marks[j] = -1;
            }
        }

        private void ArrayLinqIce()
        {
            for (int i = 0; i < _participantsIce.Length; i++)
            {
                var marks = _participantsIce[i].Marks;
                if (marks == null) continue;
                for (int j = 0; j < marks.Length; j++)
                    marks[j] = -1;
            }
        }

        private void CheckStructFigure(bool evaluated)
        {
            Assert.AreEqual(_input.Length, _participantsFigure.Length);

            for (int i = 0; i < _input.Length; i++)
            {
                Assert.AreEqual(_input[i].Name, _participantsFigure[i].Name);
                Assert.AreEqual(_input[i].Surname, _participantsFigure[i].Surname);

                if (evaluated)
                {
                    Assert.IsNotNull(_participantsFigure[i].Marks);
                    Assert.AreEqual(7, _participantsFigure[i].Marks.Length);
                    for (int j = 0; j < 7; j++)
                        Assert.AreEqual(_input[i].Marks[j] * (1.0 + 0.1 * (j + 1)), _participantsFigure[i].Marks[j], 0.0001);
                }
                else
                {
                    if (_participantsFigure[i].Marks != null)
                        for (int j = 0; j < _participantsFigure[i].Marks.Length; j++)
                            Assert.AreEqual(0, _participantsFigure[i].Marks[j]);
                }
            }
        }

        private void CheckStructIce(bool evaluated)
        {
            Assert.AreEqual(_input.Length, _participantsIce.Length);

            for (int i = 0; i < _input.Length; i++)
            {
                Assert.AreEqual(_input[i].Name, _participantsIce[i].Name);
                Assert.AreEqual(_input[i].Surname, _participantsIce[i].Surname);

                if (evaluated)
                {
                    Assert.IsNotNull(_participantsIce[i].Marks);
                    Assert.AreEqual(7, _participantsIce[i].Marks.Length);
                    for (int j = 0; j < 7; j++)
                        Assert.AreEqual(_input[i].Marks[j] * (1.0 + 0.01 * (j + 1)), _participantsIce[i].Marks[j], 0.0001);
                }
                else
                {
                    if (_participantsIce[i].Marks != null)
                        for (int j = 0; j < _participantsIce[i].Marks.Length; j++)
                            Assert.AreEqual(0, _participantsIce[i].Marks[j]);
                }
            }
        }
    }
}
