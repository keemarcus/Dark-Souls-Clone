using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK
{
    public class DestroyAfterCastingSpell : MonoBehaviour
    {
        CharacterManager characterCastingSpell;

        private void Awake()
        {
            characterCastingSpell = GetComponentInParent<CharacterManager>();
        }
        void Update()
        {
            if (characterCastingSpell.isFiringSpell)
            {
                Destroy(this.gameObject);
            }
        }
    }
}

