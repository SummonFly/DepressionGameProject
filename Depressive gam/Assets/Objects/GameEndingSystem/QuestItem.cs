using UnityEngine;
using UnityEngine.Events;

public class QuestItem : MonoBehaviour
{
    public EndingsType Type => _type;
    public bool Spawn => _spawn;

    [SerializeField] public UnityEvent OnCollect;

    [SerializeField] private bool _spawn = true;
    [SerializeField] private GameObject _item;
    [SerializeField] private Interacteable _interactable;
    [SerializeField] private EndingsType _type;

    public void Collect()
    {
        OnCollect?.Invoke();
        Hide();
    }
    public void Hide()
    {
        _spawn = false;
        _item.SetActive(false);
    }
    public void Show()
    {
        _spawn = true;
        _item.SetActive(true);
    }
}

[SerializeField]
public enum EndingsType
{
    Bad,
    Good
}