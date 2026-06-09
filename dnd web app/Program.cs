using System.Diagnostics;
using System.Xml.Linq;

namespace dnd_web_app
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Creature creature = new ConsoleUI().CreateCharacter();
            ConsoleUI.DisplayCharacter(creature);
            creature.TakeDamage(10);
            ConsoleUI.DisplayCharacter(creature);

        }
    }
    class ConsoleUI
    {
        public Creature CreateCharacter()
        {
            Console.Write("Ведите имя персонажа: ");
            string name = Console.ReadLine();

            Console.Write("Ведите класс персонажа: ");
            string characterClass = Console.ReadLine();

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

            return new Creature(name, characterClass, race, level, armorClass, health, strong, dexterity, physique, intelligence, wisdom, charisma,
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

        private static int ProficiencyBonus(Creature creature, bool savingThrows)
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

        private static string UIBonus(Creature creature, bool savingThrows, int abilityScore)
        {
            if(creature.GetModifier(abilityScore) + ProficiencyBonus(creature, savingThrows) > 0)
            {
                return "+" + (creature.GetModifier(abilityScore) + ProficiencyBonus(creature, savingThrows));
            }
            else if(creature.GetModifier(abilityScore) + ProficiencyBonus(creature, savingThrows) == 0)
            {
                return Convert.ToString(creature.GetModifier(abilityScore) + ProficiencyBonus(creature, savingThrows));
            }
            else
            {
                return Convert.ToString(creature.GetModifier(abilityScore) + ProficiencyBonus(creature, savingThrows));
            }
        }
    }

    class Creature
    {
        public string Name { get; private set; }
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


        public Creature(string name, string characterClass, string race, int level, int armorClass, int health,
                int strong, int dexterity, int physique, int intelligence, int wisdom, int charisma,
                bool strongSavingThrow = false, bool dexteritySavingThrow = false, bool physiqueSavingThrow = false, bool intelligenceSavingThrow = false, bool wisdomSavingThrow = false, bool charismaSavingThrow = false)

        {

            Name = name;
            Class = characterClass;
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

        public Creature()
        {
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
}

