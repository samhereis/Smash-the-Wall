using Helpers;
using Interfaces;
using System.Threading.Tasks;

namespace Services
{
    public class GameSavesServiesBase : IInitializable
    {
        public virtual void Initialize()
        {

        }

        protected virtual async Task LoadSaved()
        {
            await AsyncHelper.Skip();
        }

        public virtual async Task UploadSaves()
        {
            await AsyncHelper.Skip();
        }
    }
}