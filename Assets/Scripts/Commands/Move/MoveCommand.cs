using Zenject;

public class MoveCommand : ICommand
{
    private static IPieceMover _pieceMover;
    private static IMoveValidator _moveValidator;
    private IBoardPosition _from;
    private IBoardPosition _destination;
    private IGameState _gameState;
    private IBoardState _stateTransitionedFrom;

    public MoveCommand(
        IBoardPosition from,
        IBoardPosition destination,
        IPieceMover pieceMover,
        IMoveValidator moveValidator,
        IGameState gameState
        )
    {
        _gameState = gameState;
        _stateTransitionedFrom = _gameState.currentBoardState;

        _from = from;
        _destination = destination;

        _moveValidator = moveValidator;
        _pieceMover = pieceMover;
    }

    public void Execute()
    {
        _gameState.UpdateGameState(_pieceMover.Move(_gameState.currentBoardState, _from, _destination));
    }

    public bool IsValid()
    {
        if (_from == _destination) return false;
        if (_moveValidator.ValidateMove(_gameState.PossibleBoardMoves, _from, _destination))
            return true;

        _gameState.UpdateGameState(_stateTransitionedFrom);
        return false;
    }

    public void Undo()
    {
        _gameState.UpdateGameState(_stateTransitionedFrom);
    }

    public class Factory : PlaceholderFactory<IBoardPosition, IBoardPosition, MoveCommand> { }
}