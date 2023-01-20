using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK
{
    public class SpellItem : Item
    {
        public GameObject spellWarmUpFX;
        public GameObject spellCastFX;

        public string spellAnimation;

        [Header("Spell Type")]
        public bool isFaithSpell;
        public bool isMagicSpell;
        public bool isPyroSpell;

        [Header("Spell Desctiption")]
        [TextArea]
        public string spellDescription;

        public virtual void AttemptToCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats)
        {
            Debug.Log("You attempt to cast a spell");
        }
        public virtual void SuccessfullyCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats)
        {
            Debug.Log("You successfully cast a spell");
        }
    }
}

