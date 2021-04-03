using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Fire" || collision.gameObject.tag == "DeadEnemy")
        {
            GetComponent<FloatingText>().display(this.gameObject, "200");
        }
    }
}
