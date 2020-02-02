using UnityEngine;
using System.Collections;

namespace Assets.Scripts.LevelElements.PlayerStates
{
    /// <summary>
    /// The state processes when the player is Jumping or falling.
    /// </summary>
    public class Aerial : Movable
    {

        const float LandingThreshold = +0F;

        protected override void Update()
        {
            base.Update();

            playerReference.playerAnimator.SetBool("Jumping", true);
            // Check that the player is standing or has landed
            BottomChecker bottomChecker = GetComponentInChildren<BottomChecker>();
            if (bottomChecker.Landing && gameObject.GetComponent<Rigidbody2D>().velocity.y <= LandingThreshold)
            {
                playerReference.playerAnimator.SetBool("Jumping", false);
                ChangeTo<Standing>();
            }
        }
    }
}
