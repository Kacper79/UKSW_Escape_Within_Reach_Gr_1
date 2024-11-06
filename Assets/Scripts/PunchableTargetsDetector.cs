using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchableTargetsDetector : MonoBehaviour
{
    private const float LOOK_FOR_PUNCHABLE_ENEMIES_MAX_DISTANCE = 3.0f;//has to be lass than z value in detector_raycast_direction.position

    [SerializeField] private Transform detector_raycast_origin;
    [SerializeField] private Transform detector_raycast_direction;

    private Vector3 ray_origin;
    private Vector3 ray_direction;

    public void TryDamagingEnemies(int damage)
    {
        ray_origin = detector_raycast_origin.position;
        ray_direction = detector_raycast_direction.position - detector_raycast_origin.position;

        RaycastHit[] all_hits = Physics.RaycastAll(ray_origin, ray_direction, LOOK_FOR_PUNCHABLE_ENEMIES_MAX_DISTANCE);
        
        foreach(RaycastHit hit in all_hits)
        {
            if (hit.collider != null && hit.collider.TryGetComponent<IAttackable>(out IAttackable attackable))
            {
                attackable.Punched(damage, transform.position);

                return;
            }
        }
    }
}
