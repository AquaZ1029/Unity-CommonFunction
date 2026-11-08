using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float drainSpeed = 10f;
    [SerializeField] private Color barColor = Color.red;

    public UnityEvent onHealthZero;

    private float currentHealth;
    private bool isPaused;
    private Coroutine drainCoroutine;

    private void Awake()
    {
        ApplyColor();
        ResetHealth();
    }

    private void OnValidate()
    {
        ApplyColor();
    }

    [ContextMenu("Start Drain")]
    public void StartDrain()
    {
        if (currentHealth <= 0f || drainCoroutine != null) return;
        isPaused = false;
        drainCoroutine = StartCoroutine(DrainRoutine());
    }

    [ContextMenu("Pause Drain")]
    public void PauseDrain()
    {
        isPaused = true;
    }

    [ContextMenu("Resume Drain")]
    public void ResumeDrain()
    {
        isPaused = false;
    }

    [ContextMenu("Stop Drain")]
    public void StopDrain()
    {
        if (drainCoroutine != null)
        {
            StopCoroutine(drainCoroutine);
            drainCoroutine = null;
        }
        isPaused = false;
    }

    [ContextMenu("Reset Health")]
    public void ResetHealth()
    {
        StopDrain();
        currentHealth = maxHealth;
        UpdateFill();
    }

    private IEnumerator DrainRoutine()
    {
        while (currentHealth > 0f)
        {
            if (!isPaused)
            {
                currentHealth = Mathf.Max(0f, currentHealth - drainSpeed * Time.deltaTime);
                UpdateFill();

                if (currentHealth <= 0f)
                {
                    drainCoroutine = null;
                    onHealthZero?.Invoke();
                    yield break;
                }
            }
            yield return null;
        }
    }

    private void UpdateFill()
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = maxHealth > 0f ? currentHealth / maxHealth : 0f;
        }
    }

    private void ApplyColor()
    {
        if (fillImage != null)
        {
            fillImage.color = barColor;
        }
    }
}