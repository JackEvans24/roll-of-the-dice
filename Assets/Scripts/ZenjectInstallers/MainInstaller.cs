using RollOfTheDice.Controllers;
using Zenject;

namespace RollOfTheDice.ZenjectInstallers
{
    public class MainInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameController>().AsSingle();
        }
    }
}