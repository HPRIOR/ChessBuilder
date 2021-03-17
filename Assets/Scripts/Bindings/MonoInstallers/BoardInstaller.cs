﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

public class BoardInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BoardRendererInstaller.Install(Container);
        BoardGeneratorInstaller.Install(Container);
        BoardStateInstaller.Install(Container);
    }
}
