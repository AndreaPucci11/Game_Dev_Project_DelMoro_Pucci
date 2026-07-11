using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimento in avanti")]
    public float playerSpeed = 6f;

    [Header("Progressione Difficoltà")]
    public float aumentoVelocita = 1.0f;
    public int distanzaPerAumento = 50;
    public float velocitaMassima = 15f;
    private int ultimoTraguardo = 0;

    [Header("Corsie (devono corrispondere a quelle di ObstacleSpawner)")]
    public float[] corsie = new float[] { -5.0f, 0f, 5.0f };
    [SerializeField] int corsiaAttuale = 1;

    [Header("Velocita' scorrimento verso la corsia")]
    public float velocitaScorrimento = 10f;

    [Header("Salto")]
    public float altezzaSalto = 3f;
    public float durataSalto = 1f;

    public Animator animator;

    private bool canJump = true;
    private bool isRunning = false;
    private bool inTransizione = false;

    void Update()
    {
        transform.Translate(Vector3.forward * playerSpeed * Time.deltaTime, Space.World);

        float targetX = corsie[corsiaAttuale];
        float nuovaX = Mathf.MoveTowards(transform.position.x, targetX, velocitaScorrimento * Time.deltaTime);
        transform.position = new Vector3(nuovaX, transform.position.y, transform.position.z);

        if (inTransizione && Mathf.Abs(transform.position.x - targetX) < 0.05f)
            inTransizione = false;

        if (!inTransizione)
        {
            if (Input.GetKeyDown(KeyCode.A) && corsiaAttuale > 0)
            {
                corsiaAttuale--;
                inTransizione = true;
            }
            else if (Input.GetKeyDown(KeyCode.D) && corsiaAttuale < corsie.Length - 1)
            {
                corsiaAttuale++;
                inTransizione = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
            StartCoroutine(JumpRoutine());

        if (!isRunning)
        {
            isRunning = true;
            StartCoroutine(AddDistance());
        }
    }

    IEnumerator AddDistance()
    {
        yield return new WaitForSeconds(0.35f);
        MasterInfo.distanceRun += 1;

        if (MasterInfo.distanceRun >= ultimoTraguardo + distanzaPerAumento)
        {
            ultimoTraguardo += distanzaPerAumento;

            if (playerSpeed < velocitaMassima)
            {
                playerSpeed += aumentoVelocita;
                Debug.Log("Velocità aumentata! Nuova velocità: " + playerSpeed);
            }
        }

        isRunning = false;
    }

    IEnumerator JumpRoutine()
    {
        canJump = false;
        animator.SetTrigger("Salto");

        float tempoTrascorso = 0f;
        float altezzaIniziale = transform.position.y;

        while (tempoTrascorso < durataSalto)
        {
            tempoTrascorso += Time.deltaTime;
            float percentuale = tempoTrascorso / durataSalto;
            float altezzaAttuale = 4f * altezzaSalto * percentuale * (1f - percentuale);
            transform.position = new Vector3(transform.position.x, altezzaIniziale + altezzaAttuale, transform.position.z);
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, altezzaIniziale, transform.position.z);
        canJump = true;
    }
}
