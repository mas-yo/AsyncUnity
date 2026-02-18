﻿using UnityEngine;
using UnityEngine.UI;

namespace AsyncUnity.Views
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