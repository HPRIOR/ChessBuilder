﻿using Models.Services.Moves.Interfaces;
using Models.Services.Moves.MoveGenerators;
using Zenject;

namespace Bindings.Installers.ModelInstallers.Move
{
    public class MovesGeneratorRepositoryInstaller : Installer<MovesGeneratorRepositoryInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IMovesGeneratorRepository>().To<MovesGeneratorRepository>().AsSingle();
        }
    }
}