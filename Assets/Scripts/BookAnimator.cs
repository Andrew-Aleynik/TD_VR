using UnityEngine;

public class BookAnimator : MonoBehaviour
{
    [Header("Breathing Animation")]
    [SerializeField] private float breathingSpeed = 2f;
    [SerializeField] private float breathingAmount = 0.1f;

    [Header("Levitation Animation")]
    [SerializeField] private float levitationSpeed = 0.8f;
    [SerializeField] private float levitationHeight = 0.1f;
    
    private Vector3 baseScale;
    private Vector3 startPosition;
    private bool isAnimating = false;
    private float animationTime = 0.0f;
    private Transform cachedTransform;

    void Awake()
    {
        cachedTransform = transform;
        startPosition = cachedTransform.localPosition;
        baseScale = cachedTransform.localScale;
        
        isAnimating = true;
    }

    void Update()
    {   
        if (isAnimating) 
        {
            animationTime = Time.time;
            
            float breathing = Mathf.Sin(animationTime * breathingSpeed) * breathingAmount;
            Vector3 newScale = baseScale;
            newScale.z += breathing;
            cachedTransform.localScale = newScale;

            float levitation = Mathf.Sin(animationTime * levitationSpeed) * levitationHeight;
            Vector3 newPosition = startPosition;
            newPosition.y += levitation;
            cachedTransform.localPosition = newPosition;
        }
    }

    public void SetBaseY(float y) 
    {
        startPosition.y = y;
        
        if (isAnimating)
        {
            float levitation = Mathf.Sin(animationTime * levitationSpeed) * levitationHeight;
            Vector3 newPosition = startPosition;
            newPosition.y += levitation;
            cachedTransform.localPosition = newPosition;
        }
    }

    public void StartAnimation() 
    {
        isAnimating = true;
    }

    public void PauseAnimation() 
    {
        isAnimating = false;
    }

    public void StopAnimation() 
    {
        isAnimating = false;
        cachedTransform.localScale = baseScale;
        cachedTransform.localPosition = startPosition;
        animationTime = 0.0f;
    }
    
    public void ResetToStart()
    {
        cachedTransform.localScale = baseScale;
        cachedTransform.localPosition = startPosition;
    }
}