using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class StartMenu : MonoBehaviour
{
    public static StartMenu I { get; private set; }
    [SerializeField] private GameObject _startGamePanel;
    [SerializeField] private GlobalRunData _globalRunData;
    [SerializeField] private Image _LoadingSceneProgressBar;
    [SerializeField] private TMP_InputField _inputField;

    
    private WorldSize _worldSize;

    private void Start()
    {
        if (I == null) I = this;
    }

    public void Play()
    {
        _startGamePanel.SetActive(true);
    }
    
    public void StartGame()
    {
        _globalRunData.SEED = int.Parse(_inputField.text);
        _globalRunData.WorldSize = _worldSize;
        StartCoroutine(LoadSceneASync());
    }



    public void SetWorldSize(WorldSize worldSize)
    {
        _worldSize = worldSize;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadSceneASync()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(1);

        while (!loadOperation.isDone)
        {
            _LoadingSceneProgressBar.fillAmount = Mathf.Clamp01(loadOperation.progress/0.9f);
            yield return null;
        }
    }
}
