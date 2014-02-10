using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;

namespace HtmlRenderer.Android.Test
{
    [Activity(Label = "HtmlRenderer.Android.Test", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += delegate 
            { 
                
                button.Text = string.Format("{0} clicks!", count++);

                ImageView view = FindViewById<ImageView>(Resource.Id.imageView1);

                Bitmap bitmap = Bitmap.CreateBitmap(200, 100, Bitmap.Config.Argb8888);
                //创建一个canvas对象，并且开始绘图  
                Canvas canvas = new Canvas(bitmap);  
                canvas.DrawColor(Color.White);
               

                Paint paint = new Paint();
                paint.Color = Color.Red;
                paint.SetStyle(Paint.Style.Stroke);
                paint.StrokeWidth = 1;

                canvas.DrawRect(new Rect(10, 10, 100, 100), paint);

                view.SetImageBitmap(bitmap); 
            };


        }
    }
}

