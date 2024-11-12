using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Lephone.Data;
using Lephone.Data.Common;
using Lephone.Data.Definition;
using Lephone.Data.SqlEntry;
using System.IO;

namespace Skyray.UC
{
    public partial class FrmDeleteSpec : Skyray.Language.MultipleForm
    {
        public FrmDeleteSpec()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

    

        private void btnDeleteHis_Click(object sender, EventArgs e)
        {

            DateTime currentDate = DateTime.Now;

            string time ;

            string specImageFolder = System.AppDomain.CurrentDomain.BaseDirectory + "/Image/SpecImage/EDXT/";
            string sampleImageFolder = System.AppDomain.CurrentDomain.BaseDirectory + "/Image/SampleImage/EDXT/";

            DirectoryInfo di;
            FileInfo[] files;

            if (comboDeleteSpec.SelectedIndex == 0)
            {
                DateTime curTime = currentDate.AddMonths(-1);
                time = curTime.ToString("yyyy-MM-dd HH:mm:ss");
       
                SqlStatement sqlstate  = new SqlStatement("delete  from SpectrumData where SpecTypeValue = 2 and SpecDate <'" + time + "';");


                Lephone.Data.DbEntry.Context.ExecuteNonQuery(sqlstate);
                

                di = new DirectoryInfo(specImageFolder);
                files = di.GetFiles("*.jpg"); // 只删除jpg图片，可以更改扩展名以删除其他类型
         
                foreach (FileInfo file in files)
                {
                    if (file.LastWriteTime < curTime)
                    {
                        try
                        {
                            file.Delete();
                        }
                        catch (IOException ex)
                        {

                        }
                    }
                }

                di = new DirectoryInfo(sampleImageFolder);
                files = di.GetFiles("*.jpg"); // 只删除jpg图片，可以更改扩展名以删除其他类型

                foreach (FileInfo file in files)
                {
                    if (file.LastWriteTime < curTime)
                    {
                        try
                        {
                            file.Delete();
                        }
                        catch (IOException ex)
                        {

                        }
                    }
                }


            }
            else if (comboDeleteSpec.SelectedIndex == 1)
            {
                DateTime curTime = currentDate.AddMonths(-2);
                time = curTime.ToString("yyyy-MM-dd HH:mm:ss");

                SqlStatement sqlstate = new SqlStatement("delete  from SpectrumData where SpecTypeValue = 2 and SpecDate <'" + time + "';");
                


                Lephone.Data.DbEntry.Context.ExecuteNonQuery(sqlstate);

                di = new DirectoryInfo(specImageFolder);
                files = di.GetFiles("*.jpg"); // 只删除jpg图片，可以更改扩展名以删除其他类型
         
                foreach (FileInfo file in files)
                {
                    if (file.LastWriteTime < curTime)
                    {
                        try
                        {
                            file.Delete();
                        }
                        catch (IOException ex)
                        {

                        }
                    }
                }

                di = new DirectoryInfo(sampleImageFolder);
                files = di.GetFiles("*.jpg"); // 只删除jpg图片，可以更改扩展名以删除其他类型

                foreach (FileInfo file in files)
                {
                    if (file.LastWriteTime < curTime)
                    {
                        try
                        {
                            file.Delete();
                        }
                        catch (IOException ex)
                        {

                        }
                    }
                }
            }
            else if (comboDeleteSpec.SelectedIndex == 2)
            {
                DateTime curTime = currentDate.AddMonths(-3);
                time = curTime.ToString("yyyy-MM-dd HH:mm:ss");
                SqlStatement sqlstate = new SqlStatement("delete  from SpectrumData where SpecTypeValue = 2 and SpecDate <'" + time + "';");
                


                Lephone.Data.DbEntry.Context.ExecuteNonQuery(sqlstate);

                di = new DirectoryInfo(specImageFolder);
                files = di.GetFiles("*.jpg"); // 只删除jpg图片，可以更改扩展名以删除其他类型
         
                foreach (FileInfo file in files)
                {
                    if (file.LastWriteTime < curTime)
                    {
                        try
                        {
                            file.Delete();
                        }
                        catch (IOException ex)
                        {

                        }
                    }
                }

                di = new DirectoryInfo(sampleImageFolder);
                files = di.GetFiles("*.jpg"); // 只删除jpg图片，可以更改扩展名以删除其他类型

                foreach (FileInfo file in files)
                {
                    if (file.LastWriteTime < curTime)
                    {
                        try
                        {
                            file.Delete();
                        }
                        catch (IOException ex)
                        {

                        }
                    }
                }
            }
            else if (comboDeleteSpec.SelectedIndex == 3)
            {
                DateTime curTime = currentDate.AddMonths(-6);
                time = curTime.ToString("yyyy-MM-dd HH:mm:ss");

                SqlStatement sqlstate = new SqlStatement("delete  from SpectrumData where SpecTypeValue = 2 and SpecDate <'" + time + "';");
                


                Lephone.Data.DbEntry.Context.ExecuteNonQuery(sqlstate);


                di = new DirectoryInfo(specImageFolder);
                files = di.GetFiles("*.jpg"); // 只删除jpg图片，可以更改扩展名以删除其他类型
         
                foreach (FileInfo file in files)
                {
                    if (file.LastWriteTime < curTime)
                    {
                        try
                        {
                            file.Delete();
                        }
                        catch (IOException ex)
                        {

                        }
                    }
                }

                di = new DirectoryInfo(sampleImageFolder);
                files = di.GetFiles("*.jpg"); // 只删除jpg图片，可以更改扩展名以删除其他类型

                foreach (FileInfo file in files)
                {
                    if (file.LastWriteTime < curTime)
                    {
                        try
                        {
                            file.Delete();
                        }
                        catch (IOException ex)
                        {

                        }
                    }
                }
            }
            else if (comboDeleteSpec.SelectedIndex == 4)
            {
                DateTime curTime = currentDate.AddMonths(-12);
                time = curTime.ToString("yyyy-MM-dd HH:mm:ss");

                SqlStatement sqlstate = new SqlStatement("delete  from SpectrumData where SpecTypeValue = 2 and SpecDate <'" + time + "';");
                


                Lephone.Data.DbEntry.Context.ExecuteNonQuery(sqlstate);

                di = new DirectoryInfo(specImageFolder);
                files = di.GetFiles("*.jpg"); // 只删除jpg图片，可以更改扩展名以删除其他类型
         
                foreach (FileInfo file in files)
                {
                    if (file.LastWriteTime < curTime)
                    {
                        try
                        {
                            file.Delete();
                        }
                        catch (IOException ex)
                        {

                        }
                    }
                }

                di = new DirectoryInfo(sampleImageFolder);
                files = di.GetFiles("*.jpg"); // 只删除jpg图片，可以更改扩展名以删除其他类型

                foreach (FileInfo file in files)
                {
                    if (file.LastWriteTime < curTime)
                    {
                        try
                        {
                            file.Delete();
                        }
                        catch (IOException ex)
                        {

                        }
                    }
                }
            }
            else if (comboDeleteSpec.SelectedIndex == 5)
            {
                DateTime curTime = currentDate.AddMonths(-24);
                time = curTime.ToString("yyyy-MM-dd HH:mm:ss");

                SqlStatement sqlstate = new SqlStatement("delete  from SpectrumData where SpecTypeValue = 2 and SpecDate <'" + time + "';");
                


                Lephone.Data.DbEntry.Context.ExecuteNonQuery(sqlstate);


                di = new DirectoryInfo(specImageFolder);
                files = di.GetFiles("*.jpg"); // 只删除jpg图片，可以更改扩展名以删除其他类型
         
                foreach (FileInfo file in files)
                {
                    if (file.LastWriteTime < curTime)
                    {
                        try
                        {
                            file.Delete();
                        }
                        catch (IOException ex)
                        {

                        }
                    }
                }

                di = new DirectoryInfo(sampleImageFolder);
                files = di.GetFiles("*.jpg"); // 只删除jpg图片，可以更改扩展名以删除其他类型

                foreach (FileInfo file in files)
                {
                    if (file.LastWriteTime < curTime)
                    {
                        try
                        {
                            file.Delete();
                        }
                        catch (IOException ex)
                        {

                        }
                    }
                }
            }


            this.Close();

        }

    
      
    }
}
