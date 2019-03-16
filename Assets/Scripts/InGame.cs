using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class InGame : MonoBehaviour
{
    public System.Action OnGameOver;
    public int Score => scoreDisplay.Score;

    [SerializeField] Transform topVertexTransform;
    [SerializeField] Vertex vertexPrefab;
    [SerializeField] Ball ballPrefab;
    [SerializeField] Wall wallPrefab;
    [SerializeField] Coin coinPrefab;
    [SerializeField] int numVertexes;
    [SerializeField] ScoreDisplay scoreDisplay;
    [SerializeField] StartCountdownDisplay startCountdownDisplay;
    [SerializeField] List<InGameKeyAssignment> keyAssignments;
    [SerializeField] ColorIterator colorIterator;
    [SerializeField] AudioSource audioSource;

    List<Vertex> vertexes;
    Vertex selectedVertex;
    List<Ball> balls;
    Wall wall;
    Coin coin;

    int gotCoinCount;

    // Start is called before the first frame update
    void Start()
    {
        vertexes = new List<Vertex>();
        var topPosition = topVertexTransform.localPosition;
        for (var i = 0; i < numVertexes; i++)
        {
            var angle = 360f * i / numVertexes;
            var position = Quaternion.Euler(0f, 0f, angle) * topPosition;
            var vertex = Instantiate(vertexPrefab);
            vertex.Initialize(this, position);
            vertexes.Add(vertex);
        }

        audioSource.clip = Resources.Load<AudioClip>("Audios/beat0203");
    }

    // Update is called once per frame
    void Update()
    {
        for (var i = 0; i < numVertexes; i++)
        {
            foreach (var keyAssignment in keyAssignments)
            {
                if (Input.GetKeyUp(keyAssignment.Code(i)))
                {
                    OnVertexClicked(vertexes[i]);
                    break;
                }
            }
        }
    }

    public void StartGame()
    {
        scoreDisplay.Score = 0;
        gotCoinCount = 0;

        balls = new List<Ball>();
        AddNewBall();

        startCountdownDisplay.StartCountdown(() => {
            balls[0].StartMove();

            audioSource.Play();
            audioSource.DOFade(1f, 0f);
        });
    }

    public void OnBallHitWall()
    {
        AddScore(1);
        if (coin == null)
        {
            CreateNewCoin();
        }
    }

    public void OnGetCoin()
    {
        gotCoinCount++;
        AddScore(gotCoinCount * 100);
        coin = null;
    }

    public void OnBallOutOfBounds(Ball ball)
    {
        balls.Remove(ball);
        Destroy(ball.gameObject);

        if (balls.Count <= 0)
        {
            audioSource.DOFade(0f, 1f);
            OnGameOver?.Invoke();
        }
    }

    void AddScore(int score)
    {
        scoreDisplay.Score = scoreDisplay.Score + score;
    }

    public void OnVertexClicked(Vertex vertex)
    {
        if (selectedVertex == vertex)
        {
            vertex.ResetColor();
            colorIterator.Next();
            selectedVertex = null;
        }
        else if (selectedVertex == null)
        {
            vertex.SetColor(colorIterator.Current);
            selectedVertex = vertex;
        }
        else
        {
            CreateNewWall(selectedVertex, vertex);
            colorIterator.Next();
            selectedVertex = null;
        }
    }

    Ball AddNewBall()
    {
        var ball = Instantiate(ballPrefab);
        balls.Add(ball);
        return ball;
    }

    void CreateNewWall(Vertex v1, Vertex v2)
    {
        var color = colorIterator.Current;

        if (wall == null)
        {   // 使い回す
            wall = Instantiate(wallPrefab);
        }
        wall.Initialize(this, v1.transform.localPosition, v2.transform.localPosition, color);

        foreach (var vertex in vertexes)
        {
            if (vertex == v1 || vertex == v2)
            {
                vertex.SetColor(color);
            }
            else
            {
                vertex.ResetColor();
            }
        }
    }

    void CreateNewCoin()
    {
        if (coin != null)
        {
            return;
        }

        coin = Instantiate(coinPrefab);

        Vector3 position;
        var topPosition = topVertexTransform.localPosition;
        do
        {
            var rate = Random.Range(0f, 0.9f);
            var angle = Random.Range(0f, 360f);
            position = Quaternion.Euler(0f, 0f, angle) * topPosition * rate;
        } while (balls.Any(_ball => Vector3.Distance(position, _ball.transform.localPosition) < 1.0f));

        coin.Initialize(this, position);
    }
}
