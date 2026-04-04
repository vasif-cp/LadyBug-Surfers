using System;
using UnityEngine;

namespace LS.Events
{
    public class GameEvents
    {
        public static Action OnPullStarted;
        public static Action<float> OnPullUpdated;
        public static Action OnPullEnded;
    }
}
