using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class SceneTransition : MonoBehaviour
{
    private static SceneTransition _instance;
    private static bool _shouldPlayEndLoadAnimation = false;

    private Animator _animator;
    private AsyncOperation _asyncTransition;

    public static void SwitchToScene(string name)
    {
        _instance._asyncTransition =  SceneManager.LoadSceneAsync(name);
        _instance._asyncTransition.allowSceneActivation = false;

        _instance._animator.SetTrigger("StartLoading");
    }

    public void OnAnimationOver()
    {
        _shouldPlayEndLoadAnimation = true;
        _instance._asyncTransition.allowSceneActivation = true;
    }

    void Update()
    {
        if(_asyncTransition != null)
        {
            //Showing progress download
            Debug.Log("Loading... " + _asyncTransition.progress + "% " + _asyncTransition.allowSceneActivation);
        }
    }

    void Start()
    {
        _instance = this;
        _animator = GetComponent<Animator>();

        if(_shouldPlayEndLoadAnimation)
        {
            _animator.SetTrigger("EndLoading");
        }
    }
}
