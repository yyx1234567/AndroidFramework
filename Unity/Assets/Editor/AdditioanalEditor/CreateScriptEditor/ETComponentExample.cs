using ETModel;
using System.Collections;
using System.Collections.Generic;
namespace ETHotfix
{
    [ObjectSystem]
    public class ScriptNameComponentAwakeSystem : AwakeSystem<ScriptNameComponent>
    {
        public override void Awake(ScriptNameComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class ScriptNameComponentStartSystem : StartSystem<ScriptNameComponent>
    {
        public override void Start(ScriptNameComponent self)
        {
            self.Start();
        }
    }

    [ObjectSystem]
    public class ScriptNameComponentUpdateSystem : UpdateSystem<ScriptNameComponent>
    {
        public override void Update(ScriptNameComponent self)
        {
            self.Update();
        }
    }

    public class ScriptNameComponent : Component
    {
        internal void Awake()
        {
            //Do SomeThing.....
        }

        internal void Start()
        {
            //Do SomeThing.....
        }

        internal void Update()
        {
            //Do SomeThing.....
        }
    }
}