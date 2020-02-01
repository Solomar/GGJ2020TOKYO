using UnityEngine;
using UnityEditor;

namespace Assets.Scripts.LevelElements.PlayerStates
{
    /// <summary>
    /// This implements some common logics of the Player's state.
    /// </summary>
    public abstract class PlayerState : MonoBehaviour
    {
        /// <summary>
        /// The player's direction. Used to choose a sprite, the object which the player acts (e.g. pickup).
        /// </summary>
        public enum Direction
        {
            Right,
            Left,
        };

        /// <summary>
        /// The player's now direction.
        /// </summary>
        public Direction CurrentDirection;

        /// <summary>
        /// Changes the player's state.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        protected void ChangeTo<T>() where T : PlayerState
        {
#if UNITY_EDITOR
            Debug.Log($"-> {typeof(T).Name}");
#endif

            Destroy(this);
            gameObject.AddComponent<T>();
        }
    }
}
