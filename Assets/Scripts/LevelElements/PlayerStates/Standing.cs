using UnityEngine;
using System.Collections;

namespace Assets.Scripts.LevelElements.PlayerStates
{
    /// <summary>
    /// 
    /// </summary>
    public class Standing : Movable
    {
        private float jumpAcceleration
        {
            get
            {
                return gameObject.GetComponent<Player>().JumpSpeed;
            }
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // Check that the player character is standing or has landed
            BottomChecker bottomChecker = GetComponentInChildren<BottomChecker>();
            if (!bottomChecker.Landing)
            {
                ChangeTo<Aerial>();
                return;
            }

            this.ProcessInput();
        }

        /// <summary>
        /// Process Jump&amp;Act(Fire) button input.
        /// Note: This function does as dirrent as that <see cref="Assets.Scripts.LevelElements.PlayerStates.Movable"/> does.
        /// </summary>
        void ProcessInput()
        {
            var player = GetComponent<Player>();
            // If the player pushed the Jump button
            if (Input.GetButtonDown(player.JumpButtonName))
            {
                // Go up and change the state
                var rb = gameObject.GetComponent<Rigidbody2D>();
                rb.AddForce(new Vector2(rb.velocity.x, jumpAcceleration), ForceMode2D.Impulse);

                ChangeTo<Aerial>();
            }
        }
    }
}
