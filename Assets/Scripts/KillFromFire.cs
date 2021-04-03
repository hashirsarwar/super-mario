using UnityEngine;

public class KillFromFire : MonoBehaviour
{
    public bool killOnlyOnce = true;
    private AudioSource audioSource;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            GameObject enemy = other.gameObject;
            GameController.AddScore(200);
            audioSource = this.gameObject.AddComponent<AudioSource>();
            AudioClip clip = (AudioClip)Resources.Load("smb_kick", typeof(AudioClip));
            audioSource.PlayOneShot(clip);
            InvertedFall(enemy);
            if (killOnlyOnce)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void InvertedFall(GameObject obj)
    {
        obj.GetComponent<MoveObject>().stopMoving = true;
        obj.transform.position += new Vector3(0, 0.2f, 0);
        obj.transform.rotation = Quaternion.Euler(0, 0, 180);
        obj.GetComponent<Collider2D>().enabled = false;
        obj.GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionY;
    }
}
