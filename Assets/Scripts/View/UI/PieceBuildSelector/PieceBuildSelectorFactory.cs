using System;
using Models.State.PieceState;
using UnityEngine;

namespace View.UI.PieceBuildSelector
{
    public class PieceBuildSelectorFactory
    {
        private readonly PieceBuildSelector.Factory _pieceBuildSelectorFactory;

        public PieceBuildSelectorFactory(PieceBuildSelector.Factory pieceBuildSelectorFactory)
        {
            _pieceBuildSelectorFactory = pieceBuildSelectorFactory;
        }

        public PieceBuildSelector Create(Vector3 renderPosition, PieceType pieceTypeToBuild,
            Action<PieceType> selectPiece, bool canBuild) =>
            _pieceBuildSelectorFactory.Create(renderPosition, pieceTypeToBuild, selectPiece, canBuild);
    }
}