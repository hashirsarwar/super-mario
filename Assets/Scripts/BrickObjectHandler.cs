using UnityEngine;

public class BrickObjectHandler : MonoBehaviour
{
    public enum BrickObjects
    {
        Coin,
        RedMushroom,
        Star,
        Flower,
    }
    public BrickObjects brickObjects;
    public int numOfCoins = 1;
    private Vector3 initPos;
    private Vector3 targetPos;
    private bool elevateBrick = false;
    private bool liftDown = false;
    private GameObject targetCoin;
    private bool elevateTargetCoin = false;
    private Vector3 initCoinPos; 
    private Vector3 targetCoinPos;
    private GameObject targetMushroom;
    private bool elevateTargetMushroom = false;
    private Vector3 targetMushroomPos;

    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        initPos = transform.position;
        targetPos = transform.position + new Vector3(0, 0.1f, 0);
        initCoinPos = targetPos + new Vector3(0, 0.15f, 0);
        targetCoinPos = initCoinPos + new Vector3(0, 0.4f, 0);
        targetMushroomPos = targetPos + new Vector3(0, 0.2f, 0);
    }

    void Update()
    {
        ElevateBrickWhenHit();
        ElevateTargetCoinWhenSpawned();
        ElevateTargetMushroomWhenSpawned();
    }

    void ElevateTargetMushroomWhenSpawned()
    {
        if (elevateTargetMushroom)
        {
            targetMushroom.transform.position = Vector3.MoveTowards(targetMushroom.transform.position,
                                                                    targetMushroomPos,
                                                                    0.5f * Time.deltaTime);
            if (targetMushroom.transform.position.y >= targetMushroomPos.y)
            {
                elevateTargetMushroom = false;
                targetMushroom.tag = "RedMushroom";
                targetMushroom.GetComponent<MoveObject>().stopMoving = false;
            }
        }
    }

    void ElevateTargetCoinWhenSpawned()
    {
        if (elevateTargetCoin)
        {
            
            targetCoin.transform.position = Vector3.MoveTowards(targetCoin.transform.position,
                                                                targetCoinPos,
                                                                2 * Time.deltaTime);
            if (targetCoin.transform.position.y >= targetCoinPos.y)
            {
                elevateTargetCoin = false;
                Destroy(targetCoin);
            }
        }
    }

    void ElevateBrickWhenHit()
    {
        if (elevateBrick)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime);
            if (transform.position.y >= targetPos.y)
            {
                liftDown = true;
                elevateBrick = false;
            }
        }

        if (liftDown)
        {
            transform.position = Vector3.MoveTowards(transform.position, initPos, Time.deltaTime);
            if (transform.position.y <= initPos.y)
                liftDown = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.tag == "Player" && other.contacts[0].normal.y > 0.5f)
        {
            ElevateBrick();
            switch (brickObjects)
            {
                case BrickObjects.Coin:
                    GameObject coin = this.transform.GetChild(0).gameObject;
                    SpawnCoin(coin);
                    break;
                case BrickObjects.Star:
                    GameObject star = this.transform.GetChild(1).gameObject;
                    break;
                case BrickObjects.Flower:
                    GameObject flower = this.transform.GetChild(2).gameObject;
                    break;
                case BrickObjects.RedMushroom:
                    GameObject mushroom = this.transform.GetChild(3).gameObject;
                    SpawnMushroom(mushroom);
                    break;
                default:
                    return;
            }            
        }    
    }

    void ElevateBrick()
    {
        elevateBrick = true;
    }

    void SpawnCoin(GameObject coin)
    {
        targetCoin = Instantiate(coin, initCoinPos, Quaternion.identity);
        targetCoin.SetActive(true);
        elevateTargetCoin = true;        
    }

    void SpawnMushroom(GameObject mushroom)
    {
        targetMushroom = Instantiate(mushroom, targetPos, Quaternion.identity);
        targetMushroom.SetActive(true);
        targetMushroom.GetComponent<MoveObject>().stopMoving = true;
        targetMushroom.tag = "Untagged";
        elevateTargetMushroom = true;
    }
}
