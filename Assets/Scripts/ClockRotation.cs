using UnityEngine;

public class ClockRotation : MonoBehaviour
{
    public Transform arrowPoint;
    private Transform startPos;

    private void Start()
    {
        startPos = arrowPoint;
    }

    public void SetRotation(float seconds)
    {
        arrowPoint.rotation = startPos.rotation;
        arrowPoint.Rotate(0, seconds * 6, 0);
    }

}
