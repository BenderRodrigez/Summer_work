using System;
using Gtk;
using System.Resources;
using System.IO;
using System.Reflection;
using Summer_work;
using System.Timers;

public partial class MainWindow: Gtk.Window
{	

	Timer timer = new Timer (1000);

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

	public void Tick (object source, ElapsedEventArgs e)
	{
		//here we change picture to dowel or screw, then we fix it in to the wall like one position
		Mount mnt = Storage.passed [Storage.GetCounter ()];
		if(mnt.is_doweled)
			if(mnt.show_dowel)
				imgFixPreview.Pixbuf = new Gdk.Pixbuf(Storage.GetStreamFromResource(mnt.img_name));
			else
				imgFixPreview.Pixbuf = new Gdk.Pixbuf(Storage.GetStreamFromResource((mnt as Screw).dwl.img_name));
		Storage.passed [Storage.GetCounter ()].show_dowel = !Storage.passed [Storage.GetCounter ()].show_dowel;
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnCombobox2Changed (object sender, EventArgs e)
	{
		switch (roofType.ActiveText)
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
		float pressure = (float)force_spin.Value*9.8f;//P=mg, http://ru.wikipedia.org/wiki/%D0%92%D0%B5%D1%81
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
		if ((pressure > 0) && (fix_point > -2 && fix_point < 2) && (lenght > 0) && (fix_pointsN > -1) && (wall_lenght > -1) && params_OK) {
			params_OK |= true;
			//подбираем
			Storage.passed.Clear();
			Calculator.GenerateByMaterialList (material, wall_material);
			Calculator.GenerateByForce (fix_point, pressure, lenght, material);
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
			timer.AutoReset = true;
			//timer.
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

	protected void OnButton65Clicked (object sender, EventArgs e)
	{
		Calculator.initConcrete ();//init prportions
		float volume = Calculator.foundamentVolume ((float)(hight.Value + deep.Value), (float)build_lenght.Value, (float)found_flat.Value, (float)addit_P.Value, (float)build_weight.Value);//total volume
		beton_vol.Value = volume;
		Calculator.Concrete mark = new Calculator.Concrete ();
		if (Calculator.Concrete.TryParse (beton.ActiveText, out mark)) {
			float[] mats = new float[3];
			mats = Calculator.concreteMaterials (volume, mark);//weight of components
			concentre.Value = mats[0]/1000;
			gravel.Value = mats[1]/1000;
			sand.Value = mats[2]/1000;
		}
		else
		{
			Warn wrn = new Warn();
			wrn.SetLabel("Проверьте, выбрали ли вы марку бетона! Если ошибка сохранится, свяжитесь с разработчиком!");
			wrn.Modal = true;
			wrn.Show();
		}
		arm.Value = Calculator.armsTotalLenght((float)(build_lenght.Value+addit_P.Value+build_weight.Value), (float)(hight.Value + deep.Value), (float)found_flat.Value);//just lenght of "arms"
		tube.Value = Calculator.formwork((float)(hight.Value + deep.Value), (float)(build_lenght.Value), (float)(build_weight.Value), (float)found_flat.Value, (float)addit_P.Value);
	}

	protected void OnButton13Clicked (object sender, EventArgs e)
	{
		float S = (float)(build_hight.Value * (build_lenght1.Value + build_weight1.Value) - holesS.Value);
		float concrete_weight = (int)conWeight.Value;
		int roof_h = (int)roofHight.Value;
		float min_roof_h = (float)minh.Value;
		if (roofType.ActiveText == "Двухскатная")
			S += (float)(0.5f * roof_h * build_weight1.Value);
		else
			S += (float)(build_weight1.Value* min_roof_h + (roof_h - min_roof_h) * build_weight.Value);
		float w = 0,/* h = 0,*/ l = 0, v = 0;
		switch (materialOfBuild.ActiveText) {//Where is we have /r in text oO
		case "Одинарный кирпич":
			l = 0.250f;
			//h = 0.65f;
			w = 0.120f;
			v = 0.00195f;
			break;
		case "Двойной кирпич":
			l = 0.250f;
			//h = 0.138f;
			w = 0.120f;
			v = 0.00414f;
			break;
		case "Полуторный кирпич":
			l = 0.250f;
			//h = 0.88f;
			w = 0.120f;
			v = 0.00264f;
			break;
		case "Пеноблок":
			l = 0.600f;
			//h = 0.200f;
			w = 0.300f;
			v = 0.036f;
			break;
		}
		float totVolume = 0;
		concrete_weight *= 0.001f;
		switch (typeOfWalls.ActiveText) {
		case "0,5 кирпича":
			totVolume = S*w;
			break;
		case "1 кирпич":
			totVolume = S*l;
			break;
		case "1,5 кирпича":
			totVolume = S*(w+l);
			break;
		case "2 кирпича":
			totVolume = S*2*l;
			break;
		case "2,5 кирпича":
			totVolume = S*2*l+S*w;
			break;
		}
		float conNeeds = totVolume*0.1f;
		float needs = (totVolume-conNeeds)/v;
		bricksN.Value = needs;
		glueN.Value = conNeeds;
	}



}
