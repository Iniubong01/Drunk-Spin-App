using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class DS_ButtonShrinkEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 originalScale;
    public float shrinkAmount = 0.9f; // Adjust for desired shrink level
    public float animationDuration = 0.1f;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleButton(originalScale * shrinkAmount));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleButton(originalScale));
    }

    private IEnumerator ScaleButton(Vector3 targetScale)
    {
        float time = 0;
        Vector3 initialScale = transform.localScale;

        while (time < animationDuration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, time / animationDuration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }
}
