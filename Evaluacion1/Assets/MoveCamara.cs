using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamara : MonoBehaviour
{
    [SerializeField] Transform _thisObject;
    private Vector3 _direction;
    Camera _cam;
    private float _speed = 6f;
    private float _zoomSpeed = 25f;
    private float _maxZoom = 10f;
    private float _minZoom = 2f;
    void Start()
    {
        _cam = gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        _direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.Translate(_direction * _speed * Time.deltaTime);

        if (Input.mouseScrollDelta.y != 0)
        {
            float zoomDelta = (Input.mouseScrollDelta.y * (-1)) * _zoomSpeed * Time.deltaTime;

            float nextZoom = _cam.orthographicSize + zoomDelta;
            _cam.orthographicSize = Mathf.Clamp(nextZoom, _minZoom, _maxZoom);
        }
    }
}
