using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Puzzle/Simple puzzle config")]
public class SimplePuzzleConfig : ScriptableObject
{
    [SerializeField] public List<SimplePieceRoot> Pieces;
}
