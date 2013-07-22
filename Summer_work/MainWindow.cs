using System;
using Gtk;
using System.Resources;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using Summer_work;
using System.Text.RegularExpressions;

public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		Storage.ReadDataResoursesAnchor();
		Storage.ReadDataResoursesDowels();
		Storage.ReadDataResoursesScrew();
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnCombobox2Changed (object sender, EventArgs e)
	{
		System.Reflection.Assembly _assembly;//read a project resource files
   		//Stream _imageStream;
		//Stream _imageStream2;
		_assembly = Assembly.GetExecutingAssembly();
		//_imageStream = _assembly.GetManifestResourceStream("Summer_work.Imgs.Graph.roof1sc.png");
		//_imageStream2 = _assembly.GetManifestResourceStream("Summer_work.Imgs.Graph.roof2sc.png");
		switch (combobox2.ActiveText)
		{
		case "Односкатная": roofImg.Pixbuf = new Gdk.Pixbuf(Storage.GetStreamFromResource("Summer_work.Imgs.Graph.roof1sc.png"));
			minh.Sensitive = true;
			minh.Value = 0;
			break;
		case "Двухскатная": roofImg.Pixbuf = new Gdk.Pixbuf(Storage.GetStreamFromResource("Summer_work.Imgs.Graph.roof2sc.png"));
			minh.Sensitive = false;
			break;
		}
	}	

	protected void OnOpenActionActivated (object sender, EventArgs e)
	{
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

	protected void OnSaveActionActivated (object sender, EventArgs e)
	{
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

	protected void OnButton3Activated (object sender, EventArgs e)
	{

	}	

	protected void OnButton3Clicked (object sender, EventArgs e)
	{
		float force = (float)force_spin.Value;
		Materials mat;
		bool success;
		switch (objMaterial.ActiveText) {
		case "Гипсокартон":
			mat = Materials.GKL;
			success = true;
			break;
		case "Дерево":
			mat = Materials.Tree;
			success = true;
			break;
		case "Металл":
			mat = Materials.Metal;
			success = true;
			break;
		default:
			success = false;
			break;
		}

		Materials wall_mat;
		switch (wallMaterial.ActiveText) {
		case "Кирпич":
			wall_mat = Materials.Brick;
			success &= true;
			break;
		case "Бетон":
			wall_mat = Materials.Concrete;
			break;
		case "Гипсокартон":
			wall_mat = Materials.GKL;
			break;
		case "Дерево":
			wall_mat = Materials.Tree;
			break;
		case "Пеноблок":
			wall_mat = Materials.FoamBlock;
			break;
		default:
			success &= false;
			break;
		}
		int vector;
		switch (planeToFix.ActiveText) {
		case "Потолок":
			vector = 1;
			break;
		case "Стена":
			vector = 0;
			break;
		case "Пол":
			vector = -1;
			break;
		default:
			success &= false;
			break;
		}
		float lenght = (float)objLenght.Value;
		int def_points_N = (int)fixPointsN.Value;
		float wall_lenght = (float)wallLenght.Value;

		if ((force <= 0) || (lenght <= 0) || (wall_lenght <= 0) || !success) {
			Warn msg = new Warn();
			msg.SetLabel("Введите корректные даные!");
			ResponseType resp = (ResponseType) msg.Run();
			msg.Destroy();
			return;
		}

		//Here we start calulatings


	}



}