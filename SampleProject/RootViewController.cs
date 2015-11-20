using System;
using System.Drawing;
using BemCheckBox;
using CoreGraphics;
using Foundation;
using UIKit;

namespace SampleProject
{
    class MyBemCheckBoxDelegate : BemCheckBoxDelegate
    {
       
        public override void DidTapCheckBox(bool checkBoxIsOn)
        {

        }
    }

    public partial class RootViewController : UIViewController
    {
        public RootViewController(IntPtr handle)
            : base(handle)
        {
        }

        #region View lifecycle

        public override void ViewDidLoad()
        {
            var checkbox = new BemCheckBox.BemCheckBox(new CGRect(10,10,50,50),new MyBemCheckBoxDelegate() );
            View.AddSubview(checkbox);
            base.ViewDidLoad();

        }


        #endregion
    }
}