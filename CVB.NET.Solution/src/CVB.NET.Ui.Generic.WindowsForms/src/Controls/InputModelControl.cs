using CVB.NET.Reflection.Caching;
using CVB.NET.Reflection.Caching.Cached;

namespace CVB.NET.Ui.Generic.WindowsForms.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Exception;
    using Ui.WindowsForms.Controls;

    public partial class InputModelControl : UserControl
    {
        public uint ControlMargin { get; set; }

        protected object Model { get; set; }

        public InputModelControl()
        {
            InitializeComponent();
        }

        private void ClearRenderPanel()
        {
            foreach (Control control in RenderPanel.Controls)
            {
                control.Dispose();
            }

            RenderPanel.Controls.Clear();
        }

        public void RenderEmptyInputModel(CachedType inputModelType)
        {
            RenderPrefilledInputModel(inputModelType, inputModelType.DefaultConstructor.InnerReflectionInfo.Invoke(null));
        }

        public void RenderEmptyInputModel<T>()
        {
            RenderEmptyInputModel(typeof (T));
        }

        public void RenderPrefilledInputModel(CachedType inputModelType, object prefilledModel)
        {
            Model = prefilledModel;

            ClearRenderPanel();

            RenderContext renderContext = new RenderContext(5);

            if (prefilledModel == null)
            {
                foreach (CachedPropertyInfo property in inputModelType.Properties)
                {
                    RenderEmptyProperty(renderContext, property);
                }
            }
            else
            {
                foreach (CachedPropertyInfo property in inputModelType.Properties)
                {
                    RenderPrefilledProperty(renderContext, property, property.InnerReflectionInfo.GetValue(prefilledModel));
                }
            }
        }

        public void RenderPrefilledInputModel<T>(T prefilledModel)
        {
            RenderPrefilledInputModel(typeof (T), prefilledModel);
        }

        public T GetInputModel<T>()
        {
            return (T) Model;
        }

        private void RenderEmptyProperty(RenderContext renderContext, CachedPropertyInfo property)
        {
            RenderPrefilledProperty(renderContext, property, null);
        }

        private void RenderPrefilledProperty(RenderContext renderContext, CachedPropertyInfo property, object value)
        {
            RenderPropertyControl(renderContext, property, GetPropertyControl(property));
        }

        private Control GetPropertyControl(CachedPropertyInfo property)
        {
            Type propertyType = property.InnerReflectionInfo.PropertyType;

            Control propertyControl;

            if (propertyType.IsEnum)
            {
                propertyControl = CreateEnumComboBox(propertyType, s => property.InnerReflectionInfo.SetValue(Model, Enum.Parse(propertyType, s)));
            }
            else if (propertyType == typeof (string))
            {
                propertyControl = CreateTextBox(text => property.InnerReflectionInfo.SetValue(Model, text));
            }
            else if (propertyType == typeof (bool))
            {
                propertyControl = CreateCheckbox(state => property.InnerReflectionInfo.SetValue(Model, state));
            }
            else if (propertyType.IsArray || typeof (IEnumerable<>).IsAssignableFrom(propertyType))
            {
                propertyControl = new EditableList();
            }
            else
            {
                throw new PropertyTypeNotHandledException(propertyType);
            }

            return propertyControl;
        }

        private Control CreateCheckbox(Action<bool> action)
        {
            CheckBox checkBox = new CheckBox();

            checkBox.CheckedChanged += (sender, args) => action(checkBox.Checked);

            return checkBox;
        }

        private Control CreateTextBox(Action<string> action)
        {
            TextBox textBox = new TextBox();

            textBox.TextChanged += (sender, args) => action(textBox.Text);

            return textBox;
        }

        private ComboBox CreateEnumComboBox(Type enumType, Action<string> valueChanged)
        {
            ComboBox enumComboBox = CreateComboBox(Enum.GetNames(enumType), valueChanged);

            enumComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            enumComboBox.SelectedIndex = 0;

            return enumComboBox;
        }

        private ComboBox CreateComboBox(string[] values, Action<string> valueChanged)
        {
            ComboBox comboBox = new ComboBox();

            comboBox.Items.AddRange(values);

            comboBox.SelectedIndexChanged += (sender, args) => valueChanged((string) comboBox.SelectedItem);

            return comboBox;
        }

        private void RenderPropertyControl(RenderContext renderContext, CachedPropertyInfo property, Control control)
        {
            RenderLabel(renderContext, property);

            control.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            control.Location = new System.Drawing.Point(0, (int) renderContext.TopOffset);
            control.Name = property.InnerReflectionInfo.Name + "_InputControl";
            control.Width = RenderPanel.Width;
            control.TabIndex = (int) renderContext.TabIndex;

            renderContext.TopOffset += (uint) control.Height + renderContext.ControlMargin;

            renderContext.TabIndex++;

            RenderPanel.Controls.Add(control);
        }

        private void RenderLabel(RenderContext renderContext, CachedPropertyInfo property)
        {
            Label propertyLabel = new Label();

            propertyLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            propertyLabel.Location = new System.Drawing.Point(0, (int) renderContext.TopOffset);
            propertyLabel.Name = property.InnerReflectionInfo.Name + "_Label";
            propertyLabel.Width = RenderPanel.Width;

            propertyLabel.Text = property.InnerReflectionInfo.Name + " :";

            renderContext.TopOffset += (uint) propertyLabel.Height + (renderContext.ControlMargin/2);

            RenderPanel.Controls.Add(propertyLabel);
        }

        private class RenderContext
        {
            public uint ControlMargin { get; }
            public uint TabIndex;

            public uint TopOffset;

            public RenderContext(uint controlMargin)
            {
                ControlMargin = controlMargin;
            }
        }
    }
}