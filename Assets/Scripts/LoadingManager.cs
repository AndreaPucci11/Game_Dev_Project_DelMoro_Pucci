using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GestoreCaricamento : MonoBehaviour
{
    [SerializeField] GameObject fadeOut;
    [SerializeField] GameObject fadeIn;

    void Start()
    {
        StartCoroutine(AspettaECarica());
    }

    IEnumerator AspettaECarica()
    {
        yield return new WaitForSeconds(1);
        fadeIn.SetActive(false);
        yield return new WaitForSeconds(4f);
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(2);
    }
}