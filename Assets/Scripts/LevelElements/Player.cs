using UnityEngine;
using System.Collections;

namespace Assets.Scripts.LevelElements
{
    /// <summary>
    /// The MonoBehaviour of the player character.
    /// </summary>
    public class Player : MonoBehaviour
    {
        /// <summary>
        /// The X velocity when the player character is walking to right.
        /// </summary>
        public float WalkingSpeed = 2F;

        /// <summary>
        /// The Y velocity when the player character has jumped.
        /// </summary>
        public float JumpSpeed = 7F;

        private Assets.Scripts.LevelElements.PlayerStates.PlayerState CurrentState;

        // Use this for initialization
        void Start()
        {
            // The default state is Standing
            if (this.CurrentState == null)
            {
                this.CurrentState = gameObject.AddComponent<Assets.Scripts.LevelElements.PlayerStates.Standing>();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
