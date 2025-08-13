using Assets.Scripts.Domain;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

public abstract class ACharacterGO : SerializedMonoBehaviour
{
	private float _baseDeathRadius = .5f;
	[SerializeField] private float _deathAreaRadius = .5f;
    [SerializeField] private SpriteRenderer _spriteRenderer;
	[SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private DeathMaskAnimatorController _deathMaskPrefab;

    public ACharacter Character { get; private set; }
    public SpriteRenderer SpriteRenderer => _spriteRenderer;
    public Rigidbody2D Rigidbody => _rigidbody;
	public event Action OnSetup;
	public bool IsSetup { get; private set; }

	public event Action OnDieAnimationEnded;

    private void Start()
    {
		_baseDeathRadius = _deathAreaRadius;
    }

    public void SetDeathRadius(float radius)
	{
		_deathAreaRadius = radius;
	}

	public void ResetDeathRadius()
	{
		_deathAreaRadius = _baseDeathRadius;
	}

    public virtual void Setup(ACharacter character)
	{
		Character = character;
		Character.OnDie += OnDie;
		IsSetup = true;
    }

	protected virtual void OnDie(ACharacter character)
	{
		Character.OnDie -= OnDie;
		ZoneManager.Instance.AddZone(new CircleZone(transform.position, _deathAreaRadius));
		StartCoroutine(TriggerOnDieAnimation(.3f));
	}

    private IEnumerator TriggerOnDieAnimation(float delayInSeconds)
    {
		yield return new WaitForSeconds(delayInSeconds);
        DeathMaskAnimatorController deathMask = Instantiate(_deathMaskPrefab, transform.position, Quaternion.identity);
        OnDieAnimationEnded?.Invoke();
		Destroy(gameObject);
    }

    protected void TriggerOnSetup() => OnSetup?.Invoke();
}
