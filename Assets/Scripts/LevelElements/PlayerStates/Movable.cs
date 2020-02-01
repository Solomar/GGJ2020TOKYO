﻿using System;
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
        private const float walkingSpeed = 1F;

        private bool isOnTheFloor, hasGotOffTheFloor;


        // Start is called before the first frame update
        protected virtual void Start()
        {

        }

        // Update is called once per frame
        protected virtual void Update()
        {
            ProcessInput();

            // TODO: make Aerial state and change to it when the player isn't on a floor
            ProcessLanding();
            // TODO: update the GameObject's sprite and more...
        }

        private void ProcessLanding()
        {
            BottomChecker bottomChecker = GetComponentInChildren<BottomChecker>();

            // Check that the player is standing or has landed
            if (bottomChecker.Landing)
            {
                var rigidBody = gameObject.GetComponent<Rigidbody2D>();

                rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
            }
        }

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
                this.CurrentDirection = Direction.Left;
                rigidBody.velocity = new Vector2(-walkingSpeed, rigidBody.velocity.y);
            }
            // Right
            else if (axis > 0.1)
            {
                this.CurrentDirection = Direction.Right;
                rigidBody.velocity = new Vector2(+walkingSpeed, rigidBody.velocity.y);
            }
            // Stop
            else
            {
                rigidBody.velocity = new Vector2(0F, rigidBody.velocity.y);
            }
        }

        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    //FIXME: If the collider is the player's bottom
        //    if (true)
        //    {
        //        // ほんらいこのくだりはAerialにやｒせるもの（それができるまでここでテスト)
        //        //// Snap the player's x coordinate to the nearest integer
        //        //var transform = gameObject.GetComponent<Transform>();
        //        //transform.position = new Vector3(transform.position.x, 0F, transform.position.z);

        //        // Check that the player is standing or has landed
        //        var rigidBody = gameObject.GetComponent<Rigidbody2D>();

        //        rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
        //    }
        //}

        //private void OnTriggerStay2D(Collider2D collision)
        //{
        //    // Check that the player is standing or has landed
        //    var rigidBody = gameObject.GetComponent<Rigidbody2D>();

        //    rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
        //}
    }
}