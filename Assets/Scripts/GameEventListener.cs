using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
        [SerializeField]
        private GameEventSO gameEvent = default; 
        [SerializeField]
        private UnityEvent response = default; 

        private void OnEnable() 
        {
            gameEvent.RegisterListener(this);
        }

        private void OnDisable() 
        {
            gameEvent.UnregisterListener(this);
        }

        public void OnEventRaised() 
        {
            response.Invoke();
        }
}

