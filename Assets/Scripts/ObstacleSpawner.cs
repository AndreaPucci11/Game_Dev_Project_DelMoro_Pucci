using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    [System.Serializable]
    public class ObstacleDefinition
    {
        public GameObject gameObject;
        [Min(1)] public int peso = 1;

        [Tooltip("Sposta su/gi� solo questo ostacolo")]
        public float yOffset = 0f;
    }

    [Header("Ostacoli disponibili")]
    [SerializeField] List<ObstacleDefinition> tipiOstacoli;

    [Header("Impostazioni Griglia")]
    [SerializeField] float zStart = 5f;
    [SerializeField] float distanzaZTraRighe = 10f;
    [SerializeField] int numeroDiRighe = 4;

    [Header("Corsie")]
    [SerializeField] float[] corsieX = new float[] { -3.5f, 0f, 3.5f };

    [Header("Monete (Comuni)")]
    [SerializeField] GameObject monetaPrefab;
    [SerializeField] float altezzaMonete = 1f;
    [Range(0f, 1f)]
    [SerializeField] float probabilitaMoneta = 0.7f;

    [Header("Gemme (Rare)")]
    [SerializeField] GameObject gemmaPrefab;
    [SerializeField] float altezzaGemme = 1.2f;
    [Range(0f, 1f)]
    [Tooltip("Consiglio: 0.05 per il 5% di possibilit� o 0.1 per il 10%")]
    [SerializeField] float probabilitaGemma = 0.05f;

    void Start()
    {
        if (tipiOstacoli == null || tipiOstacoli.Count == 0) return;

        DisattivaPrefab(monetaPrefab);
        DisattivaPrefab(gemmaPrefab);

        GeneraOstacoli();
    }

    void DisattivaPrefab(GameObject prefab)
    {
        if (prefab != null && prefab.scene.IsValid()) prefab.SetActive(false);
    }

    void GeneraOstacoli()
    {
        List<ObstacleDefinition> pool = new List<ObstacleDefinition>();
        foreach (var def in tipiOstacoli)
        {
            if (def.gameObject != null)
            {
                def.gameObject.SetActive(false);
                for (int p = 0; p < def.peso; p++) pool.Add(def);
            }
        }

        if (pool.Count == 0) return;

        for (int riga = 0; riga < numeroDiRighe; riga++)
        {
            float zPos = zStart + (riga * distanzaZTraRighe);
            int ostacoliInQuestaRiga = Random.Range(1, 3);

            List<int> indiciCorsie = new List<int> { 0, 1, 2 };
            MischiaLista(indiciCorsie);

            for (int i = 0; i < ostacoliInQuestaRiga; i++)
            {
                int corsiaScelta = indiciCorsie[i];
                float xPos = corsieX[corsiaScelta];
                ObstacleDefinition def = pool[Random.Range(0, pool.Count)];

                float altezzaWorld = def.gameObject.transform.lossyScale.y;
                float yPos = (altezzaWorld * 0.5f) + def.yOffset;

                SpawnElemento(def.gameObject, xPos, yPos, zPos);
            }

            for (int j = ostacoliInQuestaRiga; j < 3; j++)
            {
                int corsiaLibera = indiciCorsie[j];
                float xPos = corsieX[corsiaLibera];
                float randomRoll = Random.value;

                if (gemmaPrefab != null && randomRoll <= probabilitaGemma)
                {
                    SpawnElemento(gemmaPrefab, xPos, altezzaGemme, zPos);
                }
                else if (monetaPrefab != null && randomRoll <= probabilitaMoneta)
                {
                    SpawnElemento(monetaPrefab, xPos, altezzaMonete, zPos);
                }
            }
        }
    }

    void SpawnElemento(GameObject prefab, float x, float y, float z)
    {
        GameObject clone = Instantiate(prefab, this.transform);
        clone.transform.localPosition = new Vector3(x, y, z);
        clone.transform.localRotation = prefab.transform.localRotation;
        clone.SetActive(true);

        Collider col = clone.GetComponentInChildren<Collider>();
        if (col != null) col.isTrigger = true;
    }

    void MischiaLista(List<int> lista)
    {
        for (int i = 0; i < lista.Count; i++)
        {
            int temp = lista[i];
            int randomIndex = Random.Range(i, lista.Count);
            lista[i] = lista[randomIndex];
            lista[randomIndex] = temp;
        }
    }
}
