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
        public string spellCastPointName;
        protected Transform spellCastPoint;

        [Header("Spell Cast")]
        public int focusPointCost;

        [Header("Spell Type")]
        public bool isFaithSpell;
        public bool isMagicSpell;
        public bool isPyroSpell;

        [Header("Spell Desctiption")]
        [TextArea]
        public string spellDescription;

        public virtual void AttemptToCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats, bool isLeftHand)
        {
            Debug.Log("You attempt to cast a spell");

            // get the cast point for this spell
            switch (spellCastPointName)
            {
                case "Chest":
                    spellCastPoint = playerStats.chestCastPoint;
                    break;
                case "Hand":
                    if (isLeftHand)
                    {
                        spellCastPoint = playerStats.leftHandCastPoint;
                    }
                    else
                    {
                        spellCastPoint = playerStats.rightHandCastPoint;
                    }
                    break;
                default:
                    break;
            }
            if(this.spellCastPoint == null)
            {
                Debug.Log("Spell Cast Point Not Found");
                this.spellCastPoint = animatorHandler.transform;
            }
        }
        public virtual void SuccessfullyCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats)
        {
            Debug.Log("You successfully cast a spell");
            playerStats.DrainFocusPoints(focusPointCost);
        }
    }
}

