using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BoardGeneratorInstaller : Installer<BoardGeneratorInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IBoardGenerator>().To<BoardInitialiser>().AsSingle();
    }

}
