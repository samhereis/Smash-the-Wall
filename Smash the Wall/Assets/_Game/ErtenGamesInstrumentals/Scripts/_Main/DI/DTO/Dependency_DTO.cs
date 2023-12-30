using Sirenix.OdinInspector;
using System;

namespace DependencyInjection
{
    [Serializable]
    public class Dependency_DTO<T>
    {
        [HorizontalGroup, HideLabel] public T dependency;
        [HorizontalGroup, HideLabel] public string id;
    }
}