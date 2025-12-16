using UnityEngine;

public class ClockRotation : MonoBehaviour
{
    public Transform arrow_point;
    private Transform start_pos;

    private void Start()
    {
        start_pos = arrow_point;
    }

    public void rotate_arrow(float pos)
    {
        arrow_point.rotation = start_pos.rotation;
        arrow_point.Rotate(0, pos * 6, 0);
    }

}
