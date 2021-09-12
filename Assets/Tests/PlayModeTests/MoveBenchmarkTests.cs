using System.Collections;
using Models.Services.Game.Interfaces;
using Models.State.Board;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace Tests.PlayModeTests
{
    public class MoveBenchmarkTests : SceneTestFixture
    {
        [UnityTest]
        public IEnumerator Benchmark()
        {
            yield return LoadScene("SampleScene");

            var gameStateController = SceneContainer.Resolve<IGameStateController>();

            gameStateController.UpdateGameState(new Position(6, 0), new Position(7, 0));
            yield return new WaitForSeconds(1);

            gameStateController.UpdateGameState(new Position(4, 0), new Position(4, 1));
            yield return new WaitForSeconds(1);

            gameStateController.UpdateGameState(new Position(3, 2), new Position(3, 1));
            yield return new WaitForSeconds(1);

            gameStateController.UpdateGameState(new Position(5, 3), new Position(6, 2));
            yield return new WaitForSeconds(1);

            gameStateController.UpdateGameState(new Position(5, 2), new Position(7, 3));
            yield return new WaitForSeconds(1);
        }
    }
}