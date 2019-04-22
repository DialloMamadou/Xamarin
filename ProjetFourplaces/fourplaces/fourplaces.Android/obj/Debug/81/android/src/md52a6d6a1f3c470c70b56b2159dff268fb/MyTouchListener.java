package md52a6d6a1f3c470c70b56b2159dff268fb;


public class MyTouchListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.view.View.OnTouchListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onTouch:(Landroid/view/View;Landroid/view/MotionEvent;)Z:GetOnTouch_Landroid_view_View_Landroid_view_MotionEvent_Handler:Android.Views.View/IOnTouchListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Com.Syncfusion.Numericupdown.MyTouchListener, Syncfusion.SfNumericUpDown.Android", MyTouchListener.class, __md_methods);
	}


	public MyTouchListener ()
	{
		super ();
		if (getClass () == MyTouchListener.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Numericupdown.MyTouchListener, Syncfusion.SfNumericUpDown.Android", "", this, new java.lang.Object[] {  });
	}

	public MyTouchListener (md52a6d6a1f3c470c70b56b2159dff268fb.SfNumericUpDown p0, md52a6d6a1f3c470c70b56b2159dff268fb.SfNumericTextBox p1, boolean p2)
	{
		super ();
		if (getClass () == MyTouchListener.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Numericupdown.MyTouchListener, Syncfusion.SfNumericUpDown.Android", "Com.Syncfusion.Numericupdown.SfNumericUpDown, Syncfusion.SfNumericUpDown.Android:Com.Syncfusion.Numericupdown.SfNumericTextBox, Syncfusion.SfNumericUpDown.Android:System.Boolean, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public boolean onTouch (android.view.View p0, android.view.MotionEvent p1)
	{
		return n_onTouch (p0, p1);
	}

	private native boolean n_onTouch (android.view.View p0, android.view.MotionEvent p1);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
