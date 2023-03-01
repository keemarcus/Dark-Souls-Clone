using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK
{
    [CreateAssetMenu(menuName = "Spells/Projectile Spell")]
    public class ProjectileSpell : SpellItem
    {
        public float baseDamage;
        public float projectileVelocity;
        Rigidbody body;

        public override void AttemptToCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats, bool isLeftHand)
        {
            base.AttemptToCastSpell(animatorHandler, playerStats, isLeftHand);

            // instatiate the spell in the caster's hand
            GameObject instantiatedWarmupSpellFX = Instantiate(spellWarmUpFX, playerStats.rightHandCastPoint);
            //instantiatedWarmupSpellFX.gameObject.transform.localScale = new Vector3(100f, 100f, 100f);

            // play casting animation
            animatorHandler.PlayTargetAnimation(spellAnimation, true);
        }

        public override void SuccessfullyCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats)
        {
            base.SuccessfullyCastSpell(animatorHandler, playerStats);
            //animatorHandler.anim.SetBool("Is Firing Spell", false);
            //Debug.Log("cast");
        }
    }
}

