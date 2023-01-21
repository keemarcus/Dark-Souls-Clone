using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK
{
    public class Interactable : MonoBehaviour
    {
        public float radius = 0.6f;
        public string interactableText;
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        public virtual void Interact(PlayerManager playerManager)
        {
            // this is called when the player interacts with this object
            Debug.Log("you interacted with an object");
        }
    }
}

