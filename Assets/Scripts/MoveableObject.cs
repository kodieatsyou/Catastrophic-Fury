using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    public void ScootLeft()
    {
        //StartCoroutine(ScootCoroutine(transform.position + (-PlayerController.instance.gameObject.transform.right * PlayerController.instance.hitPower), 0.2f));
        GetComponent<Rigidbody>().AddForce(-PlayerController.instance.gameObject.transform.right * PlayerController.instance.hitPower, ForceMode.Impulse);
    }

    public void ScootRight()
    {
        //StartCoroutine(ScootCoroutine(transform.position + (PlayerController.instance.gameObject.transform.right * PlayerController.instance.hitPower), 0.2f));
        GetComponent<Rigidbody>().AddForce(PlayerController.instance.gameObject.transform.right * PlayerController.instance.hitPower, ForceMode.Impulse);
    }

    private IEnumerator ScootCoroutine(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Calculate the interpolation factor based on elapsed time and duration
            float t = elapsedTime / duration;

            // Interpolate between start and target positions
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, t);

            // Check for collisions before moving
            if (!CheckCollision(startPosition, newPosition))
            {
                transform.position = newPosition;
            } else
            {
                yield return null;
            }

            // Increment elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the object reaches the exact target position
        transform.position = targetPosition;
    }

    private bool CheckCollision(Vector3 startPos, Vector3 endPos)
    {
        RaycastHit hit;
        if (Physics.Linecast(startPos, endPos, out hit))
        {
            if (hit.collider.gameObject != gameObject)
            {
                // If the linecast hits something other than itself, there is a collision
                return true;
            }
        }
        return false;
    }
}
