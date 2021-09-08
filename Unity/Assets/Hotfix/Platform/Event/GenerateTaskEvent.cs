using ETModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [Event(EventIdType.GenerateTaskEvent)]
    public class GenerateTaskEvent : AEvent<string,GameObject>
    {
        public override void Run(string type, GameObject go)
        {
            var script=  go.AddComponent<ETModel.ProjectBtn>();

            // script.ConfirmBtn = go.transform.Find("WaitUpdate").GetComponentInChildren<Button>();
            // script.Progress = go.transform.Find("ExerciseTrainFrame/ExerciseTrainFrameSlider").GetComponent<Image>();
            // script.Info = go.transform.Find("WaitUpdate").GetComponentInChildren<Text>();
            //go.transform.Find("StartExercise").GetComponentInChildren<Button>().onClick.AddListener(OpenExam);
            //go.transform.Find("StartExercise").GetComponentInChildren<Button>().onClick.AddListener(()=> { script.DownLoadBundle(); });
            //script.OnUpadteCompelete += () => 
            //{
            //    UIComponent uiComponent = Game.Scene.GetComponent<UIComponent>();
            //    uiComponent.GetUIComponent<StudentTrainWindowComponent>(UIType.StudentTrainWindow).GetExamInfo(true);
            //    uiComponent.GetUIComponent<StudentTrainWindowComponent>(UIType.StudentTrainWindow).GetPracticeInfo();
            // };
            //script.OnNeedUpdate += () => 
            //{
            //    go.transform.Find("Wait").gameObject.SetActive(false);
            //    go.transform.Find("StartExercise").gameObject.SetActive(false);
            //    go.transform.Find("WaitUpdate").gameObject.SetActive(true);
            //};

            //script.ConfirmBtn.onClick.AddListener( 
            //    ()=> {
            //         var component= Game.Scene.GetComponent<UIComponent>().GetUIComponent<StudentTrainWindowComponent>(UIType.StudentTrainWindow);
            //         if (script.GetState() == ProjectBtnState.Ready)
            //        {
            //            go.transform.Find("StartExercise").GetComponentInChildren<Button>().onClick.Invoke();
            //        }
            //        Global.ExamInfo = new StudentExamInfo()
            //        {
            //            DbRecordId = component.DbRecordId,
            //            taskExamModel = component.taskExam,
            //            taskItemModel = component.taskItem
            //        };

            //    });
           
            script.InitProjectBtn(type);
        }

        //private void OpenExam()
        //{
        //    var component = Game.Scene.GetComponent<UIComponent>().GetUIComponent<StudentTrainWindowComponent>(UIType.StudentTrainWindow);
        //    Global.ExamInfo = new StudentExamInfo()
        //    {
        //        DbRecordId = component.DbRecordId,
        //        taskExamModel = component.taskExam,
        //        taskItemModel = component.taskItem
        //    };
        //}
    }
}