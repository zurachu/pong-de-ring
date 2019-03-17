using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class InGame : MonoBehaviour
{
    public System.Action OnGameOver;
    public int Score => scoreDisplay.Score;

    static class Audio
    {
        public static readonly string Bgm = "Audios/beat0203";
        public static readonly string Select = "Audios/button17";
        public static readonly string Cancel = "Audios/button14";
        public static readonly string Wall = "Audios/button32";
        public static readonly string Bound = "Audios/button69";
        public static readonly string Coin = "Audios/button70";
        public static readonly string OneUp = "Audios/button25";
        public static readonly string Out = "Audios/button24";
    }

    [SerializeField] Transform topVertexTransform;
    [SerializeField] Vertex vertexPrefab;
    [SerializeField] Ball ballPrefab;
    [SerializeField] Wall wallPrefab;
    [SerializeField] Coin coinPrefab;
    [SerializeField] OneUp oneUpPrefab;
    [SerializeField] InGameKeyAssignmentGuide keyAssignmentGuidePrefab;
    [SerializeField] int numVertexes;
    [SerializeField] int boundCountToOneUpBonus;
    [SerializeField] Canvas guideCanvas;
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
    OneUp oneUp;

    int gotCoinCount;
    int gotOneUpCount;

    AudioClip selectAudio;
    AudioClip cancelAudio;
    AudioClip wallAudio;
    AudioClip boundAudio;
    AudioClip coinAudio;
    AudioClip oneUpAudio;
    AudioClip outAudio;

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

        audioSource.clip = Resources.Load<AudioClip>(Audio.Bgm);
        selectAudio = Resources.Load<AudioClip>(Audio.Select);
        cancelAudio = Resources.Load<AudioClip>(Audio.Cancel);
        wallAudio = Resources.Load<AudioClip>(Audio.Wall);
        boundAudio = Resources.Load<AudioClip>(Audio.Bound);
        coinAudio = Resources.Load<AudioClip>(Audio.Coin);
        oneUpAudio = Resources.Load<AudioClip>(Audio.OneUp);
        outAudio = Resources.Load<AudioClip>(Audio.Out);
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
        gotOneUpCount = 0;

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

        startCountdownDisplay.StartCountdown(() => {
            balls[0].StartMove(gotOneUpCount);

            audioSource.Play();
            audioSource.DOFade(1f, 0f);
        });
    }

    public void OnBallHitWall(Ball ball)
    {
        audioSource.PlayOneShot(boundAudio);
        AddScore(1);
        ball.Bound();

        if (coin == null)
        {
            CreateNewCoin();
        }

        if (ball.BoundCount % boundCountToOneUpBonus == 0 && oneUp == null)
        {
            CreateNewOneUp();
        }
    }

    public void OnGetCoin()
    {
        audioSource.PlayOneShot(coinAudio);
        gotCoinCount++;
        AddScore(gotCoinCount * 10);
        coin = null;
    }

    public void OnGetOneUp()
    {
        audioSource.PlayOneShot(oneUpAudio);
        gotOneUpCount++;
        AddNewBall().StartMove(gotOneUpCount);
        oneUp = null;
    }

    public void OnBallOutOfBounds(Ball ball)
    {
        audioSource.PlayOneShot(outAudio);

        balls.Remove(ball);
        Destroy(ball.gameObject);

        if (balls.Count <= 0)
        {
            var seq = DOTween.Sequence();
            seq.SetDelay(1f);
            seq.Append(audioSource.DOFade(0f, 1f));
            seq.Play();

            OnGameOver?.Invoke();
        }
    }

    void AddScore(int score)
    {
        scoreDisplay.Score = scoreDisplay.Score + score * balls.Count;
    }

    public void OnVertexClicked(Vertex vertex)
    {
        if (balls == null || balls.Count <= 0)
        {
            return;
        }

        if (selectedVertex == vertex)
        {
            audioSource.PlayOneShot(cancelAudio);
            vertex.ResetColor();
            colorIterator.Next();
            selectedVertex = null;
        }
        else if (selectedVertex == null)
        {
            audioSource.PlayOneShot(selectAudio);
            vertex.SetColor(colorIterator.Current);
            selectedVertex = vertex;
        }
        else
        {
            audioSource.PlayOneShot(wallAudio);
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
