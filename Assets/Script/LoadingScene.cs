using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{

    public GameObject Canvas;
    public Slider loadingBar;
    // Start is called before the first frame update
    void Start()
    {
        if (!Canvas)
        {
            Debug.Log("Canvas Not Find!");
        }
        else 
        {
            loadingBar = Canvas.transform.Find("LoadingBar").GetComponent<Slider>();
        }
        loadingBar.value = 0;
        StartCoroutine(LoadScene());

    }

    // Update is called once per frame
    void Update()
    {
        if (loadingBar) 
        {
            loadingBar.value += Time.deltaTime * 0.1f;
        }

        if(loadingBar.value >= 1.0)
        {
            loadingBar.value = 1;
        }
    }

    IEnumerator LoadScene()
    {
        var handle = Addressables.LoadSceneAsync("Assets/Scenes/TestCDNScenes.unity", UnityEngine.SceneManagement.LoadSceneMode.Single, true);

        handle.Completed += (op) =>
        {
            Debug.Log("LoadSceneAsync Completed");
        };

        yield return null;
    }
}
