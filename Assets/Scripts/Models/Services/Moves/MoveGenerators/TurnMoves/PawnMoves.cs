﻿using System.Collections.Generic;
using Models.Services.Moves.Interfaces;
using Models.Services.Moves.Utils;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Models.Services.Moves.MoveGenerators.TurnMoves
{
    public sealed class PawnMoves : IPieceMoveGenerator
    {
        private readonly IPositionTranslator _positionTranslator;
        private readonly ITileEvaluator _tileEvaluator;

        public PawnMoves(PieceColour pieceColour, IPositionTranslatorFactory positionTranslatorFactory,
            ITileEvaluatorFactory tileEvaluatorFactory)
        {
            _positionTranslator = positionTranslatorFactory.Create(pieceColour);
            _tileEvaluator = tileEvaluatorFactory.Create(pieceColour);
        }

        // TODO: refactor me
        public List<Position> GetPossiblePieceMoves(Position originPosition, BoardState boardState)
        {
            var possibleMoves = new List<Position>();

            originPosition = _positionTranslator.GetRelativePosition(originPosition);

            if (originPosition.Y == 7) return possibleMoves; // allow to change piece

            if (_positionTranslator.GetRelativeTileAt(originPosition.Add(Move.In(Direction.N)), boardState).CurrentPiece
                == PieceType.NullPiece)
                possibleMoves.Add(
                    _positionTranslator.GetRelativePosition(originPosition.Add(Move.In(Direction.N)))
                );

            if (originPosition.X > 0)
            {
                ref var topLeftTile =
                    ref _positionTranslator.GetRelativeTileAt(originPosition.Add(Move.In(Direction.Nw)), boardState);
                if (_tileEvaluator.OpposingPieceIn(ref topLeftTile))
                    possibleMoves.Add(
                        _positionTranslator.GetRelativePosition(originPosition.Add(Move.In(Direction.Nw)))
                    );
            }

            if (originPosition.X < 7)
            {
                ref var topRightTile =
                    ref _positionTranslator.GetRelativeTileAt(originPosition.Add(Move.In(Direction.Ne)), boardState);
                if (_tileEvaluator.OpposingPieceIn(ref topRightTile))
                    possibleMoves.Add(
                        _positionTranslator.GetRelativePosition(originPosition.Add(Move.In(Direction.Ne)))
                    );
            }

            return possibleMoves;
        }

        public sealed class Factory : PlaceholderFactory<PieceColour, PawnMoves>
        {
        }
    }
}