using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FeelsBadEffect : MonoBehaviour
{
    public static FeelsBadEffect instance;
    
    private PlayerController _player;
    private DialogueManager _dialog;

    [SerializeField] private Volume _pp;
    private ColorAdjustments _color;
    private DepthOfField _depth;
    private LensDistortion _lens;

    [SerializeField] private int _effectIterationTime = 100;
    [SerializeField] private int _effectIterationCount = 4;

    public bool useDialogueOnEnd = false;
    public Dialogue dialogue;

    void Start()
    {
        _pp.enabled = false;
        _pp.profile.TryGet(out _color);
        _pp.profile.TryGet(out _depth);
        _pp.profile.TryGet(out _lens);


        _player = FindObjectOfType<PlayerController>();
        _dialog = FindObjectOfType<DialogueManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _pp.enabled = true;
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(Effect());
        }
    }

    void OnEndEffect()
    {
        _pp.enabled = false;
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    IEnumerator Effect()
    {
        
        int iters = _effectIterationTime;

        _lens.intensity.value = 0.6f;
        float distXmultMax = 1;
        float distXmultMin = -1;
        float distYmultMax = 1;
        float distYmultMin = -1;
        float distScaleMax = 1.1f;
        float distScaleMin = 0.9f;

        float depthMax = 3;
        float depthMin = 1.7f;
        for (int j = 0; j < _effectIterationCount; j++)
        {
            for (int i = 0; i < iters; i++)
            {
                _lens.scale.value = distScaleMin + (distScaleMax - distScaleMin) / iters * (iters - i - 1);
                _lens.xMultiplier.value = distXmultMin + (distXmultMax - distXmultMin) / iters * (iters - i - 1);
                _lens.yMultiplier.value = distYmultMax - (distYmultMax - distYmultMin) / iters * (iters - i - 1);
                _depth.focusDistance.value = depthMax - (depthMax - depthMin) / iters * (iters - i - 1);
                yield return new WaitForFixedUpdate();
            }
            for (int i = 0; i < iters; i++)
            {
                _lens.scale.value = distScaleMax - (distScaleMax - distScaleMin) / iters * (iters - i - 1);
                _lens.xMultiplier.value = distXmultMax - (distXmultMax - distXmultMin) / iters * (iters - i - 1);
                _lens.yMultiplier.value = distYmultMin + (distYmultMax - distYmultMin) / iters * (iters - i - 1);
                _depth.focusDistance.value = depthMin + (depthMax - depthMin) / iters * (iters - i - 1);
                yield return new WaitForFixedUpdate();
            }
        }
        OnEndEffect();
    }
}
