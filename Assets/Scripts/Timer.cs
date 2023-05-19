using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private UnityEvent<double> elapsedChanged;

    public double Elapsed { get; private set; }

    private void Update()
    {
        Elapsed += Time.deltaTime;
        elapsedChanged.Invoke(Elapsed);
    }
}