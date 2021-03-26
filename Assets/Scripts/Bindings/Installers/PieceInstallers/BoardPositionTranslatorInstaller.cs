using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

public class BoardPositionTranslatorInstaller : Installer<BoardPositionTranslatorInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IPositionTranslatorFactory>().To<BoardPositionTranslatorFactory>().AsSingle();
        Container.BindFactory<PieceColour , BoardPositionTranslator, BoardPositionTranslator.Factory>().FromNew();
    }
}
