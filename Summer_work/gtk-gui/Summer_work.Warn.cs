
// This file has been generated by the GUI designer. Do not modify.
namespace Summer_work
{
	public partial class Warn
	{
		private global::Gtk.HBox hbox1;
		private global::Gtk.Image image1;
		private global::Gtk.Label showed_message;
		private global::Gtk.Button buttonOk;
		
		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget Summer_work.Warn
			this.WidthRequest = 400;
			this.HeightRequest = 300;
			this.Name = "Summer_work.Warn";
			this.Title = global::Mono.Unix.Catalog.GetString ("Произошла ошибка!");
			this.Icon = global::Stetic.IconLoader.LoadIcon (this, "gtk-dialog-warning", global::Gtk.IconSize.Menu);
			this.TypeHint = ((global::Gdk.WindowTypeHint)(1));
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			this.Modal = true;
			this.Resizable = false;
			this.DefaultWidth = 400;
			this.DefaultHeight = 300;
			// Internal child Summer_work.Warn.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.WidthRequest = 300;
			w1.HeightRequest = 200;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(3));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox ();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.image1 = new global::Gtk.Image ();
			this.image1.Name = "image1";
			this.image1.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-dialog-warning", global::Gtk.IconSize.Dialog);
			this.hbox1.Add (this.image1);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.image1]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.showed_message = new global::Gtk.Label ();
			this.showed_message.Name = "showed_message";
			this.showed_message.LabelProp = global::Mono.Unix.Catalog.GetString ("Произошла ошибка!");
			this.hbox1.Add (this.showed_message);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.showed_message]));
			w3.Position = 1;
			w1.Add (this.hbox1);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(w1 [this.hbox1]));
			w4.Position = 0;
			w4.Expand = false;
			w4.Fill = false;
			// Internal child Summer_work.Warn.ActionArea
			global::Gtk.HButtonBox w5 = this.ActionArea;
			w5.Name = "dialog1_ActionArea";
			w5.Spacing = 10;
			w5.BorderWidth = ((uint)(6));
			w5.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button ();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseStock = true;
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = "gtk-ok";
			this.AddActionWidget (this.buttonOk, -5);
			global::Gtk.ButtonBox.ButtonBoxChild w6 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w5 [this.buttonOk]));
			w6.Expand = false;
			w6.Fill = false;
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Show ();
		}
	}
}