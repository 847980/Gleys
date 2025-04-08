using System;
using System.Collections;
using AimToMite;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsycnLoader : MonoBehaviour
{

    public Slider loadingSlider;
    public TextMeshProUGUI loadingText;
    public TextMeshProUGUI tipsText;
    public string[] tips;
    public float minLoadTime = 1;

    private bool canSwitch;
    private BaseNextPrev baseNextPrev;

    private void Awake()
    {
        baseNextPrev = GetComponent<BaseNextPrev>();
    }

    private void OnEnable()
    {
        baseNextPrev.maxIndex = tips.Length - 1;
        baseNextPrev.curIndex = UnityEngine.Random.Range(0, baseNextPrev.maxIndex);
        ChangeTips(baseNextPrev.curIndex);
        InvokeRepeating(nameof(ChangeNextIndex), 5, 5);
    }

    private void ChangeNextIndex() => baseNextPrev.OnChangeIndexRandom();

    IEnumerator LoadAsync(string scene)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(scene);
        loadingSlider.value = 0;
        loadOperation.allowSceneActivation = false;
        while (!loadOperation.isDone && !canSwitch)
        {
            var progress = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingSlider.value = progress;
            yield return null;
        }
        loadOperation.allowSceneActivation = true;
    }

    IEnumerator WaitSwitch()
    {
        yield return new WaitForSeconds(minLoadTime);
        canSwitch = true;
    }

    IEnumerator TextUpdate()
    {
        int dot = 1;
        while (true)
        {
            string _loadingText = "Loading Resources";
            for (int i = 0; i < dot; i++)
            {
                _loadingText += ".";
            }
            loadingText.SetText(_loadingText);
            dot++;
            if (dot > 3) dot = 1;
            yield return new WaitForSeconds(0.5f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadAsync("SampleScene"));
        StartCoroutine(WaitSwitch());
        StartCoroutine(TextUpdate());
    }

    public void ChangeTips(int index)
    {
        tipsText.SetText(tips[index]);
    }
}
