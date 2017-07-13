using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArrayList;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
namespace Algorithms
{
    class Algorithms
    {
        public static ListPoint havePath(int[,] a, int i1, int j1, int i2, int j2)
        {
            int[,] dadi = new int[7, 7];
            int[,] dadj = new int[7, 7];

            int[] queuei = new int[49];
            int[] queuej = new int[49];

            int fist = 0, last = 0, x, y;

            for (x = 0; x < 7; x++)
                for (y = 0; y < 7; y++) dadi[x, y] = -1;

            queuei[0] = i2;
            queuej[0] = j2;
            dadi[i2, j2] = -2;

            while (fist <= last)
            {
                x = queuei[fist];
                y = queuej[fist];
                fist++;
                if (y > 0)
                {
                    if (x == i1 & y - 1 == j1)
                    {
                        dadi[i1, j1] = x;
                        dadj[i1, j1] = y;
                        return buildPath(dadi, dadj, i1, j1);
                    }
                    if (dadi[x, y - 1] == -1)
                        if (a[x, y - 1] <= 0 )
                        {
                            last++;
                            queuei[last] = x;
                            queuej[last] = y - 1;
                            dadi[x, y - 1] = x;
                            dadj[x, y - 1] = y;
                        }
                }
                if (y < 6)
                {
                    if (x == i1 & y + 1 == j1)
                    {
                        dadi[i1, j1] = x;
                        dadj[i1, j1] = y;
                        return buildPath(dadi, dadj, i1, j1);
                    }
                    if (dadi[x, y + 1] == -1)
                        if (a[x, y + 1] <= 0 )
                        {
                            last++;
                            queuei[last] = x;
                            queuej[last] = y + 1;
                            dadi[x, y + 1] = x;
                            dadj[x, y + 1] = y;
                        }
                }
                if (x > 0)
                {
                    if (x - 1 == i1 & y == j1)
                    {
                        dadi[i1, j1] = x;
                        dadj[i1, j1] = y;
                        return buildPath(dadi, dadj, i1, j1);
                    }
                    if (dadi[x - 1, y] == -1)
                        if (a[x - 1, y] <= 0)
                        {
                            last++;
                            queuei[last] = x - 1;
                            queuej[last] = y;
                            dadi[x - 1, y] = x;
                            dadj[x - 1, y] = y;
                        }
                }
                if (x < 6)
                {
                    if (x + 1 == i1 & y == j1)
                    {
                        dadi[i1, j1] = x;
                        dadj[i1, j1] = y;
                        return buildPath(dadi, dadj, i1, j1);
                    }
                    if (dadi[x + 1, y] == -1)
                        if (a[x + 1, y] <= 0 )
                        {
                            last++;
                            queuei[last] = x + 1;
                            queuej[last] = y;
                            dadi[x + 1, y] = x;
                            dadj[x + 1, y] = y;
                        }
                }

            }
            return null;
        }
        public static ListPoint buildPath(int[,] dadi, int[,] dadj, int i1, int j1)
        {
            ListPoint arr = new ListPoint();
            int k;
            while (true)
            {
                arr.Add(new List { X = i1, Y = j1 });
                k = i1;
                i1 = dadi[i1, j1];
                if (i1 == -2) break;
                j1 = dadj[k, j1];
            }
            return arr;
        }
        public void MoveCicle(int[,] Array, int turn, ListPoint list, int sX, int sY)
        {
            Array[sX, sY] = 0;
            foreach (List item in list)
            {
                Array[item.X, item.Y] = turn;
            }
        }
        public static ListPoint checkLines(int[,] a, int iCenter, int jCenter)
        {
            ListPoint list_point = new ListPoint();

            int[] u = { 0, 1, 1, 1 };
            int[] v = { 1, 0, -1, 1 };
            int i, j, k;
            for (int t = 0; t < 4; t++)
            {
                k = 0;
                i = iCenter;
                j = jCenter;
                while (true)
                {
                    i += u[t];
                    j += v[t];
                    if (!isInside(i, j))
                        break;
                    if (a[i, j] != a[iCenter, jCenter])
                        break;
                    k++;
                }
                i = iCenter;
                j = jCenter;
                while (true)
                {
                    i -= u[t];
                    j -= v[t];
                    if (!isInside(i, j))
                        break;
                    if (a[i, j] != a[iCenter, jCenter])
                        break;
                    k++;
                }
                k++;
                if (k >= 4)
                    while (k-- > 0)
                    {
                        i += u[t];
                        j += v[t];
                        if (i != iCenter || j != jCenter)
                            list_point.Add(new List { X = i, Y = j });
                    }
            }
            if (list_point.Count > 0) list_point.Add(new List { X = iCenter, Y = jCenter });
            else list_point = null;
            return list_point;
        }

        public static ListPoint KiemTra(int[,] mt)
        {
            ListPoint arraylist = new ListPoint();
            int demngang, demdoc, demcheophai, demcheotrai, x, y;



            for (int i = 0; i < 7; i++)
                for (int j = 0; j < 7; j++)
                {
                    //Kiem tra hang ngang
                    x = i;
                    y = j;
                    demngang = 0;
                    while ((x < 6) && (mt[x, y] == mt[x + 1, y]) && (mt[x, y] != 0))
                    {
                        demngang++;
                        x++;
                    }


                    //Kiem tra hang doc
                    x = i;
                    y = j;
                    demdoc = 0;
                    while ((y < 6) && (mt[x, y] == mt[x, y + 1]) && (mt[x, y] != 0))
                    {
                        demdoc++;
                        y++;
                    }


                    //Kiem tra cheo phai
                    x = i;
                    y = j;
                    demcheophai = 0;
                    while ((x < 6) && (y < 6) && (mt[x, y] == mt[x + 1, y + 1]) && (mt[x, y] != 0))
                    {
                        demcheophai++;
                        x++;
                        y++;
                    }



                    // Kiem tra cheo phai
                    x = i;
                    y = j;
                    demcheotrai = 0;
                    while ((x > 0) && (y < 6) && (mt[x, y] == mt[x - 1, y + 1]) && (mt[x, y] != 0))
                    {
                        demcheotrai++;
                        x--;
                        y++;
                    }

                    if (demngang >= 3) { for (int k = 0; k < (demngang + 1); k++)     arraylist.Add(new List{X= i + k, Y= j});}
                    else demngang = 0;
                    if (demdoc >= 3) { for (int k = 0; k < (demdoc + 1); k++)       arraylist.Add(new List{X=i, Y=j + k});}
                    else demdoc = 0;
                    if (demcheophai >= 3) { for (int k = 0; k < (demcheophai + 1); k++)  arraylist.Add(new List{X=i + k, Y=j + k});}
                    else demcheophai = 0;
                    if (demcheotrai >= 3) { for (int k = 0; k < (demcheotrai + 1); k++)  arraylist.Add(new List{X=i - k,Y= j + k});}
                    else demcheotrai = 0;
                }
            return arraylist;
        }

        public static bool isInside(int i, int j)
        {
            return (i >= 0 && i < 7 && j >= 0 && j < 7);
        }

        public static void del_ball(int[,] a, ListPoint list_del)
        {
                foreach (List item in list_del)
                {
                    a[item.X, item.Y] = 0;
                }
        }
    }
}
