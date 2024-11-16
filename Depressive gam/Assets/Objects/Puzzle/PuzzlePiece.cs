using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PuzzlePiece : MonoBehaviour
{
    [SerializeField] public Vector2 _positionInPuzzle = Vector2.zero;
}
