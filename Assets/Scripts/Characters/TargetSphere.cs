using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class TargetSphere : MonoBehaviour
{
    public bool IsInObstacle = false;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(GameConstants.Tags.Obstacle))
        {
            IsInObstacle = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(GameConstants.Tags.Obstacle))
        {
            IsInObstacle = false;
        }
    }
}
