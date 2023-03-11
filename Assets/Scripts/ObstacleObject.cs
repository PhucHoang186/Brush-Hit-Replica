using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleObject : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] ParticleSystem hitParticle;
    [SerializeField] float turnSpeed = 200f;
    Vector3 closestPoint;
    bool isChangeColor;

    private void OnTriggerEnter(Collider collider)
    {
        if (GameManager.Instance.currentGameState != GameState.Running && GameManager.Instance.currentGameState != GameState.Frenzy)
            return;

        hitParticle.Play();
        ChangeColor();
    }

    private void ChangeColor()
    {
        if (isChangeColor) return;
        isChangeColor = true;
        meshRenderer.material = PlayerController.Instance.GetCollideMat();
        UIManager.Instance.scoreUIPanel.Score++;
    }

    private void OnTriggerStay(Collider collider)
    {
        Swaying(collider);
    }

    private void OnTriggerExit(Collider collider)
    {
        SwayingBack(collider);
    }

    private void Swaying(Collider collider)
    {
        // closestPoint = collider.transform.position;
        closestPoint = collider.ClosestPoint(transform.position);
        closestPoint.y += 5f;
        Vector3 facing = transform.position - closestPoint;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(facing), turnSpeed * Time.deltaTime);
    }

    private void SwayingBack(Collider collider)
    {
        closestPoint = collider.ClosestPoint(this.transform.position);
        Vector3 facing = transform.position - closestPoint;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, turnSpeed / 3 * Time.deltaTime);
    }
}
