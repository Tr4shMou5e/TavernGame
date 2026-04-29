using System;
using UnityEngine;
using Random = UnityEngine.Random;
using Ink.Runtime;
[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue System/Dialogue")]
public class Dialogue : ScriptableObject
{
    public string characterName;
    public Sprite characterImage;
    public TextAsset textAsset;
    public int exp;

    private void OnEnable()
    {
        exp = Random.Range(1, 10);
    }
}