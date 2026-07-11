using UnityEngine;

public class CollectGems : MonoBehaviour
{
    [SerializeField] AudioSource gemFX;

    private bool giaPresa = false;

    private void OnTriggerEnter(Collider other)
    {
        if (giaPresa || !this.gameObject.activeInHierarchy) return;

        giaPresa = true;
        MasterInfo.gemCount += 1;

        if (gemFX != null && gemFX.clip != null)
        {
            GameObject audioFantasma = new GameObject("SuonoGemmaTemp");

            AudioSource tempSource = audioFantasma.AddComponent<AudioSource>();

            tempSource.clip = gemFX.clip;
            tempSource.volume = gemFX.volume;
            tempSource.spatialBlend = 0f;

            tempSource.Play();

            Destroy(audioFantasma, gemFX.clip.length);
        }

        this.gameObject.SetActive(false);
    }
}
