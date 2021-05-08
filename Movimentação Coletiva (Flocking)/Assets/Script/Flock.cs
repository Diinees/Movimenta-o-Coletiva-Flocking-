using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{   //Instanciar classe FlockingManager
    public FlockingManager myManager;                                    
    float speed;                                                         
    bool turning = false;

    private void Start()
    {   //Atribuir velocidade aleatóriamente entre minima e maxima
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);   
    }

    void Update()
    {   //Limita espaço que os peixes nadam
        Bounds b = new Bounds(myManager.transform.position,             
                              myManager.swinLimits * 2);

        if (!b.Contains(transform.position))
        {
            turning = true;
        }
        else
        {
            turning = false;
        }
        if(turning)
        {
            Vector3 direction = myManager.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                 Quaternion.LookRotation(direction),
                                 myManager.rotationSpeed * Time.deltaTime);
        }
        else
        {   //Calcula velocidade de cada peixe
            if (Random.Range(0,100)<10)                                 
            {
                speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
                if(Random.Range(0,100)<20)
                {   //Chama Metodo para update
                    ApplyRules();                                                   
                }
            }
        }
        //aplica velocidade
        transform.Translate(0, 0, Time.deltaTime * speed);              
    }

    void ApplyRules()
    {   //Pega informações do script
        GameObject[] gos;
        gos = myManager.allFish;
        //Calcula pontos medios entre os peixes
        Vector3 vcentre = Vector3.zero;
        //Evita colisão dos peixes
        Vector3 vavoid = Vector3.zero;
        //Aciona a velocidade
        float gSpeed = 0.1f;
        //Calcula distancia de um peixe para outro
        float nDistance;
        //Calcula quantos peixes fazem parte do grupo
        int groupSize = 0;
        //Para atribuir calculos de distancia criação dos grupos
        foreach (GameObject go in gos)                                   
        {
            if(go != this.gameObject)
            {
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);
                if(nDistance <= myManager.neighbourDistance)
                {
                    vcentre += go.transform.position;
                    groupSize++;
                    //Evita que colidam
                    if (nDistance < 1.0)                                                        
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }
                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }
        //Se o numero de peixes do grupo for maior que 0
        //calcula a quantidade dos itens dentro do grupo e o ponto entre eles
        if (groupSize > 0)                                               
        {
            vcentre = vcentre / groupSize + (myManager.goalPos - this.transform.position);
            speed = gSpeed / groupSize;

            Vector3 direction = (vcentre + vavoid) - transform.position;
            //Suaviza a rotação
            if (direction != Vector3.zero)                              
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, 
                                     Quaternion.LookRotation(direction),
                                     myManager.rotationSpeed * Time.deltaTime);
            }
        }
    }
}
