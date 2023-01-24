using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MK
{
    public class AnimatorHandler : MonoBehaviour
    {
        public Animator anim;
        public bool canRotate;
        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("Can Rotate", false);
            anim.SetBool("Is Interacting", isInteracting);
            anim.CrossFade(targetAnim, 0.2f);
        }

        public virtual void TakeCriticalDamageAnimationEvent()
        {

        }
    }
}

