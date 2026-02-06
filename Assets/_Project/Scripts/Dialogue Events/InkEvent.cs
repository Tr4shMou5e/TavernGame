using Ink.Runtime;
using UnityEngine;


/// <summary>
/// This interface defines methods for binding and unbinding external functions in Ink stories.
/// This should also only be used for events that are triggered by the player.
/// </summary>
public abstract class InkEvent : ScriptableObject
{
    public abstract void Bind(Story story);
    public abstract void Unbind(Story story);
}
