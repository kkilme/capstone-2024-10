using Fusion;
using UnityEngine;

public abstract class BaseSoundController : NetworkBehaviour
{
    public Creature Creature { get; protected set; }
    public CreatureCamera CreatureCamera => Creature.CreatureCamera;
    public AudioSource CreatureAudioSource => Creature.AudioSource;
    public Define.CreatureState CreatureState => Creature.CreatureState;
    public Define.CreaturePose CreaturePose => Creature.CreaturePose;

    public AudioSource BgmAudioSource => SoundManager._audioSources[(int)Define.SoundType.Bgm];

    public bool IsChasing { get; protected set; } = false;
    public float ChasingDistance { get; protected set; }

    public override void Spawned()
    {
        Init();
    }

    protected virtual void Init()
    {
        Creature = gameObject.GetComponent<Creature>();
    }

    public abstract void PlayMove();

    public void PlayEndSound()
    {
        Managers.SoundMng.Play($"{Define.BGM_PATH}/Panic Man", Define.SoundType.Bgm, volume: 0.7f);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void Rpc_StopEffectSound()
    {
        if (!Creature || !CreatureAudioSource)
            return;

        if (CreatureAudioSource.isPlaying)
            CreatureAudioSource.Stop();
    }

    public void StopAllSound()
    {
        Managers.SoundMng.Stop(Define.SoundType.Bgm);
        Managers.SoundMng.Stop(Define.SoundType.Environment);
        Managers.SoundMng.Stop(Define.SoundType.Effect);
        Managers.SoundMng.Stop(Define.SoundType.Facility);

        Rpc_StopEffectSound();
    }

    #region Chasing

    public abstract void CheckChasing();

    public abstract void UpdateChasingValue();

    #endregion
}
