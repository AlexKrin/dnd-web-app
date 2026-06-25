using System.Diagnostics;
using System.Xml.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace dnd_web_app
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Creature creature = new ConsoleUI().CreateCharacter();
            //ConsoleUI.DisplayCharacter(creature);
            //creature.TakeDamage(10);
            //ConsoleUI.DisplayCharacter(creature);
            //SaveManeger.SaveCharacter(creature, "character.json");
            Creature loadedCreature = SaveManeger.LoadCharacter("character.json");
            ConsoleUI.DisplayCharacter(loadedCreature);
        }
    }
    class ConsoleUI
    {
        private StoryGrafManager storyGrafManager;
        public void MainMenu()
        {
            Console.WriteLine("1 - Сюжет");
            Console.WriteLine("2 - Добавить персонажа");
            Console.WriteLine("3 - Сохронить");
            Console.WriteLine("4 - Загрузить сохронения");
            Console.WriteLine("5 - Выход");

            int userInput = int.Parse(Console.ReadLine());
            switch (userInput)
            {
                case 1:
                    StoryMenu();
                    break;
                case 2:
                    CreateCharacter();
                    break;
                case 3:
                    break;
                case 4:
                    break;
            }
        }

        public void StoryMenu()
        {

            while (true)
            {
                Console.WriteLine("1 - Создать сцену");
                Console.WriteLine("2 - Удалить сцену");
                Console.WriteLine("3 - Добавить связь между сценами");
                Console.WriteLine("4 - Просмотреть все сцены");
                Console.WriteLine("5 - Вернуться в главное меню");
                int userInput = int.Parse(Console.ReadLine());
                switch (userInput)
                {
                    case 1:
                        CreateStoryGraf();
                        break;
                    case 2:
                        RemoveStotyGraf();
                        break;
                    case 3:
                        break;
                    case 4:
                        viewingAllStoryGrafs();
                        break;
                    case 5:
                        goto end_loop;

                }
            }
            end_loop:

            void CreateStoryGraf()
            {
                Console.Write("Введите название графа: ");
                string title = Console.ReadLine();
                Console.Write("Введите содержание графа: ");
                string content = Console.ReadLine();
                StoryGraf storyGraf = new StoryGraf(0, title, content);
                storyGrafManager.AddStoryGraf(storyGraf);
                Console.WriteLine("Граф успешно создан!");
            }

            void viewingAllStoryGrafs()
            {
                for (int i = 0; i < storyGrafManager.StoryGrafs.Count; i++)
                {
                    Console.WriteLine($"Сцена {i + 1}:");
                    Console.WriteLine($"Название: {storyGrafManager.StoryGrafs[i].Title}");
                    Console.WriteLine($"Содержание: {storyGrafManager.StoryGrafs[i].Content}");
                    Console.WriteLine("Следующие графы:");
                    foreach (long nextGraphId in storyGrafManager.StoryGrafs[i].NextsGraphs)
                    {
                        Console.WriteLine($"- {nextGraphId}");
                    }
                    Console.WriteLine();
                }
            }

            void RemoveStotyGraf()
            {
                for (int i = 0; i < storyGrafManager.StoryGrafs.Count; i++)
                {
                    Console.WriteLine($"Сцена {i + 1}:");
                    Console.WriteLine($"Название: {storyGrafManager.StoryGrafs[i].Title}");
                }

                Console.Write("Введите номер сцен которуя хотите УДАИТЬ:  ");
                int input = int.Parse(Console.ReadLine());

                storyGrafManager.RemoveStoryGraf(storyGrafManager.StoryGrafs[input - 1].Id);

                Console.WriteLine("Сцена удалина");
            }
        }
        public Creature CreateCharacter()
        {
            Console.Write("Ведите имя персонажа: ");
            string name = Console.ReadLine();

            Console.Write("Ведите класс персонажа: ");
            string @class = Console.ReadLine();

            Console.Write("Ведите расу персонажа: ");
            string race = Console.ReadLine();

            Console.Write("Ведите уровень персонажа: ");
            int level = int.Parse(Console.ReadLine());

            Console.Write("Ведите класс брони персонажа: ");
            int armorClass = int.Parse(Console.ReadLine());

            Console.Write("Ведите здоровье персонажа: ");
            int health = int.Parse(Console.ReadLine());


            Console.Write("Ведите силу персонажа: ");
            int strong = int.Parse(Console.ReadLine());

            Console.Write("Ведите ловкость персонажа: ");
            int dexterity = int.Parse(Console.ReadLine());

            Console.Write("Ведите телосложение персонажа: ");
            int physique = int.Parse(Console.ReadLine());

            Console.Write("Ведите интеллект персонажа: ");
            int intelligence = int.Parse(Console.ReadLine());

            Console.Write("Ведите мудрость персонажа: ");
            int wisdom = int.Parse(Console.ReadLine());

            Console.Write("Ведите харизму персонажа: ");
            int charisma = int.Parse(Console.ReadLine());

            Console.WriteLine("Выбекрите какими спасбросками владеет персонаж: ");
            Console.Write("1 Сила\n2 Ловкость\n3 Телосложение\n4 Интеллект\n5 Мудрость\n6 Харизма\n");
            Console.WriteLine("Введите цифры подряд. После ввода нажмите Enter");
            string savingThrowsInput = Console.ReadLine();

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
                }
            }

            return new Creature(name, @class, race, level, armorClass, health, strong, dexterity, physique, intelligence, wisdom, charisma,
                strongSavingThrow, dexteritySavingThrow, physiqueSavingThrow, intelligenceSavingThrow, wisdomSavingThrow, charismaSavingThrow);
        }

        public static void DisplayCharacter(Creature creature)
        {
            Console.WriteLine($"Имя: {creature.Name}");
            Console.WriteLine($"Класс: {creature.Class}");
            Console.WriteLine($"Раса: {creature.Race}");
            Console.WriteLine($"Уровень: {creature.Level}");
            Console.WriteLine($"Класс брони: {creature.ArmorClass}");
            Console.WriteLine($"Здоровье: {creature.Health}");
            Console.WriteLine($"Сила: {creature.Strong} (Модификатор: {UIBonus(creature, creature.StrongSavingThrow, creature.Strong)})");
            Console.WriteLine($"Ловкость: {creature.Dexterity} (Модификатор: {UIBonus(creature, creature.DexteritySavingThrow, creature.Dexterity)})");
            Console.WriteLine($"Телосложение: {creature.Physique} (Модификатор: {UIBonus(creature, creature.PhysiqueSavingThrow, creature.Physique)})");
            Console.WriteLine($"Интеллект: {creature.Intelligence} (Модификатор: {UIBonus(creature, creature.IntelligenceSavingThrow, creature.Intelligence)})");
            Console.WriteLine($"Мудрость: {creature.Wisdom} (Модификатор: {UIBonus(creature, creature.WisdomSavingThrow, creature.Wisdom)})");
            Console.WriteLine($"Харизма: {creature.Charisma} (Модификатор: {UIBonus(creature, creature.CharismaSavingThrow, creature.Charisma)})");
        }

        private static string UIBonus(Creature creature, bool savingThrows, int abilityScore)
        {
            if (creature.GetModifier(abilityScore) + ProficiencyBonus(creature, savingThrows) > 0)
            {
                return "+" + (creature.GetModifier(abilityScore) + ProficiencyBonus(creature, savingThrows));
            }
            else if (creature.GetModifier(abilityScore) + ProficiencyBonus(creature, savingThrows) == 0)
            {
                return Convert.ToString(creature.GetModifier(abilityScore) + ProficiencyBonus(creature, savingThrows));
            }
            else
            {
                return Convert.ToString(creature.GetModifier(abilityScore) + ProficiencyBonus(creature, savingThrows));
            }

            static int ProficiencyBonus(Creature creature, bool savingThrows)
            {
                if (savingThrows)
                {
                    return creature.GetProficiencyBonus();
                }
                else
                {
                    return 0;
                }
            }
        }
    }

    class Creature
    {
        public string Name { get; set; }
        public string Class { get; private set; }
        public string Race { get; private set; }
        public int Level { get; private set; }
        public int ArmorClass { get; private set; }
        public int Health { get; private set; }

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
        public Creature(string name, string @class, string race, int level, int armorClass, int health,
                int strong, int dexterity, int physique, int intelligence, int wisdom, int charisma,
                bool strongSavingThrow = false, bool dexteritySavingThrow = false, bool physiqueSavingThrow = false, bool intelligenceSavingThrow = false, bool wisdomSavingThrow = false, bool charismaSavingThrow = false)

        {
            Name = name;
            Class = @class;
            Race = race;
            Level = level;
            ArmorClass = armorClass;
            Health = health;
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

        public int GetProficiencyBonus()
        {
            double OwnershipBonus = Math.Ceiling((Level - 1) / 4.0);
            int RoundedOwnershipBonus = (int)OwnershipBonus;
            return RoundedOwnershipBonus + 2;
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

    class SaveManeger
    {
        public static void SaveCharacter(Creature creature, string filePath)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(creature);
            File.WriteAllText(filePath, json);
        }

        public static Creature LoadCharacter(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                Creature creature = JsonSerializer.Deserialize<Creature>(json);
                return creature;
            }
            else
            {
                Console.WriteLine("Файл не найден.");
                return null;
            }
        }
    }

    class StoryGraf
    {
        public long Id { get; private set; }
        public string Title { get; private set; }
        public string Content { get; private set; }
        public List<long> NextsGraphs { get; private set; } = new List<long>();

        public StoryGraf(int id, string title, string content)
        {
            Id = CreateId();
            Title = title;
            Content = content;
        }

        public static long CreateId()
        {
            Random random = new Random();

            string result = "";

            for (int i = 0; i < 10; i++)
            {
                result += random.Next(0, 10);
            }

            long id = long.Parse(result);

            return id;
        }

        //Метод AddNearestGraph добавляет идентификатор следующего графа в список NextsGraphs,
        //если он еще не присутствует в списке. Это позволяет создавать связи между графами и строить структуру сюжета.
        public void AddNearestGraph(long graphId)
        {
            if (!NextsGraphs.Contains(graphId))
            {
                NextsGraphs.Add(graphId);
            }
        }
    }

    class StoryGrafManager
    {
        public List<StoryGraf> StoryGrafs { get; private set; }

        public StoryGrafManager()
        {
            StoryGrafs = new List<StoryGraf>();
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
                    List<long> RemovingId = StoryGrafs[i].NextsGraphs;
                    for (int j = 0; j < StoryGrafs.Count; j++)
                    {
                        for (int k = 0; k < RemovingId.Count; k++)
                        {
                            if (StoryGrafs[j].NextsGraphs.Contains(RemovingId[k]))
                            {
                                StoryGrafs[j].NextsGraphs.Remove(RemovingId[k]);
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
}