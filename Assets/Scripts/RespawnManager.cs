using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class RespawnManager : MonoBehaviour
{
    [SerializeField] GameObject thePlayer;
    [SerializeField] GameObject playerAnim;
    [SerializeField] GameObject respawnPanel;
    [SerializeField] TMP_Text gemCostText;
    [SerializeField] TMP_Text gemsAvailableText;
    [SerializeField] TMP_Text coinCostText;
    [SerializeField] TMP_Text coinsAvailableText;
    [SerializeField] float invincibilitySeconds = 3f;
    [SerializeField] int costoRespawnGemme = 1;
    [SerializeField] int costoRespawnMonete = 100;
    [SerializeField] AudioSource buttonAudio;

    private Vector3 savedPosition;
    private CollisionDetect collisionDetect;

    private void Start()
    {
        if (respawnPanel != null)
            respawnPanel.SetActive(false);
    }

    public void MostraPannelloRespawn(Vector3 posizioneCollisione, CollisionDetect caller)
    {
        savedPosition = posizioneCollisione;
        collisionDetect = caller;

        int savedGems = PlayerPrefs.GetInt("GEMSAVE", 0);
        int savedCoins = PlayerPrefs.GetInt("COINSAVE", 0);
        int gemmeDisponibili = savedGems + MasterInfo.gemCount;
        int moneteDisponibili = savedCoins + MasterInfo.coinCount;

        if (gemmeDisponibili < costoRespawnGemme && moneteDisponibili < costoRespawnMonete)
        {
            Debug.Log($"Respawn non disponibile. Gemme totali={gemmeDisponibili}, Monete totali={moneteDisponibili}");
            if (caller != null) caller.ProcediGameOver();
            return;
        }

        if (gemCostText != null)
            gemCostText.text = "Costo: " + costoRespawnGemme + " gemme";
        if (gemsAvailableText != null)
            gemsAvailableText.text = "Hai: " + gemmeDisponibili + " gemme";

        if (coinCostText != null)
            coinCostText.text = "Costo: " + costoRespawnMonete + " monete";
        if (coinsAvailableText != null)
            coinsAvailableText.text = "Hai: " + moneteDisponibili + " monete";

        respawnPanel.SetActive(true);
    }

    public void OnClickUsaRespawn()
    {
        int savedGems = PlayerPrefs.GetInt("GEMSAVE", 0);
        int savedCoins = PlayerPrefs.GetInt("COINSAVE", 0);
        int gemmeDisponibili = savedGems + MasterInfo.gemCount;
        int moneteDisponibili = savedCoins + MasterInfo.coinCount;

        bool haGemmePerRespawn = gemmeDisponibili >= costoRespawnGemme;
        bool haMonetePerRespawn = moneteDisponibili >= costoRespawnMonete;

        if (!haGemmePerRespawn && !haMonetePerRespawn)
        {
            Debug.Log("Risorse insufficienti per il respawn!");
            ChiudiPannelloEGameOver();
            return;
        }

        if (buttonAudio != null) buttonAudio.Play();

        if (haGemmePerRespawn)
            DetraGemme(costoRespawnGemme);
        else
            DetraMonete(costoRespawnMonete);

        StopAllCoroutines();
        respawnPanel.SetActive(false);
        StartCoroutine(EseguiRespawn());
    }

    private void DetraGemme(int costo)
    {
        int gemmeInGame = MasterInfo.gemCount;

        if (gemmeInGame >= costo)
        {
            MasterInfo.gemCount -= costo;
        }
        else
        {
            int resto = costo - gemmeInGame;
            MasterInfo.gemCount = 0;
            int gemSalvate = PlayerPrefs.GetInt("GEMSAVE", 0);
            PlayerPrefs.SetInt("GEMSAVE", gemSalvate - resto);
        }

        Debug.Log("Respawn pagato con gemme! Gemme rimanenti: " + MasterInfo.gemCount);
    }

    private void DetraMonete(int costo)
    {
        int moneteInGame = MasterInfo.coinCount;

        if (moneteInGame >= costo)
        {
            MasterInfo.coinCount -= costo;
        }
        else
        {
            int resto = costo - moneteInGame;
            MasterInfo.coinCount = 0;
            int moneteSalvate = PlayerPrefs.GetInt("COINSAVE", 0);
            PlayerPrefs.SetInt("COINSAVE", moneteSalvate - resto);
        }

        Debug.Log("Respawn pagato con monete! Monete rimanenti: " + MasterInfo.coinCount);
    }

    public void OnClickRifiutaRespawn()
    {
        if (buttonAudio != null) buttonAudio.Play();
        ChiudiPannelloEGameOver();
    }

    IEnumerator EseguiRespawn()
    {
        thePlayer.transform.position = savedPosition;
        thePlayer.GetComponent<PlayerMovement>().enabled = true;

        Animator anim = playerAnim.GetComponent<Animator>();
        if (anim != null) anim.Play("Running");

        CollisionDetect cd = thePlayer.GetComponent<CollisionDetect>();
        if (cd == null) cd = thePlayer.GetComponentInChildren<CollisionDetect>();
        if (cd != null) cd.enabled = false;

        StartCoroutine(LampeggiaPlayer(invincibilitySeconds));
        yield return new WaitForSeconds(invincibilitySeconds);

        if (cd != null)
        {
            cd.ResetGameOver();
            cd.enabled = true;
        }

        Debug.Log("Respawn completato! Il giocatore è di nuovo vulnerabile.");
    }

    IEnumerator LampeggiaPlayer(float durata)
    {
        Renderer[] tuttiIrenderers = thePlayer.GetComponentsInChildren<Renderer>();
        System.Collections.Generic.List<Renderer> renderersAttivi = new System.Collections.Generic.List<Renderer>();

        foreach (Renderer r in tuttiIrenderers)
        {
            if (r.enabled == true)
            {
                renderersAttivi.Add(r);
            }
        }

        float elapsed = 0f;
        bool visibile = true;

        while (elapsed < durata)
        {
            visibile = !visibile;

            foreach (Renderer r in renderersAttivi)
                r.enabled = visibile;

            yield return new WaitForSeconds(0.15f);
            elapsed += 0.15f;
        }

        foreach (Renderer r in renderersAttivi)
            r.enabled = true;
    }

    private void ChiudiPannelloEGameOver()
    {
        StopAllCoroutines();
        if (respawnPanel != null)
            respawnPanel.SetActive(false);

        if (collisionDetect != null)
            collisionDetect.ProcediGameOver();
    }
}