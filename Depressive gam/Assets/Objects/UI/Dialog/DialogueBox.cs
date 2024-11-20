using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] private Image _avatar;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _text;

    public void SetAvatar(Sprite avatar)
    {
        _avatar.sprite = avatar;
    }
    public void SetName(string name)
    {
        _name.text = name;
    }
    public void SetText(string text)
    {
        _text.text = text;
    }

}
