using ScottPlot;
using ScottPlot.Plottable;
using System.Drawing;

namespace App.PdfPageProduct.AnalysisPageFeature
{
    public static class GenGraph
    {
        public static byte[] ClassStudendLinearRegression(double[] values)
        {
            // grafik 3 : öğrencilerin sınıf başarı grubu grafiği.N kadar aralık üzerinden.
            var plt = new Plot(750, 750);


            // Create some linear but noisy data
            double[] ys = DataGen.NoisyLinear(null, pointCount: 100, noise: 30);
            double[] xs = DataGen.Consecutive(ys.Length);
            double x1 = xs[0];
            double x2 = xs[xs.Length - 1];

            // use the linear regression fitter to fit these data
            var model = new ScottPlot.Statistics.LinearRegressionLine(xs, ys);

            // plot the original data and add the regression line
            plt.Title("Linear Regression\n" +
                $"Y = {model.slope:0.0000}x + {model.offset:0.0} " +
                $"(R² = {model.rSquared:0.0000})");
            plt.AddScatter(xs, ys, lineWidth: 0);
            plt.AddLine(model.slope, model.offset, (x1, x1
                ), lineWidth: 2);


            return plt.GetImageBytes();
        }

        public static byte[] LabeledClassStudendHistogram(double[] values)
        {
            Plot plt = new(750, 750);

            // create a histogram with a fixed number of bins
            ScottPlot.Statistics.Histogram hist = new(min: 140, max: 220, binCount: 100);

            // add random data to the histogram
            Random rand = new(0);
            double[] heights = ScottPlot.DataGen.RandomNormal(rand, pointCount: 1234, mean: 178.4, stdDev: 7.6);
            hist.AddRange(heights);

            // display histogram probabability as a bar plot
            double[] probabilities = hist.GetProbability();
            var bar = plt.AddBar(values: probabilities, positions: hist.Bins);
            bar.BarWidth = 1;
            bar.FillColor = ColorTranslator.FromHtml("#9bc3eb");
            bar.BorderColor = ColorTranslator.FromHtml("#82add9");

            // display histogram probability curve as a line plot
            plt.AddFunction(hist.GetProbabilityCurve(heights, true), Color.Black, 2, LineStyle.Dash);

            // customize the plot style
            plt.Title("Adult Male Height");
            plt.YAxis.Label("Probability");
            plt.XAxis.Label("Height (cm)");
            plt.SetAxisLimits(yMin: 0);



            return plt.GetImageBytes();
        }


        public static byte[] LabeledClassStudendSegmentPie(double[] values)
        {
            // grafik 3 : öğrencilerin sınıf başarı grubu grafiği.N kadar aralık üzerinden.
            
            Plot plot = new(750, 750);
            plot.Title("Sınıf başarı durumu pasta grafiği");
            plot.Legend();
            
            PiePlot pie = plot.AddPie(values);
            pie.Explode = true;
            pie.ShowPercentages = true;
            pie.ShowValues = true;
            pie.ShowLabels = true;

            return plot.GetImageBytes();
        }

        public static byte[] LabeledQuestScoreBar(double[] values)
        {
            // grafik 1 : n kadar soruya verilen n kadar sorunun puan ortalamsı sütun grafiği.y = puan ortalaması.x = n kadar soru

            double[] roundedValues = values.Select(v => Math.Round(v, 2)).ToArray();

            IList<string> labelsOfQuest = [];
            IList<double> positionsOfQuest = [];

            Plot plot = new(750, 750);
            plot.Title("Soru Bazında Ortalama Puan");
            
            for (int i = 0; i < values.Length; i++)
            {
                labelsOfQuest.Add($"#{i + 1}");
                positionsOfQuest.Add(i);
            }

            BarPlot barPlot = plot.AddBar(roundedValues, [.. positionsOfQuest]);
            barPlot.ShowValuesAboveBars = true;
          



            plot.XTicks([.. positionsOfQuest], [.. labelsOfQuest]);
            plot.SetAxisLimits(yMin: 0, yMax: roundedValues.Max()+1);
            return plot.GetImageBytes();

        }
    }
}