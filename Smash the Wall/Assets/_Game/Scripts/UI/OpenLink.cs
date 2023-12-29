using Sirenix.OdinInspector;
using UnityEngine;

namespace UI
{
    public class OpenLink : MonoBehaviour
    {
        [Required]
        [SerializeField] private string _linkURL = "https://samhereis.github.io/PortfiloWebsite/";

        public void Open()
        {
            Application.OpenURL(_linkURL);
        }
    }
}