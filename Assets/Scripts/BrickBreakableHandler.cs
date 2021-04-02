using System.Collections;
using UnityEngine;

public class BrickBreakableHandler : MonoBehaviour
{
    private ParticleSystem particle;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;

    void Start()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && other.contacts[0].normal.y > 0.5f)
        {
            if (other.gameObject.GetComponent<PlayerController>().getPlayerHealth() == 100)
            {
                StartCoroutine(Break());
            }

            else
            {
                GetComponent<ElevateWhenHit>().Elevate();
            }
        }
    }

    private IEnumerator Break()
    {
        particle.Play();
        spriteRenderer.enabled = false;
        boxCollider2D.enabled = false;
        yield return new WaitForSeconds(particle.main.startLifetime.constantMax);
        Destroy(gameObject);
    }
}
