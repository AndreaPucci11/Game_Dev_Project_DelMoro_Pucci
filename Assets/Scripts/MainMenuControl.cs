using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.Burst.CompilerServices;

public class MainMenuControl : MonoBehaviour
{
    [SerializeField] GameObject fadeOut;
    [SerializeField] GameObject bounceText;
    [SerializeField] GameObject bigButton;
    [SerializeField] GameObject animCam;
    [SerializeField] GameObject mainCam;
    [SerializeField] GameObject menuControls;
    [SerializeField] AudioSource buttonSelect;
    public static bool hasClicked;
    [SerializeField] GameObject staticCam;
    [SerializeField] GameObject fadeIn;
    [SerializeField] int loadedCoins;
    [SerializeField] int loadedGems;
    [SerializeField] int loadedRecord;
    [SerializeField] GameObject coinDisplay;
    [SerializeField] GameObject gemDisplay;
    [SerializeField] GameObject runDisplay;

    void Start()
    {
        StartCoroutine(FadeInTurnOff());
        if (hasClicked == true)
        {
            staticCam.SetActive(true);
            animCam.SetActive(false);
            menuControls.SetActive(true);
            bounceText.SetActive(false);
            bigButton.SetActive(false);
        }
    }

    void Update()
    {
    }

    public void StartGame()
    {
        StartCoroutine(startButton());
    }

    IEnumerator startButton()
    {
        buttonSelect.Play();
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1);
    }

    public void menuBeginButton()
    {
        StartCoroutine(AnimCam());
    }

    IEnumerator AnimCam()
    {
        animCam.GetComponent<Animator>().Play("AnimMenuCam");
        bounceText.SetActive(false);
        bigButton.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        fadeIn.SetActive(false);
        mainCam.SetActive(true);
        animCam.SetActive(false);
        menuControls.SetActive(true);
        hasClicked = true;
    }

    IEnumerator FadeInTurnOff()
    {
        yield return new WaitForSeconds(0.05f);
        loadedCoins = PlayerPrefs.GetInt("COINSAVE");
        loadedGems = PlayerPrefs.GetInt("GEMSAVE");
        loadedRecord = PlayerPrefs.GetInt("RECORDSAVE");
        coinDisplay.GetComponent<TMPro.TMP_Text>().text = "" + loadedCoins;
        gemDisplay.GetComponent<TMPro.TMP_Text>().text = "" + loadedGems;
        runDisplay.GetComponent<TMPro.TMP_Text>().text = "" + loadedRecord;
        yield return new WaitForSeconds(1);
        fadeIn.SetActive(false);
    }
}
