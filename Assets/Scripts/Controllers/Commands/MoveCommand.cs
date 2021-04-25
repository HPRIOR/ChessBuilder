using Zenject;

public class MoveCommand : ICommand
{
    private static IPieceMover _pieceMover;
    private static IMoveValidator _moveValidator;
    private readonly IBoardPosition _from;
    private readonly IBoardPosition _destination;
    private readonly IGameState _gameState;
    private readonly IBoardState _stateTransitionedFrom;

    public MoveCommand(
        IBoardPosition from,
        IBoardPosition destination,
        IPieceMover pieceMover,
        IMoveValidator moveValidator,
        IGameState gameState
        )
    {
        _gameState = gameState;
        _stateTransitionedFrom = _gameState.CurrentBoardState;

        _from = from;
        _destination = destination;

        _moveValidator = moveValidator;
        _pieceMover = pieceMover;
    }

    public void Execute()
    {
        var newBoardState = _pieceMover.Move(_gameState.CurrentBoardState, _from, _destination);
        _gameState.UpdateGameState(newBoardState);
    }

    public bool IsValid()
    {
        if (_from == _destination) return false;
        if (_moveValidator.ValidateMove(_gameState.PossiblePieceMoves, _from, _destination))
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