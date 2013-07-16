using System;
using Gtk;
using System.Resources;
using System.IO;
using System.Reflection;

public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnCombobox2Changed (object sender, EventArgs e)
	{
		System.Reflection.Assembly _assembly;//read a project resource files
   		Stream _imageStream;
		Stream _imageStream2;
		_assembly = Assembly.GetExecutingAssembly();
		_imageStream = _assembly.GetManifestResourceStream("Summer_work.Imgs.Graph.roof1sc.png");
		_imageStream2 = _assembly.GetManifestResourceStream("Summer_work.Imgs.Graph.roof2sc.png");
		switch (combobox2.ActiveText)
		{
		case "Односкатная": roofImg.Pixbuf = new Gdk.Pixbuf(_imageStream);
			minh.Sensitive = true;
			minh.Value = 0;
			break;
		case "Двухскатная": roofImg.Pixbuf = new Gdk.Pixbuf(_imageStream2);
			minh.Sensitive = false;
			break;
		}
	}	

	protected void OnOpenAction1Activated (object sender, EventArgs e)
	{
		Gtk.FileChooserDialog fc=
		new Gtk.FileChooserDialog("Выберете файл",
		                            this,
		                            FileChooserAction.Open,
		                            "Отмена",ResponseType.Cancel,
		                            "Открыть",ResponseType.Accept);

		if (fc.Run() == (int)ResponseType.Accept) 
		{
			//System.IO.FileStream file=System.IO.File.OpenRead(fc.Filename);
			//file.Close();
		}
		//Don't forget to call Destroy() or the FileChooserDialog window won't get closed.
		fc.Destroy();
	}
	protected void OnSaveAction1Activated (object sender, EventArgs e)
	{
		Gtk.FileChooserDialog fc =
			new FileChooserDialog ("Выберете имя файла",
			                      this, FileChooserAction.Save,
			                      "Отмена", ResponseType.Cancel,
			                      "Выбрать", ResponseType.Accept);
		if (fc.Run () == (int)ResponseType.Accept)
		{
			//
		}
		fc.Destroy();
	}

}
