using Controllers.Commands;
using Models.State.Board;
using Models.State.PieceState;

namespace Controllers.Factories
{
    public class BuildCommandFactory
    {
        private readonly BuildCommand.Factory _buildCommandFactory;

        public BuildCommandFactory(BuildCommand.Factory buildCommandFactory)
        {
            _buildCommandFactory = buildCommandFactory;
        }

        public BuildCommand Create(Position at, PieceType pieceType) =>
            _buildCommandFactory.Create(at, pieceType);
    }
}