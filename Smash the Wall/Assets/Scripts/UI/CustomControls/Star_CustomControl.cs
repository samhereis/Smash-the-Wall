using Helpers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.CustomControls
{
    public class Star_CustomControl : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<Star_CustomControl, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlIntAttributeDescription _starsCount = new UxmlIntAttributeDescription { name = "Stars Count", defaultValue = 3 };

            UxmlIntAttributeDescription _activeStarsCount = new UxmlIntAttributeDescription { name = "Active Stars Count", defaultValue = 3 };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                Star_CustomControl star = (Star_CustomControl)ve;
                star.starsCount = _starsCount.GetValueFromBag(bag, cc);
                star.activeStarsCount = _activeStarsCount.GetValueFromBag(bag, cc);
            }
        }

        private const string styleResource = "StarControl";

        private const string ussContainer = "starContainer";
        private const string ussOuterStar = "outerStar";
        private const string ussInnerStar = "innerStar";

        private const string _starContainerName = "StarContainer";
        private const string _outerStarName = "OuterStar";
        private const string _innerStarName = "InnerStar";

        private VisualElement _starContainer;

        private int _starsCount = 3;
        public int starsCount
        {
            get => _starsCount;
            set
            {
                _starsCount = value;
                UpdateStarCount();
            }
        }

        private int _activeStarsCount = 3;
        public int activeStarsCount
        {
            get => _activeStarsCount;
            set
            {
                _activeStarsCount = value;
                SetActiveStars(_activeStarsCount);
            }
        }

        public Star_CustomControl()
        {
            _starContainer = new VisualElement { name = _starContainerName };
            hierarchy.Add(_starContainer);
            _starContainer.AddToClassList(ussContainer);

            UpdateStarCount();
        }

        private void UpdateStarCount()
        {
            foreach (VisualElement star in _starContainer.Children().ToList())
            {
                if (star.name == _outerStarName)
                {
                    star.RemoveFromHierarchy();
                }
            }

            for (int i = 0; i < _starsCount; i++)
            {
                VisualElement outerStar = new VisualElement { name = _outerStarName };
                _starContainer.hierarchy.Add(outerStar);
                outerStar.AddToClassList(ussOuterStar);

                VisualElement innerStar = new VisualElement { name = _innerStarName };
                outerStar.hierarchy.Add(innerStar);
                innerStar.AddToClassList(ussInnerStar);

                float scale = NumberHelper.GetPercentageOf1(i + 1, _starsCount) / 2;
                outerStar.style.scale = new Vector2(0.5f + scale, 0.5f + scale);
            }
        }

        public void SetActiveStars(int newActiveStarsCount)
        {
            try
            {
                var stars = _starContainer.GetAllChildren(_innerStarName);

                foreach (var star in stars)
                {
                    star.visible = false;
                }

                if (newActiveStarsCount < 0) newActiveStarsCount = 0;
                if (newActiveStarsCount <= 0) return;

                if(newActiveStarsCount > stars.Count) activeStarsCount = newActiveStarsCount = stars.Count;

                for (int i = 0; i < newActiveStarsCount; i++)
                {
                    var star = stars[i];
                    star.visible = true;
                }
            }
            catch (System.Exception exception)
            {
                Debug.LogWarning("Error in SetActiveStar: " + exception.Message);
            }
        }
    }

    public static class A
    {
        public static List<VisualElement> GetAllChildren(this VisualElement element, string name)
        {
            List<VisualElement> innerElements = new List<VisualElement>();

            foreach (var child in element.Children())
            {
                if(child.name == name)
                {
                    innerElements.Add(child);
                }

                innerElements.AddRange(GetAllChildren(child, name));
            }

            return innerElements;
        }
    }
}