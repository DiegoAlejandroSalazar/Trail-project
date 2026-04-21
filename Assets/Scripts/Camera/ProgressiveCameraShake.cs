using DG.Tweening;
using UnityEngine;

public class ProgressiveCameraShake : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] private float _baseAmplitude = 0.05f;
    [SerializeField] private float _baseFrequency = 1.5f;

    [Header("Intensity Multipliers")]
    [SerializeField] private float _easyMult = 1f;
    [SerializeField] private float _mediumMult = 1.5f;
    [SerializeField] private float _hardMult = 2.2f;
    [SerializeField] private float _insaneMult = 3f;

    private float _currentIntensity = 1f;
    private float _targetIntensity = 1f;

    private Vector3 _startPos;
    private float _noiseOffset;

    void Start()
    {
        _startPos = transform.localPosition;
        _noiseOffset = Random.Range(0f, 999f);
    }

    void Update()
    {
        // Smooth transition
        _currentIntensity = Mathf.Lerp(_currentIntensity, _targetIntensity, Time.deltaTime * 2f);

        float time = Time.time * _baseFrequency;

        float x = (Mathf.PerlinNoise(time, _noiseOffset) - 0.5f) * _baseAmplitude * _currentIntensity;
        float y = (Mathf.PerlinNoise(_noiseOffset, time) - 0.5f) * _baseAmplitude * _currentIntensity;

        transform.localPosition = _startPos + new Vector3(x, y, 0);
    }

    private void SetDifficultyIntensity(float multiplier)
    {
        // Smooth DOTween transition
        DOTween.To(() => _targetIntensity, x => _targetIntensity = x, multiplier, 0.5f)
               .SetEase(Ease.OutQuad);
    }
    public void UpdateIntensityFromDifficulty()
    {
        switch (GameManager.Instance.GetDifficultyTier())
        {
            case GameManager.DifficultyTier.Easy:
                SetDifficultyIntensity(_easyMult);
                break;

            case GameManager.DifficultyTier.Medium:
                SetDifficultyIntensity(_mediumMult);
                break;

            case GameManager.DifficultyTier.Hard:
                SetDifficultyIntensity(_hardMult);
                break;

            case GameManager.DifficultyTier.Insane:
                SetDifficultyIntensity(_insaneMult);
                break;
        }
    }

}
