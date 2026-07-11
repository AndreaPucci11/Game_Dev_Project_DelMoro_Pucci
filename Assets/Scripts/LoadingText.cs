using UnityEngine;
using TMPro;
using System.Collections;

public class TestoLoading : MonoBehaviour
{
    private TextMeshProUGUI mioTesto;

    void Start()
    {
        mioTesto = GetComponent<TextMeshProUGUI>();
        StartCoroutine(AnimaLoading());
    }

    IEnumerator AnimaLoading()
    {
        while (true)
        {
            mioTesto.text = "Loading";
            yield return new WaitForSeconds(1f);

            mioTesto.text = "Loading.";
            yield return new WaitForSeconds(1f);

            mioTesto.text = "Loading..";
            yield return new WaitForSeconds(1f);

            mioTesto.text = "Loading...";
            yield return new WaitForSeconds(1f);
        }
    }
}