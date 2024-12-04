﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    public GameTimeManager gameTimeManager;
    public GroundSpawner groundSpawner;

    public float immunityDuration = 2f;
    public SkinnedMeshRenderer playerRenderer;

    private bool isImmune = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && !isImmune)
        {
            Debug.Log("Player hit an obstacle!");

            // Trigger game slow-down
            if (gameTimeManager != null)
            {
                gameTimeManager.SlowDownOnCollision();
            }

            // Start immunity period
            StartCoroutine(HandleImmunity());
        }
    }

    private IEnumerator HandleImmunity()
    {
        isImmune = true;

        // Flash animation (or other visual effect)
        StartCoroutine(FlashPlayer());

        // Temporarily disable collisions with obstacles
        Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Obstacle"), true);

        yield return new WaitForSeconds(immunityDuration);

        // Re-enable collisions
        Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Obstacle"), false);

        isImmune = false;
    }

    private IEnumerator FlashPlayer()
    {
        float flashInterval = 0.2f;
        float elapsedTime = 0f;

        while (elapsedTime < immunityDuration)
        {
            // Alternate between the normal color and a warning color
            playerRenderer.material.color = Color.red;
            yield return new WaitForSeconds(flashInterval / 2);
            playerRenderer.material.color = Color.white;
            yield return new WaitForSeconds(flashInterval / 2);

            elapsedTime += flashInterval;
        }

        // Reset to original color
        playerRenderer.material.color = Color.white;
    }
}