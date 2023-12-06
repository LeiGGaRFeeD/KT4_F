using System;
using System.Collections.Generic;

ILogger logger = new ConsoleLogger();

IHero swordHero = HeroFactory.CreateSwordHero("Sword Hero", logger);
IHero fireHero = HeroFactory.CreateFireHero("Fire Hero", logger);
IHero freezeHero = HeroFactory.CreateFreezeHero("Freeze Hero", logger);

swordHero.Attack(fireHero);
fireHero.Attack(freezeHero);
freezeHero.Attack(swordHero);

public interface ILogger
{
    void Log(string message);
}

    public interface IAbility
    {
        string Name { get; }
        void Use(IHero user, IHero target);
    }

    public interface IHero
    {
        string Name { get; }
        List<IAbility> Abilities { get; }
        int AttackDamage { get; set; }
        void Attack(IHero target);
        void TakeDamage(int damage, IHero attacker);
    }

    public class Hero : IHero
    {
        public string Name { get; private set; }
        public List<IAbility> Abilities { get; private set; }
        public int AttackDamage { get; set; }
        private ILogger logger;

        public Hero(string name, List<IAbility> abilities, ILogger logger)
        {
            Name = name;
            Abilities = abilities;
            this.logger = logger;
        }

        public void Attack(IHero target)
        {
            logger.Log($"{Name} is attacking {target.Name}");
            foreach (var ability in Abilities)
            {
                ability.Use(this, target);
            }
        }

        public void TakeDamage(int damage, IHero attacker)
        {
            logger.Log($"{Name} takes {damage} damage from {attacker.Name}");
        }
    }

    public class BaseAbility : IAbility
    {
        public string Name { get; private set; }
        public int Damage { get; private set; }
        public int Range { get; private set; }
        private ILogger logger;

        public BaseAbility(string name, int damage, int range, ILogger logger)
        {
            Name = name;
            Damage = damage;
            Range = range;
            this.logger = logger;
        }

        public void Use(IHero user, IHero target)
        {
            logger.Log($"{user.Name} uses {Name} on {target.Name}");
            target.TakeDamage(Damage, user);
        }
    }

    public class SwordAbility : IAbility
    {
        public string Name { get; private set; }
        public int DamageMultiplier { get; private set; }
        private ILogger logger;

        public SwordAbility(string name, int damageMultiplier, ILogger logger)
        {
            Name = name;
            DamageMultiplier = damageMultiplier;
            this.logger = logger;
        }

        public void Use(IHero user, IHero target)
        {
            int damage = user.AttackDamage * DamageMultiplier;
            target.TakeDamage(damage, user);
            logger.Log($"{user.Name} uses {Name} on {target.Name}");
        }
    }

    public class FireAbility : IAbility
    {
        public string Name { get; private set; }
        public int BurnTime { get; private set; }
        private ILogger logger;

        public FireAbility(string name, int burnTime, ILogger logger)
        {
            Name = name;
            BurnTime = burnTime;
            this.logger = logger;
        }

        public void Use(IHero user, IHero target)
        {
            target.TakeDamage(user.AttackDamage, user);
            logger.Log($"{user.Name} uses {Name} on {target.Name}");
            logger.Log($"{target.Name} is burning for {BurnTime} seconds");
        }
    }

    public class FreezeAbility : IAbility
    {
        public string Name { get; private set; }
        public Color IceColor { get; private set; }
        public int CooldownTime { get; private set; }
        private ILogger logger;

        public FreezeAbility(string name, Color iceColor, int cooldownTime, ILogger logger)
        {
            Name = name;
            IceColor = iceColor;
            CooldownTime = cooldownTime;
            this.logger = logger;
        }

        public void Use(IHero user, IHero target)
        {
            target.TakeDamage(user.AttackDamage, user);
            logger.Log($"{user.Name} uses {Name} on {target.Name}");
            logger.Log($"Freezing {target.Name} with {IceColor} ice");
            logger.Log($"Cooldown time: {CooldownTime} seconds");
        }
    }



    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }

    public enum Color
    {
        White,
        Blue,
        Green,
        Yellow,
        Red
    }

    public static class HeroFactory
    {
        public static IHero CreateSwordHero(string name, ILogger logger)
        {
            var abilities = new List<IAbility>
            {
                new BaseAbility("Basic Attack", 10, 1, logger),
                new SwordAbility("Sword Attack", 2, logger)
            };

            return new Hero(name, abilities, logger);
        }

        public static IHero CreateFireHero(string name, ILogger logger)
        {
            var abilities = new List<IAbility>
            {
                new BaseAbility("Basic Attack", 10, 1, logger),
                new FireAbility("Fire Attack", 3, logger)
            };

            return new Hero(name, abilities, logger);
        }

        public static IHero CreateFreezeHero(string name, ILogger logger)
        {
            var abilities = new List<IAbility>
            {
                new BaseAbility("Basic Attack", 10, 1, logger),
                new FreezeAbility("Freeze Attack", Color.Blue, 4, logger)
            };

            return new Hero(name, abilities, logger);
        }
    }