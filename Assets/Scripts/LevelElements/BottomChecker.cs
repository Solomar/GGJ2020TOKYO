using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LevelElements
{
    /// <summary>
    /// This is used to check if an level element is landing.
    /// </summary>
    public class BottomChecker : MonoBehaviour
    {
        /// <summary>
        /// Gets if the element is landing.
        /// </summary>
        public bool Landing
        {
            get;
            private set;
        }

        private bool gotOnLand, isOnLand, gotNotOnLand;

        /// <summary>
        /// Check the <paramref name="collider"/> is a "land".
        /// </summary>
        /// <param name="collider"></param>
        /// <returns></returns>
        private static bool IsCollisionLand(Collider2D collider)
        {
            //FIXME: すり抜けたいものが生まれたら常時trueを解除
            return true || collider.CompareTag("Land");
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            //Debug.Log("aaaa");
            if (IsCollisionLand(collider))
                this.gotOnLand = true;
        }

        private void OnTriggerStay2D(Collider2D collider)
        {
            //Debug.Log("bbbb");
            if (IsCollisionLand(collider))
                this.isOnLand = true;
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            //Debug.Log("cccc");
            if (IsCollisionLand(collider))
            {
                this.gotNotOnLand = true;
            }
        }

        private void Update()
        {
            //Debug.Log($"{gotOnLand} {isOnLand} {gotNotOnLand}");
            // set the collision status and reset the status flags
            if (gotOnLand || isOnLand)
                this.Landing = true;
            else if (gotNotOnLand)
                this.Landing = false;
        }
        private void LateUpdate()
        {
            gotOnLand = isOnLand = gotNotOnLand = false;
        }
    }
}
