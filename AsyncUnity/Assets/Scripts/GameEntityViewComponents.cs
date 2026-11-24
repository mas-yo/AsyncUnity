using UnityEngine;

namespace AsyncUnity.Views
{
    public class GameEntityViewComponents : MonoBehaviour
    {
        [SerializeField] public Transform Transform;
        [SerializeField] public Animator? Animator;
        [SerializeField] public AnimationEventListener? AnimationEventListener;
    }
}