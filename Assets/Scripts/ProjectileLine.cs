using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S; // ��������

    [Header("Set in Inspector")]
    public float MinDist = 0.1f;

    private LineRenderer _lineRenderer;
    private GameObject _pointOfInterest;
    private List<Vector3> _points;

    private void Awake()
    {
        S = this; // ���������� ������ �� ������-��������
        // �������� ������ �� LineRenderer
        _lineRenderer = GetComponent<LineRenderer>();
        // ��������� LineRenderer ���� �� �� �����������
        _lineRenderer.enabled = false;
        // ���������������� ������ �����
        _points = new List<Vector3>();
    }

    private void FixedUpdate()
    {
        if (PointOfInterest == null)
        {
            // ���� �������� PointOfInterest �������� ������ ��������, ����� ������������ ������
            if (FollowCam.POI != null)
            {
                if (FollowCam.POI.tag == "Projectile")
                {
                    PointOfInterest = FollowCam.POI;
                }
                else
                {
                    return; // �����, ���� ������������ ������ �� ������
                }
            }
            else
            {
                return; // �����, ���� ������������ ������ �� ������
            }
        }
        // ���� ������������ ������ ������, ���������� �������� ����� � ��� ������������ � ������ FixedUpdate()
        AddPoint();
        if (FollowCam.POI == null)
        {
            // ���� FollowCam �������� null, �������� null � PointsOfInterest
            PointOfInterest = null;
        }

    }

    // ���� ����� ����� ������� ���������������, ���� ������� �����
    public void Clear()
    {
        _pointOfInterest = null;
        _lineRenderer.enabled = false;
        _points = new List<Vector3>();
    }

    // ���������� ��� ���������� ����� � �����
    public void AddPoint()
    {
        Vector3 pt = _pointOfInterest.transform.position;
        if (_points.Count > 0 && (pt - LastPoint).magnitude < MinDist)
        {
            // ���� ����� ������������ ������ �� ����������, ������ �����
            return;
        }

        if (_points.Count == 0) // ���� ��� ����� �������...
        {
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS; // ��� �����������
            // ...�������� �������������� �������� �����, ����� ������ ����� ����������� � �������
            _points.Add(pt + launchPosDiff);
            _points.Add(pt);
            _lineRenderer.positionCount = 2;
            // ���������� ������ ��� �����
            _lineRenderer.SetPosition(0, _points[0]);
            _lineRenderer.SetPosition(1, _points[1]);
            // �������� LineRenderer
            _lineRenderer.enabled = true;
        }
        else
        {
            // ������� ������������������ ���������� �����
            _points.Add(pt);
            _lineRenderer.positionCount = _points.Count;
            _lineRenderer.SetPosition(_points.Count - 1, LastPoint);
            _lineRenderer.enabled = true;
        }
    }

    // ��� �������� (�.�. �����, ������������� ��� ����)
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
                // ���� ���� _PointsOfInterest �������� �������������� ������, ������� ��� ��������� ��������� � �������� ���������
                _lineRenderer.enabled = false;
                _points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    // ���������� ��������� ��������� ����������� �����
    public Vector3 LastPoint
    {
        get
        {
            if (_points == null)
            {
                // ���� ����� ���, ������� Vector3.zero
                return (Vector3.zero);
            }
            return (_points[_points.Count - 1]);
        }
    }    
}
