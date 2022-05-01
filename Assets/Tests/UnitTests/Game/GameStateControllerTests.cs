using System.Collections.Generic;
using System.Linq;
using Bindings.Utils;
using Models.Services.Game.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;
using NUnit.Framework;
using UnityEngine;
using Zenject;

namespace Tests.UnitTests.Game
{
    [TestFixture]
    public class GameStateControllerTests : ZenjectUnitTestFixture
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

        private IGameStateController _gameStateController;

        private void InstallBindings()
        {
            BindAllDefault.InstallAllTo(Container);
        }

        private void ResolveContainer()
        {
            _gameStateController = Container.Resolve<IGameStateController>();
        }

        [Test]
        public void HasNullGameState_WhenInitialised()
        {
            Assert.IsNull(_gameStateController.CurrentGameState);
        }

        [Test]
        public void BoardStateIsUpdated_WhenPassedBoardState()
        {
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                {
                    new Position(1, 1), PieceType.BlackKing
                },
                {
                    new Position(7, 7), PieceType.WhiteKing
                }
            };
            var initialBoardState = new BoardState(pieceDict);
            _gameStateController.InitializeGame(initialBoardState);

            Assert.AreSame(initialBoardState, _gameStateController.CurrentGameState.BoardState);
        }

        [Test]
        public void EventInvoked_WhenStateUpdated()
        {
            var turnEventInvoker = _gameStateController as ITurnEventInvoker;
            var count = 0;

            void MockFunc(BoardState prev, BoardState newState) => count += 1;

            Debug.Assert(turnEventInvoker != null, nameof(turnEventInvoker) + " != null");
            turnEventInvoker.BoardStateChangeEvent += MockFunc;

            var pieceDict = new Dictionary<Position, PieceType>()
            {
                {
                    new Position(1, 1), PieceType.BlackKing
                },
                {
                    new Position(7, 7), PieceType.WhiteKing
                }
            };
            var boardState = new BoardState(pieceDict);
            _gameStateController.InitializeGame(boardState);

            Assert.AreEqual(1, count);
        }

        [Test]
        public void WhenInCheck_NoBuildMovesAvailable()
        {
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                {
                    new Position(4, 4), PieceType.WhiteKing
                },
                {
                    new Position(6, 0), PieceType.BlackQueen
                }
            };

            var initialBoardState =
                new BoardState(pieceDict);
            _gameStateController.InitializeGame(initialBoardState);

            _gameStateController.UpdateGameState(new Position(4, 4), new Position(5, 4));
            _gameStateController.UpdateGameState(new Position(6, 0), new Position(5, 0));

            var expectedBuildMoves =
                new BuildMoves(new List<Position>(), new List<PieceType>());
            Assert.That(_gameStateController.CurrentGameState.PossibleBuildMoves.BuildPieces,
                Is.EquivalentTo(expectedBuildMoves.BuildPieces));
            Assert.That(_gameStateController.CurrentGameState.PossibleBuildMoves.BuildPositions,
                Is.EquivalentTo(expectedBuildMoves.BuildPositions));
        }


        [Test]
        public void ResolvesBuildsOnBoard()
        {
            // setup board
            var pieceDict = new Dictionary<Position, (PieceType, BuildTileState)>()
            {
                {
                    new Position(1, 1), (PieceType.WhiteKing, new BuildTileState())
                },
                {
                    new Position(7, 7), (PieceType.BlackKing, new BuildTileState())
                },
                { new Position(4, 4), (PieceType.NullPiece, new BuildTileState(0, PieceType.WhitePawn)) }
            };

            var initialBoardState = new BoardState(pieceDict);

            // Initialise board state
            _gameStateController.InitializeGame(initialBoardState);

            //Make white turn
            _gameStateController.UpdateGameState(new Position(1, 1), new Position(2, 2));

            Assert.That(_gameStateController.CurrentGameState.BoardState.GetTileAt(4, 4).CurrentPiece,
                Is.EqualTo(PieceType.WhitePawn));
        }


        [Test]
        public void BuildIsBlocked_ByFriendlyPiece()
        {
            // setup board
            var pieceDict = new Dictionary<Position, ( PieceType, BuildTileState )>()
            {
                {
                    new Position(4, 4), (PieceType.WhiteKing, new BuildTileState(PieceType.WhitePawn))
                },
                {
                    new Position(7, 7), (PieceType.BlackKing, new BuildTileState())
                }
            };

            var initialBoardState = new BoardState(pieceDict);

            //generate initial game state
            _gameStateController.InitializeGame(initialBoardState);

            Assert.That(_gameStateController.CurrentGameState.BoardState.GetTileAt(4, 4).CurrentPiece,
                Is.EqualTo(PieceType.WhiteKing));
        }


        [Test]
        public void BuildIsBlocked_ByOpposingPiece()
        {
            // setup board
            var pieceDict = new Dictionary<Position, (PieceType, BuildTileState)>()
            {
                {
                    new Position(4, 4), (PieceType.BlackKing, new BuildTileState())
                },
                {
                    new Position(7, 7), (PieceType.WhiteKing, new BuildTileState(PieceType.WhitePawn))
                }
            };

            var initialBoardState = new BoardState(pieceDict);

            //generate initial game state
            _gameStateController.InitializeGame(initialBoardState);

            Assert.That(_gameStateController.CurrentGameState.BoardState.GetTileAt(4, 4).CurrentPiece,
                Is.EqualTo(PieceType.BlackKing));
        }


        [Test]
        public void BuildIsBlockedByOpposingPiece_ButRemainsInBuildingState()
        {
            // setup board
            var pieceDict = new Dictionary<Position, (PieceType, BuildTileState)>()
            {
                {
                    new Position(7, 7), (PieceType.WhiteKing, new BuildTileState())
                },
                {
                    new Position(4, 4), (PieceType.BlackKing, new BuildTileState(PieceType.WhitePawn))
                }
            };

            var initialBoardState = new BoardState(pieceDict);

            //generate initial game state
            _gameStateController.InitializeGame(initialBoardState);

            // iterate through game state
            _gameStateController.UpdateGameState(new Position(3, 3), new Position(3, 3)); // pseudo-move

            Assert.That(_gameStateController.CurrentGameState.BoardState.GetTileAt(4, 4).BuildTileState.Turns,
                Is.EqualTo(0));
            Assert.That(
                _gameStateController.CurrentGameState.BoardState.GetTileAt(4, 4).BuildTileState.BuildingPiece,
                Is.EqualTo(PieceType.WhitePawn));
        }


        [Test]
        public void BuildIsBlockedByFriendlyPiece_ButRemainsInBuildingState()
        {
            // setup board
            var pieceDict = new Dictionary<Position, (PieceType, BuildTileState)>()
            {
                {
                    new Position(4, 4), (PieceType.WhiteKing, new BuildTileState(PieceType.WhitePawn))
                },
                {
                    new Position(7, 7), (PieceType.BlackKing, new BuildTileState())
                },
            };

            var initialBoardState = new BoardState(pieceDict);

            //generate initial game state
            _gameStateController.InitializeGame(initialBoardState);

            //iterate through game state
            _gameStateController.UpdateGameState(new Position(3, 3), new Position(3, 3)); // pseudo-move

            Assert.That(_gameStateController.CurrentGameState.BoardState.GetTileAt(4, 4).BuildTileState.Turns,
                Is.EqualTo(0));
            Assert.That(
                _gameStateController.CurrentGameState.BoardState.GetTileAt(4, 4).BuildTileState.BuildingPiece,
                Is.EqualTo(PieceType.WhitePawn));
        }


        [Test]
        public void BuildStateIsReset_AfterResolving()
        {
            // setup board
            var pieceDict = new Dictionary<Position, (PieceType, BuildTileState)>()
            {
                {
                    new Position(0, 0), (PieceType.WhiteKing, new BuildTileState())
                },
                {
                    new Position(7, 7), (PieceType.BlackKing, new BuildTileState())
                },
                { new Position(4, 4), (PieceType.NullPiece, new BuildTileState(0, PieceType.WhitePawn)) }
            };
            var initialState = new BoardState(pieceDict);

            //generate initial game state
            _gameStateController.InitializeGame(initialState);
            // _gameStateController.UpdateGameState(initialState);

            //iterate through game state
            _gameStateController.UpdateGameState(new Position(0, 0), new Position(1, 1));

            Assert.That(_gameStateController.CurrentGameState.BoardState.GetTileAt(4, 4).BuildTileState.Turns,
                Is.EqualTo(0));
            Assert.That(
                _gameStateController.CurrentGameState.BoardState.GetTileAt(4, 4).BuildTileState.BuildingPiece,
                Is.EqualTo(PieceType.NullPiece));
        }

        [Test]
        public void BuildStateWillNotResolve_WhenOpposingTurn()
        {
            // setup board
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                {
                    new Position(7, 7), PieceType.WhiteKing
                },
                {
                    new Position(0, 0), PieceType.BlackKing
                }
            };
            var initialBoardState = new BoardState(pieceDict);

            //generate initial game state
            _gameStateController.InitializeGame(initialBoardState);

            //iterate through game state
            _gameStateController.UpdateGameState(new Position(4, 4), PieceType.WhitePawn);

            _gameStateController.UpdateGameState(new Position(0, 0), new Position(1, 1));

            Assert.That(_gameStateController.CurrentGameState.BoardState.GetTileAt(4, 4).BuildTileState.Turns,
                Is.EqualTo(0));
            Assert.That(_gameStateController.CurrentGameState.BoardState.GetTileAt(4, 4).CurrentPiece,
                Is.EqualTo(PieceType.NullPiece));
        }


        [Test]
        public void BuildIsUnblocked_WhenBlockingPieceMoves()
        {
            // setup board
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                {
                    new Position(4, 4), PieceType.WhiteKing
                },
                {
                    new Position(7, 0), PieceType.WhitePawn
                },
                {
                    new Position(7, 7), PieceType.BlackKing
                }
            };
            var initialBoardState = new BoardState(pieceDict);

            //generate initial game state
            _gameStateController.InitializeGame(initialBoardState);

            // white spawns piece
            _gameStateController.UpdateGameState(new Position(4, 4), PieceType.WhitePawn);

            // black turn 
            _gameStateController.UpdateGameState(new Position(7, 7), new Position(7, 6)); // pseudo-move
            Assert.That(_gameStateController.CurrentGameState.BoardState.GetTileAt(4, 4).BuildTileState
                            .BuildingPiece ==
                        PieceType.WhitePawn);

            // white turn
            _gameStateController.UpdateGameState(new Position(4, 4), new Position(4, 5));

            Assert.That(_gameStateController.CurrentGameState.BoardState.GetTileAt(4, 4).CurrentPiece,
                Is.EqualTo(PieceType.WhitePawn));
        }

        [Test]
        public void CheckMate_WithTwoMajorPieces()
        {
            // setup board
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                {
                    new Position(1, 1), PieceType.WhiteKing
                },
                {
                    new Position(1, 6), PieceType.WhiteRook
                },
                {
                    new Position(0, 5), PieceType.WhiteQueen
                },
                {
                    new Position(6, 7), PieceType.BlackKing
                }
            };

            var initialBoardState = new BoardState(pieceDict);
            _gameStateController.InitializeGame(initialBoardState);

            _gameStateController.UpdateGameState(new Position(0, 5), new Position(0, 7));

            Assert.That(_gameStateController.Turn, Is.EqualTo(PieceColour.Black));
            Assert.That(_gameStateController.CurrentGameState.PossiblePieceMoves.Count, Is.EqualTo(1));
            Assert.That(_gameStateController.CurrentGameState.PossiblePieceMoves[new Position(6, 7)].Count,
                Is.EqualTo(0));
            Assert.That(_gameStateController.CurrentGameState.Check, Is.True);
            Assert.That(_gameStateController.CurrentGameState.CheckMate, Is.True);
        }


        [Test]
        public void CheckMate_OnBackRank()
        {
            // setup board
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                {
                    new Position(6, 7), PieceType.BlackKing
                },
                {
                    new Position(5, 6), PieceType.BlackPawn
                },
                {
                    new Position(6, 6), PieceType.BlackPawn
                },
                {
                    new Position(7, 6), PieceType.BlackPawn
                },

                {
                    new Position(2, 0), PieceType.WhiteRook
                },
                {
                    new Position(6, 0), PieceType.WhiteKing
                }
            };

            // initialise game state
            var initialBoardState = new BoardState(pieceDict);
            _gameStateController.InitializeGame(initialBoardState);

            _gameStateController.UpdateGameState(new Position(2, 0), new Position(2, 7));

            Assert.That(_gameStateController.Turn, Is.EqualTo(PieceColour.Black));
            _gameStateController.CurrentGameState.PossiblePieceMoves.ToList()
                .ForEach(turn => Assert.That(turn.Value.Count, Is.EqualTo(0)));

            Assert.That(_gameStateController.CurrentGameState.CheckMate, Is.True);
        }

        [Test]
        public void CheckMate_QueenAndKnight()
        {
            // setup board
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                {
                    new Position(4, 7), PieceType.BlackKing
                },
                {
                    new Position(5, 4), PieceType.WhiteKnight
                },
                {
                    new Position(7, 6), PieceType.WhiteQueen
                },
                {
                    new Position(6, 0), PieceType.WhiteKing
                }
            };

            // initialise game state
            var initialBoardState = new BoardState(pieceDict);
            _gameStateController.InitializeGame(initialBoardState);

            _gameStateController.UpdateGameState(new Position(7, 6), new Position(4, 6));

            Assert.That(_gameStateController.Turn, Is.EqualTo(PieceColour.Black));
            _gameStateController.CurrentGameState.PossiblePieceMoves.ToList()
                .ForEach(turn => Assert.That(turn.Value.Count, Is.EqualTo(0)));
            Assert.That(_gameStateController.CurrentGameState.CheckMate, Is.True);
        }


        [Test]
        public void CheckMate_QueenAndBishop()
        {
            // setup board
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                {
                    new Position(6, 7), PieceType.BlackKing
                },
                {
                    new Position(5, 6), PieceType.BlackPawn
                },
                {
                    new Position(6, 6), PieceType.BlackPawn
                },
                {
                    new Position(7, 6), PieceType.BlackPawn
                },

                {
                    new Position(2, 2), PieceType.WhiteQueen
                },
                {
                    new Position(1, 1), PieceType.WhiteBishop
                },
                {
                    new Position(6, 0), PieceType.WhiteKing
                }
            };

            // initialise game state
            var initialBoardState = new BoardState(pieceDict);
            _gameStateController.InitializeGame(initialBoardState);

            _gameStateController.UpdateGameState(new Position(2, 2), new Position(6, 6));

            Assert.That(_gameStateController.Turn, Is.EqualTo(PieceColour.Black));
            _gameStateController.CurrentGameState.PossiblePieceMoves.ToList()
                .ForEach(turn => Assert.That(turn.Value.Count, Is.EqualTo(0)));
            Assert.That(_gameStateController.CurrentGameState.CheckMate, Is.True);
        }


        [Test]
        public void CheckMate_TwoBishop()
        {
            // setup board
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                {
                    new Position(7, 7), PieceType.BlackKing
                },
                {
                    new Position(7, 6), PieceType.BlackPawn
                },

                {
                    new Position(3, 4), PieceType.WhiteBishop
                },
                {
                    new Position(2, 4), PieceType.WhiteBishop
                },
                {
                    new Position(6, 0), PieceType.WhiteKing
                }
            };

            // initialise game state
            var initialBoardState = new BoardState(pieceDict);
            _gameStateController.InitializeGame(initialBoardState);

            _gameStateController.UpdateGameState(new Position(2, 4), new Position(3, 3));

            Assert.That(_gameStateController.Turn, Is.EqualTo(PieceColour.Black));
            _gameStateController.CurrentGameState.PossiblePieceMoves.ToList()
                .ForEach(turn => Assert.That(turn.Value.Count, Is.EqualTo(0)));
            Assert.That(_gameStateController.CurrentGameState.CheckMate, Is.True);
        }


        [Test]
        public void CheckMate_BishopKnight()
        {
            // setup board
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                { new Position(6, 7), PieceType.BlackKing },
                { new Position(5, 6), PieceType.BlackPawn },
                { new Position(6, 5), PieceType.BlackPawn },
                { new Position(7, 6), PieceType.BlackPawn },
                { new Position(5, 7), PieceType.BlackRook },

                { new Position(5, 5), PieceType.WhiteBishop },
                { new Position(6, 3), PieceType.WhiteKnight },
                { new Position(6, 0), PieceType.WhiteKing }
            };

            // initialise game state
            var initialBoardState = new BoardState(pieceDict);
            _gameStateController.InitializeGame(initialBoardState);

            _gameStateController.UpdateGameState(new Position(6, 3), new Position(7, 5));

            Assert.That(_gameStateController.Turn, Is.EqualTo(PieceColour.Black));
            _gameStateController.CurrentGameState.PossiblePieceMoves.ToList()
                .ForEach(turn => Assert.That(turn.Value.Count, Is.EqualTo(0)));
            Assert.That(_gameStateController.CurrentGameState.CheckMate, Is.True);
        }


        [Test]
        public void CheckMate_KingPawn()
        {
            // setup board
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                { new Position(3, 7), PieceType.BlackKing },
                { new Position(7, 2), PieceType.BlackPawn },

                { new Position(3, 6), PieceType.WhitePawn },
                { new Position(2, 5), PieceType.WhitePawn },
                { new Position(3, 5), PieceType.WhiteKing }
            };

            // initialise game state
            var initialBoardState = new BoardState(pieceDict);
            _gameStateController.InitializeGame(initialBoardState);

            _gameStateController.UpdateGameState(new Position(2, 5), new Position(2, 6));

            Assert.That(_gameStateController.Turn, Is.EqualTo(PieceColour.Black));
            _gameStateController.CurrentGameState.PossiblePieceMoves.ToList()
                .ForEach(turn => Assert.That(turn.Value.Count, Is.EqualTo(0)));
            Assert.That(_gameStateController.CurrentGameState.CheckMate, Is.True);
        }


        [Test]
        public void CheckMate_Smothered()
        {
            // setup board
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                { new Position(7, 7), PieceType.BlackKing },
                { new Position(7, 6), PieceType.BlackPawn },
                { new Position(6, 6), PieceType.BlackPawn },
                { new Position(6, 7), PieceType.BlackRook },
                { new Position(6, 4), PieceType.WhiteKnight },
                { new Position(3, 5), PieceType.WhiteKing }
            };

            // initialise game state
            var initialBoardState = new BoardState(pieceDict);
            _gameStateController.InitializeGame(initialBoardState);

            _gameStateController.UpdateGameState(new Position(6, 4), new Position(5, 6));

            Assert.That(_gameStateController.Turn, Is.EqualTo(PieceColour.Black));
            _gameStateController.CurrentGameState.PossiblePieceMoves.ToList()
                .ForEach(turn => Assert.That(turn.Value.Count, Is.EqualTo(0)));
            Assert.That(_gameStateController.CurrentGameState.CheckMate, Is.True);
        }


        [Test]
        public void CheckMate_Anastasia()
        {
            // setup board
            var pieceDict = new Dictionary<Position, PieceType>()
            {
                { new Position(7, 6), PieceType.BlackKing },
                { new Position(6, 6), PieceType.BlackPawn },
                { new Position(5, 6), PieceType.BlackPawn },
                { new Position(5, 7), PieceType.BlackRook },

                { new Position(4, 6), PieceType.WhiteKnight },
                { new Position(3, 3), PieceType.WhiteRook },
                { new Position(1, 1), PieceType.WhiteKing },
            };

            // initialise game state
            var initialBoardState = new BoardState(pieceDict);
            _gameStateController.InitializeGame(initialBoardState);

            _gameStateController.UpdateGameState(new Position(3, 3), new Position(7, 3));

            Assert.That(_gameStateController.Turn, Is.EqualTo(PieceColour.Black));
            _gameStateController.CurrentGameState.PossiblePieceMoves.ToList()
                .ForEach(turn => Assert.That(turn.Value.Count, Is.EqualTo(0)));
            Assert.That(_gameStateController.CurrentGameState.CheckMate, Is.True);
        }


        [Test]
        public void CheckMate_MorphyWithBuild()
        {
            // setup board
            var pieceDict = new Dictionary<Position, (PieceType, BuildTileState)>()
            {
                { new Position(7, 7), (PieceType.BlackKing, new BuildTileState()) },
                { new Position(7, 6), (PieceType.BlackPawn, new BuildTileState()) },
                { new Position(5, 6), (PieceType.BlackPawn, new BuildTileState()) },

                { new Position(4, 6), (PieceType.WhiteBishop, new BuildTileState()) },
                { new Position(1, 1), (PieceType.WhiteKing, new BuildTileState()) },
                { new Position(6, 0), (PieceType.NullPiece, new BuildTileState(0, PieceType.WhiteRook)) }
            };

            var initialBoardState = new BoardState(pieceDict);
            _gameStateController.InitializeGame(initialBoardState);

            _gameStateController.UpdateGameState(new Position(4, 6), new Position(5, 5));

            Assert.That(_gameStateController.Turn, Is.EqualTo(PieceColour.Black));
            _gameStateController.CurrentGameState.PossiblePieceMoves.ToList()
                .ForEach(turn => Assert.That(turn.Value.Count, Is.EqualTo(0)));
            Assert.That(_gameStateController.CurrentGameState.CheckMate, Is.True);
        }

        [Test]
        public void IsValidMove_RejectsIf_FromEqualsDestination()
        {
            // setup board
            var pieceDict = new Dictionary<Position, (PieceType, BuildTileState)>()
            {
                { new Position(7, 7), (PieceType.BlackKing, new BuildTileState()) },
                { new Position(1, 1), (PieceType.WhiteKing, new BuildTileState()) },
            };

            var initialBoardState = new BoardState(pieceDict);
            _gameStateController.InitializeGame(initialBoardState);

            var sut = _gameStateController.IsValidMove(new Position(1, 1), new Position(1, 1));

            Assert.That(sut, Is.False);
        }

        [Test]
        public void IsValidMove_RejectsIf_NoMovesFound()
        {
            // setup board
            var pieceDict = new Dictionary<Position, (PieceType, BuildTileState)>()
            {
                { new Position(7, 7), (PieceType.BlackKing, new BuildTileState()) },
                { new Position(0, 0), (PieceType.WhiteKing, new BuildTileState()) },
            };

            var initialBoardState = new BoardState(pieceDict);
            _gameStateController.InitializeGame(initialBoardState);

            var sut = _gameStateController.IsValidMove(new Position(0, 0), new Position(5, 5));

            Assert.That(sut, Is.False);
        }

        [Test]
        public void IsValidMove_AcceptsIf_MovesFound()
        {
            // setup board
            var pieceDict = new Dictionary<Position, (PieceType, BuildTileState)>()
            {
                { new Position(7, 7), (PieceType.BlackKing, new BuildTileState()) },
                { new Position(0, 0), (PieceType.WhiteKing, new BuildTileState()) },
            };

            var initialBoardState = new BoardState(pieceDict);
            _gameStateController.InitializeGame(initialBoardState);

            var sut = _gameStateController.IsValidMove(new Position(0, 0), new Position(1, 1));

            Assert.That(sut, Is.True);
        }
        
        /*
         * Build validation tests rely on the specific implementation of IBuildMoveGenerator.
         * In this case it is the simple HomeBaseBuildMovesGenerator, which will generate possible
         * build moves along the players first two rows (according to which players turn it is)
         */
        [Test]
        public void IsValidMove_AcceptsIf_BuildFound()
        {
            // setup board
            var pieceDict = new Dictionary<Position, (PieceType, BuildTileState)>()
            {
                { new Position(7, 7), (PieceType.BlackKing, new BuildTileState()) },
                { new Position(0, 0), (PieceType.WhiteKing, new BuildTileState()) },
            };

            var initialBoardState = new BoardState(pieceDict);
            _gameStateController.InitializeGame(initialBoardState);

            var sut = _gameStateController.IsValidMove(new Position(1, 1), PieceType.WhiteQueen);

            Assert.That(sut, Is.True);
        }
        
        [Test]
        public void IsValidMove_RejectIf_BuildFound()
        {
            // setup board
            var pieceDict = new Dictionary<Position, (PieceType, BuildTileState)>()
            {
                { new Position(7, 7), (PieceType.BlackKing, new BuildTileState()) },
                { new Position(0, 0), (PieceType.WhiteKing, new BuildTileState()) },
            };

            var initialBoardState = new BoardState(pieceDict);
            _gameStateController.InitializeGame(initialBoardState);

            var sut = _gameStateController.IsValidMove(new Position(5,5), PieceType.WhiteQueen);

            Assert.That(sut, Is.False);
        }
        
        [Test]
        public void IsValidMove_Rejects_BuildOnOppositePlayersBase()
        {
            // setup board
            var pieceDict = new Dictionary<Position, (PieceType, BuildTileState)>()
            {
                { new Position(7, 7), (PieceType.BlackKing, new BuildTileState()) },
                { new Position(0, 0), (PieceType.WhiteKing, new BuildTileState()) },
            };

            var initialBoardState = new BoardState(pieceDict);
            _gameStateController.InitializeGame(initialBoardState);

            var sut = _gameStateController.IsValidMove(new Position(7,1), PieceType.BlackQueen);

            Assert.That(sut, Is.False);
        }
    }
}