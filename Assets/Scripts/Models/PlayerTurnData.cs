namespace RollOfTheDice.Models
{
    public class PlayerTurnData
    {
        public int Attack;
        public int Defend;

        public PlayerTurnData(int attack, int defend)
        {
            Attack = attack;
            Defend = defend;
        }
    }
}