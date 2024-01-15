using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private UIDocument _document;
    [SerializeField] private StyleSheet _styleSheet;

    public static event Action<float> ScaleChanged;
    public static event Action SpinClicked;

    private void Start()
    {
        StartCoroutine(Generate());
    }

    private void OnValidate()
    {
        if (Application.isPlaying) return;
        StartCoroutine(Generate());
    }
    private IEnumerator Generate()
    {
        yield return null;

        VisualElement root = _document.rootVisualElement;
        root.Clear();

        root.styleSheets.Add(_styleSheet);


        VisualElement container = CreateVisualElement("container");

        VisualElement viewBox = CreateVisualElement("view-box", "bordered-box");
        container.Add(viewBox);

        VisualElement controlBox = CreateVisualElement("control-box", "bordered-box");
        container.Add(controlBox);

        Button spinButton = CreateVisualElementGeneric<Button>();
        spinButton.text = "Spin";
        spinButton.clicked += SpinClicked;
        controlBox.Add(spinButton);

        Slider scaleSlider = CreateVisualElementGeneric<Slider>();
        scaleSlider.lowValue = 0.5f;
        scaleSlider.highValue = 2f;
        scaleSlider.value = 1f;
        scaleSlider.RegisterValueChangedCallback(value => ScaleChanged?.Invoke(value.newValue));
        controlBox.Add(scaleSlider);


        root.Add(container);

        if(Application.isPlaying)
        {
            Vector3 targetPosition = container.worldTransform.GetPosition();
            Vector3 startPosition = targetPosition + Vector3.up * 100;

            controlBox.experimental.animation.Position(targetPosition, 2000).from = startPosition;
            controlBox.experimental.animation.Start(0, 1, 2000, (e, v) => e.style.opacity = new StyleFloat(v));
        }
    }
    
    private VisualElement CreateVisualElement(params string[] classNames)
    {
        return CreateVisualElementGeneric<VisualElement>(classNames);
    }

    T CreateVisualElementGeneric<T>(params string[] classNames) where T : VisualElement, new()
    {
        var visualElement = new T();
        foreach (var className in classNames)
        {
            visualElement.AddToClassList(className);

        }
        return visualElement;
    }
}
