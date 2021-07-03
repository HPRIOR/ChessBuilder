﻿using Models.Services.Board;
using Models.Services.Interfaces;
using Zenject;

namespace Bindings.Installers.ModelInstallers.Board
{
    public class BoardGeneratorInstaller : Installer<BoardGeneratorInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IBoardGenerator>().To<BoardGenerator>().AsSingle();
        }
    }
}