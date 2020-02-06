using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LevelElements
{
    /// <summary>
    /// A level element which can be picked up, carried and put down by the player character.
    /// </summary>
    public class PickableObject : MonoBehaviour
    {
        private RigidbodyType2D originalBodyType;
        public void OnPickUp()
        {
            var rb = GetComponent<Rigidbody2D>();
            if(rb != null)
            {
                this.originalBodyType = rb.bodyType;
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.simulated = false;
                // Reset the rotation
                rb.transform.localRotation = new Quaternion();
            }
        }

        public void OnPutDown(bool whileJumping = false)
        {

            var rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // 10 is the mountable layer
                if (whileJumping == true && (gameObject.layer == 10 || gameObject.layer == 15))
                {
                    rb.bodyType = RigidbodyType2D.Kinematic;
                    gameObject.layer = 15; // mounted layer
                }
                else
                {
                    rb.bodyType = this.originalBodyType;
                    rb.simulated = true;

                    if(gameObject.layer == 15)
                        gameObject.layer = 10;
                }
            }
        }
    }
}
