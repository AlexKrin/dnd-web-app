using System.Diagnostics;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;

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
                Console.WriteLine("4 - Редактировать персонажа");
                Console.WriteLine("5 - Удалить персонажа");
                Console.WriteLine("6 - Посмотреть всех персонажей");
                Console.WriteLine("7 - Выход");

                int userInput = ReadInt(1, 7);
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
                        EditCharacter();
                        break;
                    case 5:
                        RemoveCharacter();
                        break;
                    case 6:
                        ShowAllCharacter();
                        break;
                    case 7:
                        goto end_loop;
                    default:
                        Console.WriteLine("Недопустимое действие. Выберите из предоставленного списка");
                        break;
                }
            }
        end_loop:;

            void EditCharacter()
            {
                Console.Clear();
                if (_compaing.Characters.Count == 0)
                {
                    Console.WriteLine("Персонажей нет");
                    Console.WriteLine("Нажмите на любую клавишу чтобы продолжить");
                    Console.ReadKey();
                    return;
                }
                for (int i = 0; i < _compaing.Characters.Count; i++)
                {
                    Console.WriteLine($"Персонаж {i + 1}:");
                    Console.WriteLine($"Имя: {_compaing.Characters[i].Name}");
                }
                Console.WriteLine();
                Console.Write("Каккого персонажа вы хотите отредактировать?");
                int characterIndex = ReadInt(1, _compaing.Characters.Count) - 1;
                Console.Clear();
                Console.WriteLine($" 1 Имя: {_compaing.Characters[characterIndex].Name}");
                Console.WriteLine($" 2 Вид: {_compaing.Characters[characterIndex].Type}");
                Console.WriteLine($" 3 Размер: {_compaing.Characters[characterIndex].Size}");
                Console.WriteLine($" 4 Опасность: {_compaing.Characters[characterIndex].Danger}");
                Console.WriteLine($" 5 Класс брони: {_compaing.Characters[characterIndex].ArmorClass}");
                Console.WriteLine($" 6 Скорость: {_compaing.Characters[characterIndex].Speed}");
                Console.WriteLine($" 7 Здоровье: {_compaing.Characters[characterIndex].Health}");
                Console.WriteLine($" 8 Сила: {_compaing.Characters[characterIndex].Strong} (Модификатор: {UIBonus(_compaing.Characters[characterIndex], _compaing.Characters[characterIndex].Strong)})");
                Console.WriteLine($" 9 Ловкость: {_compaing.Characters[characterIndex].Dexterity} (Модификатор: {UIBonus(_compaing.Characters[characterIndex], _compaing.Characters[characterIndex].Dexterity)})");
                Console.WriteLine($" 10 Телосложение: {_compaing.Characters[characterIndex].Physique} (Модификатор: {UIBonus(_compaing.Characters[characterIndex], _compaing.Characters[characterIndex].Physique)})");
                Console.WriteLine($" 11 Интеллект: {_compaing.Characters[characterIndex].Intelligence} (Модификатор: {UIBonus(_compaing.Characters[characterIndex], _compaing.Characters[characterIndex].Intelligence)})");
                Console.WriteLine($" 12 Мудрость: {_compaing.Characters[characterIndex].Wisdom} (Модификатор: {UIBonus(_compaing.Characters[characterIndex], _compaing.Characters[characterIndex].Wisdom)})");
                Console.WriteLine($" 13 Харизма: {_compaing.Characters[characterIndex].Charisma} (Модификатор: {UIBonus(_compaing.Characters[characterIndex], _compaing.Characters[characterIndex].Charisma)})");

                for (int i = 0; i < _compaing.Characters[characterIndex].AdditionalInformation.Count; i++)
                {
                    Console.WriteLine($"Дополнительная информация {(i + 1) + 13}: {_compaing.Characters[characterIndex].AdditionalInformation[i]}");
                }

                Console.Write("Введите номер параметра который хотите изменить");
                int parameterIndex = ReadInt(1, 14 + _compaing.Characters[characterIndex].AdditionalInformation.Count) - 1;

                if (parameterIndex >= 1 && parameterIndex <= 3)
                {
                    Console.WriteLine("Введите новое значение");
                    string newValue = ReadNotEmptyString();
                    _compaing.Characters[characterIndex].EditStringParameter(newValue, parameterIndex);
                }
                else if (parameterIndex > 3 && parameterIndex <= 13)
                {
                    Console.WriteLine("Введите новое значение");
                    int newValue = ReadInt(1, 20);
                    _compaing.Characters[characterIndex].EditInrParameter( newValue, parameterIndex);
                }
                else if (parameterIndex > 14 && parameterIndex <= 13 + _compaing.Characters[characterIndex].AdditionalInformation.Count)
                {
                    Console.WriteLine("Введите новое значение");
                    string newValue = ReadNotEmptyString();
                    _compaing.Characters[characterIndex].EditAdditionalParameter(newValue, parameterIndex - 14);
                }

            }

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
                    Console.WriteLine($"Инициатива: {creature.Initiative}");
                    Console.WriteLine($"Сила: {creature.Strong} (Модификатор: {UIBonus(creature, creature.Strong)})");
                    Console.WriteLine($"Ловкость: {creature.Dexterity} (Модификатор: {UIBonus(creature, creature.Dexterity)})");
                    Console.WriteLine($"Телосложение: {creature.Physique} (Модификатор: {UIBonus(creature, creature.Physique)})");
                    Console.WriteLine($"Интеллект: {creature.Intelligence} (Модификатор: {UIBonus(creature, creature.Intelligence)})");
                    Console.WriteLine($"Мудрость: {creature.Wisdom} (Модификатор: {UIBonus(creature, creature.Wisdom)})");
                    Console.WriteLine($"Харизма: {creature.Charisma} (Модификатор: {UIBonus(creature, creature.Charisma)})");

                    //Добавить вывод спасбросков персонажа при их наличии
                    for (int i = 0; i < creature.AdditionalInformation.Count; i++)
                    {
                        Console.WriteLine($"Дополнительная информация {i + 1}: {creature.AdditionalInformation[i]}");
                    }
                }
            }
            //Исправить после изменения класса Character. Метод UIBonus должен принимать объект Character и возвращать строковое представление модификатора способности персонажа.
            // Метод UIBonus возвращает строковое представление модификатора способности персонажа с учетом владения соответствующим спасброском.
            string UIBonus(Character creature, int abilityScore)
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
                    return new Character(name, type, size, danger, armorClass, health, proficiencyBonus, strong, dexterity, physique, intelligence, wisdom, charisma, AdditionalInformation(),
                    strongSavingThrow, dexteritySavingThrow, physiqueSavingThrow, intelligenceSavingThrow, wisdomSavingThrow, charismaSavingThrow);
                case 2:
                    Console.WriteLine("Персонаж не владеет спас бросками");
                    Console.WriteLine("Нажмите на любую клавишу чтобы продолжить");
                    Console.ReadKey();
                    return new Character(name, type, size, danger, armorClass, health, proficiencyBonus, strong, dexterity, physique, intelligence, wisdom, charisma, AdditionalInformation());
                default:
                    Console.WriteLine("Невозможрое событие");
                    Console.ReadKey();
                    return null;
            }
            List<string> AdditionalInformation()
            {
                Console.WriteLine("Введите дополнительную информацию о персонаже (если её нет введите пустую строку)");
                List<string> additionalInformation = new List<string>();
                string addinput = Console.ReadLine();
                additionalInformation.Add(addinput);
                if (additionalInformation[1] == "")
                {
                    return additionalInformation;
                }
                while (true)
                {
                    Console.WriteLine("если хотите дополнить информацию нажмите 1. Если хотите завершить нажмите 2");
                    int choice = ReadInt(1, 2);
                    if (choice == 1)
                    {
                        Console.WriteLine("Введите дополнительную информацию о персонаже (если её нет введите пустую строку)");
                        string addinput2 = Console.ReadLine();
                        additionalInformation.Add(addinput2);
                    }
                    else
                    {
                        return additionalInformation;
                    }

                }
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

        public List<string> AdditionalInformation { get; private set; } = new();

        [JsonConstructor]
        public Character(string name, string type, string size, int danger, int armorClass, int health, int proficiencyBonus,
                int strong, int dexterity, int physique, int intelligence, int wisdom, int charisma, List<string> additionalInformation,
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
            AdditionalInformation = additionalInformation;

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

        public void EditAdditionalParameter(string stringParameter, int index)
        {
            AdditionalInformation[index] = stringParameter;
        }

        public void EditStringParameter(string stringParameter, int index)
        {
            switch(index)
            {
                case 1:
                    Name = stringParameter;
                    break;
                case 2:
                    Type = stringParameter;
                    break;
                case 3:
                    Size = stringParameter;
                    break;
                
            }
        }

        public void EditInrParameter(int intParameter, int index)
        {
            switch (index)
            {
                case 4:
                    Danger = intParameter;
                    break;
                case 5:
                    ArmorClass = intParameter;
                    break;
                case 6:
                    Speed = intParameter;
                    break;
                case 7:
                    Health = intParameter;
                    break;
                case 8:
                    Strong = intParameter;
                    break;
                case 9:
                    Dexterity = intParameter;
                    break;
                case 10:
                    Physique = intParameter;
                    break;
                case 11:
                    Intelligence = intParameter;
                    break;
                case 12:
                    Wisdom = intParameter;
                    break;
                case 13:
                    Charisma = intParameter;
                    break;
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