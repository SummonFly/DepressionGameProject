using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="Dialogue config")]
public class DialogueConfig : ScriptableObject
{
    public List<DialoguePhrase> dialoguePhrases;
}

[Serializable]
public class DialoguePhrase
{
    public DialogBoxOrientation orientation;
    public string CharacterName;
    public string Text;
}
[Serializable]
public enum DialogBoxOrientation
{
    left,
    rigt
}