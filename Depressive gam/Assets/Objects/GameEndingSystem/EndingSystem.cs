using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EndingSystem : MonoBehaviour
{
    [SerializeField] public UnityEvent OnQuestOver;
    [SerializeField] public UnityEvent OnBadEnding;
    [SerializeField] public UnityEvent OnGoodEnding;

    [SerializeField] private List<QuestItem> _questItems;

    [SerializeField] private Dialogue _dialogue;

    private int _totalEndingCount => _questItems.Count;
    private int _goodEndingCount;
    private int _badEndingCount;

    private int _goodScore = 0;
    private int _badScore = 0;

    private void Start()
    {
        foreach (QuestItem item in _questItems.Where((q) => q.Type == EndingsType.Good))
        {
            _goodEndingCount++;
            item.OnCollect.AddListener(() => { _goodScore++; });
            item.OnCollect.AddListener(CheckEnding);
        }
        foreach (QuestItem item in _questItems.Where((q) => q.Type == EndingsType.Bad))
        {
            _badEndingCount++;
            item.OnCollect.AddListener(() => { _badScore++; });
            item.OnCollect.AddListener(CheckEnding);
        }
    }
    private void CheckEnding()
    {
        if (_goodScore + _badScore != _totalEndingCount / 2) return;

        foreach (var item in _questItems)
        {
            item.Hide();
        }

        if(_goodScore > _badScore)
        {
            _dialogue.OnEndDialogue.AddListener(() => { OnGoodEnding?.Invoke(); });
        }
        else
        {
            _dialogue.OnEndDialogue.AddListener(() => { OnBadEnding?.Invoke(); });
        }

        _dialogue.OnEndDialogue.AddListener(() => { OnQuestOver?.Invoke(); });
    }
}
