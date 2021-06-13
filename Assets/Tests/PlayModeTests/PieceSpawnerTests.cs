using System.Collections;
using Bindings.Installers.BoardInstallers;
using Bindings.Installers.PieceInstallers;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace Tests.PlayModeTests
{
    public class PieceSpawnerTests : ZenjectIntegrationTestFixture
    {
        private IPieceSpawner _pieceSpawner;

        [Inject]
        public void Construct(IPieceSpawner pieceSpawner)
        {
            _pieceSpawner = pieceSpawner;
        }

        private void CommonInstall()
        {
            PreInstall();

            PieceSpawnerInstaller.Install(Container);
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
            var piece = _pieceSpawner.CreatePiece(pieceType, new Position(x, y));
            yield return null;
            Assert.AreEqual(new Position(x, y), piece.Position);
        }

        [UnityTest]
        public IEnumerator PiecesSpawnWithCorrectType(
            [Values] PieceType pieceType
        )
        {
            CommonInstall();
            var piece = _pieceSpawner.CreatePiece(pieceType, new Position(0, 0));
            yield return null;
            Assert.AreEqual(pieceType, piece.Info.PieceType);
        }

        [UnityTest]
        public IEnumerator PiecesSpawnAtCorrectVector(
            [Values(0, 1, 2, 3, 4, 5, 6, 7)] int x, [Values(0, 1, 2, 3, 4, 5, 6, 7)] int y
        )
        {
            CommonInstall();
            var piece = _pieceSpawner.CreatePiece(PieceType.WhitePawn, new Position(x, y));
            yield return null;
            Assert.AreEqual(new Vector2(x + 0.5f, y + 0.5f), piece.Position.Vector);
        }
    }
}