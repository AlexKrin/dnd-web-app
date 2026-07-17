using System.Diagnostics;
using System.Xml.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection.Metadata;

namespace dnd_web_app
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleUI consoleUI = new ConsoleUI();
            consoleUI.Run();
        }
    }
    class ConsoleUI
    {
        private Compaing _compaing = new Compaing();


        public void Run()
        {
            Console.WriteLine("1 - Продолжить компанию");
            Console.WriteLine("2 - Начать новую компанию");
            int input = ReadInt(1, 2);
            switch (input)
            {
                case 1:
                    _compaing = SaveManeger.LoadCompaing("filePathCompaing");
                    MainMenu();
                    break;
                case 2:
                    MainMenu();
                    break;
            }
        }
        public void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1 - Сюжет");
                Console.WriteLine("2 - Сохранить");
                Console.WriteLine("3 - Добавить персонажа");
                Console.WriteLine("4 - Удалить персонажа");
                Console.WriteLine("5 - Посмотреть всех персонажей");
                Console.WriteLine("6 - Выход");

                int userInput = ReadInt(1, 6);
                switch (userInput)
                {
                    case 1:
                        StoryMenu();
                        break;
                    case 2:
                        SaveManeger.SaveCompaing(_compaing, "filePathCompaing");
                        break;
                    case 3:
                        _compaing.Characters.Add(CreateCharacter());
                        //AddCharacter();
                        break;
                    case 4:
                        RemoveCharacter();
                        break;
                    case 5:
                        ShowAllCharacter();
                        break;
                    case 6:
                        goto end_loop;
                    default:
                        Console.WriteLine("Недопустимое действие. Выберите из предоставленного списка");
                        break;
                }
            }
        end_loop:;

            void DisplayCharacter(Character creature)
            {
                if (creature == null)
                {
                    Console.WriteLine("Первонажа нет");
                }
                else
                {
                    Console.WriteLine($"Имя: {creature.Name}");
                    Console.WriteLine($"Вид: {creature.Type}");
                    Console.WriteLine($"Размер: {creature.Size}");
                    Console.WriteLine($"Опасность: {creature.Danger}");
                    Console.WriteLine($"бонус мастерства: {creature.ProficiencyBonus}");
                    Console.WriteLine($"Класс брони: {creature.ArmorClass}");
                    Console.WriteLine($"Скорость: {creature.Speed}");
                    Console.WriteLine($"Здоровье: {creature.Health}");
                    Console.WriteLine($"Сила: {creature.Strong} (Модификатор: {UIBonus(creature, creature.StrongSavingThrow, creature.Strong)})");
                    Console.WriteLine($"Ловкость: {creature.Dexterity} (Модификатор: {UIBonus(creature, creature.DexteritySavingThrow, creature.Dexterity)})");
                    Console.WriteLine($"Телосложение: {creature.Physique} (Модификатор: {UIBonus(creature, creature.PhysiqueSavingThrow, creature.Physique)})");
                    Console.WriteLine($"Интеллект: {creature.Intelligence} (Модификатор: {UIBonus(creature, creature.IntelligenceSavingThrow, creature.Intelligence)})");
                    Console.WriteLine($"Мудрость: {creature.Wisdom} (Модификатор: {UIBonus(creature, creature.WisdomSavingThrow, creature.Wisdom)})");
                    Console.WriteLine($"Харизма: {creature.Charisma} (Модификатор: {UIBonus(creature, creature.CharismaSavingThrow, creature.Charisma)})");
                }
            }
            //Исправить после изменения класса Character. Метод UIBonus должен принимать объект Character и возвращать строковое представление модификатора способности персонажа.
            // Метод UIBonus возвращает строковое представление модификатора способности персонажа с учетом владения соответствующим спасброском.
            string UIBonus(Character creature, bool savingThrows, int abilityScore)
            {
                if (creature.GetModifier(abilityScore) > 0)
                {
                    return "+" + (creature.GetModifier(abilityScore));
                }
                else if (creature.GetModifier(abilityScore) == 0)
                {
                    return "0";
                }
                else
                {
                    return "-" + creature.GetModifier(abilityScore);
                }

                // Метод ProficiencyBonus возвращает бонус владения персонажа, если он владеет соответствующим спасброском.
                //оствить на потом
                //int ProficiencyBonus(Character creature, bool savingThrows)
                //{
                //    if (savingThrows)
                //    {
                //        return creature.GetProficiencyBonus();
                //    }
                //    else
                //    {
                //        return 0;
                //    }
                //}
            }

            void ShowAllCharacter()
            {
                Console.Clear();
                for (int i = 0; i < _compaing.Characters.Count; i++)
                {
                    Console.Write($"{i + 1}");
                    DisplayCharacter(_compaing.Characters[i]);
                }

                Console.WriteLine("Нажмите на любую клавишу чтобы продолжить");
                Console.ReadKey();
            }

            void RemoveCharacter()
            {
                Console.Clear();
                for (int i = 0; i < _compaing.Characters.Count; i++)
                {
                    Console.WriteLine($"Персонаж {i + 1}:");
                    Console.WriteLine($"Имя: {_compaing.Characters[i].Name}");
                }
                Console.Write("Введите номер персонажа которуя хотите УДАЛИТЬ:  ");
                int input = ReadInt(1, _compaing.Characters.Count);
                if (ConfirmationOfDeletion(input - 1, _compaing.Characters[input - 1].Name))
                {
                    _compaing.Characters.RemoveAt(input - 1);
                    Console.WriteLine("Персонаж удален");
                }
                else
                {
                    Console.WriteLine("Удаление отменено");
                }
                Console.WriteLine("Нажмите на любую клавишу чтобы продолжить");
                Console.ReadKey();
                Console.Clear();
            }
        }

        public void StoryMenu()
        {

            while (true)
            {
                Console.Clear();
                Console.WriteLine("1 - Создать сцену");
                Console.WriteLine("2 - Удалить сцену");
                Console.WriteLine("3 - Добавить связь между сценами");
                Console.WriteLine("4 - Разорвать связь между сценами");
                Console.WriteLine("5 - Просмотреть все сцены");
                Console.WriteLine("6 - Вернуться в главное меню");

                int userInput = ReadInt(1, 6);
                switch (userInput)
                {
                    case 1:
                        CreateStoryGraf();
                        break;
                    case 2:
                        RemoveStotyGraf();
                        break;
                    case 3:
                        AddСonnection();
                        break;
                    case 4:
                        RemoveСonnection();
                        break;
                    case 5:
                        viewingAllStoryGrafs();
                        break;
                    case 6:
                        goto end_loop;
                    default:
                        Console.WriteLine("Недопустимое действие. Выберите из предоставленного списка");
                        break;
                }
            }
        end_loop:

            void CreateStoryGraf()
            {
                Console.Clear();
                Console.Write("Введите название графа: ");
                string title = ReadNotEmptyString();
                Console.Write("Введите содержание графа: ");
                string content = ReadNotEmptyString();
                StoryGraf storyGraf = new StoryGraf(title, content);
                _compaing.StoryGrafManager.AddStoryGraf(storyGraf);
                Console.WriteLine("Граф успешно создан!");
                Console.WriteLine("Нажмите на любую клавишу чтобы продолжить");
                Console.ReadKey();
            }

            void viewingAllStoryGrafs()
            {
                Console.Clear();
                for (int i = 0; i < _compaing.StoryGrafManager.StoryGrafs.Count; i++)
                {
                    Console.WriteLine($"Сцена {i + 1}:");
                    Console.WriteLine($"Название: {_compaing.StoryGrafManager.StoryGrafs[i].Title}");
                    Console.WriteLine($"Содержание: {_compaing.StoryGrafManager.StoryGrafs[i].Content}");
                    Console.WriteLine("Id: " + _compaing.StoryGrafManager.StoryGrafs[i].Id);
                    Console.WriteLine("Следующие графы:");

                    List<StoryGraf> nextGraphs = _compaing.StoryGrafManager.StoryGrafs.FindAll(g => _compaing.StoryGrafManager.StoryGrafs[i].NextsGraphs.Contains(g.Id));
                    foreach (StoryGraf graph in nextGraphs)
                    {
                        Console.WriteLine(graph.Title);
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("Нажмите на любую клавишу чтобы продолжить");
                Console.ReadKey();

            }

            void RemoveStotyGraf()
            {
                Console.Clear();
                for (int i = 0; i < _compaing.StoryGrafManager.StoryGrafs.Count; i++)
                {
                    Console.WriteLine($"Сцена {i + 1}:");
                    Console.WriteLine($"Название: {_compaing.StoryGrafManager.StoryGrafs[i].Title}");
                }

                Console.Write("Введите номер сцены которуя хотите УДАЛИТЬ:  ");
                int input = ReadInt(1, _compaing.StoryGrafManager.StoryGrafs.Count);
                if (ConfirmationOfDeletion(input - 1, _compaing.StoryGrafManager.StoryGrafs[input - 1].Title))
                {
                    _compaing.StoryGrafManager.RemoveStoryGraf(_compaing.StoryGrafManager.StoryGrafs[input - 1].Id);
                    Console.WriteLine("Сцена удалина");
                }
                else
                {
                    Console.WriteLine("Удаление отменено");
                }
                Console.WriteLine("Нажмите на любую клавишу чтобы продолжить");
                Console.ReadKey();
                Console.Clear();
            }

            void AddСonnection()
            {
                Console.Clear();
                for (int i = 0; i < _compaing.StoryGrafManager.StoryGrafs.Count; i++)
                {
                    Console.WriteLine($"Сцена {i + 1}:");
                    Console.WriteLine($"Название: {_compaing.StoryGrafManager.StoryGrafs[i].Title}");
                }

                Console.WriteLine("Ввелите номер первой сцены");
                int Input1 = ReadInt(1, _compaing.StoryGrafManager.StoryGrafs.Count) - 1;
                Console.WriteLine("Ввелите номер второй сцены");
                int Input2 = ReadInt(1, _compaing.StoryGrafManager.StoryGrafs.Count) - 1;

                _compaing.StoryGrafManager.StoryGrafs[Input1].AddNextGraph(_compaing.StoryGrafManager.StoryGrafs[Input2].Id);
                _compaing.StoryGrafManager.StoryGrafs[Input2].AddNextGraph(_compaing.StoryGrafManager.StoryGrafs[Input1].Id);

                Console.WriteLine("Связь добавлена");
                Console.WriteLine("Нажмите на любую клавишу чтобы продолжить");
                Console.ReadKey();
            }

            void RemoveСonnection()
            {
                Console.Clear();
                for (int i = 0; i < _compaing.StoryGrafManager.StoryGrafs.Count; i++)
                {
                    Console.WriteLine($"Сцена {i + 1}:");
                    Console.WriteLine($"Название: {_compaing.StoryGrafManager.StoryGrafs[i].Title}");
                }

                Console.WriteLine("Ввелите номер первой сцены");
                int Input1 = ReadInt(1, _compaing.StoryGrafManager.StoryGrafs.Count) - 1;
                Console.WriteLine("Ввелите номер второй сцены");
                int Input2 = ReadInt(1, _compaing.StoryGrafManager.StoryGrafs.Count) - 1;

                _compaing.StoryGrafManager.StoryGrafs[Input1].RemoveNextGraph(_compaing.StoryGrafManager.StoryGrafs[Input2].Id);
                _compaing.StoryGrafManager.StoryGrafs[Input2].RemoveNextGraph(_compaing.StoryGrafManager.StoryGrafs[Input1].Id);

                Console.WriteLine("Связь удалена");
                Console.WriteLine("Нажмите на любую клавишу чтобы продолжить");
                Console.ReadKey();
            }
        }

        private int ReadInt()
        {
            while (true)
            {
                string input = ReadNotEmptyString();

                if (int.TryParse(input, out int result))
                {
                    return result;
                }
                Console.Write("Ошибка! Введите число: ");
            }
        }

        private int ReadInt(int min, int max)
        {
            while (true)
            {
                string input = ReadNotEmptyString();

                if (int.TryParse(input, out int result))
                {
                    if (result >= min && result <= max)
                    {
                        return result;
                    }
                }

                Console.WriteLine($"Введите число от {min}до {max}");
            }
        }

        private string ReadNotEmptyString()
        {
            while (true)
            {
                string input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }

                Console.WriteLine("Поле не может бытьпустым");
            }
        }


        public Character CreateCharacter()
        {
            Console.Clear();
            Console.Write("Ведите имя персонажа: ");
            string name = ReadNotEmptyString();

            Console.Write("Ведите вид персонажа: ");
            string type = ReadNotEmptyString();

            Console.Write("Ведите размер персонажа: ");
            string size = ReadNotEmptyString();

            Console.Write("Ведите опасность персонажа: ");
            int danger = ReadInt(1, 20);

            Console.Write("Ведите класс брони персонажа: ");
            int armorClass = ReadInt(1, 20);

            Console.Write("Ведите здоровье персонажа: ");
            int health = ReadInt(1, 20);

            Console.Write("Ведите бонус мастерства персонажа: ");
            int proficiencyBonus = ReadInt(1, 20);

            Console.Write("Ведите силу персонажа: ");
            int strong = ReadInt(1, 20);

            Console.Write("Ведите ловкость персонажа: ");
            int dexterity = ReadInt(1, 20);

            Console.Write("Ведите телосложение персонажа: ");
            int physique = ReadInt(1, 20);

            Console.Write("Ведите интеллект персонажа: ");
            int intelligence = ReadInt(1, 20);

            Console.Write("Ведите мудрость персонажа: ");
            int wisdom = ReadInt(1, 20);

            Console.Write("Ведите харизму персонажа: ");
            int charisma = ReadInt(1, 20);

            Console.WriteLine("Персонаж владеет спас бросками");
            Console.WriteLine("1 - Да");
            Console.WriteLine("2 - Нет");

            int input = ReadInt(1, 2);


            switch (input)
            {
                case 1:
                    Console.WriteLine("Выбекрите какими спасбросками владеет персонаж: ");
                    Console.Write("1 Сила\n2 Ловкость\n3 Телосложение\n4 Интеллект\n5 Мудрость\n6 Харизма\n");
                    Console.WriteLine("Введите цифры подряд. После ввода нажмите Enter");
                    string savingThrowsInput = ReadNotEmptyString();

                    bool strongSavingThrow = false;
                    bool dexteritySavingThrow = false;
                    bool physiqueSavingThrow = false;
                    bool intelligenceSavingThrow = false;
                    bool wisdomSavingThrow = false;
                    bool charismaSavingThrow = false;

                    for (int i = 0; i < savingThrowsInput.Length; i++)
                    {
                        int buf = int.Parse(savingThrowsInput[i].ToString());

                        switch (buf)
                        {
                            case 1:
                                strongSavingThrow = true;
                                break;
                            case 2:
                                dexteritySavingThrow = true;
                                break;
                            case 3:
                                physiqueSavingThrow = true;
                                break;
                            case 4:
                                intelligenceSavingThrow = true;
                                break;
                            case 5:
                                wisdomSavingThrow = true;
                                break;
                            case 6:
                                charismaSavingThrow = true;
                                break;
                            default:
                                Console.WriteLine("Недопустимое действие. Выберите из предоставленного списка");
                                break;
                        }
                    }
                    Console.WriteLine("Нажмите на любую клавишу чтобы продолжить");
                    Console.ReadKey();
                    return new Character(name, type, size, danger, armorClass, health, proficiencyBonus, strong, dexterity, physique, intelligence, wisdom, charisma,
                    strongSavingThrow, dexteritySavingThrow, physiqueSavingThrow, intelligenceSavingThrow, wisdomSavingThrow, charismaSavingThrow);
                case 2:
                    Console.WriteLine("Персонаж не владеет спас бросками");
                    Console.WriteLine("Нажмите на любую клавишу чтобы продолжить");
                    Console.ReadKey();
                    return new Character(name, type, size, danger, armorClass, health, proficiencyBonus, strong, dexterity, physique, intelligence, wisdom, charisma);
                default:
                    Console.WriteLine("Невозможрое событие");
                    Console.ReadKey();
                    return null;
            }
        }

        public bool ConfirmationOfDeletion(int index, string delineonObjekt)
        {
            Console.WriteLine($"Вы уверены что хотите удалить {delineonObjekt}");
            Console.WriteLine("Нажмите Y/N для подтверждения");
            ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();
            ConsoleKey keyInfo = consoleKeyInfo.Key;
            switch (keyInfo)
            {
                case ConsoleKey.Y:
                    return true;
                case ConsoleKey.N:
                    return false;
                default:
                    return ConfirmationOfDeletion(index, delineonObjekt);
            }
        }
    }

    class Compaing
    {
        public List<Character> Characters { get; private set; } = new();
        public StoryGrafManager StoryGrafManager { get; private set; } = new();

        [JsonConstructor]
        public Compaing(List<Character> characters, StoryGrafManager storyGrafManager)
        {
            Characters = characters;
            StoryGrafManager = storyGrafManager;
        }

        public Compaing()
        {
            Characters = new List<Character>();
            StoryGrafManager = new StoryGrafManager();
        }
    }

    class Character
    {
        public string Name { get; private set; }
        public string Type { get; private set; }
        public string Size { get; private set; }
        public int ArmorClass { get; private set; }
        public int Speed { get; private set; }
        public int Danger { get; private set; }
        public int Health { get; private set; }
        public int ProficiencyBonus { get; private set; }

        public int Strong { get; private set; }
        public bool StrongSavingThrow { get; private set; }

        public int Dexterity { get; private set; }
        public bool DexteritySavingThrow { get; private set; }
        public int Initiative { get; private set; }

        public int Physique { get; private set; }
        public bool PhysiqueSavingThrow { get; private set; }

        public int Intelligence { get; private set; }
        public bool IntelligenceSavingThrow { get; private set; }


        public int Wisdom { get; private set; }
        public bool WisdomSavingThrow { get; private set; }

        public int Charisma { get; private set; }
        public bool CharismaSavingThrow { get; private set; }

        [JsonConstructor]
        public Character(string name, string type, string size, int danger, int armorClass, int health, int proficiencyBonus,
                int strong, int dexterity, int physique, int intelligence, int wisdom, int charisma,
                bool strongSavingThrow = false, bool dexteritySavingThrow = false, bool physiqueSavingThrow = false, bool intelligenceSavingThrow = false, bool wisdomSavingThrow = false, bool charismaSavingThrow = false)

        {
            Name = name;
            Type = type;
            Size = size;
            Danger = danger;
            ArmorClass = armorClass;
            Health = health;
            ProficiencyBonus = proficiencyBonus;
            Strong = strong;
            Dexterity = dexterity;
            Physique = physique;
            Intelligence = intelligence;
            Wisdom = wisdom;
            Charisma = charisma;
            Initiative = GetModifier(Dexterity);
            StrongSavingThrow = strongSavingThrow;
            DexteritySavingThrow = dexteritySavingThrow;
            PhysiqueSavingThrow = physiqueSavingThrow;
            IntelligenceSavingThrow = intelligenceSavingThrow;
            WisdomSavingThrow = wisdomSavingThrow;
            CharismaSavingThrow = charismaSavingThrow;

        }

        public int GetModifier(int abilityScore)
        {
            abilityScore = (abilityScore - 10) / 2;
            return abilityScore;
        }

        public void TakeDamage(int damage)
        {
            if (Health - damage >= 0)
            {
                Health -= damage;
            }
            else
            {
                Health = 0;
            }


        }
    }

    //добавить потом когда все заработает
    class SkillSet
    {
        public bool Athletics { get; private set; }
        public bool Acrobatics { get; private set; }
        public bool SleightOfHand { get; private set; }
        public bool Stealth { get; private set; }
        public bool Analysis { get; private set; }
        public bool Story { get; private set; }
        public bool Magic { get; private set; }
        public bool Nature { get; private set; }
        public bool Religion { get; private set; }
        public bool Perception { get; private set; }
        public bool Survival { get; private set; }
        public bool Medicine { get; private set; }
        public bool Insight { get; private set; }
        public bool AnimalCare { get; private set; }
        public bool Performance { get; private set; }
        public bool Intimidation { get; private set; }
        public bool Deception { get; private set; }
        public bool Belief { get; private set; }
    }

    class StoryGraf
    {
        public long Id { get; private set; }
        public string Title { get; private set; }
        public string Content { get; private set; }
        public List<long> NextsGraphs { get; private set; } = new List<long>();

        private static Random _random = new Random();

        [JsonConstructor]
        public StoryGraf(long id, string title, string content, List<long> nextsGraphs)
        {
            Id = id;
            Title = title;
            Content = content;
            NextsGraphs = nextsGraphs;
        }

        public StoryGraf(string title, string content)
        {
            Id = CreateId();
            Title = title;
            Content = content;
        }

        public static long CreateId()
        {
            string result = "";

            for (int i = 0; i < 10; i++)
            {
                result += _random.Next(0, 10);
            }

            long id = long.Parse(result);

            return id;
        }

        //Метод AddNextGraph добавляет идентификатор следующего графа в список NextsGraphs,
        //если он еще не присутствует в списке. Это позволяет создавать связи между графами и строить структуру сюжета.
        public void AddNextGraph(long graphId)
        {
            if (!NextsGraphs.Contains(graphId))
            {
                NextsGraphs.Add(graphId);
            }
        }

        public void RemoveNextGraph(long graphId)
        {
            if (NextsGraphs.Contains(graphId))
            {
                if (NextsGraphs.Count > 0)
                {
                    for (int i = 0; NextsGraphs.Count > i; i++)
                    {
                        if (NextsGraphs[i] == graphId)
                        {
                            NextsGraphs.RemoveAt(i);
                        }
                    }
                }
            }
        }
    }

    class StoryGrafManager
    {
        public List<StoryGraf> StoryGrafs { get; private set; } = new List<StoryGraf>();


        public StoryGrafManager()
        {
            StoryGrafs = new List<StoryGraf>();
        }

        [JsonConstructor]
        public StoryGrafManager(List<StoryGraf> storyGrafs)
        {
            StoryGrafs = storyGrafs;
        }

        public void AddStoryGraf(StoryGraf storyGraf)
        {
            //Интерфейс вызывает метод AddStoryGraf, передавая ему объект StoryGraf который уже создан.
            StoryGrafs.Add(storyGraf);
        }

        public void RemoveStoryGraf(long id)
        {
            for (int i = 0; i < StoryGrafs.Count; i++)
            {
                if (StoryGrafs[i].Id == id)
                {
                    List<long> removingId = StoryGrafs[i].NextsGraphs;
                    for (int j = 0; j < StoryGrafs.Count; j++)
                    {
                        for (int k = 0; k < removingId.Count; k++)
                        {
                            if (StoryGrafs[j].NextsGraphs.Contains(id))
                            {
                                StoryGrafs[j].NextsGraphs.Remove(id);
                                break;
                            }
                        }

                    }
                    StoryGrafs.RemoveAt(i);
                    break;
                }
            }
        }
    }
    class SaveManeger
    {
        public static void SaveCompaing(Compaing compaing, string filePathCompaing)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(compaing);
            Console.WriteLine(json);
            Console.ReadKey();
            File.WriteAllText(filePathCompaing, json);
        }

        public static Compaing LoadCompaing(string filePathCompaing)
        {
            if (File.Exists(filePathCompaing))
            {
                string json = File.ReadAllText(filePathCompaing);
                Compaing compaing = JsonSerializer.Deserialize<Compaing>(json);
                if (compaing == null)
                {
                    Console.WriteLine("Файл пуст");
                    Console.ReadKey();
                    return compaing;
                }

                return compaing;
            }
            else
            {
                //Перенести в ConsoleUI вывод ошибки
                Console.WriteLine("Файл не найден.");
                Console.ReadKey();
                return null;
            }
        }
    }
}