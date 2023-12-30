using GameState;
using Helpers;
using Services;
using Servies;

namespace DependencyInjection
{
    public class Global_DependencyInjector : DependencyInjection.HardCodeDependencyInjectorBase
    {
        public override void Inject()
        {
            base.Inject();

            DependencyInjector.diBox.Add<VibrationHelper>(new VibrationHelper());
            DependencyInjector.diBox.Add<LazyUpdator_Service>(new LazyUpdator_Service());
            DependencyInjector.diBox.Add<SceneLoader>(new SceneLoader());
            DependencyInjector.diBox.Add<IGameStateChanger>(new SimpleGameStatesChanger(), asTypeProvided: true);
        }

        public override void Clear()
        {
            base.Clear();

            DependencyInjector.diBox.Remove<VibrationHelper>();
            DependencyInjector.diBox.Remove<LazyUpdator_Service>();
            DependencyInjector.diBox.Remove<SceneLoader>();
            DependencyInjector.diBox.Remove<IGameStateChanger>();
        }
    }
}