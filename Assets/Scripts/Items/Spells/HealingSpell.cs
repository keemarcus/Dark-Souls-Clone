using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK
{
    [CreateAssetMenu(menuName = "Spells/Healing Spell")]
    public class HealingSpell : SpellItem
    {
        public int healAmount;

        public override void AttemptToCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats)
        {
            GameObject instaniatedWarmUpSpellFX = Instantiate(spellWarmUpFX, animatorHandler.transform);
            animatorHandler.PlayTargetAnimation(spellAnimation, true);
            Debug.Log("Attempting to cast spell...");
        }

        public override void SuccessfullyCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats)
        {
            GameObject instantiatedSpellFX = Instantiate(spellCastFX, animatorHandler.transform);
            playerStats.HealPlayer(healAmount);
            Debug.Log("Spellcast Successful");
        }
    }
}

