using Cysharp.Threading.Tasks;
using Genie.Utils;
using UnityEngine;

namespace Genie.Views
{
    public class NewQuestOpenedVfx
    {
        private readonly NewQuestOpenedVfxComponents _components;

        public NewQuestOpenedVfx(NewQuestOpenedVfxComponents components)
        {
            _components = components;
        }

        public void Show()
        {
            _components.gameObject.SetActive(true);
        }

        public void Hide()
        {
            _components.gameObject.SetActive(false);
        }
        public async UniTask PlayAsync()
        {
            _components.Animator.Play("ShowUp");
            await _components.Animator.WaitAnimationComplete();
        }
    }
}