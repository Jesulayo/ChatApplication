using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using NaijaConnect.Controls;
using NaijaConnect.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedEditorControl), typeof(CustomEditorRenderer))]
namespace NaijaConnect.iOS.Renderers
{
    public class CustomEditorRenderer : EditorRenderer
    {
        UILabel _placeholderLabel;
        double previousHeight = -1;
        int prevLines = 0;
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Editor> e)
        {
            base.OnElementChanged(e);


            if (e.NewElement != null)
            {
                var customControl = (ExtendedEditorControl)e.NewElement;

                if (customControl.IsExpandable)
                    Control.ScrollEnabled = false;
                else
                    Control.ScrollEnabled = true;

                if (customControl.HasRoundedCorner)
                    Control.Layer.CornerRadius = 5;
                else
                    Control.Layer.CornerRadius = 0;

                Control.InputAccessoryView = new UIView(CGRect.Empty);
                Control.ReloadInputViews();

            }

            if (e.OldElement != null)
            {

            }
        }



        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var customControl = (ExtendedEditorControl)Element;

            if (e.PropertyName == Editor.TextProperty.PropertyName)
            {
                if (customControl.IsExpandable)
                {
                    CGSize size = Control.Text.StringSize(Control.Font, Control.Frame.Size, UILineBreakMode.WordWrap);

                    int numLines = (int)(size.Height / Control.Font.LineHeight);

                    if (prevLines > numLines)
                    {
                        customControl.HeightRequest = -1;

                    }
                    else if (string.IsNullOrEmpty(Control.Text))
                    {
                        customControl.HeightRequest = -1;
                    }

                    prevLines = numLines;
                }

                _placeholderLabel.Hidden = !string.IsNullOrEmpty(Control.Text);

            }
            else if (ExtendedEditorControl.HasRoundedCornerProperty.PropertyName == e.PropertyName)
            {
                if (customControl.HasRoundedCorner)
                    Control.Layer.CornerRadius = 5;
                else
                    Control.Layer.CornerRadius = 0;
            }
            else if (ExtendedEditorControl.IsExpandableProperty.PropertyName == e.PropertyName)
            {
                if (customControl.IsExpandable)
                    Control.ScrollEnabled = false;
                else
                    Control.ScrollEnabled = true;

            }
            else if (ExtendedEditorControl.HeightProperty.PropertyName == e.PropertyName)
            {
                if (customControl.IsExpandable)
                {
                    CGSize size = Control.Text.StringSize(Control.Font, Control.Frame.Size, UILineBreakMode.WordWrap);

                    int numLines = (int)(size.Height / Control.Font.LineHeight);
                    if (numLines >= 5)
                    {
                        Control.ScrollEnabled = true;

                        customControl.HeightRequest = previousHeight;

                    }
                    else
                    {
                        Control.ScrollEnabled = false;
                        previousHeight = customControl.Height;

                    }
                }
            }
        }

    }
}