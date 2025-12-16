using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private List<float> weights;
    public Transform spawn_point;
    public List<Transform> details;
    

    public int get_object_num()
    {
        float rand = Random.value;
        for (int i = 0; i < weights.Count; i++)
        {
            rand -= weights[i];
            if (rand <= 0)
            {
                return i;
            }
        }
        return weights.Count - 1;
    }

    public void spawn_detail(List<float> probs)
    {
        weights = probs;
        var detail_num = get_object_num();
        Instantiate(details[detail_num], spawn_point.position, Quaternion.identity);

    }

}
