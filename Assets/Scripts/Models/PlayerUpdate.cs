namespace RollOfTheDice.Models
{
    public class PlayerUpdate
    {
        public UpdateType UpdateType;
        public Player Player;
        public string AnimationName;

        private PlayerUpdate(Player player, UpdateType updateType)
        {
            UpdateType = updateType;
            Player = player;
            AnimationName = updateType.ToString();
        }

        public static PlayerUpdate Attack(Player player) => new PlayerUpdate(player, UpdateType.Attack);
        public static PlayerUpdate Shield(Player player) => new PlayerUpdate(player, UpdateType.Shield);
        public static PlayerUpdate Hurt(Player player) => new PlayerUpdate(player, UpdateType.Hurt);
    }
    
    public class EnemyUpdate
    {
        public UpdateType UpdateType;
        public Enemy Enemy;
        public string AnimationName;

        private EnemyUpdate(Enemy enemy, UpdateType updateType)
        {
            UpdateType = updateType;
            Enemy = enemy;
            AnimationName = updateType.ToString();
        }

        public static EnemyUpdate Attack(Enemy enemy) => new EnemyUpdate(enemy, UpdateType.Attack);
        public static EnemyUpdate Shield(Enemy enemy) => new EnemyUpdate(enemy, UpdateType.Shield);
        public static EnemyUpdate Hurt(Enemy enemy) => new EnemyUpdate(enemy, UpdateType.Hurt);
    }

    public enum UpdateType
    {
        Attack,
        Shield,
        Hurt
    }
}