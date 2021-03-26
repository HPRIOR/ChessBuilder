using Zenject;

public class BoardPositionTranslatorInstaller : Installer<BoardPositionTranslatorInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IPositionTranslatorFactory>().To<BoardPositionTranslatorFactory>().AsSingle();
        Container.BindFactory<PieceColour, PositionTranslator, PositionTranslator.Factory>().FromNew();
    }
}