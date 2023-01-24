using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK
{
    public class CharacterManager : MonoBehaviour
    {
        [Header("Lock On Transform")]
        public Transform lockOnTransform;

        [Header("Combat Colliders")]
        public BoxCollider backStabBoxCollider;
        public BackStabCollider backStabCollider;

        // damage to be inflicted during an animation event (backstab/riposte)
        public int pendingCriticalDamage;
    }
}


