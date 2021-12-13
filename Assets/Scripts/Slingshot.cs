using System.Collections;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static private Slingshot S;

    // ����, ��������������� � ����������
    [Header("Set in Inspector")]
    public GameObject PrefubProjectule;
    public float VelocityMult = 8f;

    // ����, ��������������� ����������
    [Header("Set Dynamically")]
    public GameObject LaunchPoint;
    public Vector3 LaunchPos;
    public GameObject Projectile;
    public bool AimingMode;

    private Rigidbody _projectileRigidbody;

    private void Awake()
    {
        S = this;
        Transform launchPointTrans = transform.Find("LaunchPoint");
        LaunchPoint = launchPointTrans.gameObject;
        LaunchPoint.SetActive(false);
        LaunchPos = launchPointTrans.position;
    }

    private void OnMouseEnter()
    {
        //Debug.Log("Slingshot: OnMouseEnter()");
        LaunchPoint.SetActive(true);
    }

    private void OnMouseExit()
    {
        //Debug.Log("Slingshot: OnMouseExit()");
        LaunchPoint.SetActive(false);
    }

    private void OnMouseDown()
    {
        // ����� ����� ������ ����, ����� ��������� ��������� ��� ��������
        AimingMode = true;
        // ������� ������
        Projectile = Instantiate(PrefubProjectule) as GameObject;
        // ��������� ����� � launchPoint
        Projectile.transform.position = LaunchPos;
        // ������� ��� ��������������
        _projectileRigidbody = Projectile.GetComponent<Rigidbody>();
        _projectileRigidbody.isKinematic = true;
    }

    private void Update()
    {
        // ���� ������� �� � ������ ������������, �� ��������� ���� ���
        if (!AimingMode)
        {
            return;
        }

        // �������� ������� �������� ���������� ��������� ����
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        // ����� �������� ��������� ����� LaunchPos � mousePos3D
        Vector3 mouseDelta = mousePos3D - LaunchPos;
        // ���������� mouseDelta �������� ���������� ������� Slingshot
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude >maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        // ����������� ������ � ����� �������
        Vector3 projPos = LaunchPos + mouseDelta;
        Projectile.transform.position = projPos;
        if (Input.GetMouseButtonUp(0))
        {
            // ������ ���� ��������
            AimingMode = false;
            _projectileRigidbody.isKinematic = false;
            _projectileRigidbody.velocity = -mouseDelta * VelocityMult;
            FollowCam.POI = Projectile;
            MissionDemolition.ShotFired();
            ProjectileLine.S.PointOfInterest = Projectile;
            Projectile = null;
        }
    }

    static public Vector3 LAUNCH_POS
    {
        get
        {
            if (S == null)
            {
                return Vector3.zero;
            }
            return S.LaunchPos;
        }
    }
}
