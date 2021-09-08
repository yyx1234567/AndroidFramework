using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

namespace ETModel
{
    [Sirenix.OdinInspector.Title("播放动画")]
    [Sirenix.OdinInspector.HideReferenceObjectPicker]
    public class PerformancePlayAnimation : PerformanceBase
    {
        [Sirenix.OdinInspector.LabelText("动画对象")]
        public string TargetID;
        [Sirenix.OdinInspector.LabelText("动画名称")]
        public string AnimationID;
        [Sirenix.OdinInspector.LabelText("是否循环")]
        public bool Loop;

        private GameObject _target;

        public override void Init()
        {
            _target = GameObject.Find(TargetID);
        }

        public override void Jump()
        {
            Animator Ani = GetAnimator();
            PlayableDirector director = GetDirector();
             if (director != null)
            {
                director.Pause();
                director.time = director.duration;
                director.Evaluate();
            }
            else if(Ani!=null)
            {
                Ani.speed = 1;
                int layer = Ani.GetLayerIndex(AnimationID);
                Ani.Play(AnimationID, layer, 1);
            }
        }

        public Animator GetAnimator()
        {
            if (_target == null)
            {
                _target = GameObject.Find(TargetID);
            }
            if (_target == null)
            {
                Debug.LogError($"找不到对象:{TargetID}");
                return null;
            }
            Animator Ani = _target.GetComponentInChildren<Animator>();
            return Ani;
        }

        public PlayableDirector GetDirector()
        {
            if (_target == null)
            {
                _target = GameObject.Find(TargetID);
            }
            if (_target == null)
            {
                Debug.LogError($"找不到对象:{TargetID}");
                return null;
            }
            PlayableDirector director = _target.GetComponentInChildren<PlayableDirector>();
            return director;
         }


        public override void Reset()
        {
             Animator Ani = GetAnimator();
            PlayableDirector director = GetDirector();
            if (director != null )
            {
                director.Pause();
                director.time = 0;
             }
            else if(Ani!=null)
            {
                Ani.speed = 1;
                int layer = Ani.GetLayerIndex(AnimationID);
                Ani.Play("C", layer);
            }
         }

        public override async Task StartExecute()
        {
            Animator Ani = GetAnimator();
            PlayableDirector director = GetDirector();
            if (director != null )
            {
                switch (AnimationID)
                {
                    case "Play":
                        director.time = 0;
                        director.Play();
                        double time = director.duration;
                        if (!double.IsInfinity(time))
                        {
                            await Task.Delay((int)(time * 1000));
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
                 Ani.Rebind();
                Ani.speed = 1;
                int layer = Ani.GetLayerIndex(AnimationID);
                Ani.Play(AnimationID, layer, 0);
                await Task.Delay(100);
                float time = Ani.GetCurrentAnimatorStateInfo(layer).length;
                await Task.Delay((int)(time * 1000));
            }
        }
    }
}
