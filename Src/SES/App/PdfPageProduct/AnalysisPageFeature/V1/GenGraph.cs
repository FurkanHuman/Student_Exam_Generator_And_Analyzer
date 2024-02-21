using ScottPlot;
using ScottPlot.Plottables;
using System.Drawing;

namespace App.PdfPageProduct.AnalysisPageFeature.V1
{
    public static class GenGraph
    {
        //public static byte[] ClassStudendLinearRegression(double[] values)
        //{
        //    // grafik 3 : öğrencilerin sınıf başarı grubu grafiği.N kadar aralık üzerinden.
        //    Plot plt = new Plot(750, 750);


        //    // Create some linear but noisy data
        //    double[] ys = DataGen.NoisyLinear(null, pointCount: 100, noise: 30);
        //    double[] xs = DataGen.Consecutive(ys.Length);
        //    double x1 = xs[0];
        //    double x2 = xs[xs.Length - 1];

        //    // use the linear regression fitter to fit these data
        //    var model = new ScottPlot.Statistics.LinearRegressionLine(xs, ys);

        //    // plot the original data and add the regression line
        //    plt.Title("Linear Regression\n" +
        //        $"Y = {model.slope:0.0000}x + {model.offset:0.0} " +
        //        $"(R² = {model.rSquared:0.0000})");
        //    plt.AddScatter(xs, ys, lineWidth: 0);
        //    plt.AddLine(model.slope, model.offset, (x1, x1
        //        ), lineWidth: 2);


        //    return plt.GetImageBytes();
        //}

        //public static byte[] LabeledClassStudendHistogram(double[] values)
        //{
        //    Plot plt = new(750, 750);

        //    // create a histogram with a fixed number of bins
        //    ScottPlot.Statistics.Histogram hist = new(min: 140, max: 220, binCount: 100);

        //    // add random data to the histogram
        //    Random rand = new(0);
        //    double[] heights = DataGen.RandomNormal(rand, pointCount: 1234, mean: 178.4, stdDev: 7.6);
        //    hist.AddRange(heights);

        //    // display histogram probabability as a bar plot
        //    double[] probabilities = hist.GetProbability();
        //    var bar = plt.AddBar(values: probabilities, positions: hist.Bins);
        //    bar.BarWidth = 1;
        //    bar.FillColor = ColorTranslator.FromHtml("#9bc3eb");
        //    bar.BorderColor = ColorTranslator.FromHtml("#82add9");

        //    // display histogram probability curve as a line plot
        //    plt.AddFunction(hist.GetProbabilityCurve(heights, true), Color.Black, 2, LineStyle.Dash);

        //    // customize the plot style
        //    plt.Title("Adult Male Height");
        //    plt.YAxis.Label("Probability");
        //    plt.XAxis.Label("Height (cm)");
        //    plt.SetAxisLimits(yMin: 0);



        //    return plt.GetImageBytes();
        //}


        public static byte[] LabeledClassStudendSegmentPie(int[] scores)
        {

            int excellentCount = scores.Count(score => score == 5);
            int goodCount = scores.Count(score => score == 4);
            int averageCount = scores.Count(score => score == 3);
            int passCount = scores.Count(score => score == 2);
            int failCount = scores.Count(score => score == 1);

            PieSlice pieSlice1 = new() { Value = excellentCount, FillColor = Colors.Green, Label = "5 Alan", };
            PieSlice pieSlice2 = new() { Value = goodCount, FillColor = Colors.GreenYellow, Label = "4 Alan" };
            PieSlice pieSlice3 = new() { Value = averageCount, FillColor = Colors.Yellow, Label = "3 Alan" };
            PieSlice pieSlice4 = new() { Value = passCount, FillColor = Colors.Orange, Label = "2 Alan" };
            PieSlice pieSlice5 = new() { Value = failCount, FillColor = Colors.Red, Label = "1 Alan" };

            List<PieSlice> pieSlices = [pieSlice1, pieSlice2, pieSlice3, pieSlice4, pieSlice5];

            Plot plot = new();
            plot.Title("Sınıf Başarı Dağılımı");
            plot.HideGrid();

            Pie pie = plot.Add.Pie(pieSlices);

            pie.ExplodeFraction = 1;
            pie.ShowSliceLabels = true;

            return plot.GetImageBytes(750, 750, ImageFormat.Png);
        }

        public static byte[] LabeledQuestScoreBar(double[] values)
        {
            // grafik 1 : n kadar soruya verilen n kadar sorunun puan ortalamsı sütun grafiği.y = puan ortalaması.x = n kadar soru

            double[] roundedValues = values.Select(v => Math.Round(v, 2)).ToArray();
                        
            IList<Tick> ticks = [];

            Plot plot = new();
            plot.Title("Soru Bazında Ortalama Puan");

            for (int i = 0; i < values.Length; i++)
            {
                plot.Add.Bar(position: i + 1, roundedValues[i]);
                ticks.Add(new(i+1, $"#{i + 1}"));
            }

            plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual([.. ticks]);
            plot.Axes.Bottom.MajorTickStyle.Length = 0;
            return plot.GetImageBytes(750, 750, ImageFormat.Png);
        }
    }
}