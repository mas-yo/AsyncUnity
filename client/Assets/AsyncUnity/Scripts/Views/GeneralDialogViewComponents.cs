using UnityEngine;
using UnityEngine.UI;

namespace AsyncUnity.Views
{
    public class GeneralDialogViewComponents : MonoBehaviour
    {
        [SerializeField]
        public Text TitleText;

        [SerializeField]
        public Text MessageText;

        [SerializeField]
        public Button OkButton;

        [SerializeField]
        public Button CancelButton;
    }
}
