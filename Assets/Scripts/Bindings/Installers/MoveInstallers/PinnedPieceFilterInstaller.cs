﻿using Models.Services.Moves.MoveHelpers;
using Zenject;

namespace Bindings.Installers.MoveInstallers
{
    public class PinnedPieceFilterInstaller : Installer<PinnedPieceFilterInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<PinnedPieceFilter>().AsSingle();
        }
    }
}