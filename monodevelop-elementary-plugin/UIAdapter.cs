using System;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide.Gui;
using Gtk;

namespace MonoDevelopElementary
{
	public class UIAdapter : CommandHandler
	{
		public UIAdapter ()
		{
		}

		protected override void Run ()
		{
			base.Run ();
			Console.Out.WriteLine ("MonoDevelop plugin for elementary OS");

			wnd = MonoDevelop.Ide.IdeApp.Workbench.RootWindow;
			floatingMenu = new Menu ();
			/**
			 * Let's search the menubar widget, hide it and create a new one
			 * exposed with a right-click on the MainToolBar.
			 * (MonoDevelop.Components.Commands.CommandMenuBar)
			 * 
			 * 
			 * The WorkbenchWindow is composed of a VBox where [0] is the menubar,
			 * [1] is the rest.
			 * (https://github.com/mono/monodevelop/blob/master/main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Gui/DefaultWorkbench.cs, CreateComponents method)
			 * (https://github.com/mono/monodevelop/blob/master/main/src/core/MonoDevelop.Ide/MonoDevelop.Ide.Desktop/PlatformService.cs, AttachMainToolbar method)
			 **/
			VBox fullViewVBox = (VBox)wnd.Children [0];
			foreach (var vbox_sub in fullViewVBox.AllChildren) {
				if (vbox_sub is MonoDevelop.Components.Commands.CommandMenuBar) {
					/**
					 * Here we have the CommandMenuBar. 
					 * Now move each item into the new popup menu, and then hide the menubar.
					 **/
					var cmb = (MonoDevelop.Components.Commands.CommandMenuBar)vbox_sub;
					foreach (var mitem in cmb.Children) {
						// Remove so we can add it to the other menu
						cmb.Remove (mitem);
						mitem.ReceivesDefault = false;
						floatingMenu.Add(mitem);
					}
					floatingMenu.ReceivesDefault = false;
					floatingMenu.ShowAll ();
					cmb.Visible = false;
				} else if (vbox_sub is HBox) {
					/**
					 * When <<DesktopService.AttachMainToolbar (fullViewVBox, toolbar);>>
					 * the method adds a HBox to the big VBox, where the toolbar is.
					 **/
					var toolbarBox = (HBox)vbox_sub;
					foreach (var toolbarBox_sub in toolbarBox.AllChildren) {
						if (toolbarBox_sub.GetType ().Name == "MainToolbar") {
							var mainToolbar = (Gtk.Widget)toolbarBox_sub;

							/**
							 * Prevent MonoDevelop from painting the default gradient at 
							 * MonoDevelop.Components.MainToolbar.
							 **/ 
							mainToolbar.AppPaintable = false;

							// So let's paint the maintoolbar background
							mainToolbar.ExposeEvent += (o, args) => {
								var gtkWdgt = (Widget)o;
								using (var context = Gdk.CairoHelper.Create (gtkWdgt.GdkWindow)){
									int h, w;
									h = gtkWdgt.Allocation.Height;
									w = gtkWdgt.Allocation.Width;

									context.LineWidth = 0.7;
									context.MoveTo (0, h - 0.5);
									context.RelLineTo (w, 0);
									context.SetSourceRGB(0.4259, 0.4259, 0.4259);
									context.Stroke ();

									context.Dispose();
								}
							}; // end event handler lambda

							// The menu, as a popup, is displayed with a right click
							mainToolbar.ButtonReleaseEvent += (object o, ButtonReleaseEventArgs args) => {
								if(args.Event.Button == 3){
									floatingMenu.Popup(null, null, null, 3, 0);
								}
							}; // end button event handler lambda
						}
					} // end for
				} // end if HBox
			} // end for each big VBox component
		}

		private WorkbenchWindow wnd;
		private Menu floatingMenu;
	}
}

