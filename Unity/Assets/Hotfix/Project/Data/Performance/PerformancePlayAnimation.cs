using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

namespace ETHotfix
{
    public class PerformancePlayAnimation : PerformanceBase
    {
        public string NoWaitAnimation;
        public string DelayStart;
        public string AnimatorID;
        public string AnimationClipID;
        public string DelayEnd;


        private int _layer;
        private int _Layer
        {
            get
            {
                if (_layer < 0)
                {
                    _layer = 0;
                }
                return _layer;
            }
            set
            {
                _layer = value;
            }
        }


        public bool Loop;

        private Animator Ani;
        private PlayableDirector director;
        public override void Init()
        {
            var go = SceneUnitHelper.Get(AnimatorID);
            director = GetDirector(go);
            Ani = GetAnimator(go);
            if (Ani != null)
                _Layer = Ani.GetLayerIndex(AnimationClipID);
        }

        public override void Jump()
        {
            if (director != null)
            {
                director.Pause();
                director.time = director.duration;
                director.Evaluate();
            }
            else if (Ani != null)
            {
                int layer = Ani.GetLayerIndex(AnimationClipID);
                if (Loop)
                {
                    Ani.Play("Idle", layer);
                }
                else
                {
                    Ani.Play(AnimationClipID, layer, 1);
                }
            }
        }

        public Animator GetAnimator(GameObject target)
        {
            if (target == null)
            {
                target = SceneUnitHelper.Get(AnimatorID);
            }
            if (target == null)
            {
                Debug.LogError($"找不到对象:{AnimatorID}");
                return null;
            }
            Animator Ani = target.GetComponentInChildren<Animator>();
            return Ani;
        }

        public PlayableDirector GetDirector(GameObject target)
        {
            if (target == null)
            {
                target = SceneUnitHelper.Get(AnimatorID);
            }
            if (target == null)
            {
                Debug.LogError($"找不到对象:{AnimatorID}");
                return null;
            }
            PlayableDirector director = target.GetComponentInChildren<PlayableDirector>();
            return director;
        }


        public override void Reset()
        {
            if (director != null)
            {
                director.Pause();
                director.time = 0;
            }
            else if (Ani != null)
            {
                Ani.Play("Idle", _Layer);
            }
        }

        public override void Stop()
        {
            if (director != null)
            {
                director.Pause();
                director.time = 0;
            }
            else if (Ani != null)
            {
                Ani.Play("Idle", _Layer);
            }
        }

        public override IEnumerator StartExecute()
        {
            if (director != null)
            {
                switch (AnimationClipID)
                {
                    case "Play":
                        director.time = 0;
                        director.Play();
                        float time = (float)director.duration;
                        if (!double.IsInfinity(time))
                        {
                            yield return new WaitForSeconds(time);
                        }
                        break;
                    case "Back":
                        director.Pause();
                        director.time = 0;
                        director.Evaluate();
                        break;
                }
            }
            else
            {
                Ani.enabled = true;
                Ani.Play(AnimationClipID, _Layer, 0);
                if (bool.Parse(NoWaitAnimation))
                {
                    yield break;
                }
                yield return new WaitForSeconds(0.1f);
                yield return new WaitForSeconds(float.Parse(DelayStart));
                float time = Ani.GetCurrentAnimatorStateInfo(_Layer).length;
                yield return new WaitForSeconds(time);
                yield return new WaitForSeconds(float.Parse(DelayEnd));
            }
        }
    }
}
