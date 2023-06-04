using System;
using UnityEngine;

namespace Agents
{
    public class AnimationAgent : MonoBehaviour
    {
        public Action<string> onAnimationCallback;
        [field: SerializeField] public Animator animator { get; private set; }

        public void CallCallback(string callbackName)
        {
            onAnimationCallback?.Invoke(callbackName);
        }

        public void PlayAnimation(int animationHash)
        {
            animator.Play(animationHash);
        }

        public void PlayAnimation(string animationName)
        {
            animator.Play(animationName);
        }

        public void CrossFade(int animationHash, float duration = 0.5f)
        {
            animator.CrossFade(animationHash, duration);
        }

        public void CrossFade(string animationName, float duration = 0.5f)
        {
            animator.CrossFade(animationName, duration);
        }
    }
}