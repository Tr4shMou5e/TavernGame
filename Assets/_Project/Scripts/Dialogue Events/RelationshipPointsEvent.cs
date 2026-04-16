using Ink.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "RelationshipPointsEvent", menuName = "Ink Events/RelationshipPointsEvent")]
public class RelationshipPointsEvent : InkEvent
{
    
    void GainRelationshipPoints()
    {
        var points = Random.Range(1, 10);
        Debug.Log("Relationship Points: " + points);
    }
    public override void Bind(Story story)
    {
        story.BindExternalFunction("Relationship", GainRelationshipPoints);
    }

    public override void Unbind(Story story)
    {
        story.UnbindExternalFunction("Relationship");
    }
}