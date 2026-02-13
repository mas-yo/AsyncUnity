using UnityEngine;
using UnityEngine.UI;

namespace Genie.Views
{
    public class PresentBoxViewComponents : MonoBehaviour
    {
        [SerializeField]
        public GameObject PresentBoxEntryPrefab;
        [SerializeField]
        public ScrollRect PresentScrollRect;
        [SerializeField]
        public Transform ButtonsParent;
    }
}