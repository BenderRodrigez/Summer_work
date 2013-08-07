using System;
using Gtk;
using System.Resources;
using System.IO;
using System.Reflection;
using Summer_work;

public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		//Storage.ConvertDB("База.txt");
		//Use, to create DB... Don't tuch...
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

	protected void OnButton3Clicked (object sender, EventArgs e)
	{
		bool params_OK = true;
		float pressure = (float)force_spin.Value;
		Materials material;
		switch (objMaterial.ActiveText) {
		case "Гипсокартон":
			material = Materials.GKL;
			break;
		case "Дерево":
			material = Materials.Tree;
			break;
		case "Металл":
			material = Materials.Metal;
			break;
		default:
			params_OK &= false;
			break;
		}
		int fix_point = objMaterial.Active - 1;
		float lenght = (float)objLenght.Value;
		int fix_pointsN = (int)fixPointsN.Value;
		Materials wall_material = Materials.Dowel;
		switch (objMaterial.ActiveText) {
		case "Гипсокартон":
			wall_material = Materials.GKL;
			break;
		case "Дерево":
			wall_material = Materials.Tree;
			break;
		case "Кирпич":
			wall_material = Materials.Brick;
			break;
		case "Бетон":
			wall_material = Materials.Concrete;
			break;
		case "Пеноблок":
			wall_material = Materials.FoamBlock;
			break;
		default:
			params_OK &= false;
			break;
		}
		float wall_lenght = (float)wallLenght.Value;
		if ((pressure > 0) && (fix_point > -2 && fix_point < 2) && (lenght > 0) && (fix_pointsN > -1) && (wall_lenght > -1)) {
			params_OK |=true;
			//подбираем
			Storage.GenerateByMaterialList(wall_material);
			Storage.GenerateByForce(fix_point, pressure);
			Storage.GenerateByLenght(lenght+wall_lenght);
		}
		else
			params_OK &=false;
	}

}
