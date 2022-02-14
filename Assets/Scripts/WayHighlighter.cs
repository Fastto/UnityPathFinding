using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class WayHighlighter : MonoBehaviour
{
    public static Queue<KeyValuePair<VisualAction, Cell>> Queue = new Queue<KeyValuePair<VisualAction, Cell>>();

    private static Dictionary<Vector2Int, GameObject> GameObjects = new Dictionary<Vector2Int, GameObject>();

    public GameObject ToVisitPrefab;
    public GameObject VisitingPrefab;
    public GameObject VisitedPrefab;
    public GameObject TargetPrefab;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Queue.Count > 0)
        {
            KeyValuePair<VisualAction, Cell> action = Queue.Dequeue();

            if (GameObjects.ContainsKey(action.Value.Position))
            {
                Destroy(GameObjects[action.Value.Position]);
                GameObjects.Remove(action.Value.Position);
            }

            GameObject prefab;
            switch (action.Key)
            {
                case VisualAction.Visiting:
                    prefab = VisitingPrefab;
                    break;
                case VisualAction.ToVisit:
                    prefab = ToVisitPrefab;
                    break;
                case VisualAction.Target:
                    prefab = TargetPrefab;
                    break;
                case VisualAction.Visited:
                default:
                    prefab = VisitedPrefab;
                    break;
            }

            GameObject item = Instantiate(prefab,
                new Vector3(action.Value.Position.x, .05f, action.Value.Position.y), Quaternion.identity);


            item.GetComponentInChildren<InfoLabel>().Distance = SceneController.Instance.isShowStepNumbers
                ? action.Value.Distance.ToString()
                : "";
            item.GetComponentInChildren<InfoLabel>().DistanceLeft = SceneController.Instance.isShowDistance
                ? (Math.Round(action.Value.DistanceLeft, 1)).ToString()
                : "";

            GameObjects.Add(action.Value.Position, item);
        }
    }

    public static void New()
    {
        Queue.Clear();
        foreach (GameObject item in GameObjects.Values)
        {
            Destroy(item);
        }

        GameObjects.Clear();
    }

    public static bool isVisualising
    {
        get { return Queue.Count > 0; }
    }
}