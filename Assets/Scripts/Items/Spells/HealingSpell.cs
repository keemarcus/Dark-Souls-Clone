using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK
{
    [CreateAssetMenu(menuName = "Spells/Healing Spell")]
    public class HealingSpell : SpellItem
    {
        public int healAmount;
        GameObject instaniatedWarmUpSpellFX;

        public override void AttemptToCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats, bool isLeftHand)
        {
            base.AttemptToCastSpell(animatorHandler, playerStats, isLeftHand);
            instaniatedWarmUpSpellFX = Instantiate(spellWarmUpFX, spellCastPoint);
            animatorHandler.PlayTargetAnimation(spellAnimation, true);
        }

        public override void SuccessfullyCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats)
        {
            base.SuccessfullyCastSpell(animatorHandler, playerStats);
            Destroy(instaniatedWarmUpSpellFX);
            GameObject instantiatedSpellFX = Instantiate(spellCastFX, spellCastPoint);
            playerStats.HealPlayer(healAmount);
        }
    }
}

