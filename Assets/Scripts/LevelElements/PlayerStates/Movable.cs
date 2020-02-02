using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


        // Start is called before the first frame update
        protected virtual void Start()
        {

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
            var player = GetComponent<Player>();
            if (axis < -0.1)
            {
                player.CurrentDirection = Player.Direction.Left;
                rigidBody.velocity = new Vector2(-walkingSpeed, rigidBody.velocity.y);
                player.playerAnimator.SetBool("Moving", true);
                player.spriteTransform.localScale = new Vector3(1, 1, 1);
            }
            // Right
            else if (axis > 0.1)
            {
                player.CurrentDirection = Player.Direction.Right;
                rigidBody.velocity = new Vector2(+walkingSpeed, rigidBody.velocity.y);
                player.playerAnimator.SetBool("Moving", true);
                player.spriteTransform.localScale = new Vector3(-1, 1, 1);
            }
            // Stop
            else
            {
                rigidBody.velocity = new Vector2(0F, rigidBody.velocity.y);
                player.playerAnimator.SetBool("Moving", false);
            }

            // Act
            if (Input.GetButtonDown(player.ActButtonName))
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
            var pickRange = player.transform.Find("PickPoint").GetComponent<Collider2D>();
            RaycastHit2D? nearestPickable = null;
            foreach (var objInFront in Physics2D.RaycastAll(pickRange.gameObject.transform.position,
                player.CurrentDirection == Player.Direction.Left ? Vector2.left : Vector2.right,
                1.0f))
            {
                Debug.Log(objInFront.collider.gameObject.name);
                if (objInFront.collider.GetComponentInParent<PickableObject>() == null)
                    continue;
                if (nearestPickable?.distance >= objInFront.distance)
                    continue;
                nearestPickable = objInFront;
            }
            // Pick it up if one has found
            if (nearestPickable != null)
            {
                player.PickUp(nearestPickable.Value.collider.GetComponentInParent<PickableObject>());
            }
        }
    }
}
