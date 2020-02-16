using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// The player's state machines.
/// </summary>
namespace Assets.Scripts.LevelElements.PlayerStates
{
    /// <summary>
    /// The Player's state that the player is
    /// movable (standing, walking, jumping, or falling).
    /// 
    /// The player with the state can pick or put an PickableObject.
    /// </summary>
    public abstract class Movable : PlayerState
    {
        public LayerMask layersToPickup;
        /// <summary>
        /// The player's horizontal motion speed.
        /// </summary>
        private float walkingSpeed
        {
            get
            {
                return gameObject.GetComponent<Player>().WalkingSpeed;
            }
        }

        private bool isOnTheFloor, hasGotOffTheFloor;

        protected Player playerReference;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            playerReference = GetComponent<Player>();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            ProcessInput();
            // TODO: update the GameObject's sprite and more...
        }

        //private void ProcessLanding()
        //{
        //    BottomChecker bottomChecker = GetComponentInChildren<BottomChecker>();

        //    // Check that the player is standing or has landed
        //    if (bottomChecker.Landing)
        //    {
        //        var rigidBody = gameObject.GetComponent<Rigidbody2D>();

        //        rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
        //    }
        //}

        /// <summary>
        /// Process inputs from the player.
        /// </summary>
        void ProcessInput()
        {
            float axis = Input.GetAxis("Horizontal");
            var rigidBody = gameObject.GetComponent<Rigidbody2D>();

            //FIXME: 遊びの調節
            // Turn and move left

            if (axis < -0.1)
            {
                playerReference.CurrentDirection = Player.Direction.Left;
                rigidBody.velocity = new Vector2(-walkingSpeed, rigidBody.velocity.y);
                playerReference.playerAnimator.SetBool("Moving", true);
                playerReference.transform.localScale = new Vector3(1, 1, 1);
                playerReference.holdingObjectTransform.transform.localScale = new Vector3(1, 1, 1);
            }
            // Right
            else if (axis > 0.1)
            {
                playerReference.CurrentDirection = Player.Direction.Right;
                rigidBody.velocity = new Vector2(+walkingSpeed, rigidBody.velocity.y);
                playerReference.playerAnimator.SetBool("Moving", true);
                playerReference.transform.localScale = new Vector3(-1, 1, 1);
                playerReference.holdingObjectTransform.transform.localScale = new Vector3(-1, 1, 1);
            }
            // Stop
            else
            {
                rigidBody.velocity = new Vector2(0F, rigidBody.velocity.y);
                playerReference.playerAnimator.SetBool("Moving", false);
            }

            // Act
            if (Input.GetButtonDown(playerReference.ActButtonName))
            {
                this.ProcessActButton();
            }
        }

        /// <summary>
        /// Process when the player has pressed the Act Button.
        /// </summary>
        void ProcessActButton()
        {
            var player = GetComponent<Player>();

            // put down if the player already has something
            if (player.HoldingObject != null)
            {
                player.PutDown();
                return;
            }

            // Search a PickableObject in front of the player character
            RaycastHit2D? nearestPickable = null;

            List<Collider2D> colliderFound = new List<Collider2D>();
            playerReference.pickupCollider.OverlapCollider(playerReference.pickupFilter, colliderFound);
            colliderFound.OrderBy(i => i.Distance(playerReference.pickupCollider));
            if(colliderFound.Count > 0)
            { 
                player.PickUp(colliderFound[0].GetComponentInParent<PickableObject>());
            }
        }
    }
}
