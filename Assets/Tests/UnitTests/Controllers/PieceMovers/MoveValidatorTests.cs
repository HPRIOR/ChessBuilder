using System.Collections.Generic;
using Bindings.Installers.PieceInstallers;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.Interfaces;
using NUnit.Framework;
using Zenject;

[TestFixture]
public class MoveValidatorTests : ZenjectUnitTestFixture
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

    private IMoveValidator _moveValidator;

    private void InstallBindings()
    {
        MoveValidatorInstaller.Install(Container);
    }

    private void ResolveContainer()
    {
        _moveValidator = Container.Resolve<IMoveValidator>();
    }

    [Test]
    public void RejectsIf_FromEqualsDestination()
    {
        var possibleMoves =
            new Dictionary<IBoardPosition, HashSet<IBoardPosition>>();
        IBoardPosition a = new BoardPosition(1, 1);
        IBoardPosition b = new BoardPosition(1, 1);
        Assert.IsFalse(_moveValidator.ValidateMove(possibleMoves, a, b));
    }

    [Test]
    public void RejectsIf_NoMovesFound()
    {
        var possibleMoves =
            new Dictionary<IBoardPosition, HashSet<IBoardPosition>>
            {
                {new BoardPosition(1, 1), new HashSet<IBoardPosition> {new BoardPosition(7, 7)}}
            };
        IBoardPosition a = new BoardPosition(1, 1);
        IBoardPosition b = new BoardPosition(2, 2);
        Assert.IsFalse(_moveValidator.ValidateMove(possibleMoves, a, b));
    }

    [Test]
    public void AcceptsIf_MovesFound()
    {
        var possibleMoves =
            new Dictionary<IBoardPosition, HashSet<IBoardPosition>>
            {
                {new BoardPosition(1, 1), new HashSet<IBoardPosition> {new BoardPosition(2, 2)}}
            };
        IBoardPosition a = new BoardPosition(1, 1);
        IBoardPosition b = new BoardPosition(2, 2);
        Assert.IsTrue(_moveValidator.ValidateMove(possibleMoves, a, b));
    }
}