using System;
using UnityEngine;

namespace Game.MainMenu
{
    public class LevelSelectionUi : MonoBehaviour
    {
        [SerializeField] private PhysicalButton _prefab;
        [SerializeField] private Transform _container;

        private void Awake()
        {
            var stages = Core.Stages.GetAllStageEntries();
            for (int i = 0; i < stages.Length; i++)
            {
                var stage = stages[i];
                var instance = Instantiate(_prefab, _container);
                var displayName = ConvertToRoman(i);
                
                instance.SetText(displayName);
                instance.AddListener(() => Core.Instance.LoadStageByName(stage.Levels[0]));
            }
        }
        
        private static string ConvertToRoman(int number)
        {
            if (number <= 0 || number > 3999)
                return "T";

            (int value, string numeral)[] romanMap = new[]
            {
                (1000, "M"), (900, "CM"), (500, "D"), (400, "CD"),
                (100, "C"), (90, "XC"), (50, "L"), (40, "XL"),
                (10, "X"), (9, "IX"), (5, "V"), (4, "IV"),
                (1, "I")
            };

            var result = string.Empty;

            foreach (var (value, numeral) in romanMap)
            {
                while (number >= value)
                {
                    result += numeral;
                    number -= value;
                }
            }

            return result;
        }
    }
}