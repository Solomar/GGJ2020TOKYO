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

        /// <summary>
        /// The button's name used to make the player character jump.
        /// </summary>
        public string JumpButtonName = "Jump";

        /// <summary>
        /// The button's name used to make the player character pick or put an object in front of the character.
        /// </summary>
        public string ActButtonName = "Fire1";

        /// <summary>
        /// The player's direction. Used to choose a sprite, the object which the player acts (e.g. pickup).
        /// </summary>
        public enum Direction
        {
            Right,
            Left,
        };

        /// <summary>
        /// The player's now direction.
        /// </summary>
        public Direction CurrentDirection = Direction.Right;

        private PickableObject holdingObject;
        public PickableObject HoldingObject
        {
            get
            {
                return gameObject.transform.Find("HoldingObject").GetComponentInChildren<PickableObject>();
            }
            set
            {
                if (this.HoldingObject != null)
                {
                    throw new System.InvalidOperationException($"The player is already holding {this.HoldingObject.GetType().Name}.");
                }
            }
        }

        /// <summary>
        /// The player's currfent state, a part of a FSM.
        /// </summary>
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
    }
}
