using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RevisedParticle
{
    [AddComponentMenu("Services/logging")]
    public class Logger : MonoBehaviour
    {
        [Header("Settings")]

        [SerializeField]
        bool showLogs;

        public void Log(object message, Object sender)
        {
            if (showLogs)
                Debug.Log(message, sender);

        }
    }
}
