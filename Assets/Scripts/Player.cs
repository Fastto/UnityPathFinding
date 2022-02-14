using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Cell _nextCell;

    private Stack<Cell> _way;

    private float _progress = -.1f;
    private Vector3 _positionTo;
    private Vector3 _positionFrom;

    private Animator _animator;

    private void Start()
    {
        _way = new Stack<Cell>();
    }

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.Z) && !Input.GetKey(KeyCode.X) && !Input.GetKey(KeyCode.B))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 50, 1 << 7))
            {
                SetNewGoal(hit.collider.gameObject);
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _animator.Play("Die");
        }

        if (SceneController.Instance.isStayAtPlace)
        {
            return;
        }


        if (_way.Count > 0 && (_progress >= 1f || _progress < 0f) && (!SceneController.Instance.isWaitVisualisation || !WayHighlighter.isVisualising))
        {
            Cell nextStep = _way.Pop();
            _progress = 0f;
            _positionFrom = transform.position;
            _positionTo = new Vector3(nextStep.Position.x, 0, nextStep.Position.y);
            transform.LookAt(_positionTo);

            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Run In Place"))
            {
                _animator.Play("Run In Place");
            }
            //Debug.Log("new goal " + _positionTo);
        }

        if (_progress >= 0)
        {
            transform.position = Vector3.Lerp(_positionFrom, _positionTo, _progress > 1f ? 1f : _progress);
            _progress += .1f;
        }

        if (_progress > 1 && _way.Count == 0)
        {
            _animator.Play("Idle");
            _progress = -.1f;
        }
    }

    private void SetNewGoal(GameObject newGoal)
    {
        Cell target = PathFinder.Search(
            new Cell(new Vector2Int(
                (int)Math.Round(transform.position.x),
                (int)Math.Round(transform.position.z)
            ))
            , new Cell(new Vector2Int(
                (int)Math.Round(newGoal.transform.position.x),
                (int)Math.Round(newGoal.transform.position.z)
            ))
            ,SceneController.Instance.SearchType);

        if (target != null)
        {
            _way.Clear();
            
            while (target.Parent != null)
            {
                _way.Push(target);
                target = target.Parent;
            }

            if (SceneController.Instance.isVisualiseRoute)
            {
                foreach (var item in _way)
                {
                    WayHighlighter.Queue.Enqueue(new KeyValuePair<VisualAction, Cell>(VisualAction.Target, item));
                }
            }
        }
    }
}