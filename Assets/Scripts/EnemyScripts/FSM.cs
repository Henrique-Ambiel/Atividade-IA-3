using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FSM : MonoBehaviour
{
    public enum State // Enum de estados do inimigo
    {
        Patrol,
        Chase
    }

    private State currentState; // Estado atual do inimigo
    public GameObject player; // Refer�ncia ao jogador
    private float distanceMinimun = 5.0f; // Dist�ncia de ataque do inimigo
    private bool isNearPlayer = false; // Vari�vel que verifica se o jogador est� perto do inimigo
    public float speed = 2.0f; // Velocidade do inimigo
    public float rotationSpeed = 5.0f; // Velocidade de rota��o do inimigo
    private Vector3 direction; // Dire��o do inimigo
    private CharacterController controller; // Refer�ncia ao controlador de personagem

    void Start()
    {
        currentState = State.Patrol; // Estado inicial do inimigo
        controller = GetComponent<CharacterController>();
        direction = transform.forward; // Dire��o inicial (patrulha)
        StartCoroutine(UpdateFSM()); // Iniciar a corrotina que gerencia o FSM
    }

    void Update()
    {
        isNearPlayer = VerifyIfPlayerIsNear(); // Verifica a cada frame se o jogador est� perto

        // Atualiza o estado do inimigo de acordo com a dist�ncia do jogador
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;

            case State.Chase:
                Chase();
                break;
        }
    }

    IEnumerator UpdateFSM() // Fun��o que atualiza o estado do inimigo
    {
        while (true)
        {
            if (currentState == State.Patrol && isNearPlayer)
            {
                currentState = State.Chase;
                Debug.Log("Mudou para Perseguir");
            }
            else if (currentState == State.Chase && !isNearPlayer)
            {
                currentState = State.Patrol;
                Debug.Log("Mudou para Patrulhar");
            }

            yield return new WaitForSeconds(0.2f); // Pausa entre as verifica��es
        }
    }

    bool VerifyIfPlayerIsNear() // Fun��o que verifica se o jogador est� perto do inimigo
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        return distance <= distanceMinimun;
    }

    void Patrol() // Fun��o de patrulha do inimigo
    {
        Debug.Log("Patrulhando");
        controller.SimpleMove(direction * speed);

        if (Physics.Raycast(transform.position, direction, 3.0f))
        {
            direction = -direction; // Inverte a dire��o ao colidir
        }
    }

    void Chase() // Fun��o de persegui��o do inimigo
    {
        Debug.Log("Perseguindo");

        direction = (player.transform.position - transform.position).normalized;
        controller.SimpleMove(direction * speed);
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}

