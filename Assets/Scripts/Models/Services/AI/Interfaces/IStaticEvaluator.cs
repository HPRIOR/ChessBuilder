using Models.State.GameState;

namespace Models.Services.AI.Interfaces
{
    public interface IStaticEvaluator
    {
        BoardScore Evaluate(GameState gameState);
    }
}