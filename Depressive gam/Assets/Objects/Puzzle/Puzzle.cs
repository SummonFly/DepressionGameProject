using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Puzzle : MonoBehaviour
{
    [SerializeField] public UnityEvent OnPuzzleSolved;

    [SerializeField] private int _permutationsNumber = 20;
    [SerializeField] private Transform _puzzleRoot;
    [SerializeField] private PuzzleConfig _config;

    private Vector2 _translate => Vector2.Scale(_config.PuzzleGridSize, _config._puzzleCellSize)/2f * -1f + _config._puzzleCellSize/2f;
    private Dictionary<PuzzlePiece, Vector2> _puzzleCurrentPosition = new();
    private UnityAction _initSolveTask;

    public void SetInitSolveTask(QuestItem item)
    {
        _initSolveTask = item.Collect;
    }

    public void SetPuzzleConfig(PuzzleConfig config)
    {
        if (config == null) return;
        _config = config;
        InstantiatePuzzlePieces();
    }

    public void InstantiatePuzzlePieces()
    {
        DestroyPuzzlePieces();
        foreach (var piece in _config.PuzzlePieces)
        {
            var instance = Instantiate(piece, _puzzleRoot);
            _puzzleCurrentPosition.Add(instance, instance._positionInPuzzle);
            instance.GetComponent<Button>().onClick.AddListener(() => InteractWithPuzzlePiece(instance));
            SetPuzzlePiecePosition(instance, instance._positionInPuzzle);
        }
    }

    public void ShufflePuzzle()
    {
        var pieceCount = _puzzleCurrentPosition.Keys.Count;
        var puzzlePieces = _puzzleCurrentPosition.Keys.ToArray();
        for (int i = 0; i < _permutationsNumber; i++)
        {
            PuzzlePiece first = puzzlePieces[Random.Range(0, pieceCount)];
            PuzzlePiece second = puzzlePieces[Random.Range(0, pieceCount)];
            SwapPuzzlePiece(first, second);
        }
    }


    public void InteractWithPuzzlePiece(PuzzlePiece piece)
    {
        if (piece == null) return;
        if(_puzzleCurrentPosition.TryGetValue(piece, out Vector2 value))
        {
            var emptyNeighbor = CheckEmptyPieceAround(value);
            if (emptyNeighbor != null)
            {
                SwapPuzzlePiece(piece, emptyNeighbor);
                CheckSolution();
            }
        }
    }

    private void CheckSolution()
    {
        bool solved = true;
        foreach (var piece in _puzzleCurrentPosition.Keys)
        {
            solved &= _puzzleCurrentPosition[piece] == piece._positionInPuzzle;
        }
        if(solved)
        {
            _initSolveTask();
            OnPuzzleSolved?.Invoke();
        }
    }

    private void DestroyPuzzlePieces()
    {
        if (_puzzleCurrentPosition.Count == 0) return;
        foreach (var piece in _puzzleCurrentPosition.Keys)
        {
            Destroy(piece.gameObject);
        }
        _puzzleCurrentPosition.Clear();
    }

#nullable enable
    private PuzzlePiece? CheckEmptyPieceAround(Vector2 position)
    {
        var left = position + new Vector2(-1, 0);
        var up = position + new Vector2(0, 1);
        var right = position + new Vector2(1, 0);
        var down = position + new Vector2(0, -1);

        var emptyPieces = _puzzleCurrentPosition.Keys.Where((k)=> k is EmptyPuzzlePiece).ToList<PuzzlePiece>();
        if(emptyPieces == null)
            return null;

        var emptyPiecePos = _puzzleCurrentPosition[emptyPieces.First()];
        if (emptyPiecePos == left || emptyPiecePos == right || emptyPiecePos == up || emptyPiecePos == down) 
            return emptyPieces.First();

        return null;
    }
#nullable disable

    private void SwapPuzzlePiece(PuzzlePiece first, PuzzlePiece second)
    {
        var firstPos = _puzzleCurrentPosition[first];
        var secondPos = _puzzleCurrentPosition[second];

        SetPuzzlePiecePosition(first, secondPos);
        SetPuzzlePiecePosition(second, firstPos);
    }

    private void SetPuzzlePiecePosition(PuzzlePiece piece, Vector2 position)
    {
        //Debug.Log("SetPuzzlePiecePosition попытка");
        if (piece == null) return;
        if (position.y < 0 || position.x < 0 || position.y >= _config.PuzzleGridSize.y || position.x >= _config.PuzzleGridSize.x)
            return;

        _puzzleCurrentPosition[piece] = position;
        piece.gameObject.transform.localPosition = Vector2.Scale(_config._puzzleCellSize, position) + _translate;
        //Debug.Log($"SetPuzzlePiecePosition {position.x} {position.y}");
    }

    private void Start()
    {
        if(_puzzleRoot == null)
        {
            _puzzleRoot = transform;
        }
        InstantiatePuzzlePieces();
        ShufflePuzzle();
    }
}
