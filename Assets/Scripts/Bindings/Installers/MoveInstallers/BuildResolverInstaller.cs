﻿using Models.Services.Build.Interfaces;
using Models.Services.Build.Utils;
using Zenject;

namespace Bindings.Installers.MoveInstallers
{
    public class BuildResolverInstaller : Installer<BuildResolverInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IBuildResolver>().To<BuildResolver>().AsSingle();
        }
    }
}