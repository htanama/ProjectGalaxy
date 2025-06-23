/*
    Author: Juan Contreras
    Edited by:
    Date Created: 01/22/2025
    Date Updated: 01/22/2025
    Description: Tells the enemies what target to aim at based on layer
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    [Header("TARGETTING SETTINGS")]
    [SerializeField] float detectionRadius = 10f;       //how far to look
    [SerializeField] LayerMask targetMask;              //who to target

    Transform currentTarget;                            //reference to current target

    public Transform CurrentTarget => currentTarget;    //public getter
    public float DetectionRadius => detectionRadius;    //public getter

    // Update is called once per frame
    void Update()
    {
        //AimAtTarget();
    }

    //aim at targets in the detection radius            CALL IN UPDATE
    public void AimAtTarget()
    {
        //finds targets matching the layer mask and in the radius   (future proof for multiplayer)
        Collider[] targets = Physics.OverlapSphere(transform.position , detectionRadius, targetMask);

        if(targets.Length > 0)
        {
            //store closest target
            currentTarget = GetClosestTarget(targets);
        }
        else
        {
            //no targets in range
            currentTarget = null;
        }
    }

    //find the closest target to aim at
    Transform GetClosestTarget(Collider[] targets)
    {
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider target in targets)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if(distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target.transform;
            }
        }

        return closestTarget;
    }
}
