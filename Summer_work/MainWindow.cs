using System;
using Gtk;
using System.Resources;
using System.IO;
using System.Reflection;
using Summer_work;
using System.Timers;

public partial class MainWindow: Gtk.Window
{	

	Timer timer = new Timer (60000);

	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		//Storage.ConvertDB("База.txt");
		//Use, to create DB... Don't tuch...
		Storage.ReadDataResoursesAnchor();
		Storage.ReadDataResoursesDowels();
		Storage.ReadDataResoursesScrew();
		timer.Elapsed += new ElapsedEventHandler(Tick);
	}

	public static void Tick (object source, ElapsedEventArgs e)
	{

	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnCombobox2Changed (object sender, EventArgs e)
	{
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
		float pressure = (float)force_spin.Value*10;
		Materials material = Materials.None;
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
		pressure /= fix_pointsN;
		Materials wall_material = Materials.None;
		switch (wallMaterial.ActiveText) {
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
			params_OK |= true;
			//подбираем
			Storage.passed.Clear();
			Calculator.GenerateByMaterialList (material, wall_material);
			Calculator.GenerateByForce (fix_point, pressure);
			Calculator.GenerateByLenght (wall_lenght, lenght);
			if (Storage.passed.Count < 1) {
				//Error! Can't fix it by any material:(
				Warn wrn = new Warn ();
				wrn.SetLabel ("Мы не нашли подходящего крепежа в нашей базе!");
				wrn.Modal = true;
				wrn.Show ();
			} else {
				GtkLabel4.Text = Storage.passed.Count.ToString ();
				btnForward.Sensitive = true;
				nameOfCurrentFix.Text += Storage.passed [0].NameToString ();
				imgFixPreview.Pixbuf = new Gdk.Pixbuf (Storage.GetStreamFromResource (Storage.passed [0].img_name));
				spinD.Value = Storage.passed [0].d;
				spinLenght.Value = Storage.passed [0].lenght;
				spinAvForce.Value = Storage.passed [0].max_avulsion_force;
				spinMaxCutForce.Value = Storage.passed [0].max_cut_force;
				spinMaxA.Value = Storage.passed [0].max_a;
				spinMaxS.Value = Storage.passed [0].max_s;
				spinFixPoints.Value = fixPointsN.Value;
			}
		} else {
			Warn wrn = new Warn();
			wrn.SetLabel("Что-то не так с параметрами крепежа! Проверьте данные, которые ввели. Если это не помогло, свяжитесь с разработчиком!");
			wrn.Modal = true;
			wrn.Show();
			params_OK &= false;
		}
	}

	protected void OnBtnForwardClicked (object sender, EventArgs e)
	{
		if (Storage.passed.Count != 1)
			btnBack.Sensitive = true;
		Mount mnt = Storage.GetNextPassed ();
		nameOfCurrentFix.Text = "Название: " + mnt.NameToString ();
		imgFixPreview.Pixbuf = new Gdk.Pixbuf (Storage.GetStreamFromResource (mnt.img_name));
		spinD.Value = mnt.d;
		spinLenght.Value = mnt.lenght;
		spinAvForce.Value = mnt.max_avulsion_force;
		spinMaxCutForce.Value = mnt.max_cut_force;
		spinMaxA.Value = mnt.max_a;
		spinMaxS.Value = mnt.max_s;
		spinFixPoints.Value = fixPointsN.Value;
		if (mnt.is_doweled) {
			timer.Enabled = true;
		}
	}

	protected void OnBtnBackClicked (object sender, EventArgs e)
	{
		if(Storage.passed.Count != 1)
			btnBack.Sensitive = true;
		Mount mnt = Storage.GetPrevPassed();
		nameOfCurrentFix.Text = "Название: "+mnt.NameToString();
		imgFixPreview.Pixbuf = new Gdk.Pixbuf(Storage.GetStreamFromResource(mnt.img_name));
		spinD.Value = mnt.d;
		spinLenght.Value = mnt.lenght;
		spinAvForce.Value = mnt.max_avulsion_force;
		spinMaxCutForce.Value = mnt.max_cut_force;
		spinMaxA.Value = mnt.max_a;
		spinMaxS.Value = mnt.max_s;
		spinFixPoints.Value = fixPointsN.Value;
	}

}
