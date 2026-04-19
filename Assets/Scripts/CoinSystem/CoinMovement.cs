using DG.Tweening;
using UnityEngine;

public class CoinMovement : MonoBehaviour
{
    [Header("Floating")]
    [SerializeField] float floatAmplitude = 0.2f;
    [SerializeField] float floatDuration = 1.5f;

    private Tween floatTween;

    void Start()
    {
        StartFloating(transform.localPosition);
    }

    public void StartFloating(Vector3 position)
    {

        floatTween?.Kill();


        transform.localPosition = position;

        floatTween = transform
            .DOLocalMoveY(floatAmplitude, floatDuration)
            .SetRelative()
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void StopFloating()
    {
        floatTween?.Kill();
        floatTween = null;

        transform.localPosition = Vector3.zero;
    }
}
