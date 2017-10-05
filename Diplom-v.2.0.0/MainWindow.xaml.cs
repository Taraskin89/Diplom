using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Windows.Forms.DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;

namespace Diplom_v._2._0._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static int maxA, minA, maxB, minB, n = 0;
        static double sizeArray = 0;
        public bool isCalculatedFraction = false;
        public bool isCalculatedPerturbation = false;

        List<double> result = new List<double>(); // list for Q[i]
        List<double> resultPer = new List<double>(); // list for Q[i]

        double[] arrA = new double[0];
        double[] arrB = new double[0];
        double[] arrBC = new double[0];
        public MainWindow()
        {
            InitializeComponent();

            tbn.Text = "7";
            tbAmin.Text = "1";
            tbAmax.Text = "10";
            tbBmin.Text = "1";
            tbBmax.Text = "10";
            tbEpsA.Text = "30";
            tbEpsB.Text = "10";

            maxA = Convert.ToInt32(tbAmax.Text);
            minA = Convert.ToInt32(tbAmin.Text);
            maxB = Convert.ToInt32(tbBmax.Text);
            minB = Convert.ToInt32(tbBmin.Text);
            canvas1.Width = 0;
        }
        private void BtnResolveFraction_Click(object sender, RoutedEventArgs e)
        {
            
            n = Convert.ToInt32(tbn.Text);
            sizeArray = Convert.ToInt32((2 * Math.Pow(2, n) - 2) / 2);
            Array.Resize(ref arrA, Convert.ToInt32(sizeArray));
            Array.Resize(ref arrB, Convert.ToInt32((sizeArray - 1) / 2));
            Array.Resize(ref arrBC, Convert.ToInt32((sizeArray + 1) / 2));
            Random rand = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < sizeArray; i++)
            {
                arrA[i] = rand.Next(minA, maxA);
            }
            for (int j = 0; j < (sizeArray - 1) / 2; j++)
            {              
                arrB[j] = rand.Next(minB, maxB);
            }
            for (int k = 0; k < (sizeArray + 1) / 2; k++)
            {
                arrBC[k] = rand.Next(minB, maxB);
            }
            if (n > 2)
            {
                CalcFraction();
                DrawFraction(arrA, arrB, arrBC);
            }
            else MessageBox.Show("Please select a n value > 2!");
        }
        private void CalcPerturbation_Click(object sender, RoutedEventArgs e)
        {
            resultPer.Clear();
            if (isCalculatedFraction) Perturbation();
            else MessageBox.Show("Please first calculate the fraction!");
        }
        private void BtnShowGraphic_Click(object sender, RoutedEventArgs e)
        {
            if (isCalculatedPerturbation && isCalculatedFraction) RenderChartSeries(result, resultPer);
            else MessageBox.Show("Please first calculate fraction and then perturbation!");
        }
        private void BtnClearChart_Click(object sender, RoutedEventArgs e)
        {
            lineChart.Series.Clear();
        }
        public void DrawLine(double X1, double X2, double Y1, double Y2, Line line, double thickness)
        {
            line.Stroke = System.Windows.Media.Brushes.Black;
            line.X1 = X1;
            line.X2 = X2;
            line.Y1 = Y1;
            line.Y2 = Y2;
            line.StrokeThickness = thickness;
            canvas1.Children.Add(line);
        }
        public void DrawSimbol(double leftPosition, double topPosition, TextBlock textBlock, double array)
        {
            Canvas.SetLeft(textBlock, leftPosition);
            Canvas.SetTop(textBlock, topPosition);
            textBlock.Text = array.ToString();
            canvas1.Children.Add(textBlock);
        }
        public void DrawFraction(double[] arrA, double[] arrB, double[] arrBC)
        {
            canvas1.Children.Clear();
            n = Convert.ToInt32(tbn.Text);
            double startLineWidth = 20;
            canvas1.Width = (2*arrBC.Length)*2*startLineWidth + 50;
            canvas1.Height = 25 * n + 50;
            int Counter = 0;
            double lastRightPosition = canvas1.Width - 50;
            double lastRightPositionB = canvas1.Width - 50;
            double lastBotPosition = canvas1.Height - 50;

            double plusWidth = 20;
            double elementWidth = startLineWidth;
            double heightBetweenLine = 25;
            double nextLine = 2 * (startLineWidth + plusWidth) + elementWidth;
            double nextLine2;
            double additionalWidth = 0;
            double widthBetweenTwoBigFraction = (n-3) * plusWidth;
            
            int a = 0;
            int b = 0;
            int arrTmplenght = arrB.Length;
            int stepCounter = n - 1;
            for (int i = arrB.Length, j = arrA.Length, k = arrBC.Length, z = arrBC.Length; i > 0; i--)
            {
                if (!(arrB.Length - Counter < Counter))
                {
                    a++;
                    TextBlock textBlock = new TextBlock();
                    Line startLine = new Line();
                    //first line
                    DrawLine(lastRightPosition, lastRightPosition - startLineWidth, lastBotPosition, lastBotPosition, startLine, 2);
                    //plus
                    Line plusHorizontalLine = new Line();
                    DrawLine((lastRightPosition - elementWidth - (plusWidth / 2)),lastRightPosition - elementWidth - (plusWidth / 2) - 5, lastBotPosition, lastBotPosition, plusHorizontalLine, 1);
                    Line plusVerticalLine = new Line();
                    DrawLine(plusHorizontalLine.X1 - 2.5, plusHorizontalLine.X1 - 2.5, lastBotPosition - 2.5, lastBotPosition + 2.5, plusVerticalLine, 1);
                    DrawSimbol(lastRightPosition - plusWidth / 2 - 5, lastBotPosition, textBlock, arrBC[k - 1]);
                    //second line
                    Line startLine2 = new Line();
                    DrawLine(lastRightPosition - startLineWidth - plusWidth, lastRightPosition - (2 * startLineWidth) - plusWidth, lastBotPosition, lastBotPosition, startLine2, 2);
                    //plus
                    plusHorizontalLine = new Line();
                    DrawLine(lastRightPosition - 2 * elementWidth - 2 * plusWidth + 15, lastRightPosition - 2 * elementWidth - 2 * plusWidth + 10, lastBotPosition,lastBotPosition, plusHorizontalLine, 1);
                    plusVerticalLine = new Line();
                    DrawLine(lastRightPosition - 2 * elementWidth - 2 * plusWidth + 12.5, lastRightPosition - 2 * elementWidth - 2 * plusWidth + 12.5,
                           lastBotPosition - 2.5, lastBotPosition + 2.5, plusVerticalLine, 1);
                    textBlock = new TextBlock();
                    DrawSimbol(startLine2.X2 + 4, lastBotPosition, textBlock, arrBC[k - 2]);
                    //array B number plus two fraction
                    textBlock = new TextBlock();
                    DrawSimbol(lastRightPosition - 2 * elementWidth - 2 * plusWidth, lastBotPosition - 7.5, textBlock, arrB[i - 1]);
                    //array A number on top fraction first line
                    textBlock = new TextBlock();
                    DrawSimbol(lastRightPosition - plusWidth / 2 - 5, lastBotPosition - 15, textBlock, arrA[j - 1]);
                    //array A number on top fraction second line
                    textBlock = new TextBlock();
                    DrawSimbol(startLine2.X2 + 4, lastBotPosition - 15, textBlock, arrA[j - 2]);
                    lastRightPosition = lastRightPosition - 2 * (plusWidth + startLineWidth) - elementWidth * (3);
                    //lastRightPosition = lastRightPosition - elementWidth*(7);
                    if (k > k - 2 && k % 4 != 0)
                    {
                        lastRightPosition = lastRightPosition - startLineWidth;
                        for (int x =  arrBC.Length; x > 0; x-- )
                        {
                            if (x != k && x == k/2 && x%8!=0)
                            lastRightPosition = lastRightPosition - startLineWidth;
                        }
                    }
                    k = k - 2;
                }
                else
                {
                    TextBlock textBlock = new TextBlock();
                    if (b == a / 2)
                    {
                        b = 0;
                        a = a / 2;
                        nextLine = 2 * (nextLine + plusWidth) + elementWidth;
                        heightBetweenLine += 25;
                        lastRightPositionB = canvas1.Width - 50;
                        additionalWidth += 2 * elementWidth;
                        widthBetweenTwoBigFraction -= elementWidth;
                    }
                    //first line
                    Line startLine = new Line();
                    DrawLine(lastRightPositionB, lastRightPositionB - nextLine, lastBotPosition - heightBetweenLine, lastBotPosition - heightBetweenLine, startLine, 2);
                    //plus
                    Line plusHorizontalLine = new Line();
                    DrawLine(startLine.X2 - elementWidth, startLine.X2 - elementWidth - 5, lastBotPosition - heightBetweenLine, lastBotPosition - heightBetweenLine,
                            plusHorizontalLine, 1);
                    Line plusVerticalLine = new Line();
                    DrawLine(plusHorizontalLine.X1 - 2.5, plusHorizontalLine.X1 - 2.5, lastBotPosition - heightBetweenLine - 2.5, lastBotPosition - heightBetweenLine + 2.5,
                           plusVerticalLine, 1);
                    //array A number on top fraction first line
                    textBlock = new TextBlock();
                    DrawSimbol(startLine.X1 - nextLine / 2, lastBotPosition - heightBetweenLine - 15, textBlock, arrA[j - 1]);
                    //second line
                    Line startLine2 = new Line();
                    DrawLine(plusHorizontalLine.X1 - elementWidth,  plusHorizontalLine.X1 - nextLine, lastBotPosition - heightBetweenLine, lastBotPosition - heightBetweenLine, startLine2, 2);
                    //plus
                    plusHorizontalLine = new Line();
                    DrawLine(startLine2.X2 - 7.5, startLine2.X2 - 12.5, lastBotPosition - heightBetweenLine, lastBotPosition - heightBetweenLine, plusHorizontalLine, 1);
                    plusVerticalLine = new Line();
                    DrawLine(plusHorizontalLine.X1 - 2.5, plusHorizontalLine.X1 - 2.5,  lastBotPosition - heightBetweenLine - 2.5, lastBotPosition - heightBetweenLine + 2.5, plusVerticalLine, 1);
                    //array B
                    textBlock = new TextBlock();
                    DrawSimbol(plusVerticalLine.X1 - 15, plusHorizontalLine.Y2 - 7.5, textBlock, arrB[i - 1]);
                    //array A number on top fraction second line
                    textBlock = new TextBlock();
                    DrawSimbol(startLine2.X1 - nextLine / 2, lastBotPosition - heightBetweenLine - 15, textBlock, arrA[j - 2]);                
                    //lastRightPositionB = lastRightPositionB - 7*elementWidth;
                    lastRightPositionB = lastRightPositionB - 2*nextLine - 6 * elementWidth;
                    arrTmplenght = arrTmplenght - 2;
                    b++;
                }
                j = j - 2;
                Counter++;
            }
            lastRightPositionB = canvas1.Width - 50;
            heightBetweenLine = 25;
            //main line
            Line mainLine = new Line();
            DrawLine(lastRightPositionB,  lastRightPositionB - 2*(arrA.Length-1)*startLineWidth - startLineWidth,
                     lastBotPosition - heightBetweenLine * (n - 1), lastBotPosition - heightBetweenLine * (n - 1), mainLine, 2);
            //array A number on top fraction second line
            TextBlock textBlockMain = new TextBlock();
            DrawSimbol((mainLine.X1 + mainLine.X2) / 2, mainLine.Y1 - 20, textBlockMain, arrA[0]);
        }
        public void BRAlgoritm(List<double> result, double[] arrA, double[] arrB, double[] arrBC)
        {
            double[] tmpArr = new double[arrB.Length];
            int Counter = 0;
            int arrTmplenght = arrB.Length;
            int stepCounter = n-1;
            for (int i = arrB.Length, j = arrA.Length, k = arrBC.Length, z = arrBC.Length; i > 0; i--)
            {   
                if (!(arrB.Length - Counter < Counter))
                {
                    tmpArr[i - 1] = (arrB[i - 1] + arrA[j - 1] / arrBC[k - 1] + arrA[j - 2] / arrBC[k - 2]);
                    result.Add(arrA[j - 1] / arrBC[k - 1]);
                    result.Add(arrA[j - 2] / arrBC[k - 2]);
                    k = k - 2;
                }
                else
                {  
                    tmpArr[i - 1] = (arrB[i - 1] + arrA[j - 1] / tmpArr[arrTmplenght - 1] + arrA[j - 2] / tmpArr[arrTmplenght - 2]);
                    result.Add((arrA[j - 1] / tmpArr[arrTmplenght - 1]));
                    result.Add((arrA[arrTmplenght - 2] / tmpArr[arrTmplenght - 2]));
                    arrTmplenght = arrTmplenght - 2;             
                }
                j = j - 2;
                Counter++;               
            }  
            tmpArr[0] = arrA[0] / (arrB[0] + arrA[1] / tmpArr[1] + arrA[2] / tmpArr[2]);
            result.Add(tmpArr[0]);
        }
        public void CalcFraction()
        {
            List<KeyValuePair<string, double>> series1 = new List<KeyValuePair<string, double>>();
            result.Clear();
            BRAlgoritm(result, arrA, arrB, arrBC);
            // Set Data to GridView
            List<Result> items = new List<Result>(); // for rendering to GridView
            int tmpForResult = 0;
            foreach (double Qi in result)
            {
                items.Add(new Result() { Iteration = tmpForResult, Res = Qi });
                series1.Add(new KeyValuePair<string, double>(tmpForResult.ToString(), Qi));
                tmpForResult++;
            }
            listview.ItemsSource = items;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listview.ItemsSource);
            seriesFraction.ItemsSource = series1;
            isCalculatedFraction = true;
        }
        public void Perturbation()
        {
            int maxPerA = Int32.Parse(tbEpsA.Text);
            int maxPerB = Int32.Parse(tbEpsB.Text);
            int[] perturbationArrayA = new int[arrA.Length];
            int[] perturbationArrayB = new int[arrB.Length];
            int[] perturbationArrayBC = new int[arrBC.Length];
            double[] perArrA = new double[arrA.Length];
            double[] perArrB = new double[arrB.Length];
            double[] perArrBC = new double[arrBC.Length];
            Randomizer(maxPerA, perturbationArrayA, perArrA, arrA);
            Randomizer(maxPerB, perturbationArrayB, perArrB, arrB);
            Randomizer(maxPerB, perturbationArrayBC, perArrBC, arrBC);
            BRAlgoritm(resultPer, perArrA, perArrB, perArrBC);
            List<KeyValuePair<string, double>> series2 = new List<KeyValuePair<string, double>>();
            List<ResultPer> itemsPer = new List<ResultPer>();
            int tmpForResult = 0;
            foreach (double Qi in resultPer)
            {
                itemsPer.Add(new ResultPer() { IterationPer = tmpForResult, ResPer = Qi });
                series2.Add(new KeyValuePair<string, double>(tmpForResult.ToString(), Qi));
                tmpForResult++;
            }
            listview1.ItemsSource = itemsPer;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listview1.ItemsSource);
            seriesPertutbation.ItemsSource = series2;
            isCalculatedPerturbation = true;
        }
        public void RenderChartSeries(List<double> result, List<double> resultPer)
        {
            List<KeyValuePair<string, double>> series3 = new List<KeyValuePair<string, double>>();
            
            for (int i = 0; i < result.Count; i++)
            {
                series3.Add(new KeyValuePair<string, double>(i.ToString(), Math.Abs(result[i] - resultPer[i])));
            }
            seriesFault.ItemsSource = series3;
        }
        public void Randomizer(int MaxValue, int[] perturbationValue, double[] arrayPerturbationFractions, double[] currentArray)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < perturbationValue.Length; i++)
            {
                int j = rand.Next(0, MaxValue);
                perturbationValue[i] = j;
                arrayPerturbationFractions[i] = currentArray[i] + (currentArray[i] * perturbationValue[i] / 100);
            }

        }
        public class Result
        {
            public int Iteration { get; set; }
            public double Res { get; set; }
        }
        public class ResultPer
        {
            public int IterationPer { get; set; }
            public double ResPer { get; set; }
        }
    }
}