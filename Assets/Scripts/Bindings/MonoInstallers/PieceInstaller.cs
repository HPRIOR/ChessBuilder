using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

public class PieceInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        PieceSpawnerInstaller.Install(Container);
        PieceMoverInstaller.Install(Container);
        PossibleMoveGeneratorFactoryInstaller.Install(Container);
        PossibleBoardMovesGeneratorInstaller.Install(Container);
        MoveValidatorInstaller.Install(Container);
    }
}
