﻿using UnityEngine;
using System.Collections;

namespace Assets.Scripts.LevelElements.PlayerStates
{
    /// <summary>
    /// The state processes when the player is Jumping or falling.
    /// </summary>
    public class Aerial : Movable
    {
        protected override void Start()
        {
            base.Start();

            // Set the player falls
            GetComponent<Rigidbody2D>().gravityScale = 1F;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // Check that the player is standing or has landed
            BottomChecker bottomChecker = GetComponentInChildren<BottomChecker>();
            if (bottomChecker.Landing)
            {
                ChangeTo<Standing>();
            }
        }
    }
}