using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Dros.Data;
using Dros.Views;
using Xamarin.Forms;

namespace Dros
{
	public partial class App : Application
	{
        private static string ZippedDbPath
        {
            get
            {
                return DependencyService.Get<IFileHelper>().GetLocalFilePath("Dros.dros.zip");
            }
        }

        public static string DbPath
        {
            get
            {
                return DependencyService.Get<IFileHelper>().GetLocalFilePath("dros.db");
            }
        }

		public App ()
		{
			InitializeComponent();
        }

		protected override void OnStart ()
		{
            if (!File.Exists(DbPath))
            {
                Helpers.UnzipFile(ZippedDbPath, DbPath);
            }
            if (File.Exists(DbPath))
            {
                SQLitePCL.Batteries_V2.Init();
                MainPage = new MainPage();

                using (var context = new DrosDbContext())
                {
                    Debug.WriteLine(context.Authors.FirstOrDefault().Name);
                }
            }
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
