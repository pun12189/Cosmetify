﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cosmetify.CustomControl
{
    public class CustomDatePicker : DatePicker
    {
        //static CustomDatePicker()
        //{
        //    DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomDatePicker), new
        //       FrameworkPropertyMetadata(typeof(CustomDatePicker)));
        //}

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (Template != null)
            {
                var box = (TextBox)Template.FindName("PART_TextBox", this);
                box.AddHandler(KeyDownEvent, new KeyEventHandler(OnTextBoxKeyDown), true);
            }
        }

        private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // The DatePicker set this as handled, which breaks the DataGrid commit model.
                e.Handled = false;
            }
        }
    }
}
