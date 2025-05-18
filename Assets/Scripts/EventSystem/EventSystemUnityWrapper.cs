using System;
using UnityEngine;

namespace GimGim.EventSystem {
    public class EventSystemUnityWrapper : MonoBehaviour {
        private void Update() {
            NotificationEventSystem.Flush();
        }
    }
}