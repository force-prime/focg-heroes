using System;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    private float _dragThreshold = 10;
    private float _clickThresholdS = 0.2f;

    private bool _mouseDown;
    private float _startTime;
    private IMouseEvents _currentUnderMouse;
    private Vector3 _currentUnderStartPos;
    private bool _currentInDrag;
    private IMouseEvents _defaultHandler;


    private readonly RaycastHit2D[] _results = new RaycastHit2D[10];

    static public MouseController Current { get; private set; }

    void Awake()
    {
        Current = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (_mouseDown)
        {
            if (!Input.GetMouseButton(0))
            {
                HandleMouseUp();
            }
            else
                HandleMouseHold();
        }
        else if (Input.GetMouseButton(0))
        {
            HandleMouseDown();
        }
    }

    private void HandleMouseHold()
    {
        var delta = GetMouseScreenPos() - _currentUnderStartPos;
        _currentInDrag |= delta.sqrMagnitude > _dragThreshold * _dragThreshold;
        _currentUnderMouse?.HandleMouseHold(delta, _currentInDrag);
    }

    private void HandleMouseUp()
    {
        if (_currentUnderMouse != null)
        {
            var delta = GetMouseScreenPos() - _currentUnderStartPos;

            bool needCallMouseUp = true;
            if (!_currentInDrag && (Time.unscaledTime - _startTime) < _clickThresholdS)
            {
                needCallMouseUp = _currentUnderMouse.HandleClick();
            }
            if (needCallMouseUp)
                _currentUnderMouse.HandleMouseUp(delta, _currentInDrag);
        }
        _mouseDown = false;
        _currentUnderMouse = null;
        _currentInDrag = false;
    }

    private void HandleMouseDown()
    {
        _mouseDown = true;
        _startTime = Time.unscaledTime;
        _currentUnderMouse = GetUnderMouse<IMouseEvents>() ?? _defaultHandler;
        _currentUnderStartPos = GetMouseScreenPos();
        _currentUnderMouse?.HandleMouseDown();
    }

    static public Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = GetMouseScreenPos();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        return worldPos;
    }

    public void SetDefaultHandler(IMouseEvents handler)
    {
        _defaultHandler = handler;
    }

    public T? GetUnderMouse<T>(Predicate<T>? filter = null)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var count = Physics2D.GetRayIntersectionNonAlloc(ray, _results);
        for (int i = 0; i < count; i++)
        {
            var r = _results[i];
            var b = r.transform.GetComponent<T>();
            if (b != null && (filter == null || filter(b)))
                return b;
        }
        return default(T?);
    }

    static private Vector3 GetMouseScreenPos() => new Vector3(Input.mousePosition.x, Input.mousePosition.y);

    public interface IMouseEvents
    {
        bool HandleClick();
        void HandleMouseUp(Vector3 delta, bool inDrag);
        void HandleMouseDown();
        void HandleMouseHold(Vector3 delta, bool inDrag);
    }
}
