using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BotAnimatorData
{
    public static class Params
    {
        public static readonly int Speed = Animator.StringToHash(nameof(Speed));
        public static readonly int PickUp = Animator.StringToHash(nameof(PickUp));
        public static readonly int IsCarrying = Animator.StringToHash(nameof(IsCarrying));
    }
}
