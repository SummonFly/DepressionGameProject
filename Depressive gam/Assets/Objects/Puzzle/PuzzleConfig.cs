using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Puzzle/Puzzle config")]
public class PuzzleConfig : ScriptableObject
{
    [SerializeField] public Vector2 PuzzleGridSize = Vector2.one;
    [SerializeField] public Vector2 _puzzleCellSize = Vector2.one;
    [SerializeField] public List<PuzzlePiece> PuzzlePieces;
}
