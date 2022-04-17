using System.Collections;
using Bindings.Installers.ModelInstallers.Board;
using Bindings.Installers.ViewInstallers;
using Models.Services.Moves.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using View.Utils;
using Zenject;

namespace Tests.PlayModeTests
{
    public class PieceFactoryTests : ZenjectIntegrationTestFixture
    {
        private IPieceFactory _pieceFactory;

        [Inject]
        public void Construct(IPieceFactory pieceFactory)
        {
            _pieceFactory = pieceFactory;
        }

        private void CommonInstall()
        {
            PreInstall();

            PieceFactoryInstaller.Install(Container);
            BoardGeneratorInstaller.Install(Container);

            PostInstall();
        }

        [UnityTest]
        public IEnumerator PiecesSpawnInCorrectPosition(
            [Values] PieceType pieceType,
            [Values(2, 4, 6)] int x,
            [Values(1, 3, 5)] int y
        )
        {
            CommonInstall();
            var piece = _pieceFactory.CreatePiece(pieceType, new Position(x, y));
            yield return null;
            Assert.AreEqual(new Position(x, y), piece.Position);
        }

        [UnityTest]
        public IEnumerator PiecesSpawnWithCorrectType(
            [Values] PieceType pieceType
        )
        {
            CommonInstall();
            var piece = _pieceFactory.CreatePiece(pieceType, new Position(0, 0));
            yield return null;
            Assert.AreEqual(pieceType, piece.RenderInfo.PieceType);
        }

        [UnityTest]
        public IEnumerator PiecesSpawnAtCorrectVector(
            [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6, 7)] int y
        )
        {
            CommonInstall();
            var piece = _pieceFactory.CreatePiece(PieceType.WhitePawn, new Position(x, y));
            yield return null;
            Assert.AreEqual(new Vector2(x + 0.5f, y + 0.5f), piece.Position.GetVector());
        }
    }
}