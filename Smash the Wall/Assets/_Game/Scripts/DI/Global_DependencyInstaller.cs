using GameState;
using Helpers;
using Services;
using Servies;

namespace DependencyInjection
{
    public class Global_DependencyInstaller : DependencyInstallerBase
    {
        public override void Inject()
        {
            base.Inject();

            DependencyContext.diBox.Add<VibrationHelper>(new VibrationHelper());
            DependencyContext.diBox.Add<LazyUpdator_Service>(new LazyUpdator_Service());
            DependencyContext.diBox.Add<SceneLoader>(new SceneLoader());
            DependencyContext.diBox.Add<IGameStateChanger>(new SimpleGameStatesChanger(), asTypeProvided: true);
        }

        public override void Clear()
        {
            base.Clear();

            DependencyContext.diBox.Remove<VibrationHelper>();
            DependencyContext.diBox.Remove<LazyUpdator_Service>();
            DependencyContext.diBox.Remove<SceneLoader>();
            DependencyContext.diBox.Remove<IGameStateChanger>();
        }
    }
}