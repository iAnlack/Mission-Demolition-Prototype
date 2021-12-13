using System.Collections;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI; // ������ �� ������������ ������ (POI - point of interest)

    [Header("Set in Inspector")]
    public float Easing = 0.05f;
    public Vector2 MinXY = Vector2.zero;

    [Header("Set in Dynamically")]
    public float CamZ; // �������� ���������� Z ������

    private void Awake()
    {
        CamZ = this.transform.position.z;
    }

    private void FixedUpdate()
    {
        //if (POI == null)
        //{
        //    return; // �����, ���� ��� ������������� �������
        //}

        //// �������� ������� ������������ �������
        //Vector3 destination = POI.transform.position;

        Vector3 destination;
        // ���� ��� ������������� �������, �� ������� P: [0, 0, 0]
        if (POI == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            // �������� ������� ������������� �������
            destination = POI.transform.position;
            // ���� ������������ ������ - ������, ���������, ��� �� �����������
            if (POI.tag == "Projectile")
            {
                // ���� �� ����� �� ����� (�� ���� �� ���������)
                if (POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    // ������� �������� ��������� ���� ������ ������
                    POI = null;
                    // � ��������� �����
                    return;
                }
            }
        }

        // ���������� X � Y ������������ ����������
        destination.x = Mathf.Max(MinXY.x, destination.x);
        destination.y = Mathf.Max(MinXY.y, destination.y);
        // ���������� ����� ����� ������� ��������������� ������ � destination
        destination = Vector3.Lerp(transform.position, destination, Easing);
        // ������������� ���������� �������� destination.z ������ CamZ, ����� ���������� ������ ��������
        destination.z = CamZ;
        // ��������� ������ � ������� destination
        transform.position = destination;
        // �������� ������ orthographicSize ������, ����� ����� ���������� � ���� ������
        Camera.main.orthographicSize = destination.y + 10;
    }
}
