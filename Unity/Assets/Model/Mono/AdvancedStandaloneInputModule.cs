using System.Collections.Generic;

namespace UnityEngine.EventSystems
{
    public class AdvancedStandaloneInputModule : StandaloneInputModule
    {
        public Dictionary<int, PointerEventData> PointerData => m_PointerData;
    }
}