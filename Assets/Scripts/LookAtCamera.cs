using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField]
    private Transform m_Target;
    [SerializeField]
    private float m_Speed;


    void Update()
    {
        Vector3 lTargetDir = transform.position - m_Target.position;
        lTargetDir.y = 0.0f;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lTargetDir), Time.time * m_Speed);
    }
}