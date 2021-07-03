﻿using Controllers.PieceMovers;
using Models.Services.Interfaces;
using Zenject;

namespace Bindings.Installers.ModelInstallers.Move
{
    public class MoveValidatorInstaller : Installer<MoveValidatorInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IMoveValidator>().To<MoveValidator>().AsSingle();
        }
    }
}