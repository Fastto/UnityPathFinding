using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;
    
    public GameObject GrassPrefab;
    public GameObject WallPrefab;

    public Toggle SearchToggle;
    public Toggle RouteToggle;
    public Toggle WaitVisualisationToggle;
    public Toggle StayAtPlaceToggle;
    public Toggle ShowStepNumberToggle;
    public Toggle ShowDistanceToToggle;
    public Toggle ShowLinksBetweenToggle;
    public Dropdown SearchTypeDropdown;

    public int WorldWidth = 10;
    public int WorldHeight = 10;
    public float varY = .05f;

    public bool isVisualiseSearch
    {
        get
        {
            return SearchToggle.isOn;
        }
    }
    
    public bool isVisualiseRoute
    {
        get
        {
            return RouteToggle.isOn;
        }
    }
    
    public bool isWaitVisualisation
    {
        get
        {
            return WaitVisualisationToggle.isOn;
        }
    }
    
    public bool isStayAtPlace
    {
        get
        {
            return StayAtPlaceToggle.isOn;
        }
    }
    
    public bool isShowStepNumbers
    {
        get
        {
            return ShowStepNumberToggle.isOn;
        }
    }
    
    public bool isShowDistance
    {
        get
        {
            return ShowDistanceToToggle.isOn;
        }
    }
    
    public bool isShowLinks
    {
        get
        {
            return ShowLinksBetweenToggle.isOn;
        }
    }

    public SearchType SearchType
    {
        get
        {
            return (SearchType)SearchTypeDropdown.value;
        }
    }
    
    private void Start()
    {
        Map.Init(GenerateGround(), WorldWidth, WorldHeight);
    }

    private void Awake()
    {
        Instance = this;
    }

    private List<GameObject> GenerateGround()
    {
        List<GameObject> items = new List<GameObject>();
        for (int x = 0; x < WorldWidth; x++)
        {
            for (int z = 0; z < WorldHeight; z++)
            {
                GameObject item = Instantiate(GrassPrefab, new Vector3(x, Random.Range(-varY,varY), z), Quaternion.identity);
                items.Add(item);
            }
        }
        
        return items; 
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.Z))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                GameObject gameObject = hit.transform.gameObject;
                if (gameObject.tag == "Grass")
                {
                    Vector3 pos = new Vector3((int) gameObject.transform.position.x, 0, (int) gameObject.transform.position.z);
                    Instantiate(WallPrefab, pos, Quaternion.identity);
                    Map._map[(int) gameObject.transform.position.x, (int) gameObject.transform.position.z] = CellType.Wall;
                }
            }
        }
        
        if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.X))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                GameObject gameObject = hit.transform.gameObject;
                if (gameObject.tag == "Wall")
                {
                    Map._map[(int) gameObject.transform.position.x, (int) gameObject.transform.position.z] = CellType.Grass;
                    Destroy(gameObject);
                }
            }
        }
    }
}
