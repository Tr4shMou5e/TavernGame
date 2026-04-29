To make an event we will always make a script that inherits the class InkEvent
public class RainEvent : InkEvent{}

After the class has been made our intent is to make it into a scriptable object so we add above the class name like so:
[CreateAssetMenu(fileName = "RainEvent", menuName = "Ink Events/RainEvent")]
public class RainEvent : InkEvent

After that override the inherited methods like so:
public override void Bind(Story story)
{
    story.BindExternalFunction("RainEvent", Rain);
}

public override void Unbind(Story story)
{
    story.UnbindExternalFunction("RainEvent");
}

After that code the event of what is meant to happen, it will get triggered automatically as long as you made the event and give it in as one of the parameters in the Bind():
void Rain()
{
    Debug.Log("Rain");
}
public override void Bind(Story story)
{
    story.BindExternalFunction("RainEvent", Rain);
}

Go to unity and right-click and look for the event and fill out the nessesary things for it to work and find the dialogue and add the scriptable object to the list and done!