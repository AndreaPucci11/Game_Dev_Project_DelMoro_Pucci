using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CaricamentoGioco : MonoBehaviour
{
    [SerializeField] GameObject fadeIn;

    void Start()
    {
        StartCoroutine(AspettaECarica2());
    }

    IEnumerator AspettaECarica2()
    {
        yield return new WaitForSeconds(1);
        fadeIn.SetActive(false);
    }
}
