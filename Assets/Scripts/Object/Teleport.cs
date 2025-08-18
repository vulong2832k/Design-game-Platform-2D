using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform _teleportTarget;

    private bool _isTeleporting = false;

    private IEnumerator SwapScaleAndTele(GameObject player)
    {
        _isTeleporting = true;

        Vector3 originalScale = player.transform.localScale;
        Vector3 shrinkScale = originalScale * 0.1f;

        float durationScale = 0.5f;
        float time = 0f;

        while (time < durationScale)
        {
            player.transform.localScale = Vector3.Lerp(originalScale, shrinkScale, time / durationScale);
            time += Time.deltaTime;
            yield return null;
        }
        player.transform.localScale = shrinkScale;

        yield return new WaitForSeconds(2f);

        player.transform.position = _teleportTarget.position;

        time = 0f;
        while (time < durationScale)
        {
            player.transform.localScale = Vector3.Lerp(shrinkScale, originalScale, time / durationScale);
            time += Time.deltaTime;
            yield return null;
        }
        player.transform.localScale = originalScale;
        _isTeleporting = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(SwapScaleAndTele(collision.gameObject));
        }
    }
}
