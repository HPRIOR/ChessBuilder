using System;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.PieceTypeExt;
using NUnit.Framework;
using Zenject;

namespace Tests.UnitTests.Utils.Extensions
{
    [TestFixture]
    public class PieceTypeColourTests : ZenjectUnitTestFixture
    {
        [Test]
        public void NullPieceThrowException()
        {
            Assert.Throws<Exception>(() => PieceType.NullPiece.Colour());
        }

        [Test]
        public void CorrectColour_ForBlackTypes(
            [Values(PieceType.BlackBishop, PieceType.BlackKing, PieceType.BlackKnight, PieceType.BlackPawn,
                PieceType.BlackQueen, PieceType.BlackRook)]
            PieceType pieceType
        )
        {
            Assert.That(pieceType.Colour(), Is.EqualTo(PieceColour.Black));
        }


        [Test]
        public void CorrectColour_ForWhiteTypes(
            [Values(PieceType.WhiteBishop, PieceType.WhiteKing, PieceType.WhiteKnight, PieceType.WhitePawn,
                PieceType.WhiteQueen, PieceType.WhiteRook)]
            PieceType pieceType
        )
        {
            Assert.That(pieceType.Colour(), Is.EqualTo(PieceColour.White));
        }
    }
}