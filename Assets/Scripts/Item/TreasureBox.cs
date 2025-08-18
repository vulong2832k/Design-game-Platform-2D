using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TreasureBox : MonoBehaviour
{
    private ItemSO _rewardItem;

    [Header("Drop Settings: ")]
    [SerializeField] private GameObject _itemDropPrefab;
    [SerializeField] private Vector3 _dropOffset = new Vector3(0, 1f, 0);

    [Header("Effect Settings")]
    [SerializeField] private float _openDuration = 1f;

    private bool _opened = false;

    public void SetReward(ItemSO item)
    {
        _rewardItem = item;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_opened || !other.CompareTag("Player")) return;

        _opened = true;
        StartCoroutine(OpenAnimationThenDrop());
    }

    private IEnumerator OpenAnimationThenDrop()
    {
        transform.DOPunchScale(Vector3.one * 0.3f, 0.5f, 5, 1).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(_openDuration);

        if (_rewardItem != null && _rewardItem.itemPrefab != null)
        {
            Vector3 spawnPos = transform.position + _dropOffset;
            Instantiate(_rewardItem.itemPrefab, spawnPos, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
