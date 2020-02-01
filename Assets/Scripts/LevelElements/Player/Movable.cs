using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The player's state machines.
/// </summary>
namespace Assets.Scripts.LevelElements.Player
{
    /// <summary>
    /// The Player's state that the player is standing or walking.
    /// </summary>
    public class Movable : MonoBehaviour
    {
        private static readonly Vector2 walkingSpeed = 1 * Vector2.right;

        /// <summary>
        /// The player's direction. Used to choose a sprite, the object which the player acts (e.g. pickup).
        /// </summary>
        public enum Direction
        {
            Left,
            Right,
        };
        
        /// <summary>
        /// The player's now direction.
        /// </summary>
        public Direction CurrentDirection;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            ProcessInput();

            // TODO: update the GameObject's sprite and more...
        }

        /// <summary>
        /// Process inputs from the player.
        /// </summary>
        void ProcessInput()
        {
            float axis = Input.GetAxis("Horizontal");
            var rigidBody = gameObject.GetComponent<Rigidbody2D>();

            // Turn and move left
            if (axis < -0.5)
            {
                this.CurrentDirection = Direction.Left;
                // FIXME: the movement speed
                rigidBody.velocity = -walkingSpeed;
            }
            // Right
            else if (axis > 0.5)
            {
                this.CurrentDirection = Direction.Right;
                // FIXME: the movement speed
                rigidBody.velocity = walkingSpeed;
            }
            // Stop
            else
            {
                rigidBody.velocity = Vector2.zero;
            }
        }
    }
}
