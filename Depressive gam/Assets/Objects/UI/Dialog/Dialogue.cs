using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Dialogue : MonoBehaviour
{
    [SerializeField] public UnityEvent OnEndDialogue;

    [SerializeField] private DialogueBox _leftBox;
    [SerializeField] private DialogueBox _rightBox;
    [SerializeField] private DialogueConfig _config;

    private int _dialogId = 0;
    private bool _isShow = false;

    public void OnNext(InputAction.CallbackContext context)
    {
        if(context.ReadValue<float>() == 1 && context.performed)
        {
            Next();
        }
    }

    public void Show()
    {
        if (_isShow) return;
        _isShow = true;
        Next();
    }
    public void Hide()
    {
        _leftBox.gameObject.SetActive(false);
        _rightBox.gameObject.SetActive(false);
    }
    public void Setconfig(DialogueConfig config)
    {
        _config = config;
    }

    public void Next()
    {
        Hide();
        if (!_isShow) return;
        if(_dialogId < _config.dialoguePhrases.Count)
        {
            ShowPhrase(_config.dialoguePhrases[_dialogId]);
            _dialogId++;
        }
        else
        {
            _dialogId = 0;
            _isShow=false;
            OnEndDialogue?.Invoke();
        }

    }
    private void ShowPhrase(DialoguePhrase phrase)
    {
        var box = _leftBox;
        switch (phrase.orientation)
        {
            case DialogBoxOrientation.rigt: box = _rightBox; break;
            case DialogBoxOrientation.left: box = _leftBox; break;
        }
        box.SetName(phrase.CharacterName);
        box.SetText(phrase.Text);
        box.gameObject.SetActive(true);
    }

}
