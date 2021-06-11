﻿using Models.Services.Moves.PossibleMoveHelpers;
using Zenject;

namespace Bindings.Installers.MoveInstallers
{
    // TODO delete me if possible
    public class KingMoveFilterInstaller : Installer<KingMoveFilterInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<KingMoveFilter>().AsSingle();
        }
    }
}