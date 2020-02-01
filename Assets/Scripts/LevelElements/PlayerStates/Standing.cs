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

            // If the player pushed the Jump button
            if(Input.GetButtonDown("Jump"))
            {
                // Go up and change the state
                var rb = gameObject.GetComponent<Rigidbody2D>();
                rb.velocity = new Vector2(rb.velocity.x, jumpAcceleration);

                ChangeTo<Aerial>();
            }
        }
    }
}
