using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UIHealthAlchemy
{
    [ExecuteInEditMode]
    public class MaterialHealhBar : HealthBarLogic
    {

        [SerializeField] protected Material mat;
        [SerializeField] protected Image _image;
        protected float _Value;
        [SerializeField]
        public override float Value
        {
            get
            {
                return _Value;
            }

            set
            {
                if (_Value != value)
                {
                    _Value = value;
                    if (_Value > 1)
                        _Value = 1;
                    if (_Value < 0)
                        _Value = 0;
                    mat.SetFloat("_Value", _Value * (Max - Min) + Min);
                    this.value = value;
                }
            }
        }
        [Range(0.0f, 1.0f)]
        [SerializeField]
        protected float value;
        [SerializeField] protected float Min = 0;
        [SerializeField] protected float Max = 1;

        void Start()
        {
            Value = value;
            if (_image) _image.color=new Color32(103, 255, 42, 100);
        }

        private void Update()
        {
            Value = value;
            if(_image.rectTransform.localScale.y> 1.2f)
            {
                if(_image)_image.color = new Color32(215, 21, 11, 100);
            }
            else
            {
                if (_image) _image.color = new Color32(103, 255, 42, 100);
            }
        }
    }
}
