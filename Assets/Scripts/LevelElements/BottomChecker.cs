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
        /// If the element is landing.
        /// </summary>
        public bool Landing
        {
            get;
            private set;
        }

        private bool gotOnLand, isOnLand, gotNotOnLand;

        /// <summary>
        /// Check the collision is a "land".
        /// </summary>
        /// <param name="collision"></param>
        /// <returns></returns>
        private bool IsCollisionLand(Collision2D collision)
        {
            //FIXME
            return true || collision.otherCollider.tag == "Land";
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("aaaa");
            if (IsCollisionLand(collision))
                this.gotOnLand = true;
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            Debug.Log("bbbb");
            if (IsCollisionLand(collision))
                this.isOnLand = true;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            Debug.Log("cccc");
            if (IsCollisionLand(collision))
            {
                this.gotNotOnLand = true;
            }
        }

        private void Update()
        {
            Debug.Log($"{gotOnLand} {isOnLand} {gotNotOnLand}");
            // set the collision status and reset the status flags
            if (gotOnLand || isOnLand)
                this.Landing = true;
            else if (gotNotOnLand)
                this.Landing = false;
            gotOnLand = isOnLand = gotNotOnLand = false;
        }
    }
}
