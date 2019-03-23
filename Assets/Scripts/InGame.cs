using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class InGame : MonoBehaviour
{
    public System.Action OnGameOver;
    public int Score => scoreDisplay.Score;

    public enum Level
    {
        Normal,
        Expert,
    }

    [SerializeField] Transform topVertexTransform;
    [SerializeField] Vertex vertexPrefab;
    [SerializeField] Ball ballPrefab;
    [SerializeField] Wall wallPrefab;
    [SerializeField] Coin coinPrefab;
    [SerializeField] OneUp oneUpPrefab;
    [SerializeField] InGameKeyAssignmentGuide keyAssignmentGuidePrefab;
    [SerializeField] int numVertexes;
    [SerializeField] Canvas guideCanvas;
    [SerializeField] ScoreDisplay scoreDisplay;
    [SerializeField] ScoreUpView scoreUpViewPrefab;
    [SerializeField] StartCountdownView startCountdownViewPrefab;
    [SerializeField] List<InGameKeyAssignment> keyAssignments;
    [SerializeField] ColorIterator colorIterator;

    List<Vertex> vertexes;
    Vertex selectedVertex;
    List<Ball> balls;
    Wall wall;
    Coin coin;
    OneUp oneUp;

    Level level;
    TitleConstData titleConstData;
    int gotCoinCount;
    int gotOneUpCount;

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

            var guide = Instantiate(keyAssignmentGuidePrefab, guideCanvas.transform);
            guide.Initialize(keyAssignments, i, WorldToGuideCanvasLocalPosition(vertex.transform.position));
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (var i = 0; i < numVertexes; i++)
        {
            foreach (var keyAssignment in keyAssignments)
            {
                if (keyAssignment.GetKeyUp(i))
                {
                    OnVertexClicked(vertexes[i]);
                    break;
                }
            }
        }
    }

    public void StartGame(Level level, TitleConstData titleConstData)
    {
        this.level = level;
        this.titleConstData = titleConstData;
        scoreDisplay.Score = 0;
        gotCoinCount = 0;
        gotOneUpCount = 0;
        if (level == Level.Expert)
        {
            scoreDisplay.Score = titleConstData.ExpertInitialScore;
            gotCoinCount = titleConstData.ExpertInitialGotCoinCount;
        }

        foreach (var vertex in vertexes)
        {
            vertex.ResetColor();
        }
        selectedVertex = null;

        balls = new List<Ball>();
        AddNewBall();

        if (wall != null)
        {
            Destroy(wall.gameObject);
            wall = null;
        }
        if (coin != null)
        {
            Destroy(coin.gameObject);
            coin = null;
        }
        if (oneUp != null)
        {
            Destroy(oneUp.gameObject);
            oneUp = null;
        }

        var startCountdownView = Instantiate(startCountdownViewPrefab, guideCanvas.transform);
        startCountdownView.Initialize(() => {
            balls[0].StartMove(FirstBallStartForce(), titleConstData.MaxVelocity);

            AudioManagerSingleton.Instance.PlayBgm();
        });
    }

    public void OnBallHitWall(Ball ball)
    {
        AudioManagerSingleton.Instance.PlaySe(AudioManagerSingleton.Audio.Bound);
        AddScore(titleConstData.BoundScoreBase, ball.transform.position);
        ball.Bound();

        if (coin == null)
        {
            CreateNewCoin();
        }

        if (ball.BoundCount % titleConstData.BoundCountToOneUpBonus == 0 && oneUp == null)
        {
            CreateNewOneUp();
        }
    }

    public void OnGetCoin()
    {
        AudioManagerSingleton.Instance.PlaySe(AudioManagerSingleton.Audio.Coin);
        gotCoinCount++;
        AddScore(gotCoinCount * titleConstData.CoinScoreBase, coin.transform.position);
        coin = null;
    }

    public void OnGetOneUp()
    {
        AudioManagerSingleton.Instance.PlaySe(AudioManagerSingleton.Audio.OneUp);
        gotOneUpCount++;
        AddNewBall().StartMove(AdditionalBallStartForce(), titleConstData.MaxVelocity);
        oneUp = null;
    }

    public void OnBallOutOfBounds(Ball ball)
    {
        AudioManagerSingleton.Instance.PlaySe(AudioManagerSingleton.Audio.Out);

        balls.Remove(ball);
        Destroy(ball.gameObject);

        if (balls.Count <= 0)
        {
            AudioManagerSingleton.Instance.FadeOutBgm();

            OnGameOver?.Invoke();
        }
    }

    void AddScore(int score, Vector3 position)
    {
        var scoreUp = score * balls.Count;
        scoreDisplay.Score = scoreDisplay.Score + scoreUp;

        var scoreUpView = Instantiate(scoreUpViewPrefab, guideCanvas.transform);
        scoreUpView.Initialize(score, balls.Count, WorldToGuideCanvasLocalPosition(position));
    }

    public void OnVertexClicked(Vertex vertex)
    {
        if (balls == null || balls.Count <= 0)
        {
            return;
        }

        if (selectedVertex == vertex)
        {
            AudioManagerSingleton.Instance.PlaySe(AudioManagerSingleton.Audio.Cancel);
            vertex.ResetColor();
            colorIterator.Next();
            selectedVertex = null;
        }
        else if (selectedVertex == null)
        {
            AudioManagerSingleton.Instance.PlaySe(AudioManagerSingleton.Audio.Select);
            vertex.SetColor(colorIterator.Current);
            selectedVertex = vertex;
        }
        else
        {
            AudioManagerSingleton.Instance.PlaySe(AudioManagerSingleton.Audio.Wall);
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

    float FirstBallStartForce()
    {
        var force = titleConstData.NormalStartForce;
        if (level == Level.Expert)
        {
            force = titleConstData.ExpertStartForce;
        }
        return force;
    }

    float AdditionalBallStartForce()
    {
        var force = titleConstData.NormalStartForceBaseNewBall + titleConstData.NormalStartForceAdditionalPerNewBall * gotOneUpCount;
        if (level == Level.Expert)
        {
            force = titleConstData.ExpertStartForceBaseNewBall + titleConstData.ExpertStartForceAdditionalPerNewBall * gotOneUpCount;
        }
        return force;
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
        coin.Initialize(this, NewItemsPosition());
    }

    void CreateNewOneUp()
    {
        if (oneUp != null)
        {
            return;
        }

        oneUp = Instantiate(oneUpPrefab);
        oneUp.Initialize(this, NewItemsPosition());
    }

    Vector3 NewItemsPosition()
    {
        Vector3 position;
        var topPosition = topVertexTransform.localPosition;
        do
        {
            var rate = Random.Range(0f, 0.8f);
            var angle = Random.Range(0f, 360f);
            position = Quaternion.Euler(0f, 0f, angle) * topPosition * rate;
        } while (balls.Any(_ball => Vector3.Distance(position, _ball.transform.localPosition) < 2f));

        return position;
    }

    Vector2 WorldToGuideCanvasLocalPosition(Vector3 position)
    {
        Vector2 localPosition;
        var screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            guideCanvas.GetComponent<RectTransform>(), screenPosition, Camera.main, out localPosition);
        return localPosition;
    }
}
