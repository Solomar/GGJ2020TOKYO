using UnityEngine;
using System.Linq;

namespace Assets.Scripts.LevelElements
{
    /// <summary>
    /// The MonoBehaviour of the player character.
    /// </summary>
    public class Player : MonoBehaviour
    {
        /// <summary>
        /// The X velocity when the player character is walking to right.
        /// </summary>
        public float WalkingSpeed = 2F;

        /// <summary>
        /// The Y velocity when the player character has jumped.
        /// </summary>
        public float JumpSpeed = 7F;

        /// <summary>
        /// The button's name used to make the player character jump.
        /// </summary>
        public string JumpButtonName = "Jump";

        /// <summary>
        /// The button's name used to make the player character pick or put an object in front of the character.
        /// </summary>
        public string ActButtonName = "Fire1";

        /// <summary>
        /// Names of layers which objects mounted on the player should be on.
        /// </summary>
        public string[] LayersForMountableObjects = { "Mountable" };

        /// <summary>
        /// The player's direction. Used to choose a sprite, the object which the player acts (e.g. pickup).
        /// </summary>
        public enum Direction
        {
            Right,
            Left,
        };

        [SerializeField]
        private AudioClip pickupClip;
        [SerializeField]
        private AudioClip dropClip;

        /// <summary>
        /// The player's now direction.
        /// </summary>
        public Direction CurrentDirection = Direction.Right;

        private static string GetHierarchyPath(GameObject go)
        {
            return go.transform.parent != null ? GetHierarchyPath(go.transform.parent.gameObject) + "/" + go.name : go.name;
        }

        /// <summary>
        /// The <see cref="HoldingObject"/>'s parent.
        /// </summary>
        private Transform objectHolder
        {
            get
            {
                return gameObject.transform.Find("HoldingObject");
            }
        }

        /// <summary>
        /// The <see cref="PickableObject"/> holded by the <see cref="Player"/>.
        /// </summary>
        public PickableObject HoldingObject
        {
            get
            {
                return gameObject.transform.Find("HoldingObject").GetComponentInChildren<PickableObject>();
            }
            set
            {
                var holder = gameObject.transform.Find("HoldingObject");
                // Release the holding object if value is set to null
                if (value == null)
                {
                    //FIXME: Should the parent be as same as the player's?
                    this.HoldingObject.transform.SetParent(this.GetComponent<Player>().transform.parent);
                    holder.gameObject.SetActive(false);
                    return;
                }
                if (this.HoldingObject != null)
                {
                    throw new System.InvalidOperationException($"The player is already holding {GetHierarchyPath(this.HoldingObject.gameObject)}.");
                }
                holder.gameObject.SetActive(true);
                value.transform.SetParent(holder);
                value.transform.localPosition = Vector2.zero;

                // Set the hold position
                UpdateHoldPosition();
            }
        }

        /// <summary>
        /// The player's currfent state, a part of a FSM.
        /// </summary>
        private Assets.Scripts.LevelElements.PlayerStates.PlayerState CurrentState;

        /// Init some member variables.
        void Start()
        {
            // The default state is Standing
            if (this.CurrentState == null)
            {
                this.CurrentState = gameObject.AddComponent<Assets.Scripts.LevelElements.PlayerStates.Standing>();
            }
        }

        /// <summary>
        /// Update the the holding position, which should be in front of the player.
        /// </summary>
        private void Update()
        {
            UpdateHoldPosition();
        }

        /// <summary>
        /// Update the <see cref="objectHolder"/>'s position.
        /// </summary>
        private void UpdateHoldPosition()
        {
            var obj = this.HoldingObject;
            if (obj == null)
                return;
            var holder = this.objectHolder;

            if (LayersForMountableObjects.Any(
                layer => obj.gameObject.layer == LayerMask.NameToLayer(layer)
                ))
            {
                holder.transform.localPosition = transform.Find("HoldPositionTop").transform.localPosition;
            }
            else
            {
                var pos = transform.Find("HoldPositionRight").transform.localPosition;
                if (this.CurrentDirection == Direction.Left)
                    pos = new Vector3(-pos.x, pos.y, pos.z);
                holder.transform.localPosition = pos;
            }
        }

        /// <summary>
        /// Pick up an <see cref="PickableObject"/>.
        /// </summary>
        /// <param name="obj"></param>
        public void PickUp(PickableObject obj)
        {
            this.HoldingObject = obj;
            obj.OnPickUp();
            SoundManager.Instance.PlaySound(pickupClip);
        }

        /// <summary>
        /// Put down the object the player is picked up and holding.
        /// </summary>
        public void PutDown()
        {
            var obj = this.HoldingObject;
            if (obj == null)
                return;

            this.HoldingObject = null;
            obj.OnPutDown();
            SoundManager.Instance.PlaySound(dropClip);
        }
    }
}
