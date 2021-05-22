﻿using Bindings.Installers.BoardInstallers;
using Bindings.Installers.GameInstallers;
using Bindings.Installers.InputInstallers;
using Bindings.Installers.PieceInstallers;
using Zenject;

namespace Bindings.Utils
{
    public static class BindAllDefault
    {
        public static void InstallAllTo(DiContainer container)
        {
            BoardGeneratorInstaller.Install(container);
            GameStateInstaller.Install(container);
            CommandInvokerInstaller.Install(container);
            MoveCommandInstaller.Install(container);
            BoardEvalInstaller.Install(container);
            BoardPositionTranslatorInstaller.Install(container);
            BoardScannerInstaller.Install(container);
            MoveValidatorInstaller.Install(container);
            PieceMoverInstaller.Install(container);
            PieceSpawnerInstaller.Install(container);
            AllPossibleMovesGeneratorInstaller.Install(container);
            PossibleMoveFactoryInstaller.Install(container);
        }
    }
}