﻿using UnityEngine;
using UnityEngine.UI;

namespace AsyncUnity.Views
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