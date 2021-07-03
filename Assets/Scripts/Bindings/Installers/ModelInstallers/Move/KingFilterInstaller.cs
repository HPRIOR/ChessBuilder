﻿using Models.Services.Moves.MoveHelpers;
using Zenject;

namespace Bindings.Installers.ModelInstallers.Move
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