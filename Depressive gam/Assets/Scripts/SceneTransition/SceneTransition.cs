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
        //_instance._asyncTransition.allowSceneActivation = false;
    }

    private void OnAnimationOver()
    {
        _shouldPlayEndLoadAnimation = true;
        _instance._asyncTransition.allowSceneActivation = false;
    }

    void Update()
    {
        if(_asyncTransition != null)
        {
            //Showing progress download
            Debug.Log("Loading... " + _asyncTransition.progress + "%");
        }
    }

    void Start()
    {
        _instance = this;
        _animator = GetComponent<Animator>();

        if(_shouldPlayEndLoadAnimation)
        {
            //Start animation
        }
    }
}
