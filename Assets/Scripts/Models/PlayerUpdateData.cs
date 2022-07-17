namespace RollOfTheDice.Models
{
    public class PlayerUpdateData
    {
        public Player Player;
        public string AnimationName;

        public PlayerUpdateData(Player player, string animationName)
        {
            Player = player;
            AnimationName = animationName;
        }
    }
    
    public class EnemyUpdateData
    {
        public Enemy Enemy;
        public string AnimationName;

        public EnemyUpdateData(Enemy enemy, string animationName)
        {
            Enemy = enemy;
            AnimationName = animationName;
        }
    }
}