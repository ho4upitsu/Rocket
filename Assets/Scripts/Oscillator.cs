using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] private Vector3 movementVector;
    private float _movementFactor;
    [SerializeField] private float period = 1f;

    private Vector3 _startingPosition;
    void Start()
    {
        _startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) { return; }
        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau);
        _movementFactor = rawSinWave / 2f + 0.5f;
        Vector3 offset = _movementFactor * movementVector;
        transform.position = _startingPosition + offset;
    }
}
