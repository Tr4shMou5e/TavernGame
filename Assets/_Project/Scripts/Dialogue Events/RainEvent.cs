using System;
using Ink.Runtime;
using UnityEngine;

[CreateAssetMenu(fileName = "RainEvent", menuName = "Ink Events/RainEvent")]
public class RainEvent : InkEvent
{
    void Rain()
    {
        Debug.Log("Rain");
    }
    public override void Bind(Story story)
    {
        story.BindExternalFunction("RainEvent", Rain);
    }

    public override void Unbind(Story story)
    {
        story.UnbindExternalFunction("RainEvent");
    }
}