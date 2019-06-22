using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class SnakeScr : MonoBehaviour
{
    public GameObject foodPref, tailPrefab;
    GameObject food;

    Transform lBoarder, rBoarder, uBoarder, dBoarder;

    float speed, turnTimeWall, turnTime;

    Vector2 move, vector;

    bool eat, canTurnWall, canTurne, isPlayer, isStart;
    public static bool isGameOver;

    int score;
    public static int bestScore;

    string sceneName;

    List<Transform> tail = new List<Transform>();
    Text scoreText, pauseScoreText, bestScoreText;

    /*private static SnakeScr player;
    void Awake()
    {
        DontDestroyOnLoad(this);
        if (player == null)
        {
            player = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }*/

    void Start()
    {
        lBoarder = GameObject.Find("WallL").GetComponent<Transform>();
        rBoarder = GameObject.Find("WallR").GetComponent<Transform>();
        uBoarder = GameObject.Find("WallU").GetComponent<Transform>();
        dBoarder = GameObject.Find("WallD").GetComponent<Transform>();

        sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        isPlayer = sceneName != "Menu";

        if (isPlayer)
        {
            scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
            pauseScoreText = GameObject.Find("PauseScoreText").GetComponent<Text>();
            bestScoreText = GameObject.Find("BestScoreText").GetComponent<Text>();
        }

        switch(MenuScript.colorNum)
        {
            case 1:
                GetComponent<Renderer>().material.color = Color.white;
                break;
            case 2:
                GetComponent<Renderer>().material.color = Color.red;
                break;
            case 3:
                GetComponent<Renderer>().material.color = Color.blue;
                break;
            default:
                GetComponent<Renderer>().material.color = Color.white;
                break;
        }

        turnTime = Time.time;
        turnTimeWall = Time.time;
        speed = 0.3f;
        eat = false;
        canTurnWall = true;
        isGameOver = false;
        isStart = false;
        score = 0;
        bestScore = PlayerPrefs.GetInt("BestScore", 0);

        InvokeRepeating("Movement", 0.1f, speed);
        SpawnFood();
    }

    void Update()
    {
        SnakeBehaviour();

        if (score > bestScore && isPlayer)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
        }

        if(Input.touchCount > 0 && !isStart)
        {
            isStart = true;
            vector = Vector2.up;
        }
        
        Restart();
    }

    void Restart()
    {
        if(((GameController.isRestart && isPlayer) || !isPlayer) && isGameOver )
        {
            for(int i = 0; i < tail.Count; i++)
            {
                Destroy(tail[i].gameObject);
            }

            tail.Clear();
            transform.position = new Vector2(0f, 0f);
            isGameOver = false;
            score = 0;
            GameController.isRestart = false;
        }
    }

    void Movement()
    {
        if (!isGameOver)
        {
            Vector2 v = transform.position;
            transform.Translate(move);

            if (eat)
            {
                GameObject g = Instantiate(tailPrefab, v, Quaternion.identity);
                g.GetComponent<Renderer>().material.color = this.GetComponent<Renderer>().material.color;
                
                tail.Insert(0, g.transform);
                eat = false;
            }
            else if (tail.Count > 0)
            {
                tail.Last().position = v;
                tail.Insert(0, tail.Last());
                tail.RemoveAt(tail.Count - 1);
            }
        }
    }

    void SnakeBehaviour()
    {
        if (food == null)
        {
            SpawnFood();
        }

        if (isPlayer && isStart)
        {
            if (Time.time > turnTimeWall + 0.6f)
            {
                canTurnWall = true;
            }

            if(Time.time > turnTime + 0.2f)
            {
                canTurne = true;
            }

            if ((Input.GetKeyDown(KeyCode.D) || Input.GetTouch(0).deltaPosition.x > 0f) 
            && vector != -Vector2.right && canTurnWall && canTurne)
            {
                vector = Vector2.right;
                turnTime = Time.time;
                canTurne = false;
            }
            else if ((Input.GetKeyDown(KeyCode.A) || Input.GetTouch(0).deltaPosition.x < 0f) 
            && vector != Vector2.right && canTurnWall && canTurne)
            {
                vector = -Vector2.right;
                turnTime = Time.time;
                canTurne = false;
            }
            else if ((Input.GetKeyDown(KeyCode.W) || Input.GetTouch(0).deltaPosition.y > 0f) 
            && vector != -Vector2.up && canTurnWall && canTurne)
            {
                vector = Vector2.up;
                turnTime = Time.time;
                canTurne = false;
            }
            else if ((Input.GetKeyDown(KeyCode.S) || Input.GetTouch(0).deltaPosition.y < 0f) 
            && vector != Vector2.up && canTurnWall && canTurne)
            {
                vector = -Vector2.up;
                turnTime = Time.time;
                canTurne = false;
            }

            scoreText.text = "Score: " + score.ToString();
            pauseScoreText.text = "Score: " + score.ToString();
            bestScoreText.text = "Best Score: " + bestScore.ToString();
        }

        if (!isPlayer)
        {
            Vector3 rayPos = new Vector3(0f, 0f, transform.position.z);
            bool obstacleOnWay = false;
            RaycastHit2D hit = Physics2D.Raycast (rayPos, vector);
            
            if (hit) 
            {
                if (hit.collider.gameObject.name.StartsWith("Snake")) 
                {
                    obstacleOnWay = true;
                }
            }
            if(!obstacleOnWay)
            {
                if ((transform.position.y < food.transform.position.y)
                && vector != -Vector2.up)
                {
                    vector = Vector2.up;
                    rayPos.x = transform.position.x + 0f;
                    rayPos.y = transform.position.y + 0.7f;
                }
                else if ((transform.position.y > food.transform.position.y)
                && vector != Vector2.up)
                {
                    vector = -Vector2.up;
                    rayPos.x = transform.position.x + 0f;
                    rayPos.y = transform.position.y + -0.7f;
                }
                else if ((transform.position.x < food.transform.position.x)
                && vector != -Vector2.right)
                {
                    vector = Vector2.right;
                    rayPos.x = transform.position.x + 0.7f;
                    rayPos.y = transform.position.y + 0f;
                }
                else if ((transform.position.x > food.transform.position.x)
                && vector != Vector2.right)
                {
                    vector = -Vector2.right;
                    rayPos.x = transform.position.x + -0.7f;
                    rayPos.y = transform.position.y + 0f;
                }
            }
            else if(obstacleOnWay)
            {
                if(vector == Vector2.right || vector == -Vector2.right)
                {
                    vector = Vector2.up;
                    rayPos.x = transform.position.x + 1f;
                    rayPos.y = transform.position.y + 0f;
                }
                else if(vector == Vector2.up || vector == -Vector2.up)
                {
                    vector = Vector2.right;
                    rayPos.x = transform.position.x + 0f;
                    rayPos.y = transform.position.y + 0.7f;
                }
            }
        }
        move = vector;
    }

    void SpawnFood()
    {
        float x = (int)Random.Range(lBoarder.transform.position.x + 2f, rBoarder.transform.position.x - 2f);
        float y = (int)Random.Range(dBoarder.transform.position.y + 2f, uBoarder.transform.position.y - 2f);
        food = Instantiate(foodPref, new Vector3(x, y, 5f), Quaternion.identity);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Food")
        {
            Destroy(col.gameObject);
            eat = true;
            score++;
            SpawnFood();
        }

        if (col.gameObject.name.StartsWith("Snake"))
        {
            isGameOver = true;
        }

        if (col.gameObject.name == "WallR" && vector == Vector2.right)
        {
            transform.position = new Vector2(lBoarder.transform.position.x, transform.position.y);
            turnTimeWall = Time.time;
            canTurnWall = false;
        }
        else if (col.gameObject.name == "WallL" && vector == -Vector2.right)
        {
            transform.position = new Vector2(rBoarder.transform.position.x, transform.position.y);
            turnTimeWall = Time.time;
            canTurnWall = false;
        }
        else if (col.gameObject.name == "WallU" && vector == Vector2.up)
        {
            transform.position = new Vector2(transform.position.x, dBoarder.transform.position.y);
            turnTimeWall = Time.time;
            canTurnWall = false;
        }
        else if (col.gameObject.name == "WallD" && vector == -Vector2.up)
        {
            transform.position = new Vector2(transform.position.x, uBoarder.transform.position.y);
            turnTimeWall = Time.time;
            canTurnWall = false;
        }
    }
}
