using System.Collections;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public GameObject prefab;
    public void display(GameObject gameObject, string text)
    {
        Vector2 objPos = gameObject.transform.position;
        GameObject f = Instantiate(prefab, new Vector2(objPos.x - 0.15f, objPos.y + 0.5f), Quaternion.identity);
        f.gameObject.GetComponent<TextMesh>().text = text;
        f.transform.localScale = new Vector3(0.1f, 0.1f, 1);
        StartCoroutine(DestroyFloatingText(f));
    }

    IEnumerator DestroyFloatingText(GameObject obj)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(obj);
    }
}
