using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Skyray.UC
{
    public class MockDatasGen
    {
        private DataType tPt;
        private int W;
        private int H;

        public MockDatasGen(int w, int h)
        {
            W = w;
            H = h;
            tPt = new DataType() { X = -1, Y = -1, Weight = -1 };
        }

        public List<DataType> CreateMockDatas(int nums,double[] dvalues,int col)
        {
            List<DataType> datas = new List<DataType>();
            Random rRand = new Random();
            for (int i = 0; i < nums; i++)
            {
                //if (tPt.X > 0 && tPt.Y > 0 && tPt.Weight > 0)
                //{
                   
                    //double c = np.random.rand();
                    //double c = rRand.Next(0, 10000);
                    //c = c / 10000;
                    int x = 0, y = 0;
                    //Console.WriteLine(c.ToString());
                    double c = dvalues[i];// +nums;
                    //if (c < 0.85)
                    //{
                    //    //int l = np.random.randint(1, 50);
                    //    //double d = 2 * np.random.rand() * np.pi;
                    //    int l = rRand.Next(1, 50);
                    //    double rr = rRand.Next(0, 10000);
                    //    rr = rr / 10000;
                    //    double d = 2 * rr * Math.PI;
                       
                    //    x = (int)(l * Math.Cos(d));
                    //    y = (int)(l * Math.Sin(d));
                    //    x = tPt.X + x < 0 ? 0 - x : x;
                    //    y = tPt.Y + y < 0 ? 0 - y : y;
                    //    x = tPt.X + x >= W ? 0 - x : x;
                    //    y = tPt.Y + y >= H ? 0 - y : y;
                    //    tPt.X = tPt.X + x;
                    //    tPt.Y = tPt.Y + y;
                    //    double nnn = rRand.Next(0, 10000);
                    //    nnn = nnn / 10000;
                    //    tPt.Weight = nnn * 10;
                    //    DataType data = new DataType() { X = tPt.X, Y = tPt.Y, Weight = tPt.Weight };
                    //    datas.Add(data);
                    //}
                    //else
                    //{
                    //    tPt.X = rRand.Next(1, W);
                    //    tPt.Y = rRand.Next(1, H);
                    //    double RAD = rRand.Next(0, 10000);
                    //    RAD = RAD / 10000;
                    //    tPt.Weight = c * 10;//RAD * 10;
                    //    DataType data = new DataType() { X = tPt.X, Y = tPt.Y, Weight = tPt.Weight };
                    //    datas.Add(data);
                    //}
                    if (c < 0.01)
                    {
                        int l = 5;
                        double d = 2 * 0.1 * Math.PI;
                        x = (int)(l * Math.Cos(d));
                        y = (int)(l * Math.Sin(d));
                        x = tPt.X + x < 0 ? 0 - x : x;
                        y = tPt.Y + y < 0 ? 0 - y : y;
                        x = tPt.X + x >= W ? 0 - x : x;
                        y = tPt.Y + y >= H ? 0 - y : y;
                        tPt.X = tPt.X + x;
                        tPt.Y = tPt.Y + y;
                        double nnn = rRand.Next(0, 10000);
                        nnn = nnn / 10000;
                       // tPt.Weight = nnn * 10;
                        tPt.Weight = c * 10;//RAD * 10;
                        DataType data = new DataType() { X = tPt.X, Y = tPt.Y, Weight = tPt.Weight };
                        datas.Add(data);
                    }
                    else
                    {
                        //tPt.X = (i % 10) * 50;
                        //tPt.Y = (i / 10) * 50;
                        tPt.X = (i % col) * (45 + (int)Math.Ceiling(((decimal)10 / col) * 5));
                        tPt.Y = (i / col) * (40 + 4);
                        double RAD = rRand.Next(0, 10000);
                        RAD = RAD / 10000;
                        tPt.Weight = c*10 ;//RAD * 10;
                        DataType data = new DataType() { X = tPt.X, Y = tPt.Y, Weight = tPt.Weight };
                        datas.Add(data);
                    }
                //}
                //else
                //{
                //    tPt.X = rRand.Next(1, W);
                //    tPt.Y = rRand.Next(1, H);
                //    double RAD = rRand.Next(0, 10000);
                //    RAD = RAD / 10000;
                //    tPt.Weight = RAD * 10;
                //    DataType data = new DataType() { X = tPt.X, Y = tPt.Y, Weight = tPt.Weight };
                //    datas.Add(data);
                //}
            }
            return datas;
        }


    }
}
