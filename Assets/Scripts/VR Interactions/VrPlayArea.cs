﻿using UnityEngine;

public class VrPlayArea : MonoBehaviour
{

    [SerializeField] Collider[] excludeAreas;

    [SerializeField] bool pickUpChildrenColliders = false;
    [SerializeField] bool canUse = true;

    Collider[] colliders;

    private void Awake()
    {
        if (pickUpChildrenColliders)
        {
            colliders = GetComponentsInChildren<Collider>();
        }
        else
        {
            colliders = GetComponents<Collider>();
        }


#if UNITY_EDITOR
            if (colliders.Length == 0)
        {
            Debug.LogError("No colliders given in this play area!", gameObject);
        }
#endif
    }

    public void SetUsable(bool canUse)
    {
        this.canUse = canUse;
    }

    public bool IsDestinationPointValid(Vector3 position)
    {
        if (!canUse)
            return false;

        if (excludeAreas != null)
        {
            foreach (var collider in excludeAreas)
            {
                if (!collider || !collider.gameObject.activeInHierarchy || !collider.enabled)
                    continue;

                position.y = collider.bounds.center.y;

                if (VectorUtils.IsPointWithinCollider(collider, position))
                    return false;
            }
        }

        foreach (var collider in colliders)
        {
            position.y = collider.bounds.center.y;

            if (VectorUtils.IsPointWithinCollider(collider, position))
                return true;
        }

        return false;
    }
}
