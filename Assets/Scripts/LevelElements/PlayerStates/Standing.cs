using UnityEngine;
using System.Collections;

namespace Assets.Scripts.LevelElements.PlayerStates
{
    /// <summary>
    /// 
    /// </summary>
    public class Standing : Movable
    {
        protected override void Start()
        {
            base.Start();

            // Set the player doesn't fall
            GetComponent<Rigidbody2D>().gravityScale = 0F;
        }

        protected override void Update()
        {
            base.Update();

            // Check that the player is standing or has landed
            BottomChecker bottomChecker = GetComponentInChildren<BottomChecker>();
            if (!bottomChecker.Landing)
            {
                ChangeTo<Aerial>();
            }
        }
    }
}
