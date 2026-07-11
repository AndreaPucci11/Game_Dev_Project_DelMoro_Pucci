using UnityEngine;

public class CollectCoin : MonoBehaviour
{
    [SerializeField] AudioSource coinFX;

    private bool giaPresa = false;

    private void OnTriggerEnter(Collider other)
    {
        if (giaPresa || !this.gameObject.activeInHierarchy) return;

        giaPresa = true;
        MasterInfo.coinCount += 1;

        if (coinFX != null && coinFX.clip != null)
        {
            GameObject audioFantasma = new GameObject("SuonoMonetaTemp");

            AudioSource tempSource = audioFantasma.AddComponent<AudioSource>();

            tempSource.clip = coinFX.clip;
            tempSource.volume = coinFX.volume;
            tempSource.spatialBlend = 0f;

            tempSource.Play();

            Destroy(audioFantasma, coinFX.clip.length);
        }

        this.gameObject.SetActive(false);
    }
}
