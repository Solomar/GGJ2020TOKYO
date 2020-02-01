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
        /// Changes the player's state.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        protected void ChangeTo<T>() where T : PlayerState
        {
#if UNITY_EDITOR
            Debug.Log($"Player FSM: {this.GetType().Name} -> {typeof(T).Name}");
#endif

            Destroy(this);
            gameObject.AddComponent<T>();
        }
    }
}
