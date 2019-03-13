using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame : MonoBehaviour
{
    [SerializeField] Rigidbody2D ball;
    [SerializeField] Transform topVertexTransform;
    [SerializeField] Vertex vertexPrefab;
    [SerializeField] Wall wallPrefab;
    [SerializeField] int numVertexes;
    [SerializeField] List<InGameKeyAssignment> keyAssignments;
    [SerializeField] ColorIterator colorIterator;
    [SerializeField] float startForce;

    List<Vertex> vertexes;
    Vertex selectedVertex;
    Wall wall;

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
        var vec = new Vector2(startForce, 0f);
        vec = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)) * vec;
        ball.AddForce(vec, ForceMode2D.Impulse);
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

    void CreateNewWall(Vertex v1, Vertex v2)
    {
        var color = colorIterator.Current;

        if (wall == null)
        {   // 使い回す
            wall = Instantiate(wallPrefab);
        }

        var position1 = v1.transform.localPosition;
        var position2 = v2.transform.localPosition;
        wall.transform.localPosition = (position1 + position2) / 2;

        var diff = position2 - position1;
        var rad = Mathf.Atan2(diff.y, diff.x);
        wall.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg *rad);

        wall.SetColor(color);

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
}
