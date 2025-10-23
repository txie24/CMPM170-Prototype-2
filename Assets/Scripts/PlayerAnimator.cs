using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("FX Prefabs")]
    [SerializeField] private GameObject jumpFX;
    [SerializeField] private GameObject landFX;

    [Header("Animator")]
    [SerializeField] private Animator anim;

    [HideInInspector] public bool startedJumping;
    [HideInInspector] public bool justLanded;

    private SpriteRenderer spriteRend;

    void Awake()
    {
        if (anim == null) anim = GetComponentInChildren<Animator>();
        spriteRend = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (startedJumping)
        {
            if (anim) anim.SetTrigger("Jump");
            SpawnFX(jumpFX);
            startedJumping = false;
            return;
        }

        if (justLanded)
        {
            if (anim) anim.SetTrigger("Land");
            SpawnFX(landFX);
            justLanded = false;
            return;
        }
    }

    private void SpawnFX(GameObject prefab, float lifetime = 1f, float padding = 0.02f)
    {
        if (prefab == null || spriteRend == null) return;

        Vector3 pos = BottomOfSprite(padding);
        Quaternion rot = ParticleRotationAgainstGravity();

        GameObject fx = Instantiate(prefab, pos, rot);
        Destroy(fx, lifetime);
    }

    private Vector3 BottomOfSprite(float padding = 0.02f)
    {
        Vector2 g = Physics2D.gravity;
        Vector2 down = g.sqrMagnitude > 0f ? g.normalized : Vector2.down;

        var b = spriteRend.bounds;
        var ext = b.extents;

        float halfAlongDown = Mathf.Abs(down.x) * ext.x + Mathf.Abs(down.y) * ext.y;

        return b.center + (Vector3)down * (halfAlongDown + padding);
    }

    private Quaternion ParticleRotationAgainstGravity()
    {
        Vector2 g = Physics2D.gravity;
        if (g.sqrMagnitude <= 0f) return Quaternion.identity;

        Vector3 againstGravity = -(Vector3)g.normalized;
        return Quaternion.FromToRotation(Vector3.up, againstGravity);
    }
}
