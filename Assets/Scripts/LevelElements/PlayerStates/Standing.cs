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
                0.75F, LayerMask.GetMask(player.LayersForPickableObjects)))
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
