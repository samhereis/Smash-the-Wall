namespace Sirenix.OdinInspector
{
#if OdinInspectorInstalled == false
    public class FoldoutGroupAttribute : System.Attribute
    {
        public FoldoutGroupAttribute(string groupName, float order = 0)
        {

        }

        public FoldoutGroupAttribute(string groupName, bool expanded, float order = 0)
        {

        }
    }
#endif
}
