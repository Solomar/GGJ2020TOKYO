using UnityEngine;
using System.Collections;

namespace Assets.Scripts.LevelElements
{
    public class Player : MonoBehaviour
    {
        private Assets.Scripts.LevelElements.PlayerStates.PlayerState CurrentState;

        // Use this for initialization
        void Start()
        {
            // The default state is Standing
            if (this.CurrentState == null)
            {
                this.CurrentState = gameObject.AddComponent<Assets.Scripts.LevelElements.PlayerStates.Standing>();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
