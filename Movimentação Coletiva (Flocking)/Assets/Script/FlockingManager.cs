using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingManager : MonoBehaviour
{   //Pega Prefab do peixe
    public GameObject fishPrefab;
    //Quantidade de peixes iniciais
    public int numFish = 20;
    //Array dos peixes
    public GameObject[] allFish;
    //Limite para os peixes
    public Vector3 swinLimits = new Vector3(5, 5, 5);
    //Posição do objetivo
    public Vector3 goalPos;

    //Titulo
    [Header("Configurações do Cardume")]
    [Range(0.0f, 5.0f)]
    //Velocidade maxima e minima
    public float minSpeed;
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    [Range(1.0f, 10.0f)]
    //Velocidade de rotação e distancia dos peixes
    public float neighbourDistance;
    [Range(0.0f, 5.0f)]
    public float rotationSpeed;

    private void Start()
    {
        allFish = new GameObject[numFish];
        for(int i = 0; i < numFish; i++)
        {
            //Usa for para atribuir um vector3 aleatório
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x),
                                                                Random.Range(-swinLimits.y, swinLimits.y),
                                                                Random.Range(-swinLimits.z, swinLimits.z));
            //Instancia prefabs na sena
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
            allFish[i].GetComponent<Flock>().myManager = this;
        }
        goalPos = this.transform.position;
    }
    //Segue posição
    private void Update()
    {      
        goalPos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x),
                                                        Random.Range(-swinLimits.z, swinLimits.z));
    }
}
