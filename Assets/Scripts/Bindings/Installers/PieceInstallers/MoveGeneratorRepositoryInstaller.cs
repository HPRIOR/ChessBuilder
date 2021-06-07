﻿using Models.Services.Interfaces;
using Models.Services.Moves.Factories;
using Zenject;

namespace Bindings.Installers.PieceInstallers
{
    public class MoveGeneratorRepositoryInstaller : Installer<MoveGeneratorRepositoryInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IMoveGeneratorRepository>().To<MoveGeneratorRepository>().AsSingle();
        }
    }
}