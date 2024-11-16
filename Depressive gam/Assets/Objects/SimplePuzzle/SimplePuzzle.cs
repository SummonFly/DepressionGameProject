using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SimplePuzzle : MonoBehaviour
{
    [SerializeField] public UnityEvent OnPuzzleSolved;

    [SerializeField] private Transform _pieceRoot;
    [SerializeField] private SimplePuzzleConfig _config;

    private bool _isPuzzleSolved = false;
    private List<SimplePieceRoot> _roots = new();


    public void SetPuzzleConfig(SimplePuzzleConfig config)
    {
        if (config == null) return;
        _config = config;
        InstantiatePuzzlePieces();
    }

    public void InstantiatePuzzlePieces()
    {
        DestroyPuzzlePieces();
        foreach (var piece in _config.Pieces)
        {
            var instance = Instantiate(piece, _pieceRoot);
            _roots.Add(instance);
            instance.Piece.GetComponent<SimplePuzzlePiece>().OnStateUpdate.AddListener(CheckSolution);
        }
    }


    private void CheckSolution()
    {
        bool solved = true;
        foreach (var root in _roots)
        {
            solved &= root.Piece.IsDistination;
        }
        if (solved && !_isPuzzleSolved)
        {
            OnPuzzleSolved?.Invoke();
        }
        _isPuzzleSolved = solved;
    }

    private void DestroyPuzzlePieces()
    {
        if (_roots.Count == 0) return;
        foreach (var piece in _roots)
        {
            Destroy(piece.gameObject);
        }
        _roots.Clear();
    }


    private void Start()
    {
        if (_pieceRoot == null)
        {
            _pieceRoot = transform;
        }
        if(_config != null)
        {
            InstantiatePuzzlePieces();
        }
    }
}
