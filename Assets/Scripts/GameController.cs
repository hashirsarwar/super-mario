using UnityEngine;
using UnityEngine.UI;
public class GameController : MonoBehaviour
{
    private static int score;
    public static int lives = 3;
    private static GameObject scoreText;
    private static GameObject livesText;
    // private static GameObject 

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        if (PlayerPrefs.GetInt("lives") != 0)
        {
            lives = PlayerPrefs.GetInt("lives");
        }
        else
        {
            PlayerPrefs.SetInt("lives", lives);
        }
        scoreText = GameObject.FindGameObjectWithTag("Score");
        livesText = GameObject.FindGameObjectWithTag("Lives");
        livesText.GetComponent<Text>().text = "Lives: " + PlayerPrefs.GetInt("lives");
    }

    public static void AddScore(int n)
    {
        score += n;
        scoreText.GetComponent<Text>().text = "Score: " + score;
    }

    public static void MinusLife()
    {
        --lives;
        PlayerPrefs.SetInt("lives", lives);
        livesText.GetComponent<Text>().text = "Lives: " + lives;
    }
}
