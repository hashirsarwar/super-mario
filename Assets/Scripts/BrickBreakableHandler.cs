using System.Collections;
using UnityEngine;

public class BrickBreakableHandler : MonoBehaviour
{
    private ParticleSystem particle;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;
    private AudioSource audioSource;
    public AudioClip breakSound;
    public AudioClip bumpSound;

    void Start()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        audioSource = this.gameObject.AddComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && other.contacts[0].normal.y > 0.5f)
        {
            if (other.gameObject.GetComponent<PlayerController>().getPlayerHealth() == 100)
            {
                audioSource.PlayOneShot(breakSound);
                StartCoroutine(Break());
            }

            else
            {
                audioSource.PlayOneShot(bumpSound);
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
