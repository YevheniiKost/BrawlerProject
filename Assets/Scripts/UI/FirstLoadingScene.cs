using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirstLoadingScene : MonoBehaviour
{
    [SerializeField] private Slider _progressSlider;

    private AsyncOperation async;
    void Start()
    {
        async = SceneManager.LoadSceneAsync(GameConstants.Scenes.StartMenu);
    }

    private void Update()
    {
        _progressSlider.value = async.progress;
    }

}
