using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Bot))]
public class BotAnimationEventHandler : MonoBehaviour
{
    private Bot _bot;

    private void Awake()
    {
        _bot = GetComponent<Bot>();
    }

    public void HandlePickUpFinish()
    {
        _bot.HandleInteraction();
    }
}
