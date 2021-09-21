using System.Collections.Generic;
using Bindings.Installers.AIInstallers;
using Bindings.Installers.GameInstallers;
using Bindings.Installers.ModelInstallers.Board;
using Bindings.Installers.ModelInstallers.Build;
using Bindings.Installers.ModelInstallers.Move;
using Models.Services.AI.Implementations;
using Models.Services.Board;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.GameState;
using Models.State.PieceState;
using Models.State.PlayerState;
using NUnit.Framework;
using Tests.UnitTests.TestUtils;
using Zenject;

namespace Tests.UnitTests.AI
{
    [TestFixture]
    public class AiMoveGeneratorTests : ZenjectUnitTestFixture
    {
        [SetUp]
        public void Init()
        {
            InstallBindings();
            ResolveContainer();
        }

        [TearDown]
        public void TearDown()
        {
            Container.UnbindAll();
        }

        private IBoardGenerator _boardGenerator;
        private AiMoveGenerator _aiMoveGenerator;

        private void InstallBindings()
        {
            AiPossibleMoveGeneratorInstaller.Install(Container);
            PieceMoverInstaller.Install(Container);
            GameStateUpdaterInstaller.Install(Container);
            BuilderInstaller.Install(Container);
            HomeBaseBuildMoveGeneratorInstaller.Install(Container);
            BuildPointsCalculatorInstaller.Install(Container);
            BuildResolverInstaller.Install(Container);
            GameOverEvalInstaller.Install(Container);
            MovesGeneratorInstaller.Install(Container);
            BoardGeneratorInstaller.Install(Container);
            PinnedPieceFilterInstaller.Install(Container);
            BoardInfoInstaller.Install(Container);
            MovesGeneratorRepositoryInstaller.Install(Container);
            PossibleMovesFactoryInstaller.Install(Container);
            PositionTranslatorInstaller.Install(Container);
            TileEvaluatorInstaller.Install(Container);
            BoardScannerInstaller.Install(Container);
            AiMoveGeneratorInstaller.Install(Container);
            StaticEvaluatorInstaller.Install(Container);
            MoveOrdererInstaller.Install(Container);
            CheckedStateManagerInstaller.Install(Container);
        }

        private void ResolveContainer()
        {
            _boardGenerator = Container.Resolve<IBoardGenerator>();
            _aiMoveGenerator = Container.Resolve<AiMoveGenerator>();
        }

        [Test]
        public void MoveIsGenerated()
        {
            var board = _boardGenerator.GenerateBoard();
            board[6][7].CurrentPiece = new Piece(PieceType.BlackKing);
            board[5][7].CurrentPiece = new Piece(PieceType.BlackRook);
            board[3][7].CurrentPiece = new Piece(PieceType.BlackQueen);
            board[0][7].CurrentPiece = new Piece(PieceType.BlackRook);
            board[1][6].CurrentPiece = new Piece(PieceType.BlackPawn);
            board[3][6].CurrentPiece = new Piece(PieceType.BlackBishop);
            board[5][6].CurrentPiece = new Piece(PieceType.BlackPawn);
            board[6][6].CurrentPiece = new Piece(PieceType.BlackPawn);
            board[7][6].CurrentPiece = new Piece(PieceType.BlackPawn);
            board[0][5].CurrentPiece = new Piece(PieceType.BlackPawn);
            board[2][5].CurrentPiece = new Piece(PieceType.BlackKnight);
            board[4][5].CurrentPiece = new Piece(PieceType.BlackPawn);
            board[5][5].CurrentPiece = new Piece(PieceType.BlackKnight);
            board[3][4].CurrentPiece = new Piece(PieceType.BlackPawn);

            board[6][0].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[4][0].CurrentPiece = new Piece(PieceType.WhiteRook);
            board[3][0].CurrentPiece = new Piece(PieceType.WhiteRook);
            board[1][1].CurrentPiece = new Piece(PieceType.WhitePawn);
            board[5][1].CurrentPiece = new Piece(PieceType.WhitePawn);
            board[6][1].CurrentPiece = new Piece(PieceType.WhitePawn);
            board[0][2].CurrentPiece = new Piece(PieceType.WhitePawn);
            board[2][2].CurrentPiece = new Piece(PieceType.WhiteKnight);
            board[3][2].CurrentPiece = new Piece(PieceType.WhiteQueen);
            board[5][2].CurrentPiece = new Piece(PieceType.WhiteKnight);
            board[7][2].CurrentPiece = new Piece(PieceType.WhitePawn);
            board[1][3].CurrentPiece = new Piece(PieceType.WhitePawn);
            board[5][3].CurrentPiece = new Piece(PieceType.WhiteBishop);


            var boardState = new BoardState(board);
            var possibleMoves = new Dictionary<Position, List<Position>>
            {
                { new Position(5, 5), new List<Position> { new Position(6, 6), new Position(6, 5) } },
                {
                    new Position(0, 7),
                    new List<Position> { new Position(0, 6), new Position(1, 6), new Position(1, 7) }
                }
            };

            var gameState = new GameState(false, false, new PlayerState(0), possibleMoves,
                new BuildMoves(new HashSet<Position>(), new HashSet<PieceType>()), boardState);
            // _aiMoveGenerator.GetMove(gameState, 3, PieceColour.White);

            var logTimer = new LogExecutionTimer();
            var depth = 8;
            
            logTimer.LogExecutionTime(
                $"NegaScout with depth of {depth.ToString()} - remove allocation in possible move eval",
                () => _aiMoveGenerator.GetMove(gameState, depth, PieceColour.White));

            // var move = _miniMax.GetMove(gameState, depth, PieceColour.White);
            // var newGameState = move(gameState.BoardState, PieceColour.White);
            //
            // foreach (var tile in newGameState.BoardState.Board)
            // {
            //     Debug.Log(tile);
            // }
        }
    }
}