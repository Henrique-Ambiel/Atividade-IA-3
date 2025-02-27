using System.Collections;
using UnityEngine;

public class FSM : MonoBehaviour
{
    public enum State //Enum de estados do inimigo
    {
        Patrol,
        Chase
    }

    private State currentState; //Estado atual do inimgo
    public GameObject player; //Refer�ncia ao jogador
    private float distanceMinimun = 3.0f; //Dist�ncia de ataque do inimigo
    private bool isNearPlayer = false;


    private void Start()
    {
        currentState = State.Patrol; //Estado inicial do inimigo
    }

    IEnumerator updateFms() //Fun��o que atualiza o estado do inimigo
    {
        while (true)
        {
            if(currentState == State.Patrol)
            {
                if(isNearPlayer == false)
                {
                    currentState = State.Patrol;
                    Patrol();
                }

                if(isNearPlayer == true)
                {
                    currentState = State.Chase;
                    Chase();
                }
            } else if(currentState == State.Chase)
            {
                if (isNearPlayer == false)
                {
                    currentState = State.Patrol;
                    Patrol();
                }

                if (isNearPlayer == true)
                {
                    currentState = State.Chase;
                    Chase();
                }
            }
                yield return new WaitForSeconds(0.2f);
        }
    }

    bool verifyIfPlayerIsNear() //Fun��o que verifica se o jogador est� perto do inimigo
    {
        Vector3 distance = transform.position - player.transform.position;

        if (distance.magnitude <= distanceMinimun)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    void Patrol() //Fun��o de patrulha do inimigo
    {
        Debug.Log("Patrulhando");
    }

    void Chase() //Fun��o de persegui��o do inimigo
    {
        Debug.Log("Perseguindo");
    }
}


