using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S; // Одиночка

    [Header("Set in Inspector")]
    public float MinDist = 0.1f;

    private LineRenderer _lineRenderer;
    private GameObject _pointOfInterest;
    private List<Vector3> _points;

    private void Awake()
    {
        S = this; // Установить ссылку на объект-одиночку
        // Получить ссылку на LineRenderer
        _lineRenderer = GetComponent<LineRenderer>();
        // Выключить LineRenderer пока он не понадобится
        _lineRenderer.enabled = false;
        // Инициализировать список точек
        _points = new List<Vector3>();
    }

    private void FixedUpdate()
    {
        if (PointOfInterest == null)
        {
            // Если свойство PointOfInterest содержит пустое значение, найти интересующий объект
            if (FollowCam.POI != null)
            {
                if (FollowCam.POI.tag == "Projectile")
                {
                    PointOfInterest = FollowCam.POI;
                }
                else
                {
                    return; // Выйти, если интересующий объект не найден
                }
            }
            else
            {
                return; // Выйти, если интересующий объект не найден
            }
        }
        // Если интересующий объект найден, попытаться добавить точку с его координатами в каждом FixedUpdate()
        AddPoint();
        if (FollowCam.POI == null)
        {
            // Если FollowCam содержит null, записать null в PointsOfInterest
            PointOfInterest = null;
        }

    }

    // Этот метод можно вызвать непосредственно, чтоб стереть линию
    public void Clear()
    {
        _pointOfInterest = null;
        _lineRenderer.enabled = false;
        _points = new List<Vector3>();
    }

    // Вызывается для добавления точки в линию
    public void AddPoint()
    {
        Vector3 pt = _pointOfInterest.transform.position;
        if (_points.Count > 0 && (pt - LastPoint).magnitude < MinDist)
        {
            // Если точка недостаточно далека от предыдущей, просто выйти
            return;
        }

        if (_points.Count == 0) // Если это точка запуска...
        {
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS; // Для определения
            // ...добавить дополнительный фрагмент линии, чтобы помочь лучше прицелиться в будущем
            _points.Add(pt + launchPosDiff);
            _points.Add(pt);
            _lineRenderer.positionCount = 2;
            // Установить первые две точки
            _lineRenderer.SetPosition(0, _points[0]);
            _lineRenderer.SetPosition(1, _points[1]);
            // Включить LineRenderer
            _lineRenderer.enabled = true;
        }
        else
        {
            // Обычная последовательность добавления точки
            _points.Add(pt);
            _lineRenderer.positionCount = _points.Count;
            _lineRenderer.SetPosition(_points.Count - 1, LastPoint);
            _lineRenderer.enabled = true;
        }
    }

    // Это свойство (т.е. метод, маскирующийся под поле)
    public GameObject PointOfInterest
    {
        get
        {
            return _pointOfInterest;
        }
        set
        {
            _pointOfInterest = value;
            if (_pointOfInterest != null)
            {
                // Если поле _PointsOfInterest содержит действительную ссылку, сбрость все остальные параметры в исходное состояние
                _lineRenderer.enabled = false;
                _points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    // Возвращает положение последней добавленной точки
    public Vector3 LastPoint
    {
        get
        {
            if (_points == null)
            {
                // Если точек нет, вернуть Vector3.zero
                return (Vector3.zero);
            }
            return (_points[_points.Count - 1]);
        }
    }    
}
