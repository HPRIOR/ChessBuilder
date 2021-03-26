using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

public class BoardEvalInstaller : Installer<BoardEvalInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IBoardEvalFactory>().To<BoardEvalFactory>().AsSingle();
        Container.BindFactory<PieceColour, BoardEval, BoardEval.Factory>().FromNew();
    }
}
