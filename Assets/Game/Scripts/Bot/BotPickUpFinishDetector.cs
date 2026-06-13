using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPickUpFinishDetector : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.TryGetComponent(out Bot bot);
        bot?.HandleInteraction();
    }
}
