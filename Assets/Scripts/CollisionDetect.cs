using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CollisionDetect : MonoBehaviour
{
    [Header("Componenti Giocatore")]
    [Tooltip("Trascina qui l'oggetto figlio che ha l'Animator del giocatore")]
    [SerializeField] Animator playerAnim;

    [Header("Gestori Scena e UI")]
    [SerializeField] AudioSource collisionAudio;
    [Tooltip("Trascina qui la Main Camera")]
    [SerializeField] Animator mainCamAnim;
    [SerializeField] GameObject fadeOut;
    [SerializeField] RespawnManager respawnManager;

    private bool gameOverInCorso = false;
    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacles"))
        {
            if (gameOverInCorso) return;
            gameOverInCorso = true;

            SaveLoad.saveData = true;
            StartCoroutine(CollisionStart());
        }
    }

    IEnumerator CollisionStart()
    {
        if (collisionAudio != null) collisionAudio.Play();

        if (playerMovement != null) playerMovement.enabled = false;

        if (playerAnim != null) playerAnim.Play("Stumble Backwards");
        if (mainCamAnim != null) mainCamAnim.Play("CollisionCam");

        yield return new WaitForSeconds(1.5f);

        if (respawnManager != null)
        {
            respawnManager.MostraPannelloRespawn(transform.position, this);
        }
        else
        {
            yield return new WaitForSeconds(1.5f);
            yield return StartCoroutine(FadeECarica());
        }
    }

    public void ResetGameOver()
    {
        gameOverInCorso = false;
    }

    public void ProcediGameOver()
    {
        StartCoroutine(FadeECarica());
    }

    IEnumerator FadeECarica()
    {
        if (fadeOut != null) fadeOut.SetActive(true);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
    }
}
