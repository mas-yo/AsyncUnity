using UnityEngine;
using UnityEngine.UI;

namespace Genie.Views
{
    public class QuestListViewComponents : MonoBehaviour
    {
        [SerializeField]
        public GameObject QuestListEntryPrefab;
        [SerializeField]
        public ScrollRect QuestScrollRect;
        [SerializeField]
        public Transform ButtonsParent;
    }
}