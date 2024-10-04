using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jogador : MonoBehaviour
{
    // Componentes
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sr;

    // Movimento
    [SerializeField] private float velocidadeMovimento = 5f;
    private Vector2 comandosDirecionais;

    // Pulo e Queda
    [SerializeField] private float forcaDoPulo = 8f;
    private float limiteDoChao = 0.1f;

    // Layer do Ch�o
    [SerializeField] private LayerMask layerDoChao;

    // Detectar Ch�o
    [SerializeField] private Vector2 tamanhoCaixa; // (largura e altura)
    [SerializeField] private float posicaoCaixaOffset; // Offset

    void Start()
    {

    }

    void Update()
    {
        LerComandos();
        AtualizaAnimator();
        AtualizaFlip();
    }

    void FixedUpdate()
    {
        Movimento();
    }

    void LerComandos()
    {
        // Leitura de Comandos
        comandosDirecionais = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Se o jogador pressionar o bot�o "Jump", o personagem ir� pular
        if (Input.GetButtonDown("Jump"))
        {
            Pular();
        }
    }

    void AtualizaAnimator()
    {
        if (rb.velocity.y > limiteDoChao) // Se a velocidade � positiva, est� pulando
        {
            animator.SetBool("estaPulando", true);
            animator.SetBool("estaCaindo", false);
        }
        else if (rb.velocity.y < -limiteDoChao) // Se a velocidade � negativa, est� caindo
        {
            animator.SetBool("estaPulando", false);
            animator.SetBool("estaCaindo", true);
        }
        else // se a velocidade � 0, ent�o est� no ch�o
        {
            animator.SetBool("estaPulando", false);
            animator.SetBool("estaCaindo", false);

            // Se a velocidade do nosso jogador for diferente de 0, significa que ele est� andando
            if (comandosDirecionais.x != 0)
            {
                // Jogador Andando
                animator.SetBool("estaCorrendo", true);
            }
            else
            {
                // Jogador Parado
                animator.SetBool("estaCorrendo", false);
            }
        }
    }

    void AtualizaFlip()
    {
        // Se o comando direcional em X for positivo,
        // est� apertando para direita;
        if (comandosDirecionais.x > 0)
        {
            sr.flipX = false;
        }
        // Se o comando direcional em X for negativo,
        // est� apertando para esquerda;
        else if (comandosDirecionais.x < 0)
        {
            sr.flipX = true;
        }
    }

    void Pular()
    {
        if(estaNoChao())
        {
            rb.velocity = new Vector2(rb.velocity.x, forcaDoPulo);
        }
    }

    void Movimento()
    {
        // Movimentando nosso Jogador Horizontalmente a partir
        // dos comandos lidos no Update
        rb.velocity = new Vector2(comandosDirecionais.x * velocidadeMovimento, rb.velocity.y);
    }

    bool estaNoChao()
    {
        if(Physics2D.BoxCast(transform.position, tamanhoCaixa, 0, -transform.up, posicaoCaixaOffset, layerDoChao))
        {
            // Est� encostando na layer do ch�o
            return true;
        }
        else
        {
            // N�o est� encostando na layer do ch�o

            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position - transform.up * posicaoCaixaOffset, tamanhoCaixa);
    }
}
