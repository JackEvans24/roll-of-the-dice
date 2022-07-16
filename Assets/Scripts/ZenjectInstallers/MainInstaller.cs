using RollOfTheDice.Controllers;
using RollOfTheDice.Services;
using Zenject;

namespace RollOfTheDice.ZenjectInstallers
{
    public class MainInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameController>().AsSingle();
            Container.Bind<DiceService>().AsTransient();
        }
    }
}